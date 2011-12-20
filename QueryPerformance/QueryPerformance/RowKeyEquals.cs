using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Net;

namespace QueryPerformance {
	public class RowKeyEquals : BenchmarkBase {

		protected override IQueryable<Entity> CreateBenchmarkQuery(TableServiceContext context) {
			var r = new Random();
			var value = String.Format("{0:D4}", r.Next(10000));

			return from e in context.CreateQuery<Entity>("Entities")
				   where e.PartitionKey == "0" && e.RowKey == value
				   select e;
		}
	}
}