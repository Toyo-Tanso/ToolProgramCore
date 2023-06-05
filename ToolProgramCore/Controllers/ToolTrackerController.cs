using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToolProgramCore.Models;
using static DataLibrary.BusinessLogic.TrackerLogicController;
using static DataLibrary.BusinessLogic.Fields_Change;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;

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

            checkOutTicket.WCdropDownList = getFields_dbl_lst("WC"); 
            checkOutTicket.EmplDropDownList = getFields_dbl_lst("EMP"); //TODO: Empl dropdown

            List<List<string>> unsorted_tools = getFields_dbl_lst("TOOL");

            // NOT Needed until data serialization
            //measure.ToolLocationsList = getFields_dbl_lst("LOCATE"); 

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

            // TODO: ensure WC_to and WC_From does not equal each other

            // TODO change locations DB

            try
            {
                CheckOutHelper(collection);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        // TODO: change description
        // [Helper Function] Takes out values and uses the MeasuerLogic Controller to
        // add tool check in
        private void CheckOutHelper(IFormCollection collection)
        {

            List<List<string>> toolList = getFields_dbl_lst("TOOL");
            List<List<string>> WCList = getFields_dbl_lst("WC");

            string ToolNo = collection["ToolNo"].ToString().ToUpper();
            string Promise_Return_Date = collection["Promise_Return_Date"].ToString();
            string WC_From = collection["WC_From"].ToString().ToUpper();
            string WC_To = collection["WC_To"].ToString().ToUpper();
            string EmpNo = collection["EmpNo"];
            string Date_Removed = DateTime.Now.Date.ToString();



            // User DataLibrary to insert
            saveCheckOut(ToolNo, Promise_Return_Date, WC_From, WC_To, EmpNo, Date_Removed);


            // Update locations data

            // Get WC ID
            // tuple = [*Name*, Description, WCUnder, *ID*]
            // tuple = [  0   ,       1    ,    2   ,  3  ]
            string str_WCID = findInList(WCList, WC_From, 0, 3);
            int WCID = str_WCID == "" ? -1 : int.Parse(str_WCID);


            // Get Tool ID
            // toolRow = [*ID*, *Tool_ID*, Description]
            string str_toolID = findInList(toolList, ToolNo, 1, 0);
            int toolID = str_toolID == "" ? -1 : int.Parse(str_toolID);

            // Get NewWC ID
            // tuple = [*Name*, Description, WCUnder, *ID*]
            // tuple = [  0   ,       1    ,    2   ,  3  ]
            string str_NewWCID = findInList(WCList, WC_To, 0, 3);
            int NewWCID = str_NewWCID == "" ? -1 : int.Parse(str_NewWCID);

            // Catch error that they dont exist
            if (WCID < 0 || toolID < 0 || NewWCID < 0)
            {
                throw new Exception("Could not find ID of WC");
            }

            // Find location, set as inactive
            // TODO: note for return -- delete active location, and re-activate
            setOldLocation(WCID, toolID);

            // Enter in new Location
            setNewLocation(WCID, toolID);


        }

        // Helper function that looks for a searchterm at index searchIndex and returns the result index
        public string findInList(List<List<string>> listToSearch, string searchFor, 
            int searchIndex, int resultIndex)
        {
            foreach(List<string> tuple in listToSearch)
            {
                if (tuple[searchIndex].Trim().Equals(searchFor, StringComparison.OrdinalIgnoreCase))
                {
                    return tuple[resultIndex];
                }
            }
            return "";
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
                
                // TODO: make returned person have a name instead of ID

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
                // Check if tool is checked out
                if (isBorrowed(ToolNo))
                {
                    return Json("This Tool is checked out. Please return it first.");
                }

                else
                {
                    return Json(true);
                }
                
            }

        }

        public IActionResult isValidWC(string WC_To, string WC_From)
        {
           // Ensure WC are not null
            if (WC_To ==  null)
            {
                return Json("Must include where it is going.");
            }
            else if ( WC_From == null)
            {
                return Json("No WC associated with tool.");
            }

            // Ensure WC do not equal each other
            if ( WC_To.Equals(WC_From))
            {
                return Json("Can not move to the same location.");
            }

            Console.WriteLine(WC_To+ "|" + WC_From);
            // Check if WC given is in the DB
            List<List<string>> WC_List = getFields_dbl_lst("WC");

            bool validWC = false;
            foreach (List<string> tuple in WC_List)
            {
                // tuple2 = [*Name*, Description, WCUnder, *ID*]
                if (tuple[0].Trim().Equals(WC_To))
                {
                    validWC = true;
                }
            }

            if (validWC)
            {
                return Json(true);
            }
            else
            {
                return Json("The provided is not in the WC List, please contact QA ENG.");
            }
        }

        // This verifies input in the form (Helper) [called in the model class]
        // Returns error if the employee does not exist in the Database
        // TDOD: remove duplicate
        public IActionResult VerifyEmpNo(string empNo)
        {
            List<List<string>> cur_empl = getFields_dbl_lst("EMP");

            bool EmplExists = false;

            // if it's in the list
            foreach (var row in cur_empl)
            {
                if (row[1].Trim() == empNo)
                {
                    EmplExists = true;
                }
            }

            if (EmplExists)
            {
                return Json(true);
            }
            else
            {
                return Json($"Employee does not exist. Notify QA ENG.");
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
