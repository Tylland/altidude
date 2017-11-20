using Altidude.Domain;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Altidude.Infrastructure
{
    public class AzureTableSettingsManager : ISettingsManager
    {
        private const string PartitionKey = "Default";
        private const string SettingsTableName = "Settings";
        private readonly CloudTable _table;
        public class SettingtEntity : TableEntity
        {
            public SettingtEntity(string name, string value)
            {
                PartitionKey = PartitionKey;
                RowKey = name;
                Value = value;
            }

            public SettingtEntity()
            {
            }

            public string Value { get; set; }
        }

        public AzureTableSettingsManager(string connectionString)
        {
            // Retrieve the storage account from the connection string.
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client.
           var tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            _table = tableClient.GetTableReference(SettingsTableName);
        }


        public string GetValue(string name)
        {
            var retrieveOperation = TableOperation.Retrieve<SettingtEntity>(PartitionKey, name);

             var retrievedResult = _table.Execute(retrieveOperation);

            var settings = (SettingtEntity) retrievedResult.Result;

            return settings?.Value;
        }

        public void Save(string name, string value)
        {
            var setting = new SettingtEntity(name, value);
          
            var insertOrReplaceOperation = TableOperation.InsertOrReplace(setting);

            _table.Execute(insertOrReplaceOperation);
        }
    }
}
