using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniaspFeedbackAPI.Models
{
    public class Utoken
    {
        [Key]
        public string UtokenId { get; set; }

        [Required]
        public string UserId { get; set; }

        public string IP { get; set; }

        public DateTime UTokenTimeOut { get; set; }

        public DateTime LastInTime { get; set; }

        public User User { get; set; }
    }
}
