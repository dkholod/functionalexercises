(ns azurelog.reader
  (:import [com.microsoft.azure.storage CloudStorageAccount]))

(def conn-str
  (str "DefaultEndpointsProtocol=http;"
       "AccountName=name;"
       "AccountKey=secret-key-token"))

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