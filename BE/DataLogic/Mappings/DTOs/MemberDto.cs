using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Mappings.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }

        public string HeroName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string? Introduction { get; set; }

        public string LastName { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime? LastActive { get; set; }

        public List<PhotoDto> Photos { get; set; }
    }
}
