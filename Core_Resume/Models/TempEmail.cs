using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Resume.Models
{
    public class TempEmail
    {

        public string UserEmail { get; set; }
        public string UserName { get; set; }
        [Key]
        public string Token{ get; set; }

        public DateTime Created { get; set; }
    }
}

