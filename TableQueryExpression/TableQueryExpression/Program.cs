using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Common;
using System.Linq.Expressions;
using System.Net;
using System.IO;

namespace TableQueryExpression {
	class Program {
		static void Main(string[] args) {
			//var client = WazStorageClient<Properties>.Connect("Entities", "UseDevelopmentStorage=true");
			//var q = ( from e in client.CreateQuery()
			//          where e.PartitionKey == "0" && e.RowKey.CompareTo("2000") > 0
			//          select e ).AsDynamicAZQuery();

			//var ctx = client.CreateOperationContext();

			//foreach ( var e in q.Entries ) {
			//    var name = e.Name;
			//    var age = e.Age;

			//    ctx.Update(e);
			//}

			//ctx.Insert();
			//ctx.InsertOrUpdate();
			//ctx.Update();

			var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
			//var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://ipv4.fiddler");

			var client = acc.CreateCloudTableClient();
			client.CreateTableIfNotExist("Entities");

			var ctx = client.GetDataServiceContext();
			var q = from e in ctx.CreateQuery<Properties>("Entities")
					where e.PartitionKey == "0" && e.RowKey.CompareTo("2000") > 0
					select e;
			
			var req = (HttpWebRequest) HttpWebRequest.Create(q.ToString());
			acc.Credentials.SignRequestLite(req);

			var response = (HttpWebResponse) req.GetResponse();

			using ( var r = new StreamReader(response.GetResponseStream()) ) {
				Console.WriteLine(r.ReadToEnd());
			}

			Console.ReadLine();
		}
	}

	public class Properties : TableServiceEntity {

		string PartitionKey {
			get;
			set;
		}

		string RowKey {
			get;
			set;
		}

		DateTime Timestamp {
			get;
			set;
		}

		string Name {
			get;
			set;
		}
	}
}
