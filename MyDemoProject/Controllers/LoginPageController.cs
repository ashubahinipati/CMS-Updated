using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyDemoProject.Models;

namespace MyDemoProject.Controllers
{

    public class LoginPageController : Controller
    {

        #region Objects
        public DemoProjectEntities2 usr = new DemoProjectEntities2();
        tblUser tb1 = new tblUser();

        DemoProjectEntitiescomplaint complaint = new DemoProjectEntitiescomplaint();
        tblComplaintUser tbcomplaint = new tblComplaintUser();
        #endregion

        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tblUser user)
        {
            var userexists = usr.tblUsers.FirstOrDefault(t => t.Username == user.Username && t.Password == user.Password && t.LoginId == user.LoginId);
            if (userexists != null && user.LoginId == 4)
            {
                Session["uname"] = user.Username;
                return RedirectToAction("Loginsuccessfull");
            }
            else if (userexists != null && user.LoginId == 1)
            {
                Session["uname"] = user.Username;
                return RedirectToAction("AdminLoginSuccessfull");
            }
            else if (userexists != null && user.LoginId == 2)
            {
                Session["uname"] = user.Username;
                return RedirectToAction("ProductAdminLoginSuccessfull");
            }
            else if (userexists != null && user.LoginId == 3)
            {
                Session["uname"] = user.Username;
                return RedirectToAction("ProductTeamLoginSuccessfull");
            }
            else
            {

                return RedirectToAction("LoginFailure");
            }
        }
        #endregion

        public ActionResult ForgotPassword(tblUser user)
        {
            return View();

        }

        public ActionResult Loginsuccessfull()
        {
            return View();
        }

        public ActionResult AdminLoginSuccessfull()
        {
            //DemoProjectEntities2 allemployees = new DemoProjectEntities2();
            usr.SaveChanges();
            return View(usr.tblUsers.ToList());
        }
        
        public ActionResult ProductAdminLoginSuccessfull()
        {
            // var user = complaint.tblComplaintUsers.Where(r => r.ComplaintStatus == "Pending" || r.ComplaintStatus == "Resolved");
            var user = complaint.tblComplaintUsers.Where(r => r.ComplaintId == 1 || r.ComplaintId == 3 || r.ComplaintId == 2);
            return View(user);
        }     
       
        public ActionResult ProductTeamLoginSuccessfull()
        {
            // var user = complaint.tblComplaintUsers.Where(r => r.ComplaintId==1 || r.ComplaintId==3 || r.ComplaintId==2);
            var user = complaint.tblComplaintUsers.Where(r => r.ComplaintStatus == "Pending" );
            return View(user);  
        }
       
        #region Edit Users
        [HttpGet]
        public ActionResult EditUsers(int id)
        {
            var edituser = (from e in usr.tblUsers
                            where e.Id == id
                            select e).FirstOrDefault();
            return View(edituser);
        }
        [HttpPost]
        public ActionResult EditUsers(tblUser user)
        {
            var edituser = (from e in usr.tblUsers
                            where e.Id == user.Id
                            select e).FirstOrDefault();
            edituser.Username = user.Username;
            edituser.Firstname = user.Firstname;
            edituser.Lastname = user.Lastname;
            usr.SaveChanges();
            return RedirectToAction("AdminLoginSuccessfull");
        }
        #endregion

        public ActionResult DeleteUser(int id)
        {
            var deleteuser = (from e in usr.tblUsers
                              where e.Id == id
                              select e).FirstOrDefault();
            usr.tblUsers.Remove(deleteuser);
            usr.SaveChanges();

            return RedirectToAction("AdminLoginSuccessfull");
        }
       

        #region Reistering a Complaint
        [HttpGet]
        public ActionResult RegisterComplaint()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterComplaint(tblComplaintUser user)
        {

            var UserExists = complaint.tblComplaintUsers.Where(r => r.Username == user.Username).FirstOrDefault();
            Session["Username"] = tbcomplaint.Username = user.Username;
            Session["Complaint"] = tbcomplaint.Complaint = user.Complaint;
            Session["ComplaintId"] = tbcomplaint.ComplaintId = user.ComplaintId;
            Session["ComplaintStatus"] = "pending";
            tbcomplaint.ComplaintStatus = "pending";
            
            
            complaint.tblComplaintUsers.Add(tbcomplaint);

            complaint.SaveChanges();
            
            return RedirectToAction("ViewComplaint");
        }
        #endregion

