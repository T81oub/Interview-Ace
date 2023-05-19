using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Resume.Models
{
    public class checkNumber
    {
      [Key]
      public int id { get; set; }
        public string OTP { get; set; }

    }
}
