using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBot.Models.FacebookModels
{
	public class FacebookAdjustment
	{
		/// <summary>
		/// Name of adjustment
		/// </summary>
		public string name { get; set; }
		/// <summary>
		/// Adjustment amount
		/// </summary>
		public int amount { get; set; }
	}
}