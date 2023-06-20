using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using ToolProgramCore.Models;
using static DataLibrary.BusinessLogic.Fields_Change;

namespace ToolProgramCore.Controllers.AdminChanging
{
    public class ToolEditController : Controller
    {
        // GET: ToolEditController
        // TODO: implement change add search?
        public ActionResult Index()
        {
            GetToolList();

            return View(toolList);
        }

        // GET: ToolEditController/Details/5
        // TODO: implement change desc
        public ActionResult Details(int id)
        {
            var data = getToolDetails(id);

            // get WC details
            List<List<string>> wcData = getToolWCDetails(id);

            string ? OG_WC = null;
            string ? OG_WC_ID = null;
            string? borrowed_WC = null;

            // Has 1 or more WC
            if (wcData.Count > 0)
            {
                // Structure ["ID_T", "ID_W", "WC", "Tool_ID", "Status", "Borrowed"]
                List<string> row = wcData[0];

                OG_WC = row[2];
                OG_WC_ID = row[1];

                // If there are two, the second one is the borrowed WC
                if (wcData.Count == 2 && wcData[1][4] == "True")
                {
                    row = wcData[1];
                    borrowed_WC = row[2];
                }
                else if (wcData.Count > 2)
                {
                    throw new Exception("WC DATA is incorrect");
                }
            }


            // Throw error if there is a discrency witht the WC

            ToolEdit curTool = new ToolEdit
            {
                Tool_ID = data.Tool_ID,
                Description = data.Description,
                ID = id,
                Active = data.Active.ToString(),
                WC = OG_WC,
                WC_ID = OG_WC_ID == null? null : int.Parse(OG_WC_ID),
                BorrowedWC = borrowed_WC,
            }
            ;

            return View(curTool);
        }

        // GET: ToolEditController/AddTool
        // TODO: implement change desc
        public ActionResult AddTool()
        {
            // TODO: add model
            
            return View();
            //WorkCenter emptyWorkCenter = new WorkCenter();
            //emptyWorkCenter.WCdropDownList = LoadFields_dbl_lst("WC");
            //return View(emptyWorkCenter);
            throw new NotImplementedException();

        }


        // POST: ToolEditController/AddTool
        // TODO: implement change desc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTool(IFormCollection collection)
        {
            try
            {
                // Helper Function to insert employee
                AddToolHelper(collection);
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
        private void AddToolHelper(IFormCollection collection)
        {

            string Tool_ID = collection["Tool_ID"].ToString();
            string Description = collection["Description"];

            // ensure length is less than 50
            // shouldn't be activated but just in case
            if (Description.Length > 50)
            {
                throw new Exception("Description too long");
            }

            // enter the new tool with active status
            // call to data library
            AddToolDL(Tool_ID, Description);

        }

        // GET: ToolEditController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ToolEditController/Edit/5
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

        // GET: ToolEditController/RemoveTool/5
        public ActionResult RemoveTool(int id)
        {
            return View();
        }

        // POST: ToolEditController/RemoveTool/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveTool(int id, IFormCollection collection)
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

        [AllowAnonymous]

        public IActionResult EnsureToolValidation(string Tool_ID)
        {

            string result1 = VerifyStartsWithTTU(Tool_ID);
            string result2 = VerifyUniqueID(Tool_ID);

            string trueResult = "true";

            if (result1 == trueResult)
            {
                if (result2 == trueResult)
                {
                    return Json(true);
                }
                else
                {
                    return Json(result2);
                }
            }
            else
            {
                return Json(result1);
            }

            //// Simplified version? I am not sure
            //bool startsTTU = result1 == trueResult; 
            //bool uniqueID = result2 == trueResult;

            //if (startsTTU && uniqueID) {
            //    return Json(true);
            //}
            //else
            //{
            //    return startsTTU ? Json(result2) : Json(result1);
            //}


        }

        // Ensures that every word is capitalized
        // Not 100% necessary but it's nice to have consistency in the database
        // TODO: implement change desc
        public string VerifyStartsWithTTU(string Tool_ID)
        {
            // If it's empty
            if (string.IsNullOrEmpty(Tool_ID) 
                || string.IsNullOrEmpty(Tool_ID.Trim())) {
                return ($"Cannot be empty");
            }

            // check that it is the correct length, and that it has TTU_ at the 0 index
            bool validLength = Tool_ID.Trim().Length > 4;
            int index = Tool_ID.Trim().IndexOf("TTU ");
            bool validStart = validLength ? index==0 : false;

            if (!validStart)
            {
                return ($"Must be in the following format: 'TTU ###'");
            }
            else
            {
                return ("true");
            }
            
        }

        // TODO: implement add desc
        // Kinda repeating code that is found in employee and WC
        // TODO consolidate function
        [AllowAnonymous]
        public string VerifyUniqueID(string Tool_ID)
        {
            List<List<string>> cur_tools = LoadFields_dbl_lst("TOOL_ALL");

            bool toolExists = false;

            // set formating
            Tool_ID = Tool_ID.ToUpper();

            // if it's in the list
            // structure = ["ID, Tool_ID Description, Active"]
            foreach (var row in cur_tools)
            {
                if (row[1].Trim() == Tool_ID)
                {
                    toolExists = true;
                }
            }
            if (toolExists)
            {
                return ($"Tool Number Exists. Please enter another ID.");
            }
            else
            {
                return ("true");
            }

        }



        // ** Data Loaders **

                // holds current list
        public List<ToolEdit>? toolList;

        [AllowAnonymous]
        // Gets the tool list and stores them in a new list shared in class 
        // This loads each time the index runs, gets new values every refresh
        // TODO: implement change desc
        public void GetToolList()
        {

            List<List<string>> data = LoadFields_dbl_lst("TOOL_ALL");
            List<ToolEdit> toolsTemp = new();

            // structure = ["ID, Tool_ID Description, Active"]
            foreach (List<string> row in data)
            {
                // Enter values into this model: Employee. Then add to list
                toolsTemp.Add(new ToolEdit
                {
                    ID = int.Parse(row[0]),
                    Tool_ID = (row[1]),
                    Description = (row[2]),
                    Active = row[3],

                });
                
            }
            toolList = toolsTemp;
            sortList();
        }

        public void sortList()
        {
            if (toolList!=null) { 
                // Sort Tool List
                toolList = toolList.OrderBy(x =>
                {
                    // get the first group of digits in the string
                    var match = Regex.Match(x.Tool_ID??"", @"\d+");
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
            }
            else
            {
                throw new Exception("Called before list was intialized");
            }
        }

    }
}
