using ClosedXML.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Data;
using System.Text.RegularExpressions;
using ToolProgramCore.Models;
using static DataLibrary.BusinessLogic.Fields_Change;
using static DataLibrary.BusinessLogic.MeasureLogicController;

namespace ToolProgramCore.Controllers
{
    // structure of class :
    // 1. ** Page Loaders **
    // 2. ** Data Loaders **
    // 4. ** Data Verify **
    // 3. ** HTML Helpers **
    public class MeasureController : Controller
    {

        // ** Page Loaders **

        // GET: MeasureController
        // Gets all the measure that have been made and displays only 10 on each page
        // defaults to page 1
        [AllowAnonymous]
        public ActionResult Index(string page = "1")
        {
            // TODO: make run time better (make it so query for count) then make query for only the top 10 and so on
            // Get list
            GetMeasureList();

            // get the page index and size from query string or default values
            string SetPageSize = "10";
            int pageIndex = int.Parse(page ?? "1");
            int pageSize = int.Parse(SetPageSize ?? "10");

            // get the total number of records
            int totalCount = MeasureList.Count();

            // calculate the total number of pages needed
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
        [AllowAnonymous]
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

        // GET: MeasureController/AddMeasure
        // Populates the dropdown lists for the page
        [HttpGet]
        [AllowAnonymous]
        public ActionResult AddMeasure()
        {
            ToolMeasure measure = new ToolMeasure();

            // Get lists
            measure.WCdropDownList = getFields_dbl_lst("WC");
            measure.EmplDropDownList = getFields_dbl_lst("EMP");
             
            // TODO: Is this every used?
            measure.ToolLocationsList = getFields_dbl_lst("LOCATE");
            List<List<string>> unsorted_tools = getFields_dbl_lst("TOOL");

            // Sort Tool List
            measure.ToolNoDropDownList = unsorted_tools.OrderBy(x =>
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
            measure.T_Date = DateTime.Now.Date;

            return View(measure);
        }

        // POST: MeasureController/AddMeasure
        // Verifies inputs and submits tool
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult AddMeasure(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Note: no new WC allowed
                    CreateMeasureHelper(collection);

                    // TODO : redirect to sucess page
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return RedirectToAction("Error");
                }
            }

            // If reached error catching not working
            return RedirectToAction("Error");
        }

        // [Helper Function] Takes out values and uses the MeasuerLogic Controller to
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

            // TODO : tool must contain "TTU "

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

            // if tool does not exist, insert new WC if it exist
            if (!toolExists)
            {
                // error catching 1 if success
                // Put this into Tool DB
                try
                {
                    int status = CreateTool(ToolNo);

                    int toolID = FindToolID(ToolNo);
                    int WCID = FindWCID(WC);

                    AddLocation(toolID, WCID);
                }
                catch
                {
                    throw new Exception("Error on inserting creating new tool");

                }
                // TODO: send email saying a new tool is added
            }

            // User DataLibrary to insert
            CreateMeasure(T_Date, ToolNo, S_Size, WC, EmpNo, Condition);

        }

        public IActionResult Export()
        {
            // Create a DataTable with some sample data
            DataTable dt = new DataTable("Tool Measure");
            dt.Columns.AddRange(new DataColumn[7] {

                //new DataColumn("ID", typeof(int)),
                new DataColumn("Measure Date", typeof(DateOnly)),
                new DataColumn("WC",typeof(string)),
                new DataColumn("ToolNo",typeof(string)),
                new DataColumn("EmpNo",typeof(string)),
                new DataColumn("Name",typeof(string)),

                new DataColumn("Standard Size",typeof(double)),
                new DataColumn("Condition",typeof(double)),

            });

            GetMeasureList();
            if (MeasureList == null)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (ToolMeasure toolInfo in MeasureList)
            {
                DateOnly? measuredDate = null;
                if (toolInfo.T_Date != null)
                {
                    measuredDate = DateOnly.FromDateTime((DateTime)toolInfo.T_Date);
                }


                dt.Rows.Add(measuredDate,

                    toolInfo.WC,
                    toolInfo.ToolNo, toolInfo.EmpNo, toolInfo.EmpName,
                    toolInfo.S_Size, toolInfo.Condition);
            }



            // Create a workbook with a worksheet
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt);

                // Set some properties of the workbook
                wb.Properties.Title = "Tool Measure Report";
                wb.Properties.Author = "Tool Program";
                wb.Properties.Company = "Toyo Tanso IT";

                // Set some properties of the worksheet
                ws.Name = "Checked out tools Data";
                ws.TabColor = XLColor.Red;
                ws.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                foreach (var item in ws.ColumnsUsed())
                {
                    item.AdjustToContents();
                }

                // Save the workbook as a stream
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;

                    // Write the stream to the response
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ToolMeasureData.xlsx");
                }
            }
        }

        // The following pages not in use:

        //// GET: MeasureController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: MeasureController/Edit/5
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

        //// GET: MeasureController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: MeasureController/Delete/5
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
        public List<ToolMeasure>? MeasureList;

        // Updates the MeasureList so that it gets most recent data from DB
        [AllowAnonymous]
        public void GetMeasureList()
        {
            // Get list of ToolMeasureModel from data libary
            var data = LoadMeasures();
            List<ToolMeasure> toolMeasures = new();

            // Get Employee Names once, to get name
            List<List<string>> EmplDropDownList = getFields_dbl_lst("EMP");

            foreach (var row in data)
            {
                if (row.T_Date == null)
                {
                    break;
                }
                // Convert string to required data type
                DateTime ConvertedDate = DateTime.Parse(row.T_Date);
                double S_Size_converted = double.Parse(row.S_Size ?? "");
                double Condition_converted = double.Parse(row.Condition ?? "");

                // Enter values into this model: ToolMeasure. Then add to list
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
                    EmpName = getEmployeeName(row.EmpNo, EmplDropDownList),
                });
            }
            MeasureList = toolMeasures;
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

        // This verifies input in the form WC is for the right tool (Helper) [called in the model class] 
        // the right Work center
        [AllowAnonymous]
        public IActionResult VerifyCorrectWC(string WC,
                        string ToolNo, List<List<string>> WCdropDownList,
                        List<List<string>> ToolNoDropDownList,
                        List<List<string>> ToolLocationsList)
        {
            // TODO: serialize and deserialize to pass in the lists
            // Remote does not support complex types


            WC = WC.ToUpper();
            List<List<string>> WCList = getFields_dbl_lst("WC");

            // Use helper function to get "" or the correct WC
            string verifiedWC = GetWC(ToolNo, null,
                WCList, null);

            // TODO: DO I need this? if it picks it up in GETWC
            //      shouldnt it be updated already
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

            // After seeing if it's in there
            if (verifiedWC.Equals(WC) || verifiedWC.Equals(""))
            {
                return WCExists ? Json(true)
                    : Json("Not a valid WC, If this is a correct WC please" +
                    " contact QA ENG");
            }
            else
            {
                return Json("Expected: " + verifiedWC +
                    ", please contact QA ENG to change this");
            }
        }

        // This verifies input in the form (Helper) [called in the model class]
        // Returns error if the employee does not exist in the Database
        // TODO: make sure that this is not repeating with tracker and measure
        [AllowAnonymous]
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

        // ** HTML Helpers **

        // This function is called in razor AddMeasure page to get the workcenter
        //      For a given tool, (Quality of a life thing)
        [AllowAnonymous]
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
                            return tuple2[0].Trim(); // TODO: trim String
                        }
                    }
                    break;
                }
            }
            return "";
        }

    }
}
