﻿using Microsoft.AspNetCore.Authorization;
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
            //var data = getWCDetails(id);

            //List<string>? WCUnderList = strToArray(data.WCUnder ?? "");

            //WorkCenter curWC = new WorkCenter
            //{
            //    Name = data.Name,
            //    Description = data.Description,
            //    WCUnder = WCUnderList,
            //    ID = id.ToString(),
            //    Active = data.Active.ToString(),
            //}
            //;


            //return View(curWC);
            return View();
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
                //AddEmplHelper(collection);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
            throw new NotImplementedException();
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

        // Ensures that every word is capitalized
        // Not 100% necessary but it's nice to have consistency in the database
        // TODO: implement change desc
        public IActionResult VerifyStartsWithTTU(string Tool_ID)
        {

            if (!AllCap)
            {
                return Json($"Each word must be capitalized.");
            }
            else
            {
                return Json(true);
            }
            throw new NotImplementedException();
        }

        // TODO: implement add desc
        [AllowAnonymous]
        public IActionResult VerifyUniqueID(string Tool_ID)
        {
            //List<List<string>> cur_empl = LoadFields_dbl_lst("EMP_ALL");

            //bool EmplExists = false;

            //// if it's in the list
            //foreach (var row in cur_empl)
            //{
            //    if (row[1].Trim() == Clock_Code)
            //    {
            //        EmplExists = true;
            //    }
            //}

            //if (EmplExists)
            //{
            //    return Json($"Employee Number Exists. Please enter another code.");
            //}
            //else
            //{
            //    return Json(true);
            //}

            throw new NotImplementedException();
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
