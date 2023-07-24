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
    public class PowerUserController : Controller
    {
        // GET: PowerUserController
        public ActionResult Index()
        {
            GetUserList();

            // See if user has access
            string username = base.User.Identities.ElementAt(0).Name ?? "";
            username = username != "" ? username.Split('\\')[1] : "Unknown";
            bool hasAccess = isCurrentUserSuperAdmin(username);

            if (userList == null) {
                // TODO make a helper function
                List<PowerUser> emptyList = new List<PowerUser>();
                PowerUser emptyUser = new PowerUser();
                emptyUser.isAddUserAdmin = false;
                emptyUser.isAddUserAdmin = hasAccess;
                emptyList.Add(emptyUser);


                return View(emptyList); 
            }
            userList[0].isAddUserAdmin = hasAccess;
            return View(userList);
        }

        // GET: PowerUserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PowerUserController/Create
        public ActionResult AddPowerUser()
        {
            PowerUser newUser = new PowerUser();
            string username = base.User.Identities.ElementAt(0).Name ?? "";
            username = username != "" ? username.Split('\\')[1] : "Unknown";
            newUser.UpdatedBy = username;
            newUser.isAddUserAdmin = isCurrentUserSuperAdmin(username);

            return View(newUser);
        }

        public bool isCurrentUserSuperAdmin(string UserName)
        {
            return userSuperAdmin(UserName);

        }

        // POST: PowerUserController/Create
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

        // TODO: change description
        // [Helper Function] Takes out values and uses the MeasuerLogic Controller to
        // add tool check in
        private void AddPowerUserHelper(IFormCollection collection)
        {

            string UserName = collection["UserName"];
            string EditBy = collection["UpdatedBy"];
            string SuperAdminString = collection["SuperAdmin"];
            bool SuperAdmin = bool.Parse(SuperAdminString);


            // TODO: Maybe if the username doesn't exist



            // TODO: Check to see if this turns out correct
            AddNewUser(UserName.ToLower(), EditBy, SuperAdmin);

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
        public ActionResult RemoveAccess(string username)

        {
            //string username = id;
            GetUserList();
            if (userList == null || string.IsNullOrEmpty(username)) {
                throw new Exception("No users found");
            }

            // Find out what if user can edit
            string curUser = base.User.Identities.ElementAt(0).Name ?? "";
            curUser = curUser != "" ? curUser.Split('\\')[1] : "Unknown";

            var adminFound = userList.Where(
                user => user != null && 
                user.UserName.Trim() == curUser &&
                (user.SuperAdmin ?? false)                                               
                );

            // get the user to be removed
            var list = userList.Where(user => user != null && user.UserName == username);
            PowerUser removeUser = list.ElementAt(0);

            // Enter user accessing site permissions into poweruser
            // Also if the user to be removed is not an admin too
            removeUser.isAddUserAdmin = adminFound.Count() > 0 && !(removeUser.SuperAdmin ?? false);
            
            return View(removeUser);
        }

        // POST: PowerUserController/RemoveAccess/5
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
                // TODO Error Catching
                return View();
            }
        }

        // TODO: change description
        // [Helper Function] Takes out values and uses the MeasuerLogic Controller to
        // add tool check in
        private void RemovePowerUserHelper(IFormCollection collection, string UserName)
        {
            // should be a suficient ID
            string foundUser = collection["UserName"];

            string curUser = base.User.Identities.ElementAt(0).Name ?? "";
            curUser = curUser != "" ? curUser.Split('\\')[1] : "Unknown";


            Console.WriteLine(foundUser, UserName);



            // TODO: Check to see if this turns out correct
            RemoveUser(UserName, curUser);

        }


        // GET: PowerUserController/Reinstate/5
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
                user => user != null &&
                user.UserName.Trim() == curUser &&
                (user.SuperAdmin ?? false)
                );

            // get the user to be removed
            var list = userList.Where(user => user != null && user.UserName == username);
            PowerUser removeUser = list.ElementAt(0);

            // Enter user accessing site permissions into poweruser
            // Also if the user to be removed is not an admin too
            removeUser.isAddUserAdmin = adminFound.Count() > 0 && !(removeUser.SuperAdmin ?? false);

            return View(removeUser);
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
                // TODO Error Catching
                return View();
            }
        }

        // TODO: change description
        // [Helper Function] Takes out values and uses the MeasuerLogic Controller to
        // add tool check in
        private void ReinstateHelper(IFormCollection collection, string UserName)
        {
            // should be a suficient ID
            string foundUser = collection["UserName"];

            string curUser = base.User.Identities.ElementAt(0).Name ?? "";
            curUser = curUser != "" ? curUser.Split('\\')[1] : "Unknown";

            Console.WriteLine(foundUser, UserName);



            // TODO: Check to see if this turns out correct
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
