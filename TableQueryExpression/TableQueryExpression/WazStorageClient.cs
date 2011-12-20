using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace TableQueryExpression {
	public class WazStorageClient<T> where T : TableServiceEntity {

		public CloudStorageAccount StorageAccount {
			get;
			private set;
		}

		private CloudTableClient tableClient;
		private string tableName;

		public IQueryable<T> CreateQuery() {
			var ctx = tableClient.GetDataServiceContext();
			var q = from e in ctx.CreateQuery<T>(tableName)
					select e;

			return q;
		}

		public static WazStorageClient<T> Connect(string table, string connectionString) {
			var account = CloudStorageAccount.Parse(connectionString);
			var client = account.CreateCloudTableClient();

			client.CreateTableIfNotExist(table);

			return new WazStorageClient<T>() {
				StorageAccount = account,
				tableClient = client,
				tableName = table
			};
		}
	}
}
