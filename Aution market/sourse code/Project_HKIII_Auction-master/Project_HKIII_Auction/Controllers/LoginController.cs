using Project_HKIII_Auction.Common;
using Project_HKIII_Auction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Project_HKIII_Auction.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        UserDal dal = new UserDal();
        AdminDal Adal = new AdminDal();
        public ActionResult Index_Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Check_Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(username))
                {
                    ViewBag.u = "Please enter Username";
                    return View("Index_Login");
                }
                else
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        ViewBag.UserName = username;
                        ViewBag.p = "Please enter Password";
                        return View("Index_Login");
                    }
                    else
                    {
                        User check = dal.GetUsers().SingleOrDefault(e => e.UName == username);
                        Admin admin = Adal.GetAdmins().SingleOrDefault(e => e.AName == username);
                        if (admin != null)
                        {
                            if (admin.Password.Equals(password))
                            {
                                var userSession = new UserLogin();
                                userSession.UId = admin.AId;
                                userSession.UName = admin.AName;
                                Session.Add("adminSeesion", userSession);
                                FormsAuthentication.SetAuthCookie(admin.AName, false);
                                return RedirectToAction("Index", "Admin");
                            }
                        }
                        else
                        {
                            if (check != null)
                            {
                                if (check.Password.Equals(password))
                                {
                                    var userSession = new UserLogin();
                                    userSession.UId = check.UId;
                                    userSession.UName = check.UName;
                                    Session.Add("userSession", userSession);
                                    FormsAuthentication.SetAuthCookie(check.UName, false);
                                    return RedirectToAction("Home", "User");
                                }
                                else
                                {
                                    ViewBag.MsgP = "Incorrect Password";
                                    ViewBag.UserName = username;
                                    return View("Index_Login");
                                }
                            }
                            else
                            {
                                ViewBag.MsgU = "Incorrect User Name";

                                return View("Index_Login");
                            }
                        }
                    }
                }
            }
            return View();
        }
        public ActionResult CheckLogout()
        {
            FormsAuthentication.SignOut();
            return View("Index_Login");
        }
    }
}