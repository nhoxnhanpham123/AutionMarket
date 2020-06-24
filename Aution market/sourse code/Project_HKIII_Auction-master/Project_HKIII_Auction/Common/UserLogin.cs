using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_HKIII_Auction.Common
{
    [Serializable]
    public class UserLogin
    {
        public int UId { set; get; }
        public string UName { set; get; }
    }
}