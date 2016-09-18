#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
open System
open FSharp.Data
    
let countryGenerator =
    let countries = WorldBankData.GetDataContext().Countries 
                    |> Seq.map (fun it -> (it.Name, it.CapitalCity)) 
                    |> Seq.toArray
    let rnd = Random()
    fun () -> countries |> Array.item (rnd.Next(Array.length countries))

let rec game() =
    let country, capital = countryGenerator()
    printf "What is the capital of %s: " country
    match Console.ReadLine() with
        | c when String.Compare(c, capital, StringComparison.OrdinalIgnoreCase) = 0 -> 
                                        printfn "This is correct, let's play again."
                                        game()
        | _ ->  printfn "This is wrong. The correct answer is %s. Goodbye" capital

game()