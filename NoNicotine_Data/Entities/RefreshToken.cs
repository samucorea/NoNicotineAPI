using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoNicotine_Data.Entities
{
    public class RefreshToken : BaseEntity
    {
        [ForeignKey("IdentityUser")]

        public string UserId { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
    }
}
