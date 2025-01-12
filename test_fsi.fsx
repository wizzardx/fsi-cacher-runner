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

// First test - can we run at all?
test "Basic execution" (fun () ->
    // If we got here, we're running!
    ()
)

// Test argument passing
test "Command line arguments" (fun () ->
    let args = System.Environment.GetCommandLineArgs()
    if args.Length < 1 then 
        failwith "Expected at least script name in args"
)

printfn "\nTest summary:"
printfn "Total: %d, Passed: %d, Failed: %d" !testCount !passCount !failCount
