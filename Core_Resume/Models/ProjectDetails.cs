using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Core_Resume.Models
{
    public class ProjectDetails
    {

        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Projectname { get; set; }
        public string projectLink { get; set; }
    }
}
