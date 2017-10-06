using Models;
using System.Data.Entity;

namespace HitcherAPI.DataAccess
{
    public class HitcherAPIContext : DbContext
    {
        public HitcherAPIContext() : base("name=HitcherAPIContext")
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
