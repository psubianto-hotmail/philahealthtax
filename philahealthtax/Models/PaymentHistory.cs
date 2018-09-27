using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace philahealthtax.Models
{
	public class PaymentHistory
	{
		public string Year { get; set; }
		public string Principal { get; set; }
		public string Interest { get; set; }
		public string Penalty { get; set; }
		public string Other { get; set; }
		public string Total { get; set; }
		public string LienNo { get; set; }
		public string Solicitor { get; set; }
		public string Status { get; set; }
	}

}
