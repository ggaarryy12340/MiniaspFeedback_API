using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiniaspFeedbackAPI.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        public string Tel { get; set; }

        public string Email { get; set; }

        public DateTime AddTime { get; set; }

        public List<Utoken> Utokens { get; set; }

    }
}
