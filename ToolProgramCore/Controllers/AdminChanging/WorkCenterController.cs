using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ToolProgramCore.Models;
using static DataLibrary.BusinessLogic.Fields_Change;

namespace ToolProgramCore.Controllers.AdminChanging
{
    public class WorkCenterController : Controller
    {
        // TODO: make a toast that notifies success on all pages


        // GET: WorkCenterController
        public ActionResult Index()
        {
            GetWCList();

            return View(WorkCenterList);
        }

        // GET: WorkCenterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WorkCenterController/AddWC
        public ActionResult AddWC()
        {
            WorkCenter emptyWorkCenter = new WorkCenter();
            emptyWorkCenter.WCdropDownList = LoadFields_dbl_lst("WC");
            return View(emptyWorkCenter);
        }

        // POST: WorkCenterController/AddWC
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddWC(IFormCollection collection)
        {
            try
            {
                AddWCHelper(collection);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // TODO : Add a toast that said there was an error
                return View();
            }
        }

        // TODO: change description
        // [Helper Function] Takes out values and uses the MeasuerLogic Controller to
        // add tool check in
        // TODO: Implement / change description
        private void AddWCHelper(IFormCollection collection)
        {

            string Name = collection["Name"].ToString().ToUpper();
            string Description = collection["Description"].ToString();
            string WCUnder = collection["WCUnder"];

            // TODO: WCUnder is in correct format?

            // Ensure that there is not a deactivated version of WC
            // (this is done in the model class)



            // enter the new WC with active status
            // call to data library
            AddWCDL(Name, Description, WCUnder);
        }

        // This verifies input in the form (Helper) [called in the model class]
        // Returns error if the employee does not exist in the Database
        // TODO: remove duplicate
        // TODO: update description
        public IActionResult VerifyWC(string Name)
        {
            // Returns ID if it finds it
            int ID = WC_Exists(Name);

            if (ID == -1)
            {
                return Json(true);
            }
            else if(ID == 0)
            {
                return Json($"WC Exists, Please reactivate it in the list page");
            }
            else
            {
                return Json($"WC Exists, No need to add it.");
            }
        }


        // GET: WorkCenterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WorkCenterController/Edit/5
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

        // GET: WorkCenterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WorkCenterController/Delete/5
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
        public List<WorkCenter>? WorkCenterList;

        [AllowAnonymous]
        // TODO: remove nonymous and update description
        // Gets the employee list and stores them in a new list with local Employee class
        // This loads each time the index runs, gets new values every refresh
        // TODO: IMPLEMENT
        public void GetWCList()
        {

            List<List<string>> data = LoadFields_dbl_lst("WC_ALL");
            List<WorkCenter> workCenters = new();

            // row = [Name, Description, WCUnder(wrapped), ID, Active] 
            foreach (List<string> row in data)
            {
                // Make list of WC under based on format
                // WCUnder = #WC1, #WC2

                List<string> ? WCUnderList = new List<string>();

                string WCUnder = row[2];
                if (string.IsNullOrEmpty(WCUnder))
                {
                    WCUnderList = null;
                }
                else
                {
                    string[] tempList = WCUnder.Split(',');

                    foreach (string WC in tempList)
                    {
                        if (!string.IsNullOrEmpty(WC) && WC.Length > 0)
                        {
                            WCUnderList.Add(WC.Trim());
                        }
                    }
                }

                // Enter values into this model: Workcenter. Then add to list
                workCenters.Add(new WorkCenter
                {
                    Name = row[0].Trim(),
                    Description = row[1].Trim(),
                    WCUnder = WCUnderList,
                    Active = row[4].Trim(),
                    ID = row[3].Trim(),

                });
            }
            WorkCenterList = workCenters;
        }

    }
}
