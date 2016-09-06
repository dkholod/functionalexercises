#load "../packages/FSharp.Azure.StorageTypeProvider/StorageTypeProvider.fsx"
open FSharp.Azure.StorageTypeProvider
open Microsoft.WindowsAzure.Storage.Blob

type Azure =
    AzureTypeProvider<"DefaultEndpointsProtocol=https;AccountName=name;AccountKey=secret-key-token">

let inline toKb b = (b |> double) / 1024.

let sizeOfContainer name = 
    Azure.Containers.CloudBlobClient.ListBlobs (name + "/", true)
    |> Seq.map (fun b -> b :?> CloudBlockBlob)
    |> Seq.sumBy (fun cb -> cb.Properties.Length)

let sizeOfAccount () = 
    Azure.Containers.CloudBlobClient.ListContainers()
    |> Seq.sumBy (fun x -> sizeOfContainer x.Name) 

sizeOfContainer "test" |> toKb
sizeOfAccount() |> toKb
