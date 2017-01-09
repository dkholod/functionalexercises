open System
open System.Diagnostics
open System.Threading.Tasks
open System.Threading

type Agent = MailboxProcessor<string>

let toSeconds ms = 
    float ms * 0.001

let testAgent n = Agent.Start(fun inbox-> 
    let mutable msgCount = 0
    let stopWatch = Stopwatch();

    let rec messageLoop() = async{
        let! msg = inbox.Receive()
        
        if msgCount = 0 then
            stopWatch.Start()

        if msgCount = n then
            let track = stopWatch.ElapsedMilliseconds
            let rate = (float n) / (toSeconds track)
            printf "%f msg/s" rate

        msgCount <- msgCount + 1
        return! messageLoop()  
        }

    messageLoop() 
    )

[<EntryPoint>]
let main argv = 
    
    let numOfMil = 4
    let test = testAgent (numOfMil * 1000000)
    use barrier = new Barrier(numOfMil)

    let notify() = 
        barrier.SignalAndWait();
        printfn "\nThread id %i is ready" Threading.Thread.CurrentThread.ManagedThreadId
        for i in [1..1000001] do
            test.Post "test"

    for i in [1..numOfMil] do
        Task.Run (fun _ -> notify()) |> ignore

    Console.ReadLine() |> ignore
    0