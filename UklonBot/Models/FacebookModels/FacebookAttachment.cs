using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBot.Models.FacebookModels
{
	public class FacebookAttachment
	{
		/// <summary>
		/// Value must be template
		/// </summary>
		public string type { get; set; }
		/// <summary>
		/// Palyoad of list template
		/// </summary>
		public FacebookPayload payload { get; set; }
	}
}
