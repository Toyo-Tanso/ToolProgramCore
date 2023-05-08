using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess
{
    internal class SqlDataAccess
    {
        public static string GetConnectionString(string connectionName = "NCPR_PROG1")
        {
            return "Data Source = DATALINKSRVR\\SQLEXPRESS; Initial Catalog = NC_PROG; Integrated Security = True";
        }

        public static List<T> LoadData<T>(String sql)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql).ToList();
            }
        }

        public static int SaveData<T>(String sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Execute(sql, data);
            }
        }

        public static int GetMax(string sqlMax)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                List<string> maxQuery = cnn.Query<string>(sqlMax).ToList();

                foreach (string Row in maxQuery)
                {
                    return int.Parse(Row);
                }
                return 0;

            }
        }


        public static int ReturnTool<T>(String sqlDelete, String sqlInsert, T data)
        {


            int returnStatus = 0;
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                returnStatus = cnn.Execute(sqlDelete, data);
            }



            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return returnStatus & cnn.Execute(sqlInsert, data);
            }


        }
    }
}
