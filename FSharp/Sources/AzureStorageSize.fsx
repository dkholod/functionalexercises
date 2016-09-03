#load "../packages/FSharp.Azure.StorageTypeProvider/StorageTypeProvider.fsx"
open FSharp.Azure.StorageTypeProvider
open Microsoft.WindowsAzure.Storage.Blob

type Azure =
    AzureTypeProvider<"DefaultEndpointsProtocol=https;AccountName=functiond31d29008d75;AccountKey=DYSnfB8W6/Jikmh4Cuyo7ZPW4G+cy2tiDKurVinxbvV1TlnFld6RGdWZOKkKx0IdKV8P3r+RNJPruxJJVTuBcw==;">

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
