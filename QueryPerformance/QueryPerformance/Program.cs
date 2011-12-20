using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Net;

namespace QueryPerformance {
	public class Program {

		private const string CONNECTION_STRING = "UseDevelopmentStorage=true";

		
		public static void Main(string[] args) {
			ServicePointManager.UseNagleAlgorithm = false;
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.DefaultConnectionLimit = 100;

			var method = args[0];

			if ( method == "init" ) {
				Init();
			} else if ( method == "benchmark" ) {
				Benchmark(args);
			}
		}

		private static void Benchmark(string[] args) {
			var count = int.Parse(args[1]);

			var assembly = Assembly.GetExecutingAssembly();
			var benchmarks = assembly.GetTypes()
				.Where(t => !t.IsAbstract && !t.IsInterface && t.GetInterface(typeof(IBenchmark).FullName) != null)
				.Select(t => (IBenchmark) assembly.CreateInstance(t.FullName));

			foreach ( var benchmark in benchmarks ) {
				Console.Write("{0}\t", benchmark.GetType().Name);

				var sw = new Stopwatch();
				for ( int i = 0; i < count; i++ ) {
					benchmark.Setup(CONNECTION_STRING);

					sw.Start();
					benchmark.Run();
					sw.Stop();

					benchmark.Dispose();

					Console.Write("{0}\t", sw.ElapsedMilliseconds);

					sw.Reset();
				}

				Console.WriteLine();
			}

			Console.ReadLine();
		}

		private static void Init() {
			var acc = CloudStorageAccount.Parse(CONNECTION_STRING);
			var client = acc.CreateCloudTableClient();
			client.CreateTableIfNotExist("Entities");

			var ctx = client.GetDataServiceContext();

			for ( int i = 0; i < 10000; i++ ) {
				ctx.AddObject("Entities", new Entity() {
					PartitionKey = "0",
					RowKey = String.Format("{0:D4}", i),
					Value = String.Format("{0:D4}", i)
				});

				if ( ctx.Entities.Count % 90 == 0 ) {
					ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
				}
			}

			ctx.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
		}
	}
}