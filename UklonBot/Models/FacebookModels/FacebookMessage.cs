using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBot.Models.FacebookModels
{
	public class FacebookMessage
	{
		public string text { get; set; }
		public string notification_type { get; set; }
		public FacebookAttachment attachment { get; set; }
	}
}