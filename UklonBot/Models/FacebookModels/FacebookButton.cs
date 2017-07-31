using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBot.Models.FacebookModels
{
	public class FacebookButton
	{
		/// <summary>
		/// This data will be sent back to your webhook.  1000 character limit.
		/// </summary>
		public string payload { get; set; }
		/// <summary>
		/// Type of button.  Must be web_url
		/// </summary>
		public string type { get; set; }
		/// <summary>
		/// This URL is opened in a mobile browser when the button is tapped
		/// </summary>
		public string url { get; set; }
		/// <summary>
		/// Button title. 20 character limit.
		/// </summary>
		public string title { get; set; }
		/// <summary>
		/// Must be true if using Messenger Extensions.
		/// </summary>
		public bool messenger_extensions { get; set; }
		/// <summary>
		/// Height of the Webview.  Valid Values: compact, tall, full.
		/// </summary>
		public string webview_height_ratio { get; set; }
		/// <summary>
		/// URL to use on clients that don't support Messenger Extensions.  If this is not defined, the url wil be used as the fallback.
		/// </summary>
		public string fallback_url { get; set; }
		/// <summary>
		/// Fields used in the checkout dialog.
		/// </summary>
		public FacebookPaymentSummary payment_summary { get; set; }

	}
}