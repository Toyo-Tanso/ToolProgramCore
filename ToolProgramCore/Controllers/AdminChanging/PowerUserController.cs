using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            return View(userList);
        }

        // GET: PowerUserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PowerUserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PowerUserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: PowerUserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PowerUserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
