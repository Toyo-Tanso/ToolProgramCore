using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToolProgramCore.Models;
using static DataLibrary.BusinessLogic.TrackerLogicController;
using static DataLibrary.BusinessLogic.Fields_Change;


namespace ToolProgramCore.Controllers
{
    // structure of class :
    // 1. ** Page Loaders **
    // 2. ** Data Loaders **
    // 4. ** Data Verify **
    // 3. ** HTML Helpers **
    public class ToolTrackerController : Controller
    {
        // ** Page Loaders **

        // GET: ToolTracker
        // Gets all the measure that have been made and displays only 10 on each page
        // defaults to page 1
        [AllowAnonymous]
        public ActionResult Index(string page = "1", bool viewAll = false)
        {
            // TODO: make run time better (make it so query for count) then make query for only the top 10 and so on

            // Get the correct list
            GetCheckedInList(viewAll);

            // intialize if null
            CheckedInList ??= new List<ToolTracker> { };

            // get the page index and size from query string or default values
            string SetPageSize = "10";
            int pageIndex = int.Parse(page ?? "1");
            int pageSize = int.Parse(SetPageSize ?? "10");

            // get the total number of records
            int totalCount =  CheckedInList.Count();

            // calculate the total number of pages needed
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // get the current page of records
            var shortMeasureList = CheckedInList
                .Skip((pageIndex - 1) * pageSize) // skip the previous pages
                .Take(pageSize) // take only the current page
                .ToList();

            if (shortMeasureList.Count > 0)
            {
                shortMeasureList[0].TotalPages = totalPages;
            }
            

            
            return View(shortMeasureList);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangeIndexView(string viewAll )
        {

            Console.WriteLine(viewAll);

            bool viewAllBool = bool.Parse(viewAll);

            if (viewAllBool)
            {
                return RedirectToAction(nameof(Index), new { viewAll = true });
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

        // GET: ToolTracker/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ToolTracker/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToolTracker/Create
        [AllowAnonymous]
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


        // GET: ToolTracker/Return/5
        public ActionResult Return(int id)
        {
            return View();
        }

        // POST: ToolTracker/Return/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return(int id, IFormCollection collection)
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

        // The following pages not in use:
        // TODO: allow admins to edit. Or delete
        //// GET: ToolTracker/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ToolTracker/Edit/5
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

        //// GET: ToolTracker/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ToolTracker/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
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


        // ** Data Loaders **

        // holds current list
        public List<ToolTracker>? CheckedInList;

        public void GetCheckedInList(bool includeAll = false)
        {

            // Get list of ToolMeasureModel from data libary
            
            var data = includeAll ? LoadAllBorrowedTools() : LoadBorrowedTools();
            List<ToolTracker> toolTrackers = new();

            // Get Employee Names once, to get name
            List<List<string>> EmplDropDownList = getFields_dbl_lst("EMP");

            foreach (var row in data)
            {
                // Check if they are null
                if (row.Date_Removed == null ||
                    row.Promise_Return_Date == null)
                {
                    throw new Exception("Error with Data retrieval");
                }

                // Convert string to required data type
                DateTime cvt_Removed  = DateTime.Parse(row.Date_Removed);
                DateTime cvt_Promise  = DateTime.Parse(row.Promise_Return_Date);
                

                // Enter values into this model: ToolMeasure. Then add to list
                toolTrackers.Add(new ToolTracker
                {
                    ID = row.ID,
                    ToolNo = row.ToolNo,
                    WC_From = row.WC_From,
                    WC_To = row.WC_To,
                    Date_Removed = cvt_Removed,
                    Promise_Return_Date = cvt_Promise,
                    Returned_Date = includeAll ? DateTime.Parse(row.Returned_Date ?? "") : null,
                    Return_EmpNo = includeAll ? row.Return_EmpNo : null,
                    EmpName = getEmployeeName(row.EmpNo, EmplDropDownList),
                });
            }
            CheckedInList = toolTrackers;
        }

        // Helper function: give an empNo and empList, if finds the corresponding name
        private string getEmployeeName(string? empNo, List<List<string>> emplDropDownList)
        {
            // tuple = [*Name*, *Clock_Code*]
            foreach (List<string> tuple in emplDropDownList)
            {
                if (tuple[1].Equals(empNo))
                {
                    return tuple[0];
                }
            }
            return "*Unknown*";
        }

        // Is used to populate Dropdown lists
        // Uses MeasureLogicController
        public List<List<string>> getFields_dbl_lst(string type)
        {
            // TODO add to List
            List<List<string>> fieldList = new List<List<string>>();

            fieldList = LoadFields_dbl_lst(type);

            return fieldList;
        }

    }
}
