using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    // Uses the Daily_check to see what is in there and to add
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

        // TODO: Do I need this?
        public static int UpdateTool(string T_Date, string ToolNo, string S_Size, string WC, string EmpNo, string Condition, string ID)
        {

            // TODO: Format T_Date
            string FormatedT_Date = "05-24-2022 14:22";

            ToolMeasureModel data = new ToolMeasureModel
            {
                T_Date = FormatedT_Date,
                WC = WC,
                ToolNo = ToolNo,
                S_Size = S_Size,
                EmpNo = EmpNo,
                Condition = Condition,
                ID = ID
            };

            string sql = @"UPDATE dbo.Daily_Check SET @T_Date = T_Date, @WC = WC, @ToolNo = ToolNo, @S_Size = S_Size, @EmpNo = EmpNo, @Condition = Condition WHERE ToolNo = @toolNumber;";

            return SqlDataAccess.SaveData(sql, data);
        }

        // TODO : remove all old entries in DB
        // Get list of all entries in the DB from the last 7 days
        public static List<ToolMeasureModel> LoadMeasures()
        {
            string sql = @"SELECT ID, T_Date, WC, ToolNo, S_Size, EmpNo, Condition
                            FROM dbo.Daily_Check
                            WHERE T_Date > TRY_CONVERT(DATETIME, '" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd hh:mm") + "'); ";


            return SqlDataAccess.LoadData<ToolMeasureModel>(sql);
        }

        // Commented out for now

        //public static List<ToolMeasureModel> LoadToolsHistory()
        //{
        //    string sql = @"SELECT ToolNo as toolNumber, D_Remove, P_Return, WC, EmpNo, DateReturned, ID
        //                    FROM dbo.Tool_Move_History; ";


        //    return SqlDataAccess.LoadData<ToolMeasureModel>(sql);
        //}


        //public static int getMaxInHistory()
        //{
        //    string sqlMax = @"SELECT Max(ID) AS ID FROM dbo.Tool_Move_History;";
        //    return SqlDataAccess.GetMax(sqlMax);
        //}


        //public static int returnCheckedOutTool(string toolNumber, string D_Remove, string P_Return, string WC, string EmpNo)
        //{
        //    ToolMeasureModel data = new ToolMeasureModel
        //    {
        //        toolNumber = toolNumber,
        //        D_Remove = D_Remove,
        //        P_Return = P_Return,
        //        WC = WC,
        //        EmpNo = EmpNo,
        //        DateReturned = DateTime.Now.ToString("MM/dd/yyyy"),
        //        ID = (getMaxInHistory() + 1).ToString(),

        //    };

        //    string sqlDelete = @"DELETE FROM dbo.Tool_Move
        //                        WHERE ToolNo = @toolNumber; ";

        //    string sqlInsert = @"insert into dbo.Tool_Move_History (ID, ToolNo, D_Remove, P_Return, WC, EmpNo, DateReturned) 
        //                    Values (@ID, @toolNumber, @D_Remove, @P_Return, @WC, @EmpNo, @DateReturned);";


        //    return SqlDataAccess.ReturnTool(sqlDelete, sqlInsert, data);
        //}


        // TODO : Finish implementation
        // Change based on WCs, EmplNo and Tools
        public static List<string> LoadFields(string type)
        {
            string sql = "";

            if (type.Equals("WC")) {
                sql = @"SELECT Name
                            FROM dbo.WorkCenter
                            WHERE Active = 1;";
                return SqlDataAccess.LoadData<string>(sql);
            }
            else if (type.Equals("TOOL"))
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new Exception("Invalid WC entered");
            }

        }
        public static List<List<string>> LoadFields_dbl_lst(string type)
        {
            string sql = "";
            if (type.Equals("EMP"))
            {
                sql = @"SELECT Name, Clock_Code
                            FROM dbo.Employee1
                            WHERE Active = 1;";
               
                return SqlDataAccess.LoadDictData<List<string>>(sql);
            }
            else { throw new Exception("Incorrect type entered"); }

        }
    

            // Not needed right now

            //// Return true if the toolNo is in the database is in there
            //public static bool InDB(string toolNo)
            //{

            //    string sql = @"SELECT COUNT(ToolNo) FROM dbo.Tool_Move WHERE ToolNo = '" + toolNo + "';";

            //    return SqlDataAccess.LoadData<string>(sql)[0] == "0";

            //}
        }
}
