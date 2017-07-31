using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBot.Models.FacebookModels
{
	public class FacebookAddress
	{
		/// <summary>
		/// Street address, line 1
		/// </summary>
		public string street_1 { get; set; }
		/// <summary>
		/// Street address, line 2
		/// </summary>
		public string street_2 { get; set; }
		/// <summary>
		/// City
		/// </summary>
		public string city { get; set; }
		/// <summary>
		/// Postal code
		/// </summary>
		public string postal_code { get; set; }
		/// <summary>
		/// State abbreviation or Region/Province (international)
		/// </summary>
		public string state { get; set; }
		/// <summary>
		/// Two-letter country abbreviation
		/// </summary>
		public string country { get; set; }
	}

}