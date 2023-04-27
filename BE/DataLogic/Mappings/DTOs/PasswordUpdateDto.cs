using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Mappings.DTOs
{
    public class PasswordUpdateDto
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
