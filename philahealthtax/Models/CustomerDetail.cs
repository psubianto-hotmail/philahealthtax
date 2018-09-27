using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace philahealthtax.Models
{
	public class CustomerDetail
	{
		public string BRTNo { get; set; }
		public string Address { get; set; }
		public string PostalCode { get; set; }
		public string OwnerName { get; set; }
		public string LienSaleAccount { get; set; }
		public string EndPaymentDate { get; set; }
		public List<PaymentHistory> Payments { get; set; }
	}
}
