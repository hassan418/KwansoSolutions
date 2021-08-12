using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kwanso.Model.Poco;
using Microsoft.EntityFrameworkCore;

namespace Kwanso.Repository.DataAccess
{
    public class KwansoContext : DbContext
    {
        public KwansoContext(DbContextOptions<KwansoContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Tasks> Tasks { get; set; }

    }
}
