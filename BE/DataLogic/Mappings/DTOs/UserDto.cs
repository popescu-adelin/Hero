using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Mappings.DTOs
{
    public class UserDto
    {
        public string? HeroName { get; set; }

        public string? Email { get; set; }

        public DateTime? LastActive { get; set; }

        public string? Token { get; set; }

        public string? PhotoUrl { get; set; }
    }
}
