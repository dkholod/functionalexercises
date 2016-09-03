import scala.collection.JavaConversions._
import com.microsoft.azure.storage._
import com.microsoft.azure.storage.blob._


def toKb (b: Long) = b.toDouble / 1024

object Azure {
  val storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=functiond31d29008d75;AccountKey=DYSnfB8W6/Jikmh4Cuyo7ZPW4G+cy2tiDKurVinxbvV1TlnFld6RGdWZOKkKx0IdKV8P3r+RNJPruxJJVTuBcw==;"
  val account = CloudStorageAccount.parse(storageConnectionString);
  val serviceClient = account.createCloudBlobClient

  def sizeOfContainer (name: String) =
    serviceClient.getContainerReference(name)
    .listBlobs("", true).iterator.toSeq
    .map(_.asInstanceOf[CloudBlockBlob].getProperties.getLength)
    .sum

  def sizeOfAccount =
    serviceClient.listContainers
    .iterator.toSeq
    .map(c => sizeOfContainer(c.getName))
    .sum
}

toKb (Azure sizeOfContainer "test")
toKb (Azure sizeOfAccount)