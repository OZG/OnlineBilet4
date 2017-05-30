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
    public class MessageController : Controller
    {
        private TicketSystemEntities db = new TicketSystemEntities();

        //
        // GET: /Message/

        public ActionResult Index()
        {
            var message = db.Message.Include(m => m.User);
            return View(message.ToList());
        }

        //
        // GET: /Message/MessagesControlPanel 

        public ActionResult MessagesControlPanel() //Message control panel with Message Operations for User, Employee and Manager roles
        {
            return View();
        }

        //
        // GET: /Message/ViewMessages

        public ActionResult ViewMessages() //View Sent and Recieved Messages for User, Employee and Manager roles
        {
            string userName = Session["LogedUserName"].ToString();
            int userId = int.Parse(Session["LogedUserID"].ToString());
            var message = db.Message.Where(a => a.MessageUserName == userName || a.MessageReceptorID == userId);
            return View(message.ToList());
        }

        //
        // GET: /Message/Details/5

        public ActionResult Details(int id = 0)
        {
            Message message = db.Message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        //
        // GET: /Message/Create

        public ActionResult Create()
        {
            ViewBag.MessageReceptorID = new SelectList(db.User, "UserID", "UserName");
            return View();
        }

        //
        // POST: /Message/Create

        [HttpPost]
        public ActionResult Create(Message message)
        {
            if (ModelState.IsValid)
            {
                db.Message.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MessageReceptorID = new SelectList(db.User, "UserID", "UserName", message.MessageReceptorID);
            return View(message);
        }

        //
        // GET: /Message/CreateMessage

        public ActionResult CreateInternalMessage() //Create new Internal Message restricted for IT Employees and Managers
        {
            ViewBag.MessageReceptorID = new SelectList(db.User.Where(a => a.Area == "IT"), "UserID", "UserName");
            return View();
        }

        //
        // POST: /Message/CreateMessage

        [HttpPost]
        public ActionResult CreateInternalMessage(Message message) //Create new Internal Message restricted for IT Employees and Managers
        {
            message.MessageType = "Internal";
            message.MessageUserName = Session["LogedUserName"].ToString();

            if (ModelState.IsValid)
            {
                db.Message.Add(message);
                db.SaveChanges();
                if (int.Parse(Session["LogedUserRoleID"].ToString()) == 2)
                {
                    return RedirectToAction("EmployeeHome", "Home");
                }
                else if (int.Parse(Session["LogedUserRoleID"].ToString()) == 3)
                {
                    return RedirectToAction("ManagerHome", "Home");
                }
            }

            ViewBag.MessageReceptorID = new SelectList(db.User.Where(a => a.Area == "IT"), "UserID", "UserName", message.MessageReceptorID);
            return View(message);
        }

        //
        // GET: /Message/CreateMessage

        public ActionResult CreateNormalMessage() //Create new Message for User, Employee and Manager roles
        {
            if (int.Parse(Session["LogedUserRoleID"].ToString()) == 1)
            {
                ViewBag.MessageReceptorID = new SelectList(db.User.Where(a => a.RoleID == 2) , "UserID", "UserName");
            }
            ViewBag.MessageReceptorID = new SelectList(db.User, "UserID", "UserName");
            return View();
        }

        //
        // POST: /Message/CreateMessage

        [HttpPost]
        public ActionResult CreateNormalMessage(Message message) //Create new Message for User, Employee and Manager roles
        {
            message.MessageType = "Normal";
            message.MessageUserName = Session["LogedUserName"].ToString();

            if (ModelState.IsValid)
            {
                db.Message.Add(message);
                db.SaveChanges();
                if (int.Parse(Session["LogedUserRoleID"].ToString()) == 2)
                {
                    return RedirectToAction("EmployeeHome", "Home");
                }
                else if (int.Parse(Session["LogedUserRoleID"].ToString()) == 3)
                {
                    return RedirectToAction("ManagerHome", "Home");
                }
                else if (int.Parse(Session["LogedUserRoleID"].ToString()) == 1)
                {
                    return RedirectToAction("UserHome", "Home");
                }
            }

            ViewBag.MessageReceptorID = new SelectList(db.User, "UserID", "UserName", message.MessageReceptorID);
            return View(message);
        }

        //
        // GET: /Message/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Message message = db.Message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.MessageReceptorID = new SelectList(db.User, "UserID", "UserName", message.MessageReceptorID);
            return View(message);
        }

        //
        // POST: /Message/Edit/5

        [HttpPost]
        public ActionResult Edit(Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MessageReceptorID = new SelectList(db.User, "UserID", "UserName", message.MessageReceptorID);
            return View(message);
        }

        //
        // GET: /Message/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Message message = db.Message.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        //
        // POST: /Message/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Message.Find(id);
            db.Message.Remove(message);
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