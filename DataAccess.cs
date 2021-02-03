using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace GameProj
{
 

    public class DataAccess
    {
        string ConnectionString = string.Empty;
        //@"Server=www.leaserp.com;Database=devtemp;User Id=devtemp;Password=an?bc2+6; Integrated Security =false;";
        string directory;



        public DataAccess()
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            directory = Path.GetDirectoryName(Application.ResourceAssembly.Location);
            directory += "\\images\\news\\";
        }

        public IList<BindedTableInfo> GetTableInfos(string gamecode)
        {
            var resultList = new List<BindedTableInfo>();
            var con = new SqlConnection(ConnectionString);
            con.Open();
            var command = new SqlCommand("select * from [dbo].[f_get_game_stats_headers](@gamecode)", con);
            command.Parameters.AddWithValue("@gamecode", gamecode);
            using (var sqlReader = command.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    var bindedTableInfo = new BindedTableInfo();
                    bindedTableInfo.ColNo = sqlReader.GetInt32(0);
                    bindedTableInfo.ColDesc = sqlReader.GetString(1);
                    bindedTableInfo.ColType = sqlReader.GetString(2);
                    bindedTableInfo.ColShowIn = sqlReader.GetString(3);
                    bindedTableInfo.DetPos = sqlReader.GetString(4);
                    resultList.Add(bindedTableInfo);
                }
            }
            return resultList;
        }

        internal IList<NewsViewModel> LoadNews(int pi_frcount, int pi_tocount)
        {
           
            // DO NOT DELETE COMMENTED CODE

            var resultList = new List<NewsViewModel>();
            var con = new SqlConnection(ConnectionString);
            con.Open();
            var command = new SqlCommand("select * from [dbo].[f_get_newsitems](@pi_frcount,@pi_tocount)", con);
            command.Parameters.AddWithValue("@pi_frcount", pi_frcount);
            command.Parameters.AddWithValue("@pi_tocount", pi_tocount);
            using (var sqlReader = command.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    var newsVM = new NewsViewModel();

                    string newsImage = sqlReader.GetString(1);                  

                    if (File.Exists(directory + newsImage))
                    {
                        newsVM.NewsImage = directory + newsImage;
                    }
                    else
                    {
                        newsVM.NewsImage = directory + "news1.png";
                    }

                    newsVM.NewsText = Convert.ToString(sqlReader.GetString(2));
                    newsVM.HTML = Convert.ToString(sqlReader.GetString(3));
                    newsVM.PostBy = Convert.ToString("Posted by " + sqlReader.GetString(4));
                    newsVM.PostDt = Convert.ToString(", on " + Convert.ToDateTime(sqlReader.GetString(5)).ToString("dd/MM/yyyy"));
                    resultList.Add(newsVM);
                }
            }
            return resultList;
        }

        public IList<UserViewModel> GetUserInfos()
        {
            var resultList = new List<UserViewModel>();
            var con = new SqlConnection(ConnectionString);
            con.Open();
            var command = new SqlCommand("select * from [f_get_user_details_all]()", con);
            using (var sqlReader = command.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    var userViewModel = new UserViewModel();
                    userViewModel.UserName = sqlReader.GetString(1);

                    string userImage = sqlReader.GetString(2);
                    var directory = Path.GetDirectoryName(Application.ResourceAssembly.Location);
                    directory = directory + "\\images\\users\\";

                    if (File.Exists(directory + userImage))
                    {
                        userViewModel.UserImage = directory + userImage;
                    }
                    else
                    {
                        userViewModel.UserImage = directory + "user.png";
                    }

                    //userViewModel.UserImage = sqlReader.GetString(2);
                    userViewModel.LoyaltyPoints = Convert.ToString(sqlReader.GetInt32(3));
                    userViewModel.CenterRanking = Convert.ToString(sqlReader.GetInt64(4));
                    resultList.Add(userViewModel);
                }
            }
            return resultList;
        }

        public IList<GamesList> LoadGamesList()
        {
           var gamesList = new List<GamesList>();
            try
            {
                var con = new SqlConnection(ConnectionString);
                con.Open();
                var command = new SqlCommand("select * from [dbo].f_get_leaderboard_gamelist()", con);
                using (var sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        var game = new GamesList();
                        game.GameCode = sqlReader.GetString(0);
                        game.GameName = sqlReader.GetString(1);

                        var regions = sqlReader.GetString(2).Split(',').ToList();                       
                        if (regions[0].Length > 0)
                        {
                            game.Regions = regions.Select(x => x.Replace(x, x.Split('#')[1].ToString())).ToList();                            
                        }

                        game.GameModes = sqlReader.GetString(3).Split(',').ToList();                      

                        gamesList.Add(game);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error while loading Games List into Combo box. " + ex.Message);
            }
           return gamesList;           
        }

        public IList<DetailViewModel> GetTableData(string gamecode, string regioncode, string gamemode)
        {
            try
            { 
            var resultList = new List<DetailViewModel>();
            var con = new SqlConnection(ConnectionString);
            con.Open();
            var command = new SqlCommand("select* from[dbo].[f_get_game_stats] (@gamecode,@regioncode,@gamemode)", con);
            command.Parameters.AddWithValue("@gamecode", gamecode);
            command.Parameters.AddWithValue("@regioncode", regioncode); 
            command.Parameters.AddWithValue("@gamemode", gamemode);

            using (var sqlReader = command.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    var detailViewModel = new DetailViewModel();
                    detailViewModel.Col1 = !sqlReader.IsDBNull(1) ? sqlReader.GetString(1) : null;

                   // detailViewModel.Col2 = !sqlReader.IsDBNull(2) ? sqlReader.GetString(2): null;

                    string userImage = sqlReader.GetString(2);
                    var directory = Path.GetDirectoryName(Application.ResourceAssembly.Location);
                    directory = directory + "\\images\\users\\";

                    if (File.Exists(directory + userImage))
                    {
                       detailViewModel.Col2 = directory + userImage;
                    }
                    else
                    {
                       detailViewModel.Col2 = directory + "user.png";
                    }

                    detailViewModel.Col3 = !sqlReader.IsDBNull(3) ? sqlReader.GetString(3) : null;
                    detailViewModel.Col4 = !sqlReader.IsDBNull(4) ? sqlReader.GetString(4) : null;
                    detailViewModel.Col5 = !sqlReader.IsDBNull(5) ? sqlReader.GetString(5) : null;
                    detailViewModel.Col6 = !sqlReader.IsDBNull(6) ? sqlReader.GetString(6) : null;
                    detailViewModel.Col7 = !sqlReader.IsDBNull(7) ? sqlReader.GetString(7) : null;
                    detailViewModel.Col8 = !sqlReader.IsDBNull(8) ? sqlReader.GetString(8) : null;
                    detailViewModel.Col9 = !sqlReader.IsDBNull(9) ? sqlReader.GetString(9) : null;
                    detailViewModel.Col10 = !sqlReader.IsDBNull(10) ? sqlReader.GetString(10) : null;
                    detailViewModel.Col11 = !sqlReader.IsDBNull(11) ? sqlReader.GetString(11) : null;
                    detailViewModel.Col12 = !sqlReader.IsDBNull(12) ? sqlReader.GetString(12) : null;
                    detailViewModel.Col13 = !sqlReader.IsDBNull(13) ? sqlReader.GetString(13) : null;
                    detailViewModel.Col14 = !sqlReader.IsDBNull(14) ? sqlReader.GetString(14) : null;
                    detailViewModel.Col15 = !sqlReader.IsDBNull(15) ? sqlReader.GetString(15) : null;
                    detailViewModel.Col16 = !sqlReader.IsDBNull(16) ? sqlReader.GetString(16) : null;
                    detailViewModel.Col17 = !sqlReader.IsDBNull(17) ? sqlReader.GetString(17) : null;
                    detailViewModel.Col18 = !sqlReader.IsDBNull(18) ? sqlReader.GetString(18) : null;
                    detailViewModel.Col19 = !sqlReader.IsDBNull(19) ? sqlReader.GetString(19) : null;
                    detailViewModel.Col20 = !sqlReader.IsDBNull(20) ? sqlReader.GetString(20) : null;
                    detailViewModel.Col21 = !sqlReader.IsDBNull(21) ? sqlReader.GetString(21) : null;
                    detailViewModel.Col22 = !sqlReader.IsDBNull(22) ? sqlReader.GetString(22) : null;
                    detailViewModel.Col23 = !sqlReader.IsDBNull(23) ? sqlReader.GetString(23) : null;
                    detailViewModel.Col24 = !sqlReader.IsDBNull(24) ? sqlReader.GetString(24) : null;
                    detailViewModel.Col25 = !sqlReader.IsDBNull(25) ? sqlReader.GetString(25) : null;
                    detailViewModel.Col26 = !sqlReader.IsDBNull(26) ? sqlReader.GetString(26) : null;
                    detailViewModel.Col27 = !sqlReader.IsDBNull(27) ? sqlReader.GetString(27) : null;
                    detailViewModel.Col28 = !sqlReader.IsDBNull(28) ? sqlReader.GetString(28) : null;
                    detailViewModel.Col29 = !sqlReader.IsDBNull(29) ? sqlReader.GetString(29) : null;
                    detailViewModel.Col30 = !sqlReader.IsDBNull(30) ? sqlReader.GetString(30) : null;


                    resultList.Add(detailViewModel);
                }
            }
            return resultList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading Games Details Grid. " + ex.Message);
            }
        }

    }
}
