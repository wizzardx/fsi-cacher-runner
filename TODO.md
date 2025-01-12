# TODO List for FSI Cacher Runner

## Testing

### Basic Error Scenarios
- [ ] Test with malformed F# scripts (syntax errors)
- [ ] Test with missing dependencies in scripts
- [ ] Test with various command line argument patterns
- [ ] Test scripts that read/write files
- [ ] Test scripts with different working directories

### Cross-Platform Testing
- [ ] Test on Windows WSL
- [ ] Test on macOS
- [ ] Test on different Linux distributions (Ubuntu, Fedora, etc.)
- [ ] Document any platform-specific issues or requirements

### Advanced F# Scenarios
- [ ] Test scripts that reference external F# libraries
- [ ] Test scripts with F# interactive directives (#r, #load)
- [ ] Test performance with larger scripts (1000+ lines)
- [ ] Test scripts that use F# type providers
- [ ] Test scripts with different encoding types (UTF-8, ASCII, etc.)

## Documentation

### Error Handling Documentation
- [ ] Document common error messages and their solutions
- [ ] Add a troubleshooting section to README
- [ ] Document known limitations
- [ ] Add examples of error scenarios and how to resolve them

### Usage Examples
- [ ] Add more complex script examples
- [ ] Document performance tips
- [ ] Add examples of using external dependencies
- [ ] Document cache management and cleanup

## Development Infrastructure

### CI/CD Setup
- [ ] Set up basic GitHub Actions workflow
- [ ] Add automated testing across different environments
- [ ] Add automated release process
- [ ] Add code quality checks

### Code Improvements
- [ ] Add verbose mode for debugging
- [ ] Improve error messages
- [ ] Add cache size management
- [ ] Consider adding configuration options

## Community Engagement

### Pre-Release Tasks
- [ ] Create comprehensive release notes
- [ ] Prepare r/fsharp post draft
- [ ] Create example repository showcasing different use cases
- [ ] Document comparison with existing tools (if found)

### Post-Release Monitoring
- [ ] Monitor GitHub issues
- [ ] Track community feedback
- [ ] Document common questions and answers
- [ ] Plan future improvements based on feedback

## Future Considerations
- [ ] Consider integration with existing F# tooling
- [ ] Investigate potential performance optimizations
- [ ] Consider adding support for script dependencies tracking
- [ ] Evaluate need for configuration file support
