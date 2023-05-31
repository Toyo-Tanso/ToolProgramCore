using DataLibrary.DataAccess;
using DataLibrary.Models;

namespace DataLibrary.BusinessLogic
{
    // Intended to be the class to manipulate some of the fields
    public class Fields_Change
    {
        // Creates a new tool with it being active,
        // does not verify format nor does it add a locations
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
        public static int FindToolID(string ToolNo)
        {

            string sql = @"SELECT ID FROM dbo.Gage_List_Main  
                            WHERE Tool_ID='" + ToolNo + "';";

            return SqlDataAccess.LoadData<int>(sql)[0];
        }


        // Adds Location given a toolID, and a WCID
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

    }
}
