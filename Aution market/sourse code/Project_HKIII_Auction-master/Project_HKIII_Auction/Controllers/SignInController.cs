using Project_HKIII_Auction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_HKIII_Auction.Controllers
{
    public class SignInController : Controller
    {
        // GET: SignIn
        UserDal dal = new UserDal();
        AdminDal ADal = new AdminDal();
        public ActionResult Index_Signin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Check_Signin(User user, string passcon)
        {
            if (ModelState.IsValid)
            {
                if (user.UName == null)
                {
                    ViewBag.u = "please enter username";

                    return View("Index_Signin");
                }
                else
                {
                    if (user.Password == null)
                    {
                        ViewBag.P = "Please enter password";
                        return View("Index_Signin");
                    }
                    else
                    {
                        if (!user.Password.Equals(passcon))
                        {
                            ViewBag.C = "Password must be the same";
                            return View("Index_Signin");
                        }
                        else
                        {
                            if (dal.Create(user))
                            {
                                return RedirectToAction("Index_Login", "Login");
                            }
                            else
                            {
                                ViewBag.Msg = "This user name has already existed. Please recreate another user name";
                                return View("SignInWithAdmin");
                            }
                        }
                    }
                }
            }
            else
            {
                return View();
            }
            
        }
        public ActionResult SignInWithAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignInWithAdmin(Admin admin, string passcon)
        {
            if (ModelState.IsValid)
            {
                if (admin.AName == null)
                {
                    ViewBag.u = "Please enter username";

                    return View("SignInWithAdmin");
                }
                else
                {
                    if (admin.Password == null)
                    {
                        ViewBag.P = "Please enter password";
                        return View("SignInWithAdmin");
                    }
                    else
                    {
                        if (!admin.Password.Equals(passcon))
                        {
                            ViewBag.C = "Password must be the same";
                            return View("SignInWithAdmin");
                        }
                        else
                        {
                            if (ADal.Create(admin))
                            {
                                return RedirectToAction("Index_Login", "Login");
                            }
                            else
                            {
                                ViewBag.Msg = "This user name has already existed. Please recreate another user name";
                                return View("SignInWithAdmin");
                            }
                        }
                    }
                }
            }
            else
            {
                return View();
            }
        }
    }
}