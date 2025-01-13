#!/usr/bin/env fsi

// Basic self-test
printfn "Running self-test suite..."

let testCount = ref 0
let passCount = ref 0
let failCount = ref 0

let test name f =
    incr testCount
    try
        f()
        incr passCount
        printfn "✓ %s" name
    with e ->
        incr failCount
        printfn "✗ %s: %s" name e.Message

// Helper to create and run a test script
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
    psi.UseShellExecute <- false
    let p = System.Diagnostics.Process.Start(psi)
    let output = p.StandardOutput.ReadToEnd()
    p.WaitForExit()
    
    // Cleanup
    System.IO.File.Delete(scriptPath)
    
    // Return exit code and output
    (p.ExitCode, output)

// Basic execution tests
test "Basic execution" (fun () ->
    // If we got here, we're running!
    ()
)

// Test simple argument passing
test "Basic argument passing" (fun () ->
    let script = """#!/usr/bin/env fsi
let args = System.Environment.GetCommandLineArgs()
printfn "%d" (args.Length - 1)  // -1 to exclude script name
"""
    let (code, output) = runTestScript "args" script ["arg1"; "arg2"]
    if code <> 0 then 
        failwithf "Script failed with code %d" code
    if output.Trim() <> "2" then 
        failwithf "Expected 2 args, got output: %s" output
)

// Test special characters in arguments
test "Special characters in arguments" (fun () ->
    let script = """#!/usr/bin/env fsi
let args = System.Environment.GetCommandLineArgs()
args |> Array.skip 1 |> Array.iter (printfn "%s")
"""
    let (code, output) = runTestScript "special_chars" script 
                            ["hello world"; 
                             "path/with/slashes"; 
                             "quotes\"here"; 
                             "$SHELL"]
    if code <> 0 then 
        failwithf "Script failed with code %d" code
    // We'll check that we got 4 lines of output
    let lines = output.Split('\n') |> Array.filter (fun s -> s <> "")
    if lines.Length <> 4 then 
        failwithf "Expected 4 lines of output, got %d" lines.Length
)

// Test no arguments
test "No arguments" (fun () ->
    let script = """#!/usr/bin/env fsi
let args = System.Environment.GetCommandLineArgs()
printfn "%d" (args.Length - 1)  // -1 to exclude script name
"""
    let (code, output) = runTestScript "no_args" script []
    if code <> 0 then 
        failwithf "Script failed with code %d" code
    if output.Trim() <> "0" then 
        failwithf "Expected 0 args, got output: %s" output
)

// Test many arguments
test "Many arguments" (fun () ->
    let script = """#!/usr/bin/env fsi
let args = System.Environment.GetCommandLineArgs()
printfn "%d" (args.Length - 1)  // -1 to exclude script name
"""
    let args = List.map string [1..100]
    let (code, output) = runTestScript "many_args" script args
    if code <> 0 then 
        failwithf "Script failed with code %d" code
    if output.Trim() <> "100" then 
        failwithf "Expected 100 args, got output: %s" output
)

printfn "\nTest summary:"
printfn "Total: %d, Passed: %d, Failed: %d" !testCount !passCount !failCount
if !failCount > 0 then exit 1
