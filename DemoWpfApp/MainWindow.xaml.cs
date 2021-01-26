using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;

namespace DemoWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //int frcount = 1;
        //int tocount = 10;

        string cat_filter = string.Empty;
        string seachtext_filter = string.Empty;
        string listopt = string.Empty;

        DataTable dtCategory = new DataTable();
        DataSet dsGameList = new DataSet();
        DataSet dsRecentlyPlayed = new DataSet();
        DataSet dsNewList = new DataSet();

        BackgroundWorker bg = new BackgroundWorker();

        ArrayList ArrFilter = new ArrayList();
        ArrayList ArrCatName = new ArrayList();

        Common objcommon = new Common();

        List<UserViewModel> usersVM = new List<UserViewModel>();
                

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                LoadUser();
                lbUsers.ItemsSource = usersVM;
            }
            catch (Exception ex)
            {
               // objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }

        private void LoadUser()
        {
            try
            {
                usersVM.Add(new UserViewModel { CenterRanking = "1587", LoyaltyPoints = "62", UserName = "User1" });
                usersVM.Add(new UserViewModel { CenterRanking = "1546", LoyaltyPoints = "85", UserName = "User2" });
                usersVM.Add(new UserViewModel { CenterRanking = "8945", LoyaltyPoints = "83", UserName = "User3" });
                usersVM.Add(new UserViewModel { CenterRanking = "1598", LoyaltyPoints = "99", UserName = "User4" });
                usersVM.Add(new UserViewModel { CenterRanking = "5555", LoyaltyPoints = "26", UserName = "User5" });
                usersVM.Add(new UserViewModel { CenterRanking = "7848", LoyaltyPoints = "78", UserName = "User6" });                
            }
            catch (Exception ex)
            {
                //objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }

        private void btnLogout_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
               // var objconfirm = new ConfirmBox();
              //  objconfirm.ShowDialog();
            }
            catch (Exception ex)
            {
               // objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }

        private void ResetMenuButton()
        {
            try
            {
                btn_index.Style = (Style)FindResource("style_mainwindow_button_index_sidemenu");
                btn_gamesLink.Style = (Style)FindResource("style_mainwindow_button_second_sidemenu");
                btn_three.Style = (Style)FindResource("style_mainwindow_button_third_sidemenu");
                btn_four.Style = (Style)FindResource("style_mainwindow_button_fourth_sidemenu");
                btn_five.Style = (Style)FindResource("style_mainwindow_button_fifth_sidemenu");
                btn_six.Style = (Style)FindResource("style_mainwindow_button_sixth_sidemenu");
                btn_seven.Style = (Style)FindResource("style_mainwindow_button_seventh_sidemenu");
                btn_eight.Style = (Style)FindResource("style_mainwindow_button_eighth_sidemenu");

                btn_index.Tag = "";
                btn_gamesLink.Tag = "";
                btn_three.Tag = "";
                btn_four.Tag = "";
                btn_five.Tag = "";
                btn_six.Tag = "";
                btn_seven.Tag = "";
                btn_eight.Tag = "";
            }
            catch (Exception ex)
            {
               // objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }
        private void ShowContent(string name)
        {
            try
            {
                if (name == "main")
                {
                    //sv_content.Visibility = Visibility.Visible;
                    //sv_dummy.Visibility = Visibility.Collapsed;
                }
                else
                {
                    //sv_content.Visibility = Visibility.Collapsed;
                    //sv_dummy.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                //objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }

        private void btn_index_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetMenuButton();
                ShowContent("");
                var obj = (Button)sender;
                if (obj.Tag.ToString() == "clicked")
                {
                    obj.Tag = "";
                    var style = (Style)FindResource("style_mainwindow_button_index_sidemenu");
                    obj.Style = style;
                }
                else
                {
                    obj.Tag = "clicked";
                    var style = (Style)FindResource("style_mainwindow_button_index_sidemenu_clicked");
                    obj.Style = style;
                }

            }
            catch (Exception ex)
            {
              //  objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }
        private void btn_gamesLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetMenuButton();
                ShowContent("main");
                var obj = (Button)sender;
                if (obj.Tag.ToString() == "clicked")
                {
                    obj.Tag = "";
                    var style = (Style)FindResource("style_mainwindow_button_second_sidemenu");
                    obj.Style = style;
                }
                else
                {
                    obj.Tag = "clicked";
                    var style = (Style)FindResource("style_mainwindow_button_second_sidemenu_clicked");
                    obj.Style = style;
                }

            }
            catch (Exception ex)
            {
               // objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }
        private void btn_three_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetMenuButton();
                ShowContent("");
                var obj = (Button)sender;
                if (obj.Tag.ToString() == "clicked")
                {
                    obj.Tag = "";
                    var style = (Style)FindResource("style_mainwindow_button_third_sidemenu");
                    obj.Style = style;
                }
                else
                {
                    obj.Tag = "clicked";
                    var style = (Style)FindResource("style_mainwindow_button_third_sidemenu_clicked");
                    obj.Style = style;
                }

            }
            catch (Exception ex)
            {
              //  objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }
        private void btn_four_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetMenuButton();
                ShowContent("");
                var obj = (Button)sender;
                if (obj.Tag.ToString() == "clicked")
                {
                    obj.Tag = "";
                    var style = (Style)FindResource("style_mainwindow_button_fourth_sidemenu");
                    obj.Style = style;
                }
                else
                {
                    obj.Tag = "clicked";
                    var style = (Style)FindResource("style_mainwindow_button_fourth_sidemenu_clicked");
                    obj.Style = style;
                }

            }
            catch (Exception ex)
            {
               // objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }
        private void btn_five_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetMenuButton();
                ShowContent("");
                var obj = (Button)sender;
                if (obj.Tag.ToString() == "clicked")
                {
                    obj.Tag = "";
                    var style = (Style)FindResource("style_mainwindow_button_fifth_sidemenu");
                    obj.Style = style;
                }
                else
                {
                    obj.Tag = "clicked";
                    var style = (Style)FindResource("style_mainwindow_button_fifth_sidemenu_clicked");
                    obj.Style = style;
                }

            }
            catch (Exception ex)
            {
               // objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }
        private void btn_six_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetMenuButton();
                ShowContent("");
                var obj = (Button)sender;
                if (obj.Tag.ToString() == "clicked")
                {
                    obj.Tag = "";
                    var style = (Style)FindResource("style_mainwindow_button_sixth_sidemenu");
                    obj.Style = style;
                }
                else
                {
                    obj.Tag = "clicked";
                    var style = (Style)FindResource("style_mainwindow_button_sixth_sidemenu_clicked");
                    obj.Style = style;
                }

            }
            catch (Exception ex)
            {
              //  objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }

        private void btn_seven_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetMenuButton();
                ShowContent("");
                var obj = (Button)sender;
                if (obj.Tag.ToString() == "clicked")
                {
                    obj.Tag = "";
                    var style = (Style)FindResource("style_mainwindow_button_seventh_sidemenu");
                    obj.Style = style;
                }
                else
                {
                    obj.Tag = "clicked";
                    var style = (Style)FindResource("style_mainwindow_button_seventh_sidemenu_clicked");
                    obj.Style = style;
                }

            }
            catch (Exception ex)
            {
              //  objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }

        private void btn_eight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetMenuButton();
                ShowContent("");
                var obj = (Button)sender;
                if (obj.Tag.ToString() == "clicked")
                {
                    obj.Tag = "";
                    var style = (Style)FindResource("style_mainwindow_button_eighth_sidemenu");
                    obj.Style = style;
                }
                else
                {
                    obj.Tag = "clicked";
                    var style = (Style)FindResource("style_mainwindow_button_eighth_sidemenu_clicked");
                    obj.Style = style;
                }

            }
            catch (Exception ex)
            {
              //  objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }

    }  
    
}
