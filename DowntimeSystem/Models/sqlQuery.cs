using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DowntimeSystem.Models
{
    public class DatabaseManagement
    {
        //Host=cnwuxm1medb01;Database=EC;Username=ECUser;Password=Jabil123
        private static string db = "data source=cnwuxm1medb01;initial catalog=EC;persist security info=True;user id=ECUser;password=Jabil123;";
        private SqlConnection conn;

        public DatabaseManagement()
        {
            conn = new SqlConnection(db);
        }

        public DataTable Query(string sqlText)
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlText, conn);
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                dt.Clear();
                ad.Fill(dt);
                cmd.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                conn.Dispose();
            }
        }
    }
}
