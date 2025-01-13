#!/usr/bin/env fsi

// Basic self-test
printfn "Running self-test suite..."

type TestCase = {
    Name: string
    Description: string
    Category: string
    Test: unit -> unit
    Skip: bool
}

let passCount = ref 0
let failCount = ref 0

// Test helper to create and run a test script
let runTestScript (name: string) (content: string) (args: string list) =
    // Create temporary script
    let scriptPath = sprintf "/tmp/test_%s.fsx" name
    System.IO.File.WriteAllText(scriptPath, content.TrimStart())
    
    // Make executable
    let psi = new System.Diagnostics.ProcessStartInfo()
    psi.FileName <- "chmod"
    psi.Arguments <- sprintf "+x %s" scriptPath
    let p = System.Diagnostics.Process.Start(psi)
    p.WaitForExit()
    
    // Run script with args
    let escapeArg (arg: string) =
        // Replace any embedded quotes with escaped quotes
        let escaped = arg.Replace("\"", "\\\"")
        sprintf "\"%s\"" escaped
        
    let escapedArgs = args |> List.map escapeArg
    let psi = new System.Diagnostics.ProcessStartInfo()
    psi.FileName <- "./fsi"
    psi.Arguments <- sprintf "%s %s" scriptPath (String.concat " " escapedArgs)
    psi.RedirectStandardOutput <- true
    psi.RedirectStandardError <- true
    psi.UseShellExecute <- false
    let p = System.Diagnostics.Process.Start(psi)
    let output = p.StandardOutput.ReadToEnd()
    let error = p.StandardError.ReadToEnd()
    p.WaitForExit()
    
    // Cleanup
    System.IO.File.Delete(scriptPath)
    
    // Return exit code and combined output
    (p.ExitCode, output + error)

// Define test functions
let basicExecutionTest() = ()  // If we got here, we're running!

let basicArgumentTest() =
    let script = """#!/usr/bin/env fsi
let args = System.Environment.GetCommandLineArgs()
printfn "%d" (args.Length - 1)  // -1 to exclude script name
"""
    let (code, output) = runTestScript "args" script ["arg1"; "arg2"]
    if code <> 0 then failwithf "Script failed with code %d" code
    if output.Trim() <> "2" then failwithf "Expected 2 args, got output: %s" output

let specialCharsTest() =
    let script = """#!/usr/bin/env fsi
let args = System.Environment.GetCommandLineArgs()
args |> Array.skip 1 |> Array.iter (printfn "%s")
"""
    let (code, output) = runTestScript "special_chars" script 
                            ["hello world"; 
                             "path/with/slashes"; 
                             "quotes\"here"; 
                             "$SHELL"]
    if code <> 0 then failwithf "Script failed with code %d" code
    let lines = output.Split('\n') |> Array.filter (fun s -> s <> "")
    if lines.Length <> 4 then failwithf "Expected 4 lines of output, got %d" lines.Length

let noArgsTest() =
    let script = """#!/usr/bin/env fsi
let args = System.Environment.GetCommandLineArgs()
printfn "%d" (args.Length - 1)  // -1 to exclude script name
"""
    let (code, output) = runTestScript "no_args" script []
    if code <> 0 then failwithf "Script failed with code %d" code
    if output.Trim() <> "0" then failwithf "Expected 0 args, got output: %s" output

let manyArgsTest() =
    let script = """#!/usr/bin/env fsi
let args = System.Environment.GetCommandLineArgs()
printfn "%d" (args.Length - 1)  // -1 to exclude script name
"""
    let args = List.map string [1..100]
    let (code, output) = runTestScript "many_args" script args
    if code <> 0 then failwithf "Script failed with code %d" code
    if output.Trim() <> "100" then failwithf "Expected 100 args, got output: %s" output

let malformedScriptTest() =
    let script = """#!/usr/bin/env fsi
let x = 1
printfn "%
"""  // Unclosed string literal
    let (code, output) = runTestScript "malformed" script []
    if code = 0 then failwith "Expected non-zero exit code for malformed script"

