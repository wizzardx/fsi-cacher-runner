#!/bin/bash

script_path="$1"
if [ ! -f "$script_path" ]; then
    echo "Script file not found: $script_path"
    exit 1
fi

cache_dir="/tmp/fsharp-cache"
mkdir -p "$cache_dir"

cache_name=$(echo "$script_path" | md5sum | cut -d' ' -f1)
cached_binary="$cache_dir/$cache_name.exe"

rebuild=0
if [ ! -f "$cached_binary" ]; then
    rebuild=1
elif [ "$script_path" -nt "$cached_binary" ]; then
    rebuild=1
fi

if [ $rebuild -eq 1 ]; then
    fsharpc --target:exe --out:"$cached_binary" "$script_path" 2>&1 | grep -v "F# Compiler for F# 4.0" | grep -v "Freely distributed"
    if [ ${PIPESTATUS[0]} -ne 0 ]; then
        exit 1
    fi
fi

exec mono "$cached_binary" "${@:2}"
