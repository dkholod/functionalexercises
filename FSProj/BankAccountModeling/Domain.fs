namespace Domain
module Domain = 
    
    type Amount = decimal
    
    type CustomerName = string    

    type Transaction = 
        | Withdraw of Amount
        | Deposit of Amount

    type Account = { Name: CustomerName; Transactions: Transaction seq }

    type TransactionResult = 
        | Accepted of Account
        | Rejected of Account

    let (>>=) v f = 
        match v with
            | Accepted a -> f a
            | Rejected b -> f b
    let newAccount name = 
        { Name = name; Transactions = [Deposit 0M] }

    let balance account = 
        let toDecimal = function
            | Withdraw w -> -w 
            | Deposit d -> d        
        account.Transactions 
        |> Seq.map toDecimal
        |> Seq.reduce (+)

    let deposit amount account=
        Accepted { account with Transactions = account.Transactions |> Seq.append [Deposit amount] }

    let withdraw amount account =
        if (balance account) > amount then 
            Accepted { account with Transactions = account.Transactions |> Seq.append [Withdraw amount] }
        else
            Rejected account
    let testAcc = newAccount "Denys"
    let acc = (deposit 10M testAcc) >>= (withdraw 3M) >>= (withdraw 5M)
    
    let amount = function
        | Accepted a -> balance a
        | Rejected b -> balance b

    amount acc