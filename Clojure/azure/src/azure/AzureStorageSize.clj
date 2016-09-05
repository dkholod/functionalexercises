(ns azurelog.reader
  (:import [com.microsoft.azure.storage CloudStorageAccount]
           [com.microsoft.azure.storage.blob]))

(def conn-str
  (str "DefaultEndpointsProtocol=http;"
       "AccountName=functiond31d29008d75;"
       "AccountKey=DYSnfB8W6/Jikmh4Cuyo7ZPW4G+cy2tiDKurVinxbvV1TlnFld6RGdWZOKkKx0IdKV8P3r+RNJPruxJJVTuBcw==;"))

(def blob-client
  (.createCloudBlobClient (CloudStorageAccount/parse conn-str)))

(defn size-of-container [name]
  (reduce + (map #(.. % getProperties getLength)
                 (iterator-seq
                   (.. blob-client (getContainerReference name) (listBlobs "" true) iterator))
                 ))
  )

(defn size-of-account []
  (reduce + (map #(-> (.getName %) size-of-container)
                 (iterator-seq (.. blob-client listContainers iterator))
                 ))
  )

(defn to-kb [b] (double (/ b 1024)))

(-> "test" size-of-container to-kb)
(to-kb (size-of-account))