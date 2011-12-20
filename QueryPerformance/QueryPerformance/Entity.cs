using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Common;

namespace QueryPerformance {

	[DataServiceKey("PartitionKey", "RowKey")]
	public class Entity {

		public string PartitionKey {
			get;
			set;
		}

		public string RowKey {
			get;
			set;
		}

		public DateTime Timestamp {
			get;
			set;
		}

		public string Value {
			get;
			set;
		}
	}
}
