using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using ToolProgramCore.Models;
using static DataLibrary.BusinessLogic.MeasureLogicController;
using static DataLibrary.BusinessLogic.Fields_Change;
using System.Text.RegularExpressions;

namespace ToolProgramCore.Controllers
{
    public class MeasureController : Controller
    {
        // holds current list
        public List<ToolMeasure>? MeasureList;

        // Updates the Measure so that it gets most recent data from DB
        public void GetMeasureList()
        {
            var data = LoadMeasures();
            List<ToolMeasure> toolMeasures = new();

            foreach (var row in data)
            {
                if (row.T_Date == null) {
                    break;
                }
                DateTime ConvertedDate = DateTime.Parse(row.T_Date);
                double S_Size_converted = double.Parse(row.S_Size);
                double Condition_converted = double.Parse(row.Condition);

                // Get Employee Names once
                List<List<string>> EmplDropDownList = getFields_dbl_lst("EMP");

                toolMeasures.Add(new ToolMeasure
                {
                    ID = row.ID,
                    T_Date = ConvertedDate,
                    WC = row.WC,
                    ToolNo = row.ToolNo,
                    S_Size = S_Size_converted,
                    EmpNo = row.EmpNo,
                    Condition = Condition_converted,
                    EmplDropDownList = EmplDropDownList,
                    EmpName = getEmployeeName(row.EmpNo,EmplDropDownList),
                });
            }
            MeasureList = toolMeasures;
        }

        private string getEmployeeName(string? empNo, List<List<string>> emplDropDownList)
        {
            foreach (List<string> tuple in emplDropDownList)
            {
                if (tuple[1].Equals(empNo))
                {
                    return tuple[0];
                }
            }
            return "*Unknown*";
        }

        public string GetWC(string Tool, List<List<string>> Locations
                        , List<List<string>> WC, List<List<string>> Tools)
        {
            // TODO : pass in the lists to make runtime better (specifically
            //      from form page

            if(Locations == null || Locations.Count == 0) Locations = getFields_dbl_lst("LOCATE");
            if (WC == null || WC.Count == 0) WC = getFields_dbl_lst("WC");
            if (Tools == null || Tools.Count == 0) Tools = getFields_dbl_lst("TOOL");
            string ToolID = "";

            // Find ID of the tool
            // toolRow = [*ID*, *Tool_ID*, Description]
            foreach (List<string> toolRow in Tools )
            {
                if ( toolRow[1].Equals(Tool) )
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
                    foreach(List<string> tuple2 in WC)
                    {
                        if (tuple2[3].Equals(tuple[1]))
                        {
                            return tuple2[0].Trim(); // TODO: trim String
                        }
                    }
                    break;
                }
            }


            return "";
        }

        // Is used to populate the ID Dropdown lists in a tool object
        public List<string> getFieldsList(string type)
        {
            var data = LoadFields(type);
            List<string>fieldList = new List<string>();

            foreach (var row in data)
            {
                fieldList.Add(row);
            }
            return fieldList;
        }

        // Is used to populate the ID Dropdown lists in a tool object
        public List<List<string>> getFields_dbl_lst(string type)
        {
            // TODO add to List
            List<List<string>> fieldList = new List<List<string>>();

            fieldList = LoadFields_dbl_lst(type);

            return fieldList; 
        }

        // GET: MeasureController
        // Gets list from the last 7 days and displays them
        public ActionResult Index(string page = "1")
        {
            GetMeasureList();
            string SetPageSize = "10";
            // get the page index and size from query string or default values
            int pageIndex = int.Parse(page ?? "1");
            int pageSize = int.Parse(SetPageSize ?? "10");

            // get the total number of records
            int totalCount = MeasureList.Count();
            // calculate the total number of pages
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            
            // get the current page of records
            var shortMeasureList = MeasureList
                .Skip((pageIndex - 1) * pageSize) // skip the previous pages
                .Take(pageSize) // take only the current page
                .ToList();

            shortMeasureList[0].TotalPages = totalPages;


            return View(shortMeasureList);
        }

