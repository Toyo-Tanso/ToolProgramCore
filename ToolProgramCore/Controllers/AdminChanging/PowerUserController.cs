using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToolProgramCore.Models;

using static DataLibrary.BusinessLogic.Fields_Change;

namespace ToolProgramCore.Controllers.AdminChanging
{
    [Authorize(Policy = "MustBeAdmin")]
    public class PowerUserController : Controller
    {
        // GET: PowerUserController
        // home page start up for power users
        public ActionResult Index()
        {
            GetUserList();

            // See if user has access
            string username = base.User.Identities.ElementAt(0).Name ?? "";
            username = username != "" ? username.Split('\\')[1] : "Unknown";
            bool hasAccess = isCurrentUserSuperAdmin(username);

            if (userList == null) {
                // Sets user as not privlidged and loads
                List<PowerUser> emptyList = new List<PowerUser>();
                PowerUser emptyUser = new PowerUser();
                emptyUser.isAddUserAdmin = false;
                emptyUser.isAddUserAdmin = hasAccess;
                emptyList.Add(emptyUser);

                return View(emptyList); 
            }

            // access here allows them to add users; usually reserved for just the IT department
            userList[0].isAddUserAdmin = hasAccess;
            return View(userList);
        }

        // GET: PowerUserController/Details/5
        // Do you need me to explain it?
        // JK it's not being used
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PowerUserController/Create
        // Loads form and sends over current users username
        public ActionResult AddPowerUser()
        {
            PowerUser newUser = new PowerUser();
            string username = base.User.Identities.ElementAt(0).Name ?? "";
            username = username != "" ? username.Split('\\')[1] : "Unknown";
            newUser.UpdatedBy = username;
            newUser.isAddUserAdmin = isCurrentUserSuperAdmin(username);

            return View(newUser);
        }

        //
        public bool isCurrentUserSuperAdmin(string UserName)
        {
            return userSuperAdmin(UserName);

        }

