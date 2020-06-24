using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_HKIII_Auction.Models
{
    public class HistoryDal
    {
        Context context = new Context();
        public List<HistoryAuction> GetHistoryAuctions()
        {
            return context.HistoryAuctions.ToList();
        }
        public bool DeleteByP(int PId)
        {
            HistoryAuction history = context.HistoryAuctions.SingleOrDefault(e=>e.PId.Equals(PId));
            if (history!=null)
            {
                context.HistoryAuctions.Remove(history);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeleteByU(int UId)
        {
            HistoryAuction history = context.HistoryAuctions.SingleOrDefault(e => e.PId.Equals(UId));
            if (history != null)
            {
                context.HistoryAuctions.Remove(history);
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