# TODO List for FSI Cacher Runner

## Phase 1 - Core Testing (Pre-Reddit Announcement)

### CI/CD Infrastructure
- [x] Set up basic GitHub Actions workflow ✅ 2025-01-12
- [x] Ubuntu Latest (GitHub Actions) ✅ 2025-01-12
- [x] macOS Latest (GitHub Actions) ✅ 2025-01-12
- [x] Windows WSL2 (Ubuntu) Support ✅ 2025-01-13
  - [x] Basic script execution ✅ 2025-01-13
  - [x] Cache directory paths ✅ 2025-01-13
  - [x] md5/md5sum compatibility ✅ 2025-01-13
  - [x] .NET and Mono detection/execution ✅ 2025-01-13

### Comprehensive Test Suite
- [ ] Update test_fsi.fsx with core scenarios
  - [x] Basic execution with different argument patterns ✅ 2025-01-13
  - [ ] Error handling (malformed scripts, missing deps)
  - [ ] File path edge cases (spaces, special chars)
  - [ ] Cache behavior verification
  - [ ] Runtime detection logic

### Performance Verification
- [ ] Add benchmark tests to verify documented speeds
  - [ ] Mono/F# (30-50ms claim)
  - [ ] .NET Core (200-250ms claim)
  - [ ] First-run compilation times
- [ ] Update README.md with verified numbers

### Documentation Updates
- [ ] Add "Known Issues" section to README.md
- [ ] Add "Limitations" section
- [ ] Document WSL-specific considerations
- [ ] Add real-world usage examples
- [ ] Document cache location and management
- [ ] Verify all installation instructions

### Basic Cache Management
- [ ] Implement cache size limits
- [ ] Add cache statistics command
- [ ] Document cache cleanup procedures

## Phase 2 - Community Launch

### Reddit Announcement Preparation
- [ ] Create comprehensive release notes
- [ ] Prepare r/fsharp post draft
- [ ] Create example repository
- [ ] Document comparison with alternatives

### Post-Launch Monitoring
- [ ] Monitor GitHub issues
- [ ] Track community feedback
- [ ] Document FAQs based on initial questions
- [ ] Plan improvements based on feedback

## Phase 3 - Platform Expansion

### Additional Linux Support
- [ ] Fedora Latest
- [ ] Debian Stable
- [ ] Arch Linux
- [ ] Alpine Linux (minimal environment test)

### Extended macOS Support
- [ ] macOS Intel verification
- [ ] macOS ARM (M1/M2) testing

### Windows Native Support
- [ ] PowerShell Implementation
  - [ ] Create fsi.ps1
  - [ ] Cache directory management
  - [ ] Hash computation equivalent
  - [ ] Runtime detection (.NET/Mono)
  - [ ] Path handling and normalization
- [ ] Windows with Git Bash
- [ ] CMD Implementation
  - [ ] Create fsi.cmd
  - [ ] Decide on PowerShell dependency

## Future Considerations
- [ ] Integration with existing F# tooling
- [ ] Advanced performance optimizations
- [ ] Script dependencies tracking
- [ ] Configuration file support
- [ ] Comprehensive Runtime Testing
  - [ ] Test matrix across F# implementations
    - [ ] Mono F# (versions: latest, LTS)
    - [ ] .NET Core F# (versions: latest, LTS)
  - [ ] Feature compatibility testing
    - [ ] Document minimum F# version requirements
    - [ ] Test for F# version-specific features
    - [ ] Add feature detection where needed
  - [ ] Error handling consistency
    - [ ] Verify error messages across implementations
    - [ ] Document implementation-specific differences
- [ ] Runtime Configuration Testing
  - [ ] Environment Variables
    - [ ] F# specific variables
    - [ ] .NET runtime variables
    - [ ] Mono runtime variables
  - [ ] Runtime Flags
    - [ ] Optimization levels
    - [ ] Memory limits
    - [ ] Debug/verbose modes
    - [ ] Assembly loading paths
  - [ ] Document supported configurations
    - [ ] Required/optional settings
    - [ ] Default values
    - [ ] Performance implications
