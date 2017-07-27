using System;
using System.ComponentModel.DataAnnotations;

namespace UklonBot.Models
{
    public class ChannelUser
    {
        [Key]
        [Required]
        public string ProviderId { get; set; }

        public string Provider { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }

        public Cities City { get; set; }

        public string UklonUserToken { get; set; }

        public DateTime? UklonTokenExpirationDate { get; set; }
    }
}