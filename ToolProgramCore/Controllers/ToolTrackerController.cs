using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToolProgramCore.Models;
using static DataLibrary.BusinessLogic.TrackerLogicController;
using static DataLibrary.BusinessLogic.Fields_Change;
using System.Text.RegularExpressions;

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
        public ActionResult ChangeIndexView(string viewAll)
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

        // TODO : complete details
        // GET: ToolTracker/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ToolTracker/CheckOut
        [AllowAnonymous]
        public ActionResult CheckOut()
        {
            // TODO: Add date borrowed
            // TODO: Add WC borrowed

            ToolTracker checkOutTicket = new ToolTracker();
            // TODO : add getFields in a different class because : repeating code
            // TODO same thing with getWC

            // Get lists

            //measure.WCdropDownList = getFields_dbl_lst("WC"); //TODO: WC dropdown
            //measure.EmplDropDownList = getFields_dbl_lst("EMP"); //TODO: Empl dropdown
            //measure.ToolLocationsList = getFields_dbl_lst("LOCATE"); //TODO: locations dropdown
            List<List<string>> unsorted_tools = getFields_dbl_lst("TOOL");

            // Sort Tool List
            checkOutTicket.ToolNoDropDownList = unsorted_tools.OrderBy(x =>
            {
                // get the first group of digits in the string
                var match = Regex.Match(x[1], @"\d+");
                // if there is a match, parse it as an int
                if (match.Success)
                {
                    return int.Parse(match.Value);
                }
                // otherwise, return a default value
                else
                {
                    return 0;
                }

            }).ToList();

            // Add today's date as default
            checkOutTicket.Date_Removed = DateTime.Now.Date;

            return View(checkOutTicket);
        }

        // TODO : complete checkout post
        // POST: ToolTracker/CheckOut
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(IFormCollection collection)
        {
            // TODO, verify that it is the correct WC
            //      If not make it so it returns an error
            Console.WriteLine(collection.ToList());
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // TODO : complete return
        // GET: ToolTracker/Return/5
        public ActionResult Return(int id)
        {
            // Maybe Use a different model
            return View();
        }

        // POST: ToolTracker/Return/5
        // TODO : return Post
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
                    Returned_Date = includeAll && (row.Returned_Date) != null ? DateTime.Parse(row.Returned_Date) : null,
                    Return_EmpNo = includeAll && (row.Returned_Date) != null ? row.Return_EmpNo : null,
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

        // ** Data Verify **

        public IActionResult VerifyCorrectTool(
                        string ToolNo, List<List<string>> WCdropDownList,
                        List<List<string>> ToolNoDropDownList,
                        List<List<string>> ToolLocationsList)
        {
            // TODO: serialize and deserialize to pass in the lists
            // Remote does not support complex types
            // If no WC From
            List<List<string>> WCList = getFields_dbl_lst("WC");

            string verifiedWC = GetWC(ToolNo, null,
                WCList, null);

            if (verifiedWC.Equals(null) || (verifiedWC).Equals("") )
            {
                return Json("No Work center found for this tool. Please contact QA ENG to move this tool");
            }
            else
            {
                return Json(true);
            }

        }


        // ** HTML Helper **

        public string GetWC(string Tool, List<List<string>> Locations
                        , List<List<string>> WC, List<List<string>> Tools)
        {
            // TODO : pass in the lists to make runtime better (specifically
            //      from form page

            // Get lists, if they are null
            if (Locations == null || Locations.Count == 0) Locations = getFields_dbl_lst("LOCATE");
            if (WC == null || WC.Count == 0) WC = getFields_dbl_lst("WC");
            if (Tools == null || Tools.Count == 0) Tools = getFields_dbl_lst("TOOL");
            string ToolID = "";

            // Find ID of the tool
            // toolRow = [*ID*, *Tool_ID*, Description]
            foreach (List<string> toolRow in Tools)
            {
                if (toolRow[1].Equals(Tool))
                {
                    ToolID = toolRow[0];
                    break;
                }
            }

            if (ToolID.Equals(""))
            {
                return "";
            }

            // Look through locations to see if its in there
            // tuple = [*Tool_ID*, *WC_ID*]
            foreach (List<string> tuple in Locations)
            {
                if (tuple[0].Equals(ToolID))
                {
                    // Look through the WC to find that the name is given the ID
                    // tuple2 = [*Name*, Description, WCUnder, *ID*]
                    foreach (List<string> tuple2 in WC)
                    {
                        if (tuple2[3].Equals(tuple[1]))
                        {
                            return tuple2[0].Trim(); 
                        }
                    }
                    break;
                }
            }
            return "";
        }



    }
}
