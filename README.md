# FSI Cacher Runner

**FSI Cacher Runner** is a simple tool to optimize the execution of F# scripts. It automatically detects available F# runtimes, caches compiled scripts for better performance, and uses the fastest available runtime while maintaining cross-compatibility.

## Features

- **Smart Runtime Detection**: Automatically uses the fastest available F# runtime
- **Cross-Runtime Compatibility**: Works with both Mono/F# and .NET Core
- **Performance Optimized**: Achieves optimal execution times based on available runtime
- **Automatic Caching**: Compiles and caches scripts for faster subsequent runs
- **Change Detection**: Automatically recompiles when scripts are modified
- **Cross-Platform**: Works on Linux, macOS, and Windows (via WSL)
- **CI/CD Verified**: Automated testing on Ubuntu, macOS, and Windows/WSL

## Installation

### Linux (Ubuntu/Debian)
```bash
git clone https://github.com/wizzardx/fsi-cacher-runner.git
cd fsi-cacher-runner
chmod +x fsi
sudo mv fsi /usr/local/bin/fsi
sudo apt install fsharp  # For Mono/F# (faster execution times)
```

### macOS
```bash
git clone https://github.com/wizzardx/fsi-cacher-runner.git
cd fsi-cacher-runner
chmod +x fsi
sudo mv fsi /usr/local/bin/fsi
brew install mono  # For Mono/F# (faster execution times)
```

### Windows (WSL)
1. Install WSL if you haven't already:
```powershell
wsl --install Ubuntu
```

2. Inside WSL Ubuntu terminal:
```bash
git clone https://github.com/wizzardx/fsi-cacher-runner.git
cd fsi-cacher-runner
chmod +x fsi
sudo mv fsi /usr/local/bin/fsi
sudo apt update
sudo apt install -y fsharp  # For Mono/F# (faster execution times)
```

For all platforms, you can alternatively install .NET Core from https://dot.net

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

## Benchmark Environment

These performance numbers were measured on the following system:

- **CPU**: Intel Core i7-8750H @ 2.20GHz (6 cores/12 threads)
- **RAM**: 16GB
- **Storage**: NVMe SSD
- **OS**: Ubuntu 24.04.1 LTS (64-bit)
- **Runtime Versions**:
  - **.NET**: 8.0.404
  - **Mono**: 6.8.0.105
  - **F#**: 4.0 (Open Source Edition)

Note: Your performance may vary depending on your system specifications and installed runtime versions.

## Contributing

Contributions are welcome! Please follow these guidelines:

1. **Testing**: All changes must be accompanied by tests
   - Add tests to `test_fsi.fsx` or related test modules
   - All tests must pass before submitting PRs
   - The test suite is self-hosted and runs through FSI Cacher itself

2. **Running Tests**:
   ```bash
   ./test_fsi.fsx
   ```

3. **Adding New Tests**:
   - Add test cases to `test_fsi.fsx`
   - Follow the existing test pattern:
     ```fsharp
     test "Description of the test" (fun () ->
         // Test code here
         // Throw exception if test fails
     )
     ```

4. **Documentation**:
   - Update README.md if adding new features
   - Document any new test patterns or test helper functions
   - Keep performance numbers up to date if relevant

Pull requests that don't include tests or don't pass existing tests will not be accepted.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
```
