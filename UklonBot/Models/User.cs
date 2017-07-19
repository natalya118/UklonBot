using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UklonBot.Models
{
    public class User
    {
        [Key]
        [Required]
        public string ViberId { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }

        public Cities City { get; set; }

        public string UklonUserToken { get; set; }

        public DateTime? UklonTokenExpirationDate { get; set; }
    }
}