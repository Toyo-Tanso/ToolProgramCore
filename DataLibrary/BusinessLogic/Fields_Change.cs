using DataLibrary.DataAccess;
using DataLibrary.Models;

namespace DataLibrary.BusinessLogic
{
    // Intended to be the class to manipulate some of the fields
    public class Fields_Change
    {
        // Creates a new tool with it being active,
        // does not verify format nor does it add a locations
        // Used by Measure
        public static int CreateTool(string ToolNo)
        {
            ToolDB data = new ToolDB
            {
                Tool_ID = ToolNo,
                Active = 1
            };

            string sql = @"INSERT into dbo.Gage_List_Main (Tool_ID, Active) 
                            Values (@Tool_ID, @Active);";

            return SqlDataAccess.SaveData(sql, data);
        }

        // Finds the ID of the tool, if it exists
        // Used by Measure
        public static int FindToolID(string ToolNo)
        {

            string sql = @"SELECT ID FROM dbo.Gage_List_Main  
                            WHERE Tool_ID='" + ToolNo + "';";

            return SqlDataAccess.LoadData<int>(sql)[0];
        }


        // Adds Location given a toolID, and a WCID
        // Used by Measure
        public static int AddLocation(int ToolID, int WCID)
        {

            Locations_DB data = new Locations_DB
            {
                Tool_ID = (ToolID),
                WC_ID = (WCID),
                Status = 1
            };

            string sql = @"INSERT INTO dbo.Tool_Locations1
                            (Tool_ID, WC_ID, Status) 
                            VALUES (@Tool_ID, @WC_ID, @Status)
                             ;";

            return SqlDataAccess.SaveData(sql, data);
        }

        // TODO implement: sets tool as inactive in the DB
        // Used by Measure
        public static int DeleteTool(string ToolNo)
        {
            throw new NotImplementedException();
        }

        // Find the wc ID given a WC
        public static int FindWCID(string WC)
        {

            string sql = @"SELECT ID FROM dbo.WorkCenter3  
                            WHERE Name='" + WC + "'" +
                            "AND Active=1;";

            return SqlDataAccess.LoadData<int>(sql)[0];
        }


        // Recieves type of list needed and executes sql code to return a list
        //      for dropdown lists
        // Used by Measure
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
