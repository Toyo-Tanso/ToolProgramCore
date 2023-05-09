using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult AddMeasure()
        {
            ToolMeasure measure = new ToolMeasure();
            measure.WCdropDownList     = getFieldsList("WC");
            measure.EmplDropDownList   = getFieldsList("EMP");
            measure.ToolNoDropDownList = getFieldsList("TOOL");



            measure.T_Date = DateTime.Now;



            return View();
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
                    CreateMeasure(collection);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            // TODO: compare to original tool Program
            return View();
        }

        private void CreateMeasure(IFormCollection collection)
        {
            throw new NotImplementedException();
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
