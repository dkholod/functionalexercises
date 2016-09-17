#r "../packages/FsCheck/lib/net45/FsCheck.dll"
open FsCheck

let badSort xs = xs

let rec quickSort (lst: int list) = 
    match lst with
    | [] | [_] -> lst
    | x::xs -> let l, g = xs |> List.partition ((>=) x)
               quickSort(l) @ [x] @ quickSort(g)

let ``neighbour pairs from a list should be ordered`` sort (xs:int list) = 
    xs |> sort |> Seq.pairwise |> Seq.forall (fun (x,y) -> x <= y )

let ``sorted list should have same length as original`` sort (xs:int list) = 
    (xs |> sort |> List.length) = (xs |> List.length)

// --- combining properties      
let ``list is sorted`` sortFn (lst:int list) = 
    let prop1 = ``neighbour pairs from a list should be ordered`` sortFn lst 
                |@ "neighbour pairs from a list should be ordered"
    let prop2 = ``sorted list should have same length as original`` sortFn lst 
                |@ "sort should have same length as original"
    prop1 .&. prop2 

#time
let config = { Config.Quick with MaxTest = 100000 }
Check.One (config, ``list is sorted`` badSort)
Check.One (config, ``list is sorted`` quickSort)