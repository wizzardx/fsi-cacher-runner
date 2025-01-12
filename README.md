# FSI Cacher Runner

**FSI Cacher Runner** is a simple tool to optimize the execution of F# scripts. It caches compiled F# scripts to improve performance for repeated runs and automatically recompiles scripts when changes are detected.

## Features

- **Caching**: Compiles and caches scripts for faster subsequent runs.
- **Automatic Rebuilds**: Detects changes to the script and recompiles as needed.
- **Cross-Platform**: Works on Linux, macOS, and Windows (with a compatible shell and Mono).

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/<your-username>/fsi-cacher-runner.git
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

## Usage

1. Create an F# script, for example, `test.fsx`:
   ```fsharp
   #!/usr/bin/env fsi
   printfn "Hello, world! Again!"
   ```

2. Make your script executable:
   ```bash
   chmod +x test.fsx
   ```

3. Run your script using:
   ```bash
   ./test.fsx
   ```

   The script will be compiled and cached on the first run. Subsequent executions will be much faster unless the script is modified.

## How It Works

The `fsi` tool:
1. Checks if the script has a cached compiled version.
2. Compiles the script if necessary.
3. Executes the cached binary using Mono.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
