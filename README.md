# FSI Cacher Runner

**FSI Cacher Runner** is a simple tool to optimize the execution of F# scripts. It automatically detects available F# runtimes, caches compiled scripts for better performance, and uses the fastest available runtime while maintaining cross-compatibility.

## Features

- **Smart Runtime Detection**: Automatically uses the fastest available F# runtime
- **Cross-Runtime Compatibility**: Works with both Mono/F# and .NET Core
- **Performance Optimized**: Achieves optimal execution times based on available runtime
- **Automatic Caching**: Compiles and caches scripts for faster subsequent runs
- **Change Detection**: Automatically recompiles when scripts are modified
- **Cross-Platform**: Works on Linux, macOS, and Windows (with compatible shell)

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/wizzardx/fsi-cacher-runner.git
   cd fsi-cacher-runner
   ```

2. Make the `fsi` script executable:
   ```bash
   chmod +x fsi
   ```

3. Move it to a directory in your `PATH` (e.g., `/usr/local/bin`):
   ```bash
   mv fsi /usr/local/bin/fsi
   ```

4. Install either (or both) F# runtime:
   ```bash
   # Mono/F# (faster execution times):
   sudo apt install fsharp  # Ubuntu/Debian
   brew install mono        # macOS with Homebrew

   # .NET Core:
   # Install from https://dot.net
   ```

## Usage

1. Create an F# script, for example, `test.fsx`:
   ```fsharp
   #!/usr/bin/env fsi
   printfn "Hello, world!"
   ```

2. Make your script executable:
   ```bash
   chmod +x test.fsx
   ```

3. Run your script:
   ```bash
   ./test.fsx
   ```

The script will be compiled and cached on first run. Performance depends on the available runtime:
- Mono/F#: ~30-50ms for cached runs
- .NET Core: ~200-250ms for cached runs

## How It Works

The `fsi` tool:
1. Detects available F# runtimes (Mono/F# and/or .NET Core)
2. Selects the runtime that will provide fastest execution
3. Creates a cache key based on the script path
4. Compiles the script if needed (no cache or script modified)
5. Executes the cached binary using the selected runtime
6. Preserves all script arguments for the executed program

## Performance

First run compilation times:
- Mono/F#: ~1-2 seconds
- .NET Core: ~2-3 seconds

Cached execution times:
- Mono/F#: ~30-50ms
- .NET Core: ~200-250ms

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
