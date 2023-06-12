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
            return View();
        }

        // POST: WorkCenterController/AddWC
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddWC(IFormCollection collection)
        {
            try
            {
                AddWC(collection);
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
        // TODO: Implement / change description
        private void AddWCHelper(IFormCollection collection)
        {

            //string FirstName = collection["FirstName"].ToString();
            //string LastName = collection["LastName"].ToString();
            //string Clock_Code = collection["Clock_Code"];

            //// Clock_Code will be unique because of model class data annotation
            //// combine first and last name
            //string fullName = LastName.Trim() + ", " + FirstName.Trim();

            //// ensure length is less than 50
            //// shouldn't be activated but just in case
            //if (fullName.Length > 50)
            //{
            //    throw new Exception("Name too long");
            //}

            //// enter the new employee with active status
            //// call to data library
            //AddEmployeeDL(fullName, Clock_Code);
            throw new NotImplementedException();

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