        // POST: PowerUserController/Create
        // Uses helper function
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPowerUser(IFormCollection collection)
        {
            try
            {
                AddPowerUserHelper(collection);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // [Helper Function] Takes out values and uses the Fields_Change Controller to
        // add a new pweruser
        private void AddPowerUserHelper(IFormCollection collection)
        {
            string?  UserName = collection["UserName"];
            string? EditBy = collection["UpdatedBy"];
            string? SuperAdminString = collection["SuperAdmin"];

            // Maybe if the username doesn't exist
            if(UserName is null | EditBy is null | SuperAdminString is null)
            {
                throw new Exception("Missing data recieved");
            }
            else
            {
                // Double question mark is to get rid of the warning
                bool SuperAdmin = bool.Parse(SuperAdminString??"");
                AddNewUser(UserName??"".ToLower(), EditBy??"", SuperAdmin);
            }

        }

        // GET: PowerUserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PowerUserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PowerUserController/RemoveAccess/5
        // removes access from a user. ? oo what happened?
        public ActionResult RemoveAccess(string username)

        {
            //string username = id;
            GetUserList();

            // Edge case, not likely to happen
            if (userList == null || string.IsNullOrEmpty(username)) {
                throw new Exception("No users found");
            }

            // Find out what if user can edit
            string curUser = base.User.Identities.ElementAt(0).Name ?? "";
            curUser = curUser != "" ? curUser.Split('\\')[1] : "Unknown";

            var adminFound = userList.Where(
                user => user is not null & user!.UserName is not null & 
                user.UserName?.Trim() == curUser &
                (user.SuperAdmin ?? false)                                               
                );

            // get the user to be removed
            var list = userList.Where(user => user != null && user.UserName == username);
            PowerUser UserToBeRemoved = list.ElementAt(0);

            // Allow user to go to removeuser page, if current user is an admin
            // Also if the user that is supposed to be removed is not an also admin
            UserToBeRemoved.isAddUserAdmin = adminFound.Count() > 0 && !(UserToBeRemoved.SuperAdmin ?? false);
            
            return View(UserToBeRemoved);
        }

        // POST: PowerUserController/RemoveAccess/5
        // Users helper function
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveAccess(string username, IFormCollection collection)
        {
            try
            {
                RemovePowerUserHelper(collection, username);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw new Exception("Could not remove user");
            }
        }


        // [Helper Function] that will take in username and remove it using fields change controller
        private void RemovePowerUserHelper(IFormCollection collection, string UserName)
        {
            // should be a sufficient ID
            string foundUser = collection["UserName"]!;

            string curUser = base.User.Identities.ElementAt(0).Name ?? "";
            curUser = curUser != "" ? curUser.Split('\\')[1] : "Unknown";


            // Does not truly removes, it just disables
            RemoveUser(UserName, curUser);
        }


        // GET: PowerUserController/Reinstate/5
        // Add the user back by finding the ID
        public ActionResult Reinstate(string username)

        {
            //string username = id;
            GetUserList();
            if (userList == null || string.IsNullOrEmpty(username))
            {
                throw new Exception("No users found");
            }

            // Find out what if user can edit
            string curUser = base.User.Identities.ElementAt(0).Name ?? "";
            curUser = curUser != "" ? curUser.Split('\\')[1] : "Unknown";

            var adminFound = userList.Where(
                user => user is not null & user!.UserName is not null &
                user.UserName?.Trim() == curUser &&
                (user.SuperAdmin ?? false)
            );

            // get the user to be removed
            var list = userList.Where(user => user != null && user.UserName == username);
            PowerUser UserToBeReinstated = list.ElementAt(0);

            // Enter user accessing site permissions into poweruser
            // Also if the user to be removed is not an admin too
            UserToBeReinstated.isAddUserAdmin = adminFound.Count() > 0 && !(UserToBeReinstated.SuperAdmin ?? false);

            return View(UserToBeReinstated);
        }

        // POST: PowerUserController/RemoveAccess/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reinstate(string username, IFormCollection collection)
        {
            try
            {
                ReinstateHelper(collection, username);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw new Exception("Could not reinstate user. Unknown error.");  
            }

        }

        // TODO: change description
        // [Helper Function] Takes out values and uses the MeasuerLogic Controller to
        // add tool check in
        private void ReinstateHelper(IFormCollection collection, string UserName)
        {
            // should be a suficient ID
            string? foundUser = collection["UserName"];

            string curUser = base.User.Identities.ElementAt(0).Name ?? "";
            curUser = curUser != "" ? curUser.Split('\\')[1] : "Unknown";

            if (foundUser is null) {
                throw new Exception("User not found");
            }

            GiveBackAccess(UserName, curUser);
        }

        // ** Data Loaders **

        // holds current list
        public List<PowerUser>? userList;

        [AllowAnonymous]
        // Gets the tool list and stores them in a new list shared in class 
        // This loads each time the index runs, gets new values every refresh
        public void GetUserList()
        {

            List<List<string>> data = LoadFields_dbl_lst("POWER_USERS");
            List<PowerUser> usersTemp = new();




            // structure = ["UserName", "Date_Changed", "Access", "UpdateBy",
            //                  "SuperAdmin");

            foreach (List<string> row in data)
            {


                DateTime ? dateChanged = null;

                if (row[1] != "")
                {
                    dateChanged = DateTime.Parse(row[1]);
                }

                

                // Enter values into this model: Employee. Then add to list
                usersTemp.Add(new PowerUser
                {
                    UserName = row[0],
                    DateChanged = dateChanged,
                    Access = Convert.ToBoolean( int.Parse(row[2])),
                    UpdatedBy = row[3],
                    SuperAdmin = Convert.ToBoolean(int.Parse(row[4])),

                });

            }
            userList = usersTemp;
            
        }



    }
}
