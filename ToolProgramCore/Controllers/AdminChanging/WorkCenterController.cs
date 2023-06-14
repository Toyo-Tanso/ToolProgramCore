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

            var data = getWCDetails(id);

            List<string> ? WCUnderList = strToArray(data.WCUnder??"");

            WorkCenter curWC = new WorkCenter
                {
                Name = data.Name,
                Description = data.Description,
                WCUnder = WCUnderList,
                ID = id.ToString(),
                Active = data.Active.ToString(),
                }
            ;

            
            return View(curWC);
        }

        public IActionResult ToolPartial(int id, bool removal=false)
        {
            Console.WriteLine(removal);
            WorkCenter toolCarrier = new WorkCenter
            {
                Tools = getToolofWC(id),
                IsRemoval = removal,
            };

            return PartialView("_ToolAtWC", toolCarrier);
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

        // TODO: future project, this requires changing all existing Databases?
        // It's more complex since all DB do not go off of ID
        //public ActionResult Edit(int id)
        //{
        //    var data = getWCDetails(id);

        //    List<string>? WCUnderList = strToArray(data.WCUnder ?? "");

        //    WorkCenter curWC = new WorkCenter
        //    {
        //        Name = data.Name,
        //        Description = data.Description,
        //        WCUnder = WCUnderList,
        //        ID = id.ToString(),
        //        Active = data.Active.ToString(),
        //    };

        //    return RedirectToAction(nameof(Index));

        //    throw new NotImplementedException();
        //}

        //// POST: WorkCenterController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: WorkCenterController/Reactivate/5
        // Reactivate a deactivated WC if there was a mistake
        public ActionResult Reactivate(int id)
        {
            var data = getWCDetails(id);

            List<string>? WCUnderList = strToArray(data.WCUnder ?? "");

            WorkCenter curWC = new WorkCenter
            {
                Name = data.Name,
                Description = data.Description,
                WCUnder = WCUnderList,
                ID = id.ToString(),
                Active = data.Active.ToString(),
               
            };

            return View(curWC);
        }

        // POST: WorkCenterController/Reactivate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reactivate(int id, IFormCollection collection)
        {
            try
            {
                AddReactivateHelper(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Helper FUnction for Reactivate, does the logic
        private void AddReactivateHelper(int id)
        {
            // Update the WorkCenter DB making it active
            ReactivateWC(id);
        }

        // GET: WorkCenterController/Deactivate/5
        // Checks to see if there are any tools that include the WC
        //      that you plan to deactivate
        public ActionResult Deactivate(int id)
        {

            var data = getWCDetails(id);

            List<string>? WCUnderList = strToArray(data.WCUnder ?? "");

            WorkCenter curWC = new WorkCenter
            {
                Name = data.Name,
                Description = data.Description,
                WCUnder = WCUnderList,
                ID = id.ToString(),
                Active = data.Active.ToString(),
                // See if there are checked out tools
                canDelete = !CheckOutToolExists((data.Name??"").Trim()),
            };

            return View(curWC);
        }

        // POST: WorkCenterController/Deactivate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(int id, IFormCollection collection)
        {
            try
            {
                AddDeactivateHelper(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Helper FUnction for deactivate, does the logic
        private void AddDeactivateHelper(int id)
        {
            // Update the WorkCenter DB making it inactive
            DeactivateWC(id);

            // remove all Location Data where WCID = WC
            // Note: does not get here if there are checked out tools
            DeleteWCToolLocation(id);
            
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

                strToArray(row[2]);

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

        public List<string> ? strToArray(string list)
        {
            List<string>? WCUnderList = new List<string>();

            string WCUnder = list;
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

            return WCUnderList;

        }

    }
}
