using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Net;

namespace QueryPerformance {
	public abstract class BenchmarkBase : IBenchmark {
		private HttpWebRequest req;
		private WebResponse response;

		public void Setup(string connectionString) {
			var acc = CloudStorageAccount.Parse(connectionString);
			var client = acc.CreateCloudTableClient();

			var ctx = client.GetDataServiceContext();

			var q = CreateBenchmarkQuery(ctx);

			this.req = (HttpWebRequest) HttpWebRequest.Create(q.ToString());
			acc.Credentials.SignRequestLite(req);
		}

		public void Run() {
			this.response = req.GetResponse();
		}

		public void Dispose() {
			response.Close();
		}

		protected abstract IQueryable<Entity> CreateBenchmarkQuery(TableServiceContext context);
	}
}
