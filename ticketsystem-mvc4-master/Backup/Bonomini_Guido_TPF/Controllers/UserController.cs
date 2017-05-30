using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bonomini_Guido_TPF.Models;

namespace Bonomini_Guido_TPF.Controllers
{
    public class UserController : Controller
    {
        private TicketSystemEntities db = new TicketSystemEntities();

        //
        // GET: /User/

        public ActionResult Index()
        {
            var user = db.User.Include(u => u.Role);
            return View(user.ToList());
        }

        //
        // GET: /User/Login
        public ActionResult Login() //Log in Operation
        {
            return View();
        }

        //
        // POST: /User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User u) //Log in Operation
        {
            if (ModelState.IsValid)
            {
                using (TicketSystemEntities dc = new TicketSystemEntities())
                {
                    var v = dc.User.Where(a => a.UserName.Equals(u.UserName) && a.Password.Equals(u.Password)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LogedUserID"] = v.UserID.ToString();
                        Session["LogedUserName"] = v.UserName.ToString();
                        Session["LogedUserRoleID"] = v.RoleID.ToString();
                        return RedirectToAction("AfterLogin");
                    }
                }

            }

            return View(u);
        }

        //
        // GET: /User/AfterLogin
        public ActionResult AfterLogin() //AfterLogin Operation
        {
            if (Session["LogedUserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
        }

        //
        // GET: /User/Logout
        public ActionResult Logout() //Log out Operation
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Home", "Home");
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "RoleName");
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}