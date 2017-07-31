using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBot.Models.FacebookModels
{
	public class FacebookPriceList
	{
		/// <summary>
		/// Label for line item.
		/// </summary>
		public string label { get; set; }
		/// <summary>
		/// Amount of line item.
		/// </summary>
		public string amount { get; set; }
	}
}