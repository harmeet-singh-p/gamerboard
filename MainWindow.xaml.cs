using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Documents;
using System.Globalization;

namespace GameProj
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///  

    public partial class MainWindow : Window
    {
        MainWindowViewModel mainWindowViewModel;
        LeadersViemModel leadersViemModel;
        HomeViewModel homeViewModel;

        public MainWindow()
        {
            InitializeComponent();
            leadersViemModel = new LeadersViemModel();
            homeViewModel = new HomeViewModel();
            mainWindowViewModel = new MainWindowViewModel();
            mainWindowViewModel.LoadedObject = homeViewModel;
            this.DataContext = mainWindowViewModel;
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



        private void btn_index_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetMenuButton();
                ShowContent("main");
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
                mainWindowViewModel.LoadedObject = leadersViemModel;
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

        private void ShowContent(string name)
        {
            try
            {
                if (name == "main")
                {
                    mainWindowViewModel.LoadedObject = homeViewModel;
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

    }
}

