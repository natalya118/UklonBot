using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBot.Models.FacebookModels
{
	public class FacebookSummary
	{
		/// <summary>
		/// Subtotal
		/// </summary>
		public int subtotal { get; set; }
		/// <summary>
		/// Cost of shipping
		/// </summary>
		public int shipping_cost { get; set; }
		/// <summary>
		/// Total tax
		/// </summary>
		public int total_tax { get; set; }
		/// <summary>
		/// Total cost
		/// </summary>
		public int total_cost { get; set; }

	}
}