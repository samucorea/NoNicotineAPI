using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoNicotineAPI.Models
{
    public class LinkRequestStatus
    {
        public int LinkRequestStatusId { get; set; }
        public string Name { get; set; }

        public List<LinkRequest> LinkRequests { get; set; }
    }
}
