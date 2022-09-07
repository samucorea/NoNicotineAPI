using Microsoft.EntityFrameworkCore;

namespace NoNicotineAPI
{
    public class NoNicotineContext : DbContext
    {
        public NoNicotineContext(DbContextOptions<NoNicotineContext> options) : base(options)
        {

        }

    }
}
