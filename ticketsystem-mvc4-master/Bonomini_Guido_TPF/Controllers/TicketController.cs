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
    public class TicketController : Controller
    {
        private TicketSystemEntities db = new TicketSystemEntities();

        //
        // GET: /Ticket/

        public ActionResult Index()
        {
            var ticket = db.Ticket.Include(t => t.Category).Include(t => t.Priority).Include(t => t.State).Include(t => t.User);
            return View(ticket.ToList());
        }

        //
        // GET: /Ticket/

        public ActionResult TicketManager() //TicketManager Page for Manager Role with Ticket Operations
        {
            return View();
        }

        //
        // GET: /Ticket/TicketState

        public ActionResult TicketState() //View State of own Tickets operation for User Role
        {
            string UserName = Session["LogedUserName"].ToString();
            var ticket = db.Ticket.Include(t => t.Category).Include(t => t.State).Include(t => t.Priority).Where(t => t.TicketUserName == UserName);
            return View(ticket.ToList());
        }

        //
        // GET: /Ticket/AssignedTickets

        public ActionResult AssignedTickets() //View Assigned Tickets operation for Employee Role
        {
            int UserName = int.Parse(Session["LogedUserID"].ToString());
            var ticket = db.Ticket.Include(t => t.Category).Include(t => t.State).Include(t => t.Priority).Where(t => t.User.UserID == UserName);
            ticket = ticket.OrderByDescending(t => t.PriorityID == 1);
            return View(ticket.ToList());
        }

        //
        // GET: /Ticket/NewTickets

        public ActionResult NewTickets() //View new tickets operation for Manager Role
        {
            var ticket = db.Ticket.Include(t => t.Category).Where(t => t.State.StateID == 1).Include(t => t.Priority).Include(t => t.User);
            return View(ticket.ToList());
        }

        //
        // GET: /Ticket/ManageTickets

        public ActionResult ManageTickets() //Ticket list with operations for Manager Role
        {
            var ticket = db.Ticket.Include(t => t.Category).Where(t => t.State.StateName == "Assigned" || t.State.StateName == "Waiting for additional details" || t.State.StateName == "In Treatment" || t.State.StateName == "Raised").Include(t => t.Priority).Include(t => t.User);
            return View(ticket.ToList());
        }

        //
        // GET: /Ticket/Report

        public ActionResult Report(string userName) //Ticket Report operation for Manager Role
        {
            var ticket = db.Ticket.Include(t => t.User).OrderBy(a => a.User.UserName).Include(t => t.Category).Where(t => t.StateID == 6 || t.StateID == 7).Include(t => t.Priority);
            return View(ticket.ToList());
        }

        //
        // GET: /Ticket/Details/5

        public ActionResult Details(int id = 0)
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        //
        // GET: /Ticket/Create

        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName");
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType");
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName");
            ViewBag.UserInChargeID = new SelectList(db.User, "UserID", "UserName");
            return View();
        }

        //
        // POST: /Ticket/Create

        [HttpPost]
        public ActionResult Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Ticket.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType", ticket.PriorityID);
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName", ticket.StateID);
            ViewBag.UserInChargeID = new SelectList(db.User, "UserID", "UserName", ticket.UserInChargeID);
            return View(ticket);
        }

        //
        // GET: /Ticket/CreateTicket

        public ActionResult CreateTicket() //Create Ticket operation for User Role
        {
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName");
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType");
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName");
            ViewBag.UserInChargeID = new SelectList(db.User, "UserID", "UserName");
            return View();
        }

        //
        // POST: /Ticket/CreateTicket

        [HttpPost]
        public ActionResult CreateTicket(Ticket ticket) //Create Ticket operation for User Role
        {
            ticket.TicketID = 1;
            ticket.StateID = 1;
            ticket.TicketUserName = Session["LogedUserName"].ToString();

            if (true)
            {
                db.Ticket.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("UserHome", "Home");
            }

            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType", ticket.PriorityID);
            ViewBag.UserInChargeID = new SelectList(db.User, "UserID", "UserName", ticket.UserInChargeID);

            return View(ticket);
        }

        //
        // GET: /Ticket/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType", ticket.PriorityID);
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName", ticket.StateID);
            ViewBag.UserInChargeID = new SelectList(db.User, "UserID", "UserName", ticket.UserInChargeID);
            return View(ticket);
        }

        //
        // POST: /Ticket/Edit/5

        [HttpPost]
        public ActionResult Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType", ticket.PriorityID);
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName", ticket.StateID);
            ViewBag.UserInChargeID = new SelectList(db.User, "UserID", "UserName", ticket.UserInChargeID);
            return View(ticket);
        }

        //
        // GET: Ticket/AssignPriority

        public ActionResult AssignPriority(int id = 0) //Assign Priority operation for Manager Role
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType", ticket.PriorityID);
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName", ticket.StateID);
            ViewBag.UserInChargeID = new SelectList(db.User, "UserID", "UserName", ticket.UserInChargeID);
            return View(ticket);
        }

        //
        // POST: Ticket/AssignPriority

        [HttpPost]
        public ActionResult AssignPriority(Ticket ticket)//Assign Priority operation for Manager Role
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NewTickets", "Ticket");
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType", ticket.PriorityID);
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName", ticket.StateID);
            ViewBag.UserInChargeID = new SelectList(db.User, "UserID", "UserName", ticket.UserInChargeID);
            return View(ticket);
        }

        //
        // GET: /Ticket/AssignEmployee

        public ActionResult AssignEmployee(int id = 0)//Assign Employee to ticket operation for Manager Role
        {
            Ticket ticket = db.Ticket.Find(id);

            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType", ticket.PriorityID);
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName", ticket.StateID);
            ViewBag.UserInChargeID = new SelectList(db.User.Where(a => a.Area == "IT"), "UserID", "UserName", ticket.UserInChargeID);
            return View(ticket);
        }

        //
        // POST: /Ticket/AssignEmployee

        [HttpPost]
        public ActionResult AssignEmployee(Ticket ticket)//Assign Employee to ticket operation for Manager Role
        {
            ticket.StateID = 2;

            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NewTickets", "Ticket");
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType", ticket.PriorityID);
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName", ticket.StateID);
            ViewBag.UserInChargeID = new SelectList(db.User.Where(a => a.Area == "IT"), "UserID", "UserName", ticket.UserInChargeID);
            return View(ticket);
        }

        //
        // GET: Ticket/AddDetails

        public ActionResult AddDetails(int id = 0) //Add details to tickets operation for Manager Role
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType", ticket.PriorityID);
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName", ticket.StateID);
            ViewBag.UserInChargeID = new SelectList(db.User, "UserID", "UserName", ticket.UserInChargeID);
            return View(ticket);
        }

        //
        // POST: Ticket/AddDetails

        [HttpPost]
        public ActionResult AddDetails(Ticket ticket)//Add details to tickets operation for Manager Role
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NewTickets", "Ticket");
            }
            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType", ticket.PriorityID);
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName", ticket.StateID);
            ViewBag.UserInChargeID = new SelectList(db.User, "UserID", "UserName", ticket.UserInChargeID);
            return View(ticket);
        }

        //
        // GET: /Ticket/CloseTicket

        public ActionResult CloseTicket(int id = 0) //Close ticket operation for Manager and Employee Role
        {
            Ticket ticket = db.Ticket.Find(id);

            if (ticket == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType", ticket.PriorityID);
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName", ticket.StateID);
            ViewBag.UserInChargeID = new SelectList(db.User, "UserID", "UserName", ticket.UserInChargeID);

            return View(ticket);
        }

        //
        // POST: /Ticket/CloseTicket

        [HttpPost]
        public ActionResult CloseTicket(Ticket ticket)//Close ticket operation for Manager and Employee Role
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                if (int.Parse(Session["LogedUserRoleID"].ToString()) == 2)
                {
                    return RedirectToAction("EmployeeHome", "Home");
                }
                else if (int.Parse(Session["LogedUserID"].ToString()) == 3)
                {
                    return RedirectToAction("ManagerHome", "Home");
                }
            }

            ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", ticket.CategoryID);
            ViewBag.PriorityID = new SelectList(db.Priority, "PriorityID", "PriorityType", ticket.PriorityID);
            ViewBag.StateID = new SelectList(db.State, "StateID", "StateName", ticket.StateID);
            ViewBag.UserInChargeID = new SelectList(db.User, "UserID", "UserName", ticket.UserInChargeID);

            return View(ticket);
        }
        
        //
        // GET: /Ticket/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        //
        // POST: /Ticket/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Ticket.Find(id);
            db.Ticket.Remove(ticket);
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