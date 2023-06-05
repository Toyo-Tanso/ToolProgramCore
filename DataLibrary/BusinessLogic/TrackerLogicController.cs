using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public class TrackerLogicController
    {

        // Loads the all the tools currently checked Out
        public static List<BorrowedToolModel> LoadBorrowedTools()
        {
            // 
            string sql = @"SELECT ID, ToolNo, Date_Removed, Promise_Return_Date,
                            WC_From, WC_To, EmpNo
                            FROM dbo.Tool_Moves1
                            WHERE Returned_Date is NULL
                            ORDER BY Date_Removed ASC
                            ; ";

            return SqlDataAccess.LoadData<BorrowedToolModel>(sql);
        }

        // Loads the all records of tools being checked Out
        public static List<BorrowedToolModel> LoadAllBorrowedTools()
        {

            string sql = @"SELECT ID, ToolNo, Date_Removed, Promise_Return_Date,
                            WC_From, WC_To, EmpNo, Returned_Date, Return_EmpNo
                            FROM dbo.Tool_Moves1
                            ORDER BY Date_Removed DESC
                            ; ";

            return SqlDataAccess.LoadData<BorrowedToolModel>(sql);
        }

        // Loads the all the tools currently checked Out
        public static bool isBorrowed(string ToolNo)
        {
            // get all instance of tool number, true if its in there, and it has not been returned
            string sql = @"SELECT ID
                            FROM dbo.Tool_Moves1
                            WHERE ToolNo='" + ToolNo +
                            "' AND Returned_Date is NULL" +
                            ";";

            return SqlDataAccess.LoadData<int>(sql).Count != 0;
        }

        // Class to help unify data into one usable datatype
        public class LocationData
        {
            public int WC_ID { get; set; }
            public int Tool_ID { get; set; }
        }

        public static int setOldLocation(int WCID, int toolID)
        {
            
            // all instances of location ID that have WC and tool ID
            string sql = @"SELECT Status
                            FROM dbo.Tool_Locations1
                            WHERE WC_ID='" + WCID +
                            "' AND Tool_ID='" + toolID +
                            "';";

            // If location is not found
            if (SqlDataAccess.LoadData<int>(sql).Count == 0)
            {
                throw new Exception("Location data not found");
            }
            // If more than one something is wrong, all duplicates should be deleted when returned
            LocationData data = new LocationData();
            data.WC_ID = WCID;
            data.Tool_ID = toolID;


            sql = @"UPDATE 
                            dbo.Tool_Locations1

                            SET Status = 0 
                            WHERE WC_ID = @WC_ID " +
                            "AND Tool_ID = @Tool_ID "+
                            ";";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int setNewLocation(int WCID, int toolID)
        {
            LocationData data = new LocationData();
            data.WC_ID = WCID;
            data.Tool_ID = toolID;

            // Add new location and make it active, and set it as borrowed
            string sql = @"INSERT INTO 
                            dbo.Tool_Locations1
                            (Tool_ID, WC_ID, Status, Borrowed)
                            VALUES (@Tool_ID, @WC_ID, 1, 1) " +
                            ";";

            // Return status of query
            return SqlDataAccess.SaveData(sql, data);


        }


            public static int saveCheckOut(string ToolNo, string Promise_Return_Date, 
                string WC_From, string WC_To, string EmpNo, string Date_Removed)
        {
            // Turns data into local toolMeasure class
            BorrowedToolModel data = new BorrowedToolModel
            {
                Promise_Return_Date = Promise_Return_Date,
                WC_From = WC_From,
                ToolNo = ToolNo,
                WC_To = WC_To,
                EmpNo = EmpNo,
                Date_Removed = Date_Removed,
            };

            string sql = @"INSERT into dbo.Tool_Moves1 (ToolNo, WC_From, WC_To, EmpNo, Date_Removed, Promise_Return_Date) 
                            Values (@ToolNo, @WC_From, @WC_To, @EmpNo, @Date_Removed, @Promise_Return_Date);";

            return SqlDataAccess.SaveData(sql, data);
        }

    }
}
