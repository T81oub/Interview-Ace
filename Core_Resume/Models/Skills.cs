using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Resume.Models
{
    public class Skills
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int id { get; set; }
        public string Skillname { get; set; }

        public string Username { get; set; }

    }
}
