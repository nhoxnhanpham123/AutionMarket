using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project_HKIII_Auction.Models
{
    public class Context : DbContext
    {
        public Context() : base("hihi") { }
        public DbSet<Admin> Admins { set; get; }
        public DbSet<User> Users { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<Category> Categories { set; get; }
        public DbSet<HistoryAuction> HistoryAuctions { set; get; }
        public DbSet<Notification> Notifications { set; get; }
    }
}