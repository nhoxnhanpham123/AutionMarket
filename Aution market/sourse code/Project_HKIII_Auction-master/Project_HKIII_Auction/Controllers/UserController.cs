using Newtonsoft.Json;
using Project_HKIII_Auction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;

namespace Project_HKIII_Auction.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        ProductDal PDal = new ProductDal();
        UserDal UDal = new UserDal();
        CategoryDal CDal = new CategoryDal();
        HistoryDal HDal = new HistoryDal();
        NotificationDal NDal = new NotificationDal();
        Context context = new Context();
        [Authorize]
        public ActionResult AboutUs()
        {
            return View();
        }
        [Authorize]
        public ActionResult ContactUs()
        {
            return View();
        }
        [Authorize]
        public ActionResult CreateProduct()
        {
            ViewBag.Start = DateTime.Now;
            ViewBag.Start = String.Format("{0:dd/MM/yyyy}", ViewBag.Start);
            ViewBag.Cate = new SelectList(CDal.GetCategories(), "CId", "CName");
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateProduct(Product product)
        {
            ViewBag.Cate = new SelectList(CDal.GetCategories(), "CId", "CName");
            var username = (Project_HKIII_Auction.Common.UserLogin)Session["userSession"];
            product.UId = username.UId;
            product.Incremenent = 0;
            ViewBag.Start = DateTime.Now;
            ViewBag.Start = String.Format("{0:dd/MM/yyyy}", ViewBag.Start);
            if (ModelState.IsValid)
            {
                if (PDal.Create(product))
                {
                    ModelState.Clear();
                    return RedirectToAction("Home");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        [Authorize]
        public ActionResult Home()
        {

            var U = UDal.GetUsers();
            var P = PDal.GetProducts();
            var C = CDal.GetCategories();
            var list = from u in U join p in P on u.UId equals p.UId join c in C on p.CId equals c.CId select new {u.UName, p.PName, p.Image, p.MinimumPrice, c.CName, p.DateStart, p.DateEnd, p.PId, p.Incremenent, p.Status };
            var convert = (object)JsonConvert.SerializeObject(list);
            return View(convert);
        }
        [Authorize]
        public ActionResult Profiles(int UId)
        {
            User user = UDal.GetUser(UId); 
            return View(user);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Profiles(User user)
        {
          
            if (ModelState.IsValid)
            {
                UDal.Update(user);
                return RedirectToAction("Profiles", user);
            }
            else
            {
                return View();
            }
        }
        [Authorize]
        public ActionResult GetProduct(int PId)
        {
            Product product = PDal.GetProduct(PId);
            return View(product);
        }
        [Authorize]
        public ActionResult HistoryAuction()
        {
            var Pro = PDal.GetProducts();
            var Us = UDal.GetUsers();
            var His = HDal.GetHistoryAuctions();
            var user = (Project_HKIII_Auction.Common.UserLogin)Session["userSession"];

            var list = from u in Us join h in His on u.UId equals h.UId join p in Pro on h.PId equals p.PId select new{u.UName ,p.PName, h.DateBid, h.PriceAuction, h.Status, p.DateEnd, p.PId };
            var lists = list.Where(e => e.UName.Equals(user.UName));
            var HisList = (object)JsonConvert.SerializeObject(lists);

            return View(HisList);
        }
        [Authorize]
        public ActionResult Auction(int PId)
        {
           
            Product product = PDal.GetProduct(PId);
            return View(product);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Auction(int PId, int Price)
        {
            Product product = PDal.GetProduct(PId);
            if (Price > product.MinimumPrice)
            {
                if (Price > product.Incremenent)
                {
                    var user = (Project_HKIII_Auction.Common.UserLogin)Session["userSession"];
                    string sCon = ConfigurationManager.ConnectionStrings["hihi"].ConnectionString;
                    SqlConnection conn = new SqlConnection(sCon);



                    string query = "Update Products Set Incremenent = @Price Where PId = @PId";
                    string queryHis = "Insert into HistoryAuctions Values(@PId, @UId, @Price, @Status, @DateBid)";
                    string queryUpdate = "Update HistoryAuctions Set Status = @status Where PriceAuction < @Price and PId = "+PId;

                    conn.Open();
                    //Update Price Auction
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.Add(new SqlParameter("Price", Price));
                    command.Parameters.Add(new SqlParameter("PId", PId));
                    //Add History Auction
                    SqlCommand command1 = new SqlCommand(queryHis, conn);
                    command1.Parameters.Add(new SqlParameter("PId", PId));
                    command1.Parameters.Add(new SqlParameter("UId", user.UId));
                    command1.Parameters.Add(new SqlParameter("Price", Price));
                    command1.Parameters.Add(new SqlParameter("Status", "Waiting"));
                    command1.Parameters.Add(new SqlParameter("DateBid", DateTime.Now));
                    command1.ExecuteNonQuery();
                    //Update Status History
                    SqlCommand command2 = new SqlCommand(queryUpdate, conn);
                    command2.Parameters.Add(new SqlParameter("status", "There are people who bid more than you"));
                    command2.Parameters.Add(new SqlParameter("Price", Price));
                    command2.ExecuteNonQuery();
                    int i = command.ExecuteNonQuery();
                    conn.Close();
                    if (i > 0)
                    {
                        return RedirectToAction("Home");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    ViewBag.U = "Price must be greater than Current price";
                    return View("Auction", product);
                }
            }
            else
            {
                ViewBag.Y = "Price must be greater than original price";
                return View("Auction", product);
            }
        }
        [Authorize]
        public ActionResult MyProduct(string UName)
        {
            var P = PDal.GetProducts();
            var C = CDal.GetCategories();
            var U = UDal.GetUsers();

            var list = from u in U join p in P on u.UId equals p.UId join c in C on p.CId equals c.CId select new { u.UName, p.PName, p.Image, p.MinimumPrice, c.CName, p.DateStart, p.DateEnd, p.PId, p.Incremenent, p.Status };
            var MyList = list.Where(e => e.UName.Equals(UName));

            var MyProduct = (object)JsonConvert.SerializeObject(MyList);

            return View(MyProduct);
        }
        [Authorize]
        public ActionResult Notification(int UId)
        {
            var U = UDal.GetUsers();
            var P = PDal.GetProducts();
            var N = NDal.GetNotifications();

            var list = from u in U join n in N on u.UId equals n.UId join p in P on n.PId equals p.PId select new {u.UId, p.PName, p.Incremenent, n.Status };
            var lists = list.Where(e=>e.UId.Equals(UId));
            var NList = (object)JsonConvert.SerializeObject(lists);
            return View(NList);
        }
        [Authorize]
        public ActionResult Category(int CId)
        {
            var U = UDal.GetUsers();
            var P = PDal.GetProducts();
            var C = CDal.GetCategories();

            var list = from u in U join p in P on u.UId equals p.UId join c in C on p.CId equals c.CId select new { u.UName, p.PName, p.Image, p.MinimumPrice, c.CName, p.DateStart, p.DateEnd, p.PId, p.Incremenent, p.Status, c.CId };
            var l = list.Where(e => e.CId.Equals(CId));
            var lists = (object)JsonConvert.SerializeObject(l);
            return View(lists);
        }

    }
}