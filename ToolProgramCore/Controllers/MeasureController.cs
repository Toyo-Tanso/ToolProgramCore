using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Globalization;
using ToolProgramCore.Models;
using static DataLibrary.BusinessLogic.MeasureLogicController;

namespace ToolProgramCore.Controllers
{
    public class MeasureController : Controller
    {
        // holds current list
        private List<ToolMeasure>? MeasureList;

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

                toolMeasures.Add(new ToolMeasure
                {
                    ID = row.ID,
                    T_Date = ConvertedDate,
                    WC = row.WC,
                    ToolNo = row.ToolNo,
                    S_Size = S_Size_converted,
                    EmpNo = row.EmpNo,
                    Condition = Condition_converted

                });
            }
            MeasureList = toolMeasures;
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


            //foreach (var row in data)
            //{
            //    fieldList.Add(row);
            //}
            //return fieldList;

            fieldList = LoadFields_dbl_lst(type);

            return fieldList; 
        }

        // GET: MeasureController
        // Gets list from the last 7 days and displays them
        public ActionResult Index()
        {
            GetMeasureList();
            return View(MeasureList);
        }

        // GET: MeasureController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
                return Json($"Employee does not exist. Notify Leo");
            }
        }

        // GET: MeasureController/AddMeasure
        // Populates the dropdown lists for the page
        [HttpGet]
        public ActionResult AddMeasure()
        {
            // TODO hide button in the view that let's you change submit date
            ToolMeasure measure = new ToolMeasure();
            measure.WCdropDownList     = getFieldsList("WC");
            // TODO : add the following lists
            measure.EmplDropDownList = getFields_dbl_lst("EMP");
            //measure.ToolNoDropDownList = getFieldsList("TOOL");

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
                    // TODO : if new tool insert into gauge, with new WC
                    // TODO : extra credit remove make consistent case system
                    //        ex: tool2 == TOOL2
                    // TODO : (should I do this?) if tool exist, insert new WC if it exist
                    // If valid user helper function
                    // TODO if new employee maybe send an email?
                    // if collection has an string instead of in send email
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
            toolMeasure.WCdropDownList = getFieldsList("WC");
            return View(toolMeasure);
        }

        // Takes out values and uses the MeasuerLogic Controller to
        // add tool check in
        private void CreateMeasureHelper(IFormCollection collection)
        {
            string T_Date = collection["T_Date"];
            string ToolNo = collection["ToolNo"].ToString().ToUpper();
            string S_Size = collection["S_Size"].ToString();
            string WC = collection["WC"];
            string EmpNo = collection["EmpNo"];
            string Condition = collection["Condition"].ToString();
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
