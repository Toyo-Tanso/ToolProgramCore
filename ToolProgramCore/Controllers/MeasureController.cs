using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ToolProgramCore.Models;
using static DataLibrary.BusinessLogic.MeasureLogicController;

namespace ToolProgramCore.Controllers
{
    public class MeasureController : Controller
    {

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
                toolMeasures.Add(new ToolMeasure
                {
                    ID = row.ID,
                    T_Date = ConvertedDate,
                    WC = row.WC,
                    ToolNo = row.ToolNo,
                    S_Size = row.S_Size,
                    EmpNo = row.EmpNo,
                    Condition = row.Condition

                });
            }
            MeasureList = toolMeasures;
        }

        // Is used to populate the ID Dropdown list in a tool object
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

        // GET: MeasureController
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

        // GET: MeasureController/AddMeasure
        // Populates the dropdown lists for the page
        [HttpGet]
        public ActionResult AddMeasure()
        {
            ToolMeasure measure = new ToolMeasure();
            measure.WCdropDownList     = getFieldsList("WC");
            //measure.EmplDropDownList   = getFieldsList("EMP");
            //measure.ToolNoDropDownList = getFieldsList("TOOL");

            measure.T_Date = DateTime.Now.Date;


            return View(measure);
        }

        // POST: MeasureController/Create
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
                    
                    CreateMeasureHelper(collection);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return RedirectToAction("Error");
                    
                }
            }

            // Set tool Defaults
            ToolMeasure toolMeasure = new ToolMeasure();
            toolMeasure.WCdropDownList = getFieldsList("WC");

            //toolMeasure.T_Date = DateTime.Now.Date;
            


            return View(toolMeasure);
        }

        private void CreateMeasureHelper(IFormCollection collection)
        {
            string T_Date = collection["T_Date"];
            string ToolNo = collection["ToolNo"];
            string S_Size = collection["S_Size"];
            string WC = collection["WC"];
            string EmpNo = collection["EmpNo"];
            string Condition = collection["Condition"];


            Console.WriteLine("123456789");

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
