using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using TranyrLogistics.Filters;
using TranyrLogistics.Models;
using WebMatrix.WebData;

namespace TranyrLogistics.Controllers
{
    [InitializeSimpleMembership]
    public class SystemUserController : Controller
    {
        TranyrMembershipDb db = new TranyrMembershipDb();

        //
        // GET: /SystemUser/Index

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            return View(db.UserProfiles.ToList());
        }

        //
        // GET: /SystemUser/Create

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SystemUser/Create

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(
                        model.UserName,
                        model.Password,
                        new { FirstName = model.FirstName, LastName = model.LastName, EmailAddress = model.EmailAddress, IsActive = model.IsActive, CreateDate = DateTime.Now }
                    );
                    return RedirectToAction("Index", "SystemUser");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /SystemUser/Edit

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            return View(userProfile);
        }

        //
        // POST: /SystemUser/Edit

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(UserProfile userProfile)
        {
            using (TranyrMembershipDb userDb = new TranyrMembershipDb())
            {
                UserProfile currentUserProfile = userDb.UserProfiles.Find(userProfile.UserId);
                if (currentUserProfile.UserName != userProfile.UserName)
                {
                    RedirectToAction("Error");
                }
                userProfile.CreateDate = currentUserProfile.CreateDate;
            }

            if (ModelState.IsValid)
            {
                db.Entry(userProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userProfile);
        }

        //
        // GET: /SystemUser/Details/

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Details(int id = 0)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
        }

        //
        // GET: /SystemUser/Delete

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }
            return View(userProfile);
        }

        //
        // POST: /SystemUser/Delete/

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);UserProfile systemUser = db.UserProfiles.Find(id);

            // Only delete the SystemUser if the currently logged in user is the owner
            if (userProfile.UserName == User.Identity.Name)
            {
                return RedirectToAction("Error");
            }
            Membership.DeleteUser(userProfile.UserName, true);
            db.UserProfiles.Remove(userProfile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /SystemUser/AssignRole

        [Authorize(Roles = "Administrator")]
        public ActionResult AssignRole(int id)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);

            List<string> roles = new List<string>();
            foreach (string role in Roles.GetAllRoles())
            {
                roles.Add(role);
            }
            string[] usersRoles = Roles.GetRolesForUser(userProfile.UserName);
            string defaultRole = string.Empty;
            if (usersRoles.Count() > 1)
            {
                defaultRole = usersRoles[0];
            }

            ViewBag.RoleName = new SelectList(roles, defaultRole);
            ViewBag.UserRoles = usersRoles;

            UserRole userRole = new UserRole();
            userRole.UserName = userProfile.UserName;

            return View(userRole);
        }

        //
        // GET: /SystemUser/AssignRole

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult AssignRole(UserRole userRole)
        {
            try
            {
                Roles.AddUserToRole(userRole.UserName, userRole.RoleName);
            }
            catch { }

            UserProfile userProfile = db.UserProfiles.FirstOrDefault(x => x.UserName == userRole.UserName);

            return RedirectToAction("AssignRole", new { id = userProfile.UserId });
        }

        //
        // GET: /SystemUser/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /SystemUser/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            UserProfile systemUser = db.UserProfiles.FirstOrDefault(x => x.UserName == model.UserName);

            if (systemUser != null && ModelState.IsValid && systemUser.IsActive && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /SystemUser/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /SystemUser/ChangePassword

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult ChangePassword(int id, ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            
            TranyrMembershipDb usersContext = new TranyrMembershipDb();
            UserProfile systemUser = usersContext.UserProfiles.Find(id);

            LocalPasswordModel localPasswordModel = new LocalPasswordModel();
            localPasswordModel.UserName = systemUser.UserName;

            ViewBag.DisplayName = systemUser.DisplayName;

            return View(localPasswordModel);
        }

        //
        // POST: /SystemUser/ChangePassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {            
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    var userPasswordToken = WebSecurity.GeneratePasswordResetToken(model.UserName);
                    changePasswordSucceeded = WebSecurity.ResetPassword(userPasswordToken, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePassword", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