let missingDependencyTest() =
    let script = """#!/usr/bin/env fsi
open NonExistentModule
printfn "Should not get here"
"""
    let (code, output) = runTestScript "missing_dep" script []
    if code = 0 then failwith "Expected non-zero exit code for script with missing dependency"

let syntaxErrorTest() =
    let script = """#!/usr/bin/env fsi
let 1 = 2  // Invalid binding
"""
    let (code, output) = runTestScript "syntax_error" script []
    if code = 0 then failwith "Expected non-zero exit code for script with syntax error"

let runtimeErrorTest() =
    let script = """#!/usr/bin/env fsi
let items = []
printfn "%d" (items |> List.head)  // Empty list, will throw
"""
    let (code, output) = runTestScript "runtime_error" script []
    if code = 0 then failwith "Expected non-zero exit code for script with runtime error"

// Define test suite
let tests = [|
    { Name = "basic_execution"; Description = "Basic execution test"; Category = "Core"; Test = basicExecutionTest; Skip = false }
    { Name = "basic_arguments"; Description = "Basic argument passing"; Category = "Arguments"; Test = basicArgumentTest; Skip = false }
    { Name = "special_chars"; Description = "Special characters in arguments"; Category = "Arguments"; Test = specialCharsTest; Skip = false }
    { Name = "no_args"; Description = "No arguments"; Category = "Arguments"; Test = noArgsTest; Skip = false }
    { Name = "many_args"; Description = "Many arguments (100)"; Category = "Arguments"; Test = manyArgsTest; Skip = false }
    { Name = "malformed"; Description = "Malformed F# script"; Category = "Errors"; Test = malformedScriptTest; Skip = false }
    { Name = "missing_dep"; Description = "Missing dependency"; Category = "Errors"; Test = missingDependencyTest; Skip = false }
    { Name = "syntax_error"; Description = "Syntax error"; Category = "Errors"; Test = syntaxErrorTest; Skip = false }
    { Name = "runtime_error"; Description = "Runtime error"; Category = "Errors"; Test = runtimeErrorTest; Skip = false }
|]

// Test runner
let runTest (testCase: TestCase) =
    if testCase.Skip then
        printfn "[SKIP] %s" testCase.Description
    else
        let current = !passCount + !failCount + 1
        let total = tests |> Array.filter (fun t -> not t.Skip) |> Array.length
        printf "[%d/%d] %s " current total testCase.Description
        
        // Buffer for capturing test output
        let mutable testOutput = ""
        let success = ref true
        try
            // Capture output
            use writer = new System.IO.StringWriter()
            let originalOut = System.Console.Out
            let originalError = System.Console.Error
            System.Console.SetOut(writer)
            System.Console.SetError(writer)
            
            try
                testCase.Test()
                // Test passed
                System.Console.SetOut(originalOut)
                System.Console.SetError(originalError)
                testOutput <- writer.ToString()
                incr passCount
            with e ->
                // Test failed
                success := false
                System.Console.SetOut(originalOut)
                System.Console.SetError(originalError)
                testOutput <- writer.ToString()
                incr failCount
                printfn "✗"
                printfn "Error: %s" e.Message
                if testOutput.Trim() <> "" then
                    printfn "Output from failed test:"
                    printfn "%s" testOutput
        finally
            System.Console.SetOut(System.Console.Out)
            System.Console.SetError(System.Console.Error)
            if !success then
                printfn "✓"

// Run all tests
tests |> Array.iter runTest

// Print summary by category
printfn "\nTest Results by Category:"
tests 
|> Array.groupBy (fun t -> t.Category)
|> Array.iter (fun (category, categoryTests) ->
    let total = categoryTests |> Array.length
    let passed = categoryTests |> Array.filter (fun t -> not t.Skip) |> Array.length
    let skipped = categoryTests |> Array.filter (fun t -> t.Skip) |> Array.length
    printfn "%s: %d total, %d ran, %d skipped" category total (total - skipped) skipped
)

printfn "\nOverall Summary:"
printfn "Total: %d, Passed: %d, Failed: %d" !passCount (!passCount - !failCount) !failCount
if !failCount > 0 then exit 1