        public ActionResult ViewComplaint()
        {
            
            return View();
        }

        #region Change Password User
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(tblUser user)
        {
            //Session["uname"] = user.Username;
            string username=Session["uname"].ToString();
            //var userexists = usr.tblUsers.Where(e => e.Username == user.Username).FirstOrDefault();
            var userexists = usr.tblUsers.Where(e => e.Username == username).FirstOrDefault();
            userexists.Password = user.Password;
            usr.SaveChanges();
            return RedirectToAction("LoginSuccessfull");
        }
        #endregion

        #region Inbox
        public ActionResult Inbox()
        {
            string username = Session["uname"].ToString();
            var userexists = complaint.tblComplaintUsers.Where(e => e.Username == username).FirstOrDefault();
            Session["Id"] = userexists.Id;
            Session["Username"] = userexists.Username;
            Session["Complaint ID"] = userexists.ComplaintId;
            Session["Complaint"] = userexists.Complaint;
            Session["ComplaintStatus"] = userexists.ComplaintStatus;
             
            return View();
        }
        #endregion

        #region Resolve Complaint
        [HttpGet]
        public ActionResult ResolveComplaint(int id)
        {
            var edituser = (from e in complaint.tblComplaintUsers
                            where e.Id == id 
                            select e).FirstOrDefault();
            return View(edituser);
           
        }
        [HttpPost]
  public ActionResult ResolveComplaint(tblComplaintUser user)
        {
            var edituser = (from e in complaint.tblComplaintUsers
                            where e.Id == user.Id
                            select e).FirstOrDefault();
            user.ComplaintStatus = "Resolved";
            edituser.ComplaintStatus = user.ComplaintStatus;
            complaint.SaveChanges();
            return RedirectToAction("ProductTeamLoginSuccessfull");
            //var resolve = complaint.tblComplaintUsers.Where(r => r.Username==user.Username).FirstOrDefault();
            //if (resolve.ComplaintStatus == "pending")
            //{
            //    resolve.ComplaintStatus = "Resolved";
            //}
            //else
            //{
            //    resolve.ComplaintStatus = "pending";
            //}
            //complaint.SaveChanges();
            //return View();
        }
        #endregion

        #region Change Passwords for Admin, Product Admin, Product Team
        [HttpGet]
        public ActionResult ChangePasswordAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePasswordAdmin(tblUser user)
        {
            //Session["uname"] = user.Username;
            string username = Session["uname"].ToString();
            //var userexists = usr.tblUsers.Where(e => e.Username == user.Username).FirstOrDefault();
            var userexists = usr.tblUsers.Where(e => e.Username == username).FirstOrDefault();
            userexists.Password = user.Password;
            usr.SaveChanges();
            return RedirectToAction("AdminLoginSuccessfull");
        }
        [HttpGet]
        public ActionResult ChangePasswordProductAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePasswordProductAdmin(tblUser user)
        {
            //Session["uname"] = user.Username;
            string username = Session["uname"].ToString();
            //var userexists = usr.tblUsers.Where(e => e.Username == user.Username).FirstOrDefault();
            var userexists = usr.tblUsers.Where(e => e.Username == username).FirstOrDefault();
            userexists.Password = user.Password;
            usr.SaveChanges();
            return RedirectToAction("ProductAdminLoginSuccessfull");
        }
        [HttpGet]
        public ActionResult ChangePasswordProductTeam()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePasswordProductTeam(tblUser user)
        {
            //Session["uname"] = user.Username;
            string username = Session["uname"].ToString();
            //var userexists = usr.tblUsers.Where(e => e.Username == user.Username).FirstOrDefault();
            var userexists = usr.tblUsers.Where(e => e.Username == username).FirstOrDefault();
            userexists.Password = user.Password;
            usr.SaveChanges();
            return RedirectToAction("ProductTeamLoginSuccessfull");
        }
        #endregion

        public ActionResult ContactUs()
        {
            return View();
        }
        public ActionResult LoginFailure()
        {
            return View();
        }
    }
}