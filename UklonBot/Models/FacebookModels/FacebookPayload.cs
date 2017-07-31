using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBot.Models.FacebookModels
{
	public class FacebookPayload
	{
		/// <summary>
		/// The type of template
		/// </summary>
		public string template_type { get; set; }
		/// <summary>
		/// Text that appears in main body
		/// </summary>
		public string text { get; set; }
		/// <summary>
		/// Set of buttons that appear as call-to-actions
		/// </summary>
		public FacebookButton[] buttons { get; set; }
		/// <summary>
		/// Data for each bubble in message
		/// </summary>
		public FacebookElement[] elements { get; set; }
		/// <summary>
		/// Value must be large or compact. Default to large if not specified
		/// </summary>
		public string top_element_style { get; set; }
		/// <summary>
		/// Recipient's name.
		/// </summary>
		public string recipient_name { get; set; }
		/// <summary>
		/// Merchant's name. If present this is shown as logo text.
		/// </summary>
		public string merchant_name { get; set; }
		/// <summary>
		/// Order number.
		/// </summary>
		public string order_number { get; set; }
		/// <summary>
		/// Currency for order.
		/// </summary>
		public string currency { get; set; }
		/// <summary>
		/// Payment method details. This can be a custom string. ex: "Visa 1234".
		/// </summary>
		public string payment_method { get; set; }
		/// <summary>
		/// Timestamp of the order, in seconds.
		/// </summary>
		public string timestamp { get; set; }
		/// <summary>
		/// URL of order.
		/// </summary>
		public string order_url { get; set; }
		/// <summary>
		/// Shipping address.
		/// </summary>
		public FacebookAddress address { get; set; }
		/// <summary>
		/// Payment summary.
		/// </summary>
		public FacebookSummary summary { get; set; }
		/// <summary>
		/// Payment adjustments
		/// </summary>
		public FacebookAdjustment[] adjustments { get; set; }
	}
}