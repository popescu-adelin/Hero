using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Mappings.DTOs
{
    public class MemberUpdateDto
    {

        public string HeroName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Introduction { get; set; }
    }
}
