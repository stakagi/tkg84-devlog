using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryPerformance {
	public class PropertyOr : BenchmarkBase {
		protected override IQueryable<Entity> CreateBenchmarkQuery(Microsoft.WindowsAzure.StorageClient.TableServiceContext context) {
			var r = new Random();
			var value1 = String.Format("{0:D4}", r.Next(10000));
			var value2 = String.Format("{0:D4}", r.Next(10000));
			var value3 = String.Format("{0:D4}", r.Next(10000));

			return from e in context.CreateQuery<Entity>("Entities")
				   where e.PartitionKey == "0" && ( e.Value == value1 || e.Value == value2 || e.Value == value3 )
				   select e;
		}
	}
}
