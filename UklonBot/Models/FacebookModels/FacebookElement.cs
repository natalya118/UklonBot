using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBot.Models.FacebookModels
{
	public class FacebookElement
	{
		/// <summary>
		/// Bubble title
		/// </summary>
		public string title { get; set; }
		/// <summary>
		/// URL that is opened when bubble is tapped.
		/// </summary>
		public string item_url { get; set; }
		/// <summary>
		/// Default action to be triggered when user taps on the element
		/// </summary>
		public FacebookButton default_action { get; set; }
		/// <summary>
		/// Bubble image
		/// </summary>
		public string image_url { get; set; }
		/// <summary>
		/// Bubble subtitle
		/// </summary>
		public string subtitle { get; set; }
		/// <summary>
		/// Set of buttons that appear as call-to-actions
		/// </summary>
		public FacebookButton[] buttons { get; set; }
		/// Currency of price
		/// </summary>
		public string currency { get; set; }
	}
}