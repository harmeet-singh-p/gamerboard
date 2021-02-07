using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProj
{
    class gamelistDB
    {
        private string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();

        public DataTable Get_Game_CatList()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select  * from dbo.f_get_game_catlist()";
                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                    {
                        ad.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataSet Get_GameList(string cat, string search, string listopt, int frcount, int tocount)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select * from dbo.f_get_gamelist('" + cat + "','" + search + "','" + listopt + "'," + frcount + "," + tocount + ") select * from dbo.f_get_gamelist('" + cat + "','" + search + "','" + listopt + "'," + (tocount + 1) + "," + (tocount+10) + ")";
                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                    {
                        ad.Fill(ds);
                    }
                }
            }
            return ds;
        }

        public DataTable Get_User_Details(string username)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select  * from dbo.f_get_user_details('" + username + "')";
                    using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                    {
                        ad.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