        // GET: MeasureController/Details/5
        public ActionResult Details(int id)
        {
            GetMeasureList();
            ToolMeasure ToolID = new ToolMeasure();

            Console.WriteLine(id);
            foreach (ToolMeasure tool in MeasureList)
            {
                if (tool.ID.Equals(id.ToString()))
                {
                    ToolID = tool;
                }
            }
            if (ToolID.ID is null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(ToolID);
        }

        // This verifies input in the form (Helper) [called in the model class]
        // Returns error if the employee does not exist in the Database
        public IActionResult VerifyEmpNo(string empNo)
        {
            List<List<string>> cur_empl = getFields_dbl_lst("EMP");

            bool EmplExists = false;

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

        public IActionResult VerifyCorrectWC(string WC, 
                        string ToolNo, List<List<string>> WCdropDownList,
                        List<List<string>> ToolNoDropDownList,
                        List<List<string>> ToolLocationsList)
        {
            WC = WC.ToUpper();
            List<List<string>> WCList = getFields_dbl_lst("WC");

            // TODO: serialize and deserialize to pass in the lists
            // Remote does not support types
            string verifiedWC = GetWC(ToolNo, null,
                WCList, null);


            // Check if it exists in WC list
            // tuple2 = [*Name*, Description, WCUnder, *ID*]
            bool WCExists = false;
            foreach (List<string> WC_Combo in WCList)
            {
                if (WC_Combo[0].Trim().Equals(WC))
                {
                    WCExists = true;
                }
            }

            if (verifiedWC.Equals(WC) || verifiedWC.Equals(""))
            {
                return WCExists ? Json(true) 
                    : Json("Not a valid WC, If this is a correct WC please" +
                    " contact QA ENG");
            }
            else
            {
                return Json("Expected: "+ verifiedWC +
                    ", please contact QA ENG to change this");
            }
        }

        // GET: MeasureController/AddMeasure
        // Populates the dropdown lists for the page
        [HttpGet]
        public ActionResult AddMeasure()
        {
            // TODO hide button in the view that let's you change submit date
            ToolMeasure measure = new ToolMeasure();
            measure.WCdropDownList     = getFields_dbl_lst("WC");

            measure.EmplDropDownList = getFields_dbl_lst("EMP");
            
            List<List<string>> unsorted_tools = getFields_dbl_lst("TOOL");
            measure.ToolNoDropDownList = unsorted_tools.OrderBy( x =>
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



            measure.ToolLocationsList = getFields_dbl_lst("LOCATE");
            measure.T_Date = DateTime.Now.Date;

            return View(measure);
        }

        // POST: MeasureController/AddMeasure
        // Verifies inputs and submits tool
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMeasure(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    // TODO : if new tool insert into Locations, and tool
                    // TODO : (should I do this?) if tool exist, insert new WC if it exist

                    // TODO : must contain "TTU"

                    // Note: no new WC allowed
                    CreateMeasureHelper(collection);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return RedirectToAction("Error");                  
                }
            }

            // If reached error catching not working
            // Set tool Defaults
            ToolMeasure toolMeasure = new ToolMeasure();
            toolMeasure.WCdropDownList = getFields_dbl_lst("WC");
            return View(toolMeasure);
        }

        // Takes out values and uses the MeasuerLogic Controller to
        // add tool check in
        private void CreateMeasureHelper(IFormCollection collection)
        {
            List<List<string>> toolList = getFields_dbl_lst("TOOL");

            
            string T_Date = collection["T_Date"];
            string ToolNo = collection["ToolNo"].ToString().ToUpper();
            string S_Size = collection["S_Size"].ToString();
            string WC = collection["WC"].ToString().ToUpper();
            string EmpNo = collection["EmpNo"];
            string Condition = collection["Condition"].ToString();
            // TODO : xtra credit, create a boolean in model class
            //      That reflects whether the tool is recognized

            bool toolExists = false;
            // Check if the tool is in the tool List
            // [*ID *, *Tool_ID *, Description]


            foreach (List<string> toolTuple in toolList)
            {
                if (toolTuple[1].Equals(ToolNo))
                {
                    toolExists = true;
                }
            }

            if (! toolExists)
            {   // error catching 1 if success
                // Put this into Tool DB
                int status = CreateTool(ToolNo);

                int toolID = FindToolID(ToolNo);
                int WCID = FindWCID(WC);

                AddLocation(toolID, WCID);

                // TODO send email saying a new tool is added
            }

            CreateMeasure(T_Date, ToolNo, S_Size, WC, EmpNo, Condition);

        }

        // GET: MeasureController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MeasureController/Edit/5
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

        // GET: MeasureController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MeasureController/Delete/5
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
    }
}
