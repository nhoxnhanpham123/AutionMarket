using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_HKIII_Auction.Models
{
    public class NotificationDal
    {
        Context context = new Context();
        public List<Notification> GetNotifications()
        {
            return context.Notifications.ToList();
        } 
    }
}