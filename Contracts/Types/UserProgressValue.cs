using System;

using ServiceStack.DataAnnotations;

namespace Qrunchtime.Domain.Models
{
    public class UserProgressValue
    {
        [PrimaryKey]
        [References(typeof(User))]
        public int UserId { get; set; }

        [PrimaryKey]
        public string ProgressName { get; set; }

        public DateTime Time { get; set; }

        public int Value { get; set; }
               
    }
}
