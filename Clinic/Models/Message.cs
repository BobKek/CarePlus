using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Max is 256 Characters!")]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Memo { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
    }
}
