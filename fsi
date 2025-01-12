#!/bin/bash

# Enable for debugging
# set -x

# Check for available F# implementations
have_fsharpc=0
have_dotnet=0

if command -v fsharpc >/dev/null 2>&1; then
    have_fsharpc=1
fi

if command -v dotnet >/dev/null 2>&1 || [ -f "$HOME/.dotnet/dotnet" ]; then
    have_dotnet=1
    export PATH="$HOME/.dotnet:$PATH"
    export DOTNET_ROOT="$HOME/.dotnet"
fi

script_path="$1"

# Handle --clean flag
if [ "$1" = "--clean" ]; then
    echo "Cleaning F# script cache..."
    rm -rf "/tmp/fsharp-cache"
    exit 0
fi

if [ ! -f "$script_path" ]; then
    echo "Script file not found: $script_path"
    exit 1
fi

# Create cache directories if they don't exist
cache_base="/tmp/fsharp-cache"
mono_cache="$cache_base/mono"
dotnet_cache="$cache_base/dotnet"

mkdir -p "$mono_cache"
mkdir -p "$dotnet_cache"
chmod 755 "$cache_base" "$mono_cache" "$dotnet_cache"

if [ $have_fsharpc -eq 1 ]; then
    # Use faster Mono implementation
    cache_name=$(echo "$script_path" | md5sum | cut -d' ' -f1)
    cached_binary="$mono_cache/$cache_name.exe"

    rebuild=0
    if [ ! -f "$cached_binary" ]; then
        rebuild=1
    elif [ "$script_path" -nt "$cached_binary" ]; then
        rebuild=1
    fi

    if [ $rebuild -eq 1 ]; then
        fsharpc --nologo -o:"$cached_binary" "$script_path" > /dev/null
        if [ $? -ne 0 ]; then
            echo "Mono compilation failed"
            exit 1
        fi
    fi

    exec mono "$cached_binary" "${@:2}"

elif [ $have_dotnet -eq 1 ]; then
    # Fall back to .NET Core
    dotnet_version=$(dotnet --version | cut -d. -f1,2)
    cache_name=$(echo "$script_path-$dotnet_version" | md5sum | cut -d' ' -f1)
    cached_dll="$dotnet_cache/$cache_name.dll"

    rebuild=0
    if [ ! -f "$cached_dll" ]; then
        rebuild=1
    elif [ "$script_path" -nt "$cached_dll" ]; then
        rebuild=1
    fi

    if [ $rebuild -eq 1 ]; then
        tmp_proj_dir=$(mktemp -d)
        
        cat > "$tmp_proj_dir/script.fsproj" << EOF
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net$dotnet_version</TargetFramework>
    <PublishSingleFile>true</PublishSingleFile>
    <SelfContained>false</SelfContained>
    <InvariantGlobalization>true</InvariantGlobalization>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="script.fs" />
  </ItemGroup>
</Project>
EOF
        
        sed '1{/^#!/d;}' "$script_path" > "$tmp_proj_dir/script.fs"
        
        echo "Building in $tmp_proj_dir..."
        (cd "$tmp_proj_dir" && dotnet build -o "$dotnet_cache" /p:AssemblyName="$cache_name" -v:q --nologo)
        status=$?
        
        rm -rf "$tmp_proj_dir"
        
        if [ $status -ne 0 ]; then
            echo "Build failed"
            exit 1
        fi
    fi

    cd "$dotnet_cache" || exit 1
    exec dotnet "$cache_name.dll" "${@:2}"

else
    echo "No F# implementation found. Please install either:"
    echo "  - Mono F# (sudo apt install fsharp)"
    echo "  - .NET Core (https://dot.net)"
    exit 1
fi
