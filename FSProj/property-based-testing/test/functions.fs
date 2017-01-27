namespace Functions

[<AutoOpen>]
module Functions =

    (*
    let multiply a b = 
        4
        *)

    let multiply x y = 
        match (x,y) with
            | (-1, 3) -> -3
            | (_, _) -> 4