using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryPerformance {
	public class PropertyEquals : BenchmarkBase {
		protected override IQueryable<Entity> CreateBenchmarkQuery(Microsoft.WindowsAzure.StorageClient.TableServiceContext context) {
			var r = new Random();
			var value = String.Format("{0:D4}", r.Next(10000));

			return from e in context.CreateQuery<Entity>("Entities")
				   where e.PartitionKey == "0" && e.Value == value
				   select e;
		}
	}
}