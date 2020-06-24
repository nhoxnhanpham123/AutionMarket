using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_HKIII_Auction.Models
{
    public class UserDal
    {
        Context context = new Context();
        public List<User> GetUsers()
        {
            return context.Users.ToList();
        }
        public bool Create(User user)
        {
            User check = context.Users.SingleOrDefault(e=>e.UName == user.UName);
            user.Image = "/Image/image_login signin/images.jpg";
            if (check == null)
            {
                context.Users.Add(user);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public User GetUser(int UId)
        {
            User user = context.Users.SingleOrDefault(e=>e.UId == UId);
            return user;
        }
        public bool Update(User user)
        {
            context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            return context.SaveChanges() > 0;
        }
    }
}