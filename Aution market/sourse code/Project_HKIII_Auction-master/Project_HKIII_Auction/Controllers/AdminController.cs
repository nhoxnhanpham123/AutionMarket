using Newtonsoft.Json;
using Project_HKIII_Auction.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;

namespace Project_HKIII_Auction.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        AdminDal Adal = new AdminDal();
        ProductDal PDal = new ProductDal();
        UserDal UDal = new UserDal();
        CategoryDal CDal = new CategoryDal();
        HistoryDal HDal = new HistoryDal();
        Context context = new Context();
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.CCount = CDal.GetCategories().Count();
            ViewBag.PCount = PDal.GetProducts().Count();
            ViewBag.UCount = UDal.GetUsers().Count();

            var Cates = CDal.GetCategories();
            var Pros = PDal.GetProducts();
            var Users = UDal.GetUsers();

            var List = from C in Cates join P in Pros on C.CId equals P.CId join U in Users on P.UId equals U.UId select new {U.UName, P.PName, C.CName, P.MinimumPrice, P.DateStart, P.DateEnd, P.PId };
            var PList = (object)JsonConvert.SerializeObject(List);
            return View(PList);
        }
        [Authorize]
        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                if (CDal.Create(category))
                {
                    return RedirectToAction("Categories");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View(category);
            }
        }
        [Authorize]
        public ActionResult Categories()
        {
            return View(CDal.GetCategories());
        }
        [Authorize]
        public ActionResult Users()
        {
            return View(UDal.GetUsers());
        }
        [Authorize]
        public ActionResult Products()
        {
            return View(PDal.GetProducts());
        }
        [HttpPost]
        public ActionResult SreachUs(string txtSearch)
        {
            if (string.IsNullOrEmpty(txtSearch))
            {
                return View("Users", UDal.GetUsers()); ;
            }
            else
            {
                var list = UDal.GetUsers().Where(e => e.UName.ToLower().Contains(txtSearch) || e.UName.ToUpper().Contains(txtSearch));
                return View("Users", list.ToList());
            }
        }
        [HttpPost]
        public ActionResult SreachPro(string txtSearch)
        {
            if (string.IsNullOrEmpty(txtSearch))
            {
                return View("Products", PDal.GetProducts());
            }
            else
            {
                var list = PDal.GetProducts().Where(e=>e.PName.ToLower().Contains(txtSearch) || e.PName.ToUpper().Contains(txtSearch));
                return View("Products", list.ToList());
            }
        }
        [HttpPost]
        public ActionResult SearchCate(string txtSearch)
        {
            if (string.IsNullOrEmpty(txtSearch))
            {
                return View("categories", CDal.GetCategories());
            }
            else
            {
                var list = CDal.GetCategories().Where(e => e.CName.ToUpper().Contains(txtSearch) || e.CName.ToLower().Contains(txtSearch));
                return View("Categories", list.ToList());
            }
        }
        [Authorize]
        public ActionResult HistoryAuction()
        {
            var His = HDal.GetHistoryAuctions();
            var Pros = PDal.GetProducts();
            var Users = UDal.GetUsers();

            var List = from U in Users join H in His on U.UId equals H.UId join P in Pros on H.PId equals P.PId select new {U.UName, P.PName, P.MinimumPrice, H.PriceAuction, H.Status, P.PId, H.DateBid };
            var model = (object)JsonConvert.SerializeObject(List);
            return View(model);
        }
        [HttpPost]
        public ActionResult SearchHist(string txtSearch)
        {
            var His = HDal.GetHistoryAuctions();
            var Pros = PDal.GetProducts();
            var Users = UDal.GetUsers();

            var List = from U in Users join H in His on U.UId equals H.UId join P in Pros on H.PId equals P.PId select new { U.UName, P.PName, P.MinimumPrice, H.PriceAuction, H.Status, P.PId };
            var model = (object)JsonConvert.SerializeObject(List);
            if (string.IsNullOrEmpty(txtSearch))
            {
                return View("HistoryAuction", model);
            }
            else
            {
                var searched = List.Where(e=>e.UName.ToLower().Contains(txtSearch) || e.UName.ToUpper().Contains(txtSearch));
                var model1 = (object)JsonConvert.SerializeObject(searched);
                return View("HistoryAuction", model1);
            }
        }
        [Authorize]
        public ActionResult DetailsProduct(int PId)
        {
            Product product = PDal.GetProduct(PId);
            return View(product);
        }
        [Authorize]
        public ActionResult DeleteProduct(int PId)
        {
            string sCon = ConfigurationManager.ConnectionStrings["hihi"].ConnectionString;
            string query = "Delete From HistoryAuctions Where PId = "+PId;
            SqlConnection conn = new SqlConnection(sCon);
            SqlCommand command = new SqlCommand(query, conn);
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
            if (PDal.Delete(PId))
            {
                return RedirectToAction("Products");
            }
            return View();
        }
        [Authorize]
        public ActionResult DetailsUser(int UId)
        {
            User user =  UDal.GetUser(UId);
            return View(user);
        }
        [Authorize]
        public ActionResult DeleteUser(int UId)
        {
            string sCon = ConfigurationManager.ConnectionStrings["hihi"].ConnectionString;
            SqlConnection conn = new SqlConnection(sCon);
            conn.Open();
            string query = "Select PId from Products Where UId = " + UId;
            SqlCommand command1 = new SqlCommand(query, conn);
            SqlDataReader reader = command1.ExecuteReader();
            while (reader.Read())
            {
                string deHis = "Delete From HistoryAuctions Where PId = " + (int)reader["PId"];
                SqlCommand comm = new SqlCommand(deHis, conn);
                comm.ExecuteNonQuery();
            }
            reader.Close();
            string query1 = "Delete From HistoryAuctions Where UId = " + UId;
            string query2 = "Delete From Products Where UId = "+UId;
            string query3 = "Delete From Users Where UId = " + UId;
            SqlCommand command = new SqlCommand(query1, conn);
            SqlCommand command2 = new SqlCommand(query2, conn);
            SqlCommand command3 = new SqlCommand(query3, conn);
            command.ExecuteNonQuery();
            command2.ExecuteNonQuery();
            command3.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult DeleteCategory(int CId)
        {
            string sCon = ConfigurationManager.ConnectionStrings["hihi"].ConnectionString;
            SqlConnection conn = new SqlConnection(sCon);
            conn.Open();
            string query = "Select PId from Products Where CId = " + CId;
            SqlCommand command1 = new SqlCommand(query, conn);
            SqlDataReader reader = command1.ExecuteReader();
            while (reader.Read())
            {
                string deHis = "Delete From HistoryAuctions Where PId = " + (int)reader["PId"];
                SqlCommand comm = new SqlCommand(deHis, conn);
                comm.ExecuteNonQuery();
            }
            reader.Close();
            string dePro = "Delete From Products Where CId = " + CId;
            string deCate = "Delete From Categories Where CId = "+CId;
            SqlCommand command2 = new SqlCommand(dePro, conn);
            SqlCommand command3 = new SqlCommand(deCate, conn);
            command2.ExecuteNonQuery();
            command3.ExecuteNonQuery();
            reader.Close();
            return RedirectToAction("Categories");
        }
    }
}