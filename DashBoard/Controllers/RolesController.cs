using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DashBoard.Models;

namespace DashBoard.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private DashboardDBEntities dashDB = new DashboardDBEntities();
        ApplicationDbContext context = new ApplicationDbContext();
        //
        // GET: /Roles/
        public ActionResult Index()
        {
            var roles = context.Roles.ToList();
            return View(roles);
        }
        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: /Roles/Edit/5
        public ActionResult Edit(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            return View(thisRole);
        }
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: /Roles/Delete/5
        public ActionResult Delete(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult ManageUserRoles()
        {
            // prepopulat user for the view dropdown 
            var userlist = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            var account = new AccountController();
            ApplicationUser user;

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(RoleName))
            {
                TempData["msg"] = "<script>alert('Enter User and Role Name !');</script>";
            }
            else
            {

                user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (!(string.IsNullOrEmpty(RoleName)))
                {
                    account.UserManager.AddToRole(user.Id, RoleName);

                    //ViewBag.ResultMessage = "Role created successfully !";

                    TempData["msg"] = "<script>alert('User mapped to the chosen role !');</script>";
                }
                else
                {
                    TempData["msg"] = "<script>alert('Kindly chose the role to map !');</script>";
                }
            }
            var userlist = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;
            // prepopulat roles for the view dropdown 
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;


            return View("ManageUserRoles");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var account = new AccountController();

                if (user != null)
                {
                    ViewBag.RolesForThisUser = account.UserManager.GetRoles(user.Id);
                }
                else
                {
                    TempData["msg"] = "<script>alert('User does not exist in db');</script>";
                }



            }
            else
            {
                TempData["msg"] = "<script>alert('Please chose a user');</script>";
            }
            var userlist = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;
            // prepopulat roles for the view dropdown 
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            var account = new AccountController();
            ApplicationUser user;
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(RoleName))
            {
                TempData["msg"] = "<script>alert('Choose User and Role Name !');</script>";
            }
            else
            {
                user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                if (account.UserManager.IsInRole(user.Id, RoleName))
                {
                    account.UserManager.RemoveFromRole(user.Id, RoleName);

                    TempData["msg"] = "<script>alert('Role removed from this user successfully !');</script>";
                }
                else
                {

                    TempData["msg"] = "<script>alert('This user doesnot belong to selected role');</script>";
                }
            }

            var userlist = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;
            // prepopulat roles for the view dropdown 
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MapTowersToUsers(string UserName, string RoleName, List<string> Towers)
        {
            var account = new AccountController();
            ApplicationUser user;

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(RoleName))
            {
                TempData["msg"] = "<script>alert('Enter User and Role Name !');</script>";
            }
            else
            {

                user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (!(string.IsNullOrEmpty(RoleName)))
                {
                    account.UserManager.AddToRole(user.Id, RoleName);

                    foreach (string tower in Towers)
                    {
                        dashDB.MapTowerToUser(UserName, tower);
                    }

                    //ViewBag.ResultMessage = "Role created successfully !";

                    TempData["msg"] = "<script>alert('User mapped to the chosen role !');</script>";
                }
                else
                {
                    TempData["msg"] = "<script>alert('Kindly chose the role to map !');</script>";
                }
            }
            var userlist = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userlist;
            // prepopulat roles for the view dropdown 
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;


            return View("ManageUserRoles");
        }

    }
}