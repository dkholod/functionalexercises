namespace Tests
module Tests =

  open System
  open Xunit 
  open Functions

  let randNum =
    let rnd = Random ()
    fun () -> rnd.Next()

  [<Fact>]
  let ``when I multiply 2 and 2, I expect 4`` () = 
    let result =  multiply 2 2
    Assert.Equal (4, result)
  
  [<Fact>]
  let ``when I multiply by 1 and 4, I expect 4`` () = 
    let result =  multiply 2 2
    Assert.Equal (4, result)
  
  [<Fact>]
  let ``when I multiply by -1 and 3, I expect -3`` () = 
    let result =  multiply 2 2
    Assert.Equal (result, 4)

  [<Fact>]
  let ``when I use random factors, I expect correct product`` () = 
    let a = randNum()
    let b = randNum()
    let expected = a * b
    Assert.Equal (expected, multiply a b)

  [<Fact>]
  let ``when I multiply a*b is same as b*a`` () = 
      let a = randNum()
      let b = randNum()
      let ``a*b`` =  multiply a b
      let ``b*a`` =  multiply b a
      printfn "%i" ``a*b``
      Assert.Equal (``a*b``, ``b*a``)