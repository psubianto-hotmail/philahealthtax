using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using philahealthtax.Models;

namespace philahealthtax.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PropertyTaxController : ControllerBase
	{

		[HttpGet("{id}", Name = "GetTaxDetail")]
		public async Task<ActionResult<CustomerDetail>> GetByBrtNo(string id)
		{
			try
			{
				var detail = new CustomerDetail();

				HttpClient client = new HttpClient();
				var response = await client.GetAsync("http://legacy.phila.gov/revenue/realestatetax/?txtBRTNo=" + id);
				var content = await response.Content.ReadAsStringAsync();

				HtmlDocument page = new HtmlDocument();
				page.LoadHtml(content);

				// See if the record is found
				var bottomLeftCol = page.DocumentNode.SelectSingleNode
					(@"//div[@class = 'bottomLeftCol']");

				if (bottomLeftCol == null)
				{
					throw new Exception("Error : no such BRT# exists");
				}

				// Get Customer Information
				String propertyTaxAccountNo = page.DocumentNode.SelectSingleNode
					(@"//span[@id = 'ctl00_BodyContentPlaceHolder_GetTaxInfoControl_frm_lblPropertyTaxAccountNo']").InnerHtml;
				String propertyAddress = page.DocumentNode.SelectSingleNode
					(@"//span[@id = 'ctl00_BodyContentPlaceHolder_GetTaxInfoControl_frm_lblPropertyAddress']").InnerHtml;
				String propertyPostalCode = page.DocumentNode.SelectSingleNode
					(@"//span[@id = 'ctl00_BodyContentPlaceHolder_GetTaxInfoControl_frm_Label1']").InnerHtml;
				String propertyOwnerName = page.DocumentNode.SelectSingleNode
					(@"//span[@id = 'ctl00_BodyContentPlaceHolder_GetTaxInfoControl_frm_lblOwnerName']").InnerHtml;
				String propertyLienSaleAccount = page.DocumentNode.SelectSingleNode
					(@"//span[@id = 'ctl00_BodyContentPlaceHolder_GetTaxInfoControl_frm_lblLienSaleAccount']").InnerHtml;
				String propertyEndPaymentDate = page.DocumentNode.SelectSingleNode
					(@"//span[@id = 'ctl00_BodyContentPlaceHolder_GetTaxInfoControl_frm_lblPaymentPostDate']").InnerHtml;

				detail.BRTNo = propertyTaxAccountNo;
				detail.Address = propertyAddress;
				detail.PostalCode = propertyPostalCode;
				detail.OwnerName = propertyOwnerName;
				detail.LienSaleAccount = propertyLienSaleAccount;
				detail.EndPaymentDate = propertyEndPaymentDate;

				// Get Payment History
				var paymentHistory = page.DocumentNode.SelectSingleNode
					(@"//table[@id = 'ctl00_BodyContentPlaceHolder_GetTaxInfoControl_grdPaymentsHistory']");

				var paymentHistoryRows = paymentHistory.SelectNodes("tr");

				List<PaymentHistory> payments = new List<PaymentHistory>();

				foreach (var p in paymentHistoryRows)
				{
					var paymentHistoryCols = p.SelectNodes("td");
					if (paymentHistoryCols != null)
					{
						payments.Add(new PaymentHistory()
						{
							Year = paymentHistoryCols[0].InnerText.Replace("&nbsp;", ""),
							Principal = paymentHistoryCols[1].InnerText.Replace("&nbsp;", ""),
							Interest = paymentHistoryCols[2].InnerText.Replace("&nbsp;", ""),
							Penalty = paymentHistoryCols[3].InnerText.Replace("&nbsp;", ""),
							Other = paymentHistoryCols[4].InnerText.Replace("&nbsp;", ""),
							Total = paymentHistoryCols[5].InnerText.Replace("&nbsp;", ""),
							LienNo = paymentHistoryCols[6].InnerText.Replace("&nbsp;", ""),
							Solicitor = paymentHistoryCols[7].InnerText.Replace("&nbsp;",""),
							Status = paymentHistoryCols[8].InnerText.Replace("&nbsp;", "")
						});

					}
				}

				detail.Payments = payments;

				return Ok(detail);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}
	}
}