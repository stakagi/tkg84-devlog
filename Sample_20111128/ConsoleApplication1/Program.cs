using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Client;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Common;

namespace ConsoleApplication1 {
	public class Program {
		public static void Main_(string[] args) {
			var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true");

			var client = acc.CreateCloudTableClient();
			client.CreateTableIfNotExist("Entities");

			var ctx = client.GetDataServiceContext();

			for ( int i = 0; i < 3000; i++ ) {
				ctx.AddObject("Entities", new Person() {
					PartitionKey = "0",
					RowKey = String.Format("{0:D4}", i),
					Name = String.Format("Sample_{0}", i)
				});

				if ( i % 80 == 0 ) {
					ctx.SaveChanges(SaveChangesOptions.Batch);
				}
			}

			ctx.SaveChanges(SaveChangesOptions.Batch);
		}

		public static void Main1(string[] args) {
			//var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
			var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://ipv4.fiddler");

			var client = acc.CreateCloudTableClient();
			client.CreateTableIfNotExist("Entities");

			var ctx = client.GetDataServiceContext();

			var q = from e in ctx.CreateQuery<Person>("Entities")
					where e.PartitionKey == "0" && e.RowKey.CompareTo("2000") > 0
					select e;

			var list = q.Take(3).ToList();

			Console.WriteLine("### Linq to Azure Table");
			foreach ( var e in list ) {
				Console.WriteLine("{0}: Name={1}", e.RowKey, e.Name);
			}
			Console.WriteLine("Count = {0}", list.Count);

			Console.ReadLine();
		}

		public static void Main2(string[] args) {
			//var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
			var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://ipv4.fiddler");

			var client = acc.CreateCloudTableClient();
			client.CreateTableIfNotExist("Entities");

			var ctx = client.GetDataServiceContext();

			var q = from e in ctx.CreateQuery<Person>("Entities").AsEnumerable()
					where e.PartitionKey == "0" && e.RowKey.CompareTo("2000") > 0
					select e;


			var list = q.Take(3).ToList();

			Console.WriteLine("### Linq to Object");
			foreach ( var e in list ) {
				Console.WriteLine("{0}: Name={1}", e.RowKey, e.Name);
			}
			Console.WriteLine("Count = {0}", list.Count);

			Console.ReadLine();
		}

		public static void Main3(string[] args) {
			//var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
			var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://ipv4.fiddler");

			var client = acc.CreateCloudTableClient();
			client.CreateTableIfNotExist("Entities");

			var ctx = client.GetDataServiceContext();

			var q = from e in Get(ctx)
					where e.RowKey.CompareTo("2000") > 0
					select e;

			var list = q.Take(3).ToList();

			Console.WriteLine("### Linq to Object");
			foreach ( var e in list ) {
				Console.WriteLine("{0}: Name={1}", e.RowKey, e.Name);
			}
			Console.WriteLine("Count = {0}", list.Count);

			Console.ReadLine();
		}

		public static void Main(string[] args) {
			//var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
			var acc = CloudStorageAccount.Parse("UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://ipv4.fiddler");

			var client = acc.CreateCloudTableClient();
			client.CreateTableIfNotExist("Entities");

			var ctx = client.GetDataServiceContext();

			var list = ctx.CreateQuery<Person>("Entities").Take(3).ToList();
			var q = from r in ctx.CreateQuery<Friend>("Friends")
					where ( r.OwnerPartitionKey == list[0].PartitionKey && r.OwnerRowKey == list[0].RowKey ) 
					|| ( r.OwnerPartitionKey == list[1].PartitionKey && r.OwnerRowKey == list[1].RowKey )
					|| ( r.OwnerPartitionKey == list[2].PartitionKey && r.OwnerRowKey == list[2].RowKey )
					select r;

			foreach ( var e in list ) {
				Console.WriteLine("{0}: Name={1}", e.Data.RowKey, e.Relations.Count);
			}

			Console.ReadLine();
		}

		public static IEnumerable<Person> Get(TableServiceContext context) {
			return from e in context.CreateQuery<Person>("Entities")
				   where e.PartitionKey == "0"
				   select e;
		}
	}



	[DataServiceKey("PartitionKey", "RowKey")]
	public class Person {

		public string PartitionKey {
			get;
			set;
		}

		public string RowKey {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}
	}



	[DataServiceKey("PartitionKey", "RowKey")]
	public class Friend {

		public string PartitionKey {
			get;
			set;
		}

		public string RowKey {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public string OwnerPartitionKey {
			get;
			set;
		}

		public string OwnerRowKey {
			get;
			set;
		}
	}


}
