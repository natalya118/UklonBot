using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBot.Models.FacebookModels
{
	public class FacebookPaymentSummary
	{
		/// <summary>
		/// Currency for price
		/// </summary>
		public string currency { get; set; }
		/// <summary>
		/// Whether this is a test payment.  Once set to true, the charge will be a dummy charge
		/// </summary>
		public bool is_test_payment { get; set; }
		/// <summary>
		/// Must be FIXED_AMOUNT or FLEXIBLE_AMOUNT
		/// </summary>
		public string payment_type { get; set; }
		/// <summary>
		/// Name of merchant.
		/// </summary>
		public string merchant_name { get; set; }
		/// <summary>
		/// Information requested from person that will render in the dialog. 
		/// Valid values: shipping_address, contact_name, contact_phone, contact_email. You can config these based on your product need.
		/// </summary>
		public string[] requested_user_in_fo { get; set; }
		/// <summary>
		/// List of objects used to calculate total price. Each label is rendered as a line item in the checkout dialog.
		/// </summary>
		public FacebookPriceList[] price_list { get; set; }
	}
}