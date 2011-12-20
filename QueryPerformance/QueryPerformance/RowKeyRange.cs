using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryPerformance {
	public class RowKeyRange : BenchmarkBase {
		protected override IQueryable<Entity> CreateBenchmarkQuery(Microsoft.WindowsAzure.StorageClient.TableServiceContext context) {
			var r = new Random();
			var num = r.Next(9990);

			var begin = String.Format("{0:D4}", num);
			var to = String.Format("{0:D4}", num + 10);

			return from e in context.CreateQuery<Entity>("Entities")
				   where e.PartitionKey == "0" && e.RowKey.CompareTo(begin) > 0 && e.RowKey.CompareTo(to) < 0
				   select e;
		}
	}
}
