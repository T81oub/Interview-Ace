using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Resume.Models
{
    public class photo
    {
        [Key]
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Username { get; set; }
    }
}
