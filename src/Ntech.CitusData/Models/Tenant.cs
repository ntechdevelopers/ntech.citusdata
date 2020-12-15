using System;

namespace Ntech.CitusData.Models
{
    public class Tenant
    {
        public Guid Id { get; set; }

        public string Domain { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
