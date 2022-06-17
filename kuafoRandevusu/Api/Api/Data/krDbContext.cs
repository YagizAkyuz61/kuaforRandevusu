using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class krDbContext : DbContext
    {
        public krDbContext(DbContextOptions<krDbContext> options) : base(options)
        {

        }
        public DbSet<UserClass> UserTable { get; set; }
        public DbSet<CustomerClass> CustomerTable { get; set; }
        public DbSet<ReservationClass> ReservationTable { get; set; }
        public DbSet<HOperationClass> HOperationTable { get; set; }
        public DbSet<OperationClass> OperationTable { get; set; }
        public DbSet<IlceClass> IlceTable { get; set; }
        public DbSet<CommentClass> CommentsTable { get; set; }
        public DbSet<TimesClass> TimesTable { get; set; }
    }
}
