using Microsoft.EntityFrameworkCore;

namespace Projekt.Data
{
    public class ConnectDbContext : DbContext
    {
        public ConnectDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
