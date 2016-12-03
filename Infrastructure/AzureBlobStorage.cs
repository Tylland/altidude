using System;
using System.Linq;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Altidude.Infrastructure
{
    public class AzureBlobStorage
    {
        private string _connectionString;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _container;
        private readonly string _containerName;

        protected AzureBlobStorage(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;

            var storageAccount = CloudStorageAccount.Parse(_connectionString);

            _blobClient = storageAccount.CreateCloudBlobClient();
            _container = _blobClient.GetContainerReference(_containerName);
        }

        public string Save(System.IO.Stream stream, string name, string fileName, string contentType)
        {
            var blockBlob = _container.GetBlockBlobReference(name);

            blockBlob.Properties.ContentType = contentType;

            blockBlob.UploadFromStream(stream);

            return blockBlob.Uri.ToString();
        }

        public string Rename(string sourceName, string targetName)
        {
            var sourceAttachment = _container.GetBlockBlobReference(sourceName);
            var targetAttachment = _container.GetBlockBlobReference(targetName);

            var res = targetAttachment.StartCopyFromBlob(sourceAttachment);
            sourceAttachment.Delete();

            return targetAttachment.Uri.ToString();
        }

        public void Delete(string name)
        {
            var blob = _container.GetBlockBlobReference(name);
            blob.DeleteIfExists();
        }

        public void Delete(string prefix, TimeSpan age)
        {
            // var attachments = container.ListBlobs(prefix, true).ToList<IListBlobItem>();
            //var attachments = container.ListBlobs(prefix, true);

            var blobs = _container.ListBlobs(prefix, true).OfType<CloudBlockBlob>().Where(b => (DateTime.UtcNow - age > b.Properties.LastModified.Value.DateTime)).ToList();

            foreach (var blob in blobs)
                blob.DeleteIfExists();
        }

    }
}