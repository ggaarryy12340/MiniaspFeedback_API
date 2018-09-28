using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiniaspFeedbackAPI.Models
{
    public class Feedback
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string TicketID { get; set; }

        public string Name { get; set; }

        public string Experience { get; set; }

        public string HowToWork { get; set; }

        public string Suggestion { get; set; }

        public string WantToLearn { get; set; }
    }
}
