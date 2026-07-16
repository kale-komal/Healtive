using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Healtive.Core.Entities
{
    public class Hospital : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string Logo { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
