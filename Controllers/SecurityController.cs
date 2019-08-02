using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ASJ.Services;
using ASJ.ViewModels.Security;
using ASJ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ASJ.Controllers
{
    public class SecurityController : BaseController
    {
        private SignInManager<AppIdentityUser> signInManager;
        private UserManager<AppIdentityUser> userManager;
        private RoleManager<AppIdentityRole> roleManager;
        private ASJDbContext dataContext;
        private IEmailSender emailSender;

        public SecurityController(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager, RoleManager<AppIdentityRole> roleManager, ASJDbContext dbContext, IEmailSender emailSender)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dataContext = dbContext;
            this.emailSender = emailSender;
        }

        [Authorize(Roles = "usermanager")]
        public IActionResult CreateUser()
        {
            CreateUserViewModel model = new CreateUserViewModel();
            model.RoleGroups = dataContext.RoleGroups.ToList();

            return View(model);
        }

        [Authorize(Roles = "usermanager")]
        public IActionResult CreateAllPOCUser()
        {
            CreateAllPOCUserViewModel model = new CreateAllPOCUserViewModel();
            model.RoleGroups = dataContext.RoleGroups.ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
            
                //create user object using the entered information
                //and then create user in the db
                var user = new AppIdentityUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    DisplayName = model.DisplayName
                };
                string userPWD = model.Password;

                IdentityResult chkUser = await userManager.CreateAsync(user, userPWD);

                //Add user to role group and all roles in role group
                if (chkUser.Succeeded)
                {
                    //get newly created user
                    IdentityUser createdUser = await userManager.FindByNameAsync(model.Username);
                    RoleGroup roleGroup = dataContext.RoleGroups.Where(x => x.RoleGroupID == model.SelectedRoleGroupID).FirstOrDefault();
                    UserRoleGroups userGroup = new UserRoleGroups { UserId = createdUser.Id, RoleGroup = roleGroup };
                    dataContext.UserRoleGroups.Add(userGroup);
                    try
                    {
                        dataContext.SaveChanges();
                 
                    }
                    catch (Exception e)
                    {
                  
                    }
                    //get all the roles for this role group and add the role to the user too
                    List<RoleGroupRoles> roleGroupRoles = dataContext.RoleGroupRoles.Where(x => x.RoleGroup.RoleGroupID == roleGroup.RoleGroupID).ToList();
                    foreach(RoleGroupRoles roleitem in roleGroupRoles)
                    {
                        var role = await roleManager.FindByIdAsync(roleitem.RoleId);
                        var result1 = await userManager.AddToRoleAsync(user, role.Name);
                    }
                     return RedirectToAction("CreateUserConfirmation");
                }
                else
                {
                    model.Error = "";
                    foreach (IdentityError err in chkUser.Errors)
                    {
                        model.Error = model.Error + " " + err.Description;
                    }
                    ModelState.AddModelError("Error", model.Error);
                }
            }
            model.RoleGroups = dataContext.RoleGroups.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAllPOCUser(CreateAllPOCUserViewModel model)
        {
            //Create bulk users. 
            //if (ModelState.IsValid)
            {
                var userList = dataContext.Organizations
                    .Distinct()
                    .ToList();

                foreach (Organization o in userList)
                {
                    var user = new AppIdentityUser
                    {
                        UserName = o.UserName,
                        
                        DisplayName = o.Name
                    };

                    string userPWD;

                    if (model.BulkCreate)
                        userPWD = model.Password;

                    else
                    {
                        userPWD = o.PasswordSecure;
                        //unencrypt this
                    }

                    IdentityResult chkUser = await userManager.CreateAsync(user, userPWD);

                    //create user object using the entered information
                    //and then create user in the db


                    //Add user to role group and all roles in role group
                    if (chkUser.Succeeded)
                    {
                        //get newly created user
                        IdentityUser createdUser = await userManager.FindByNameAsync(o.UserName);
                        RoleGroup roleGroup = dataContext.RoleGroups.Where(x => x.RoleGroupID == model.SelectedRoleGroupID).FirstOrDefault();
                        UserRoleGroups userGroup = new UserRoleGroups { UserId = createdUser.Id, RoleGroup = roleGroup };
                        dataContext.UserRoleGroups.Add(userGroup);
                        try
                        {
                            dataContext.SaveChanges();

                        }
                        catch (Exception e)
                        {

                        }
                        //get all the roles for this role group and add the role to the user too
                        List<RoleGroupRoles> roleGroupRoles = dataContext.RoleGroupRoles.Where(x => x.RoleGroup.RoleGroupID == roleGroup.RoleGroupID).ToList();
                        foreach (RoleGroupRoles roleitem in roleGroupRoles)
                        {
                            var role = await roleManager.FindByIdAsync(roleitem.RoleId);
                            var result1 = await userManager.AddToRoleAsync(user, role.Name);
                        }
                        //return RedirectToAction("CreateUserConfirmation");
                    }
                    else
                    {
                        model.Error = "";
                        foreach (IdentityError err in chkUser.Errors)
                        {
                            model.Error = model.Error + " " + err.Description;
                        }
                        ModelState.AddModelError("Error", model.Error);
                    }
                }
                model.RoleGroups = dataContext.RoleGroups.ToList();
                
            }
            return RedirectToAction("CreateUserConfirmation");
            //return View(model);
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "usermanager")]
        public IActionResult CreateUserConfirmation()
        {
            return View();
        }

        //[ValidateAntiForgeryToken]
        public IActionResult CreateRoleOrRoleGroup(CreateRoleOrRoleGroupViewModel model)
        {
            if (model.NewRole == "" && model.NewRoleGroup == "")
            {
                model.NewRoleAdded = false;
                model.NewRoleGroupAdded = false;
            }
            var RoleGroups = dataContext.RoleGroups.ToList();
            ViewData["RoleGroups"] = RoleGroups;
            var AllRoles = roleManager.Roles;
            ViewData["AllRoles"] = AllRoles;

            return View(model);
        }

        [Authorize(Roles = "usermanager")]
        public async Task<IActionResult> ManageRoles(int id)
        {
            int selectedRoleGroupID = id;
             ManageRolesViewModel model = new ManageRolesViewModel();
            model.RoleGroups = dataContext.RoleGroups.ToList();
            model.AllRoles = roleManager.Roles.ToList();
           if(selectedRoleGroupID > 0)
            {
                model.SelectedRoleGroupID = selectedRoleGroupID;
                
                List<RoleGroupRoleViewModel> roleList = new List<RoleGroupRoleViewModel>();
                foreach (RoleGroupRoles role in dataContext.RoleGroupRoles.Where(x => x.RoleGroup.RoleGroupID == selectedRoleGroupID).ToList())
                {
                    var iRole = await roleManager.FindByIdAsync(role.RoleId);
                    RoleGroupRoleViewModel rolevm = new RoleGroupRoleViewModel
                    {
                        RoleGroupId = role.RoleGroup.RoleGroupID,
                        RoleGroupName = role.RoleGroup.RoleGroupName,
                        RoleId = iRole.Id,
                        RoleName = iRole.Name
                    };
                    roleList.Add(rolevm);
                }
                model.RolesForRoleGroup = roleList;
            }
           
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "usermanager")]
        public async Task<IActionResult> CreateRole(CreateRoleOrRoleGroupViewModel model)
        {

            bool x = await roleManager.RoleExistsAsync(model.NewRole.ToLower());
            if (!x)
            {
                //if this role does not exist then add model
                var role = new AppIdentityRole
                {
                    Name = model.NewRole.ToLower()
                };
                await roleManager.CreateAsync(role);
                model.NewRoleAdded = true;
                model.NewRoleError = false;
                model.NewRoleGroupAdded = false;
                model.NewRoleGroupError = false;
            }
            else
            {
                //this role already exists, so show message that says this wasnt added
                model.NewRoleAdded = false;
                model.NewRoleError = true;
                model.NewRoleGroupAdded = false;
                model.NewRoleGroupError = false;
            }
            return RedirectToAction("CreateRoleOrRoleGroup", "Security", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "usermanager")]
        public IActionResult CreateRoleGroup(CreateRoleOrRoleGroupViewModel model)
        {

            RoleGroup rg = dataContext.RoleGroups.SingleOrDefault(p => p.RoleGroupName == model.NewRoleGroup.ToLower());
            if(rg == null)
            {
                //if this role group does not exist then add model
                RoleGroup newGroup = new RoleGroup { RoleGroupName = model.NewRoleGroup };
                dataContext.RoleGroups.Add(newGroup);
                try
                {
                    dataContext.SaveChanges();
                    model.NewRoleAdded = false;
                    model.NewRoleError = false;
                    model.NewRoleGroupAdded = true;
                    model.NewRoleGroupError = false;
                }
                catch(Exception e)
                {
                    model.NewRoleAdded = false;
                    model.NewRoleError = false;
                    model.NewRoleGroupAdded = false;
                    model.NewRoleGroupError = true;
                }
              
            }
            else
            {
                //this role already exists, so show message that says this wasnt added
                model.NewRoleAdded = false;
                model.NewRoleError = false;
                model.NewRoleGroupAdded = false;
                model.NewRoleGroupError = true;
            }
            return RedirectToAction("CreateRoleOrRoleGroup", "Security", model);
        }

        public async Task<IActionResult> DisplayRoleGroupRoles(ManageRolesViewModel model)
        {
            model.NewRoleAdded = false;
            model.NewRoleError = false;
            model.NewRoleGroupAdded = false;
            model.NewRoleGroupError = false;

            if(model.SelectedRoleGroupID != -1 || model.SelectedRoleGroupID != 0)
            {
                
                List<RoleGroupRoleViewModel> roleList = new List<RoleGroupRoleViewModel>();
                foreach (RoleGroupRoles role in dataContext.RoleGroupRoles.Where(x => x.RoleGroup.RoleGroupID == model.SelectedRoleGroupID).ToList())
                {
                    var iRole = await roleManager.FindByIdAsync(role.RoleId);
                    RoleGroupRoleViewModel rolevm = new RoleGroupRoleViewModel
                    {
                        RoleGroupId = role.RoleGroup.RoleGroupID,
                        RoleGroupName = role.RoleGroup.RoleGroupName,
                        RoleId = iRole.Id,
                        RoleName = iRole.Name
                    };
                    roleList.Add(rolevm);
                }
                model.RolesForRoleGroup = roleList;
            }
            return RedirectToAction("ManageRoles", "Security", model);
        }
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "usermanager")]
        public async Task<IActionResult> AddRoleToRoleGroup(ManageRolesViewModel model)
        {
           
            if (model.SelectedRoleForRoleGroup != "" && (model.SelectedRoleGroupID != 0 || model.SelectedRoleGroupID != -1))
            {
                RoleGroupRoles newRGR = new RoleGroupRoles();
                newRGR.RoleGroup = dataContext.RoleGroups.SingleOrDefault(p => p.RoleGroupID == model.SelectedRoleGroupID);

                var role = await roleManager.FindByNameAsync(model.SelectedRoleForRoleGroup);
                IdentityRole iRole = await roleManager.FindByNameAsync(model.SelectedRoleForRoleGroup);
               
                newRGR.RoleId = iRole.Id;
                dataContext.RoleGroupRoles.Add(newRGR);
                try
                {
                    dataContext.SaveChanges();
                
                }
                catch (Exception e)
                {
                   
                }
            }
        
            return RedirectToAction("ManageRoles", "Security", new { id = model.SelectedRoleGroupID }); 
        }

        //[Authorize(Roles = "usermanager")]
        public IActionResult DeleteRoleFromRoleGroup(int id, string role)
        {
            if (role != "" && (id != 0 || id != -1))
            {
                RoleGroup rg = dataContext.RoleGroups.SingleOrDefault(p => p.RoleGroupID == id);
                RoleGroupRoles rgr = dataContext.RoleGroupRoles.SingleOrDefault(p => p.RoleId == role && p.RoleGroup == rg);
                try
                {
                    dataContext.Remove(rgr);
                    dataContext.SaveChanges();
                }
                catch(Exception e)
                {

                }
            }
            return RedirectToAction("ManageRoles", "Security", new { id = id });
        }
        public IActionResult ForgotPassword()
        {
            ForgotPasswordViewModel model = new ForgotPasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return RedirectToAction("ForgotPasswordEmailSent");

                if (!await this.userManager.IsEmailConfirmedAsync(user))
                    return RedirectToAction("ForgotPasswordEmailSent");

                var confrimationCode =
                        await this.userManager.GeneratePasswordResetTokenAsync(user);

                var callbackurl = Url.Action(
                    controller: "Security",
                    action: "ResetPassword",
                    values: new { userId = user.Id, code = confrimationCode },
                    protocol: Request.Scheme);

                await this.emailSender.SendEmailAsync(email: user.Email, subject: "Reset Password", message: callbackurl);

                return RedirectToAction("ForgotPasswordEmailSent");
            }
            return View(model);
        }
        public IActionResult ForgotPasswordEmailSent()
        {
            return View();
        }
        public IActionResult ResetPassword(string userId, string code)
        {
            if (userId == null || code == null)
                throw new ApplicationException("Code must be supplied for password reset.");

            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await this.userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction("ResetPasswordConfirm");

            var result = await this.userManager.ResetPasswordAsync(
                                        user, model.Code, model.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirm");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = "usermanager")]
        public IActionResult CreateUserMultiple()
        {
           return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "usermanager")]
        public async Task<IActionResult> CreateUserMultiple(CreateRoleOrRoleGroupViewModel model)
        {
            //get the usernames and passwords from the MultipleUserCreator table
            string sql = "SELECT UserName,Password FROM dbo.MultipleUserCreator";
            string errorUsers = "There were errors when creating the following user accounts: ";
            var conn = dataContext.Database.GetDbConnection();
            await conn.OpenAsync();
            var command = conn.CreateCommand();
            command.CommandText = sql;
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var username = reader.GetString(0);
                var pwd = reader.GetString(1);

          
                var user = new AppIdentityUser
                {
                    UserName = username,
                    Email = "tvincent@rti.org",
                    DisplayName = username
                };
                string userPWD = pwd;

                IdentityResult chkUser = await userManager.CreateAsync(user, userPWD);

                //Add user to role group and all roles in role group
                if (chkUser.Succeeded)
                {
                    //get newly created user
                    IdentityUser createdUser = await userManager.FindByNameAsync(username);
                    RoleGroup roleGroup = dataContext.RoleGroups.Where(x => x.RoleGroupName == "POC").FirstOrDefault();
                    UserRoleGroups userGroup = new UserRoleGroups { UserId = createdUser.Id, RoleGroup = roleGroup };
                    dataContext.UserRoleGroups.Add(userGroup);
                    try
                    {
                        dataContext.SaveChanges();

                    }
                    catch (Exception e)
                    {
                            errorUsers = errorUsers + username + ", ";
                        }
                    //get all the roles for this role group and add the role to the user too
                    List<RoleGroupRoles> roleGroupRoles = dataContext.RoleGroupRoles.Where(x => x.RoleGroup.RoleGroupID == roleGroup.RoleGroupID).ToList();
                    foreach (RoleGroupRoles roleitem in roleGroupRoles)
                    {
                        var role = await roleManager.FindByIdAsync(roleitem.RoleId);
                        var result1 = await userManager.AddToRoleAsync(user, role.Name);
                    }
                
                }
                else
                {
                        errorUsers = errorUsers + username + ", ";
                }

            }
            return View();
        }
    }
}