﻿using DataLibrary.DataAccess;
using DataLibrary.Models;

namespace DataLibrary.BusinessLogic
{
    // This is the logic for accessing the measure data from the database
    public class MeasureLogicController
    {
        // Inserts tool check into create Daily check
        public static int CreateMeasure(string T_Date, string ToolNo, string S_Size, string WC, string EmpNo, string Condition)
        {
            // Turns data into local toolMeasure class
            ToolMeasureModel data = new ToolMeasureModel
            {
                T_Date = T_Date,
                WC = WC,
                ToolNo = ToolNo,
                S_Size = S_Size,
                EmpNo = EmpNo,
                Condition = Condition,
            };

            string sql = @"INSERT into dbo.Daily_Check (T_Date, WC, ToolNo, S_Size, EmpNo, Condition) 
                            Values (@T_Date, @WC, @ToolNo, @S_Size, @EmpNo, @Condition);";

            return SqlDataAccess.SaveData(sql, data);
        }


        // Loads the all the Measures
        // COMMENTED OUT: Get list of all entries in the DB from the last 7 days
        public static List<ToolMeasureModel> LoadMeasures()
        {
            string sql = @"SELECT ID, T_Date, WC, ToolNo, S_Size, EmpNo, Condition
                            FROM dbo.Daily_Check
                            ORDER BY ID DESC
                            --WHERE T_Date > TRY_CONVERT(DATETIME, '" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd hh:mm") + "'); ";

            return SqlDataAccess.LoadData<ToolMeasureModel>(sql);
        }


        // Recieves type of list needed and executes sql code to return a list
        //      for dropdown lists
        public static List<List<string>> LoadFields_dbl_lst(string type)
        {
            string sql = "";

            if (type.Equals("EMP"))
            {
                sql = @"SELECT Name, Clock_Code
                            FROM dbo.Employee1
                            WHERE Active = 1;";

                return SqlDataAccess.LoadListData<List<string>>(sql, "Name", "Clock_Code");
            }
            else if (type.Equals("WC"))
            {
                sql = @"SELECT Name, Description, WCUnder, ID
                            FROM dbo.WorkCenter3
                            WHERE Active = 1
                            ORDER BY Name ASC;";

                return SqlDataAccess.LoadListData<List<string>>(sql,
                                                "Name", "Description", "WCUnder", "ID");
            }
            else if (type.Equals("TOOL"))
            {
                sql = @"SELECT ID, Tool_ID, Description
                            FROM dbo.Gage_List_Main
                            WHERE Active = 1
                            ORDER BY Tool_ID ASC;";
                return SqlDataAccess.LoadListData<List<string>>(sql, "ID",
                                                "Tool_ID", "Description");

            }

            else if (type.Equals("LOCATE"))
            {
                sql = @"SELECT Tool_ID, WC_ID
                            FROM dbo.Tool_Locations1
                            WHERE Status = 1
                            ;";
                return SqlDataAccess.LoadListData<List<string>>(sql, "Tool_ID", "WC_ID");

            }
            else { throw new Exception("Incorrect type entered"); }

        }

    }
}
