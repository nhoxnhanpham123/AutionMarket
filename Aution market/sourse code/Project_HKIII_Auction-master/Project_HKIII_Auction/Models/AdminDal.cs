using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_HKIII_Auction.Models
{
    public class AdminDal
    {
        Context context = new Context();
        public List<Admin> GetAdmins()
        {
            return context.Admins.ToList();
        }
        public bool Create(Admin admin)
        {
            Admin admin1 = context.Admins.SingleOrDefault(e=>e.AId.Equals(admin.AId));
            if (admin1 == null)
            {
                context.Admins.Add(admin);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}