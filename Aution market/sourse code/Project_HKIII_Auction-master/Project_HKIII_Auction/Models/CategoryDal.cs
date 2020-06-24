using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_HKIII_Auction.Models
{
    public class CategoryDal
    {
        Context context = new Context();
        public List<Category> GetCategories()
        {
            return context.Categories.ToList();
        }
        public bool Create(Category category)
        {
            Category check = context.Categories.SingleOrDefault(e=>e.CId.Equals(category.CId));
            if (check == null)
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            Category check = context.Categories.SingleOrDefault(e=>e.CId == id);
            if (check != null)
            {
                context.Categories.Remove(check);
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