using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryPerformance {
	public interface IBenchmark {

		/// <summary>
		/// ベンチマークの準備を行う
		/// </summary>
		void Setup(string connectionString);


		/// <summary>
		/// ベンチマークを実施する
		/// </summary>
		void Run();

		/// <summary>
		/// ベンチマークの後片付け
		/// </summary>
		void Dispose();
	}
}
