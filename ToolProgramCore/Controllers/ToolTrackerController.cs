using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToolProgramCore.Models;
using static DataLibrary.BusinessLogic.TrackerLogicController;

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
        public ActionResult Index(string page = "1")
        {
            return View();
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

        public void GetCheckedInList()
        {


            throw new NotImplementedException();

            //// Get list of ToolMeasureModel from data libary
            //var data = LoadMeasures();
            //List<ToolMeasure> toolMeasures = new();

            //// Get Employee Names once, to get name
            //List<List<string>> EmplDropDownList = getFields_dbl_lst("EMP");

            //foreach (var row in data)
            //{
            //    if (row.T_Date == null)
            //    {
            //        break;
            //    }
            //    // Convert string to required data type
            //    DateTime ConvertedDate = DateTime.Parse(row.T_Date);
            //    double S_Size_converted = double.Parse(row.S_Size ?? "");
            //    double Condition_converted = double.Parse(row.Condition ?? "");

            //    // Enter values into this model: ToolMeasure. Then add to list
            //    toolMeasures.Add(new ToolMeasure
            //    {
            //        ID = row.ID,
            //        T_Date = ConvertedDate,
            //        WC = row.WC,
            //        ToolNo = row.ToolNo,
            //        S_Size = S_Size_converted,
            //        EmpNo = row.EmpNo,
            //        Condition = Condition_converted,
            //        EmplDropDownList = EmplDropDownList,
            //        EmpName = getEmployeeName(row.EmpNo, EmplDropDownList),
            //    });
            //}
            //MeasureList = toolMeasures;
        }

    }
}
