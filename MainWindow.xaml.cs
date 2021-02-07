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
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

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

        string cat_filter = string.Empty;
        string seachtext_filter = string.Empty;
        string listopt = string.Empty;

        DataTable dtCategory = new DataTable();
        DataSet dsGameList = new DataSet();

        BackgroundWorker bg = new BackgroundWorker();

        ArrayList ArrFilter = new ArrayList();
        ArrayList ArrCatName = new ArrayList();

        Common objcommon = new Common();

        int frcount = 1;
        int tocount = 10;

        public MainWindow()
        {
            InitializeComponent();
            leadersViemModel = new LeadersViemModel();
            homeViewModel = new HomeViewModel();
            mainWindowViewModel = new MainWindowViewModel();
            mainWindowViewModel.LoadedObject = homeViewModel;
            this.DataContext = mainWindowViewModel;
        }

        private void GameList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadUser();
                if (!bg.IsBusy)
                {
                    bg.RunWorkerAsync();

                    ButtonAutomationPeer peer = new ButtonAutomationPeer(btn_gamesLink);
                    IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProv.Invoke();
                }
            }
            catch (Exception ex)
            {
                objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }

        //}
        private void LoadUser()
        {
            try
            {
                var dt = new DataTable();
                var objdb = new gamelistDB();

                dt = objdb.Get_User_Details("VINAY");
                if (dt != null && dt.Rows.Count > 0)
                {
                    Properties.Settings.Default.UserName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dt.Rows[0]["USerName"].ToString().ToLower());
                    txtUserName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dt.Rows[0]["USerName"].ToString().ToLower());
                    txtAmount.Text = dt.Rows[0]["BalAmt"].ToString();
                    txtBalPoints.Text = dt.Rows[0]["BalPts"].ToString();
                    txtHours.Text = dt.Rows[0]["Balhrs"].ToString() + " HRs";
                    txtUrcount.Text = dt.Rows[0]["UrCount"].ToString();
                }
            }
            catch (Exception ex)
            {
                objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }

        private void LoadGameList(string gamename)
        {
            var objdb = new gamelistDB();
            try
            {
                String[] catTag = new String[3];
                dsGameList = objdb.Get_GameList(cat_filter, seachtext_filter, listopt, frcount, tocount);
                if (dsGameList != null && dsGameList.Tables[0].Rows.Count > 0)
                {
                    StackPanel sp_container = new StackPanel();
                    sp_container.Name = "sp_container_" + cat_filter;
                    sp_container.Orientation = Orientation.Vertical;
                    sp_container.HorizontalAlignment = HorizontalAlignment.Left;
                    sp_container.Margin = new Thickness(0, 10, 0, 0);

                    Grid gd_heading = new Grid();
                    ColumnDefinition c1 = new ColumnDefinition();
                    c1.Width = new GridLength(1, GridUnitType.Star);
                    ColumnDefinition c2 = new ColumnDefinition();
                    c2.Width = new GridLength(1, GridUnitType.Star);
                    gd_heading.ColumnDefinitions.Add(c1);
                    gd_heading.ColumnDefinitions.Add(c2);

                    TextBlock txtCatName = new TextBlock();
                    txtCatName.Margin = new Thickness(10, 0, 0, 0);
                    txtCatName.VerticalAlignment = VerticalAlignment.Center;
                    txtCatName.FontSize = 16;
                    txtCatName.FontWeight = FontWeights.Bold;
                    //txtCatName.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#8F8983"));
                    txtCatName.Style = (Style)FindResource("style_mainwindow_textblock_categoryname");
                    txtCatName.Text = gamename.ToUpper();

                    Grid.SetColumn(txtCatName, 0);
                    Grid.SetRow(txtCatName, 0);
                    gd_heading.Children.Add(txtCatName);

                    TextBlock txtShowMore = new TextBlock();
                    txtShowMore.Margin = new Thickness(0, 0, 20, 0);
                    txtShowMore.VerticalAlignment = VerticalAlignment.Center;
                    txtShowMore.HorizontalAlignment = HorizontalAlignment.Right;
                    txtShowMore.FontSize = 14;
                    //txtShowMore.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#8F8983"));
                    txtShowMore.Style = (Style)FindResource("style_mainwindow_textblock_showmore");
                    txtShowMore.Text = "More >";
                    txtShowMore.PreviewMouseUp += showmore_previewmousedown;
                    catTag[0] = tocount.ToString();
                    catTag[1] = cat_filter;
                    catTag[2] = gamename;
                    txtShowMore.Tag = catTag;
                    if (dsGameList.Tables[1].Rows.Count > 0)
                    {
                        txtShowMore.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtShowMore.Visibility = Visibility.Collapsed;
                    }

                    Grid.SetColumn(txtShowMore, 1);
                    Grid.SetRow(txtShowMore, 0);
                    gd_heading.Children.Add(txtShowMore);

                    sp_container.Children.Add(gd_heading);

                    int count = 0;

                    StackPanel sp_catgame = new StackPanel();
                    sp_catgame.Orientation = Orientation.Vertical;
                    sp_catgame.Margin = new Thickness(0, 20, 0, 0);

                    StackPanel sp_catgame_inner = new StackPanel();
                    sp_catgame_inner.Orientation = Orientation.Horizontal;

                    foreach (DataRow drr in dsGameList.Tables[0].Rows)
                    {
                        count = count + 1;

                        StackPanel sp_game = new StackPanel();
                        sp_game.Width = 200;
                        sp_game.Margin = new Thickness(10, 10, 2, 2);

                        Image img_game = new Image();
                        img_game.Height = 230;
                        img_game.Width = 180;
                        try
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(drr["ImgFIle"].ToString());
                            bitmap.EndInit();
                            img_game.Source = bitmap;
                        }
                        catch (Exception)
                        {
                            img_game.Source = null;
                        }

                        TextBlock txtgamename = new TextBlock();
                        //txtgamename.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#999"));
                        txtgamename.Style = (Style)FindResource("style_mainwindow_textblock_gamename");
                        txtgamename.FontSize = 12;
                        txtgamename.HorizontalAlignment = HorizontalAlignment.Center;
                        txtgamename.Margin = new Thickness(10);
                        txtgamename.TextAlignment = TextAlignment.Left;
                        txtgamename.TextWrapping = TextWrapping.Wrap;
                        txtgamename.Text = drr["GameName"].ToString();

                        sp_game.Children.Add(img_game);
                        sp_game.Children.Add(txtgamename);


                        sp_catgame_inner.Children.Add(sp_game);

                        if (count >= 5)
                        {
                            count = 0;
                            sp_catgame.Children.Add(sp_catgame_inner);
                            sp_container.Children.Add(sp_catgame);

                            sp_catgame_inner = new StackPanel();
                            sp_catgame_inner.Orientation = Orientation.Horizontal;

                            sp_catgame = new StackPanel();
                            sp_catgame.Orientation = Orientation.Vertical;
                            sp_catgame.Margin = new Thickness(0, 20, 0, 0);
                        }
                    }
                    if (count != 0)
                    {
                        sp_catgame.Children.Add(sp_catgame_inner);
                    }
                    sp_container.Children.Add(sp_catgame);
                    sp_gamelist.Children.Add(sp_container);
                }
            }
            catch (Exception ex)
            {
                objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }

        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (dtCategory != null && dtCategory.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCategory.Rows)
                    {
                        Button objbtn = new Button();
                        objbtn.Style = (Style)FindResource("style_mainwindow_category_filter");
                        ViewAddElement.Child = objbtn;

                        var objborder = (Border)Helpers.FindVisualElement(ViewAddElement, "category_filter_btn_border");
                        var objtb = (TextBlock)Helpers.FindVisualElement(ViewAddElement, "category_filter_btn_textblock");

                        objborder.Tag = "GLC_tb_" + dr["GametypCode"].ToString();
                        objborder.PreviewMouseDown += cat_fliter_click_PreviewMouseDown;

                        objtb.Name = "GLC_tb_" + dr["GametypCode"].ToString();
                        objtb.Text = dr["GametypName"].ToString().ToUpper();

                        ViewAddElement.Child = null;
                        wp_fliter_items.Children.Add(objbtn);

                        cat_filter = dr["GametypCode"].ToString();
                        listopt = "CAT";

                        LoadGameList(dr["GametypName"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }

        private void cat_fliter_click_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var obj = (Border)sender;
            var objtag = obj.Tag.ToString();
            try
            {
                var objtb = (TextBlock)Helpers.FindVisualElement(obj, objtag);
                if (obj.BorderBrush == Brushes.White)
                {
                    //obj.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#595959"));
                    obj.BorderBrush = (SolidColorBrush)FindResource("color_bg_category_filter");
                    objtb.Foreground = Brushes.Black;
                    DoRemoveFilterOption("Remove", objtag.Replace("GLC_tb_", ""), objtb.Text);
                }
                else
                {
                    obj.BorderBrush = Brushes.White;
                    objtb.Foreground = Brushes.White;
                    DoRemoveFilterOption("Add", objtag.Replace("GLC_tb_", ""), objtb.Text);
                }

            }
            catch (Exception ex)
            {
                objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }

        private void DoRemoveFilterOption(string action, string val, string catname)
        {
            try
            {
                if (action == "Add")
                {
                    ArrFilter.Add(val);
                    ArrCatName.Add(catname);
                }
                else
                {
                    ArrFilter.Remove(val);
                    ArrCatName.Remove(catname);
                }
                //cat_filter = string.Join(",", ArrFilter.ToArray());
                listopt = "CAT";
                sp_gamelist.Children.Clear();
                if (ArrFilter != null && ArrFilter.Count > 0)
                {
                    for (int i = 0; i < ArrFilter.Count; i++)
                    {
                        cat_filter = ArrFilter[i].ToString();
                        LoadGameList(ArrCatName[i].ToString());
                    }
                }
                else
                {
                    if (!bg.IsBusy)
                    {
                        //objloading.Show();
                        bg.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }



        private void showmore_previewmousedown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var objdb = new gamelistDB();

                var obj = (TextBlock)sender;
                var objVal = (string[])obj.Tag;
                var increasedCount = Convert.ToInt32(objVal[0]) + 10;
                var catFilterVal = objVal[1];
                var catName = objVal[2];

                dsGameList = objdb.Get_GameList(catFilterVal, seachtext_filter, listopt, frcount, increasedCount);

                if (dsGameList != null && dsGameList.Tables[0].Rows.Count > 0)
                {
                    String[] catTag = new String[3];

                    var objSpcontainer = (StackPanel)Helpers.FindVisualElement(sp_gamelist, "sp_container_" + catFilterVal);
                    objSpcontainer.Children.Clear();

                    Grid gd_heading = new Grid();
                    ColumnDefinition c1 = new ColumnDefinition();
                    c1.Width = new GridLength(1, GridUnitType.Star);
                    ColumnDefinition c2 = new ColumnDefinition();
                    c2.Width = new GridLength(1, GridUnitType.Star);
                    gd_heading.ColumnDefinitions.Add(c1);
                    gd_heading.ColumnDefinitions.Add(c2);

                    TextBlock txtCatName = new TextBlock();
                    txtCatName.Margin = new Thickness(10, 0, 0, 0);
                    txtCatName.VerticalAlignment = VerticalAlignment.Center;
                    txtCatName.FontSize = 16;
                    txtCatName.FontWeight = FontWeights.Bold;
                    //txtCatName.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#8F8983"));
                    txtCatName.Style = (Style)FindResource("style_mainwindow_textblock_categoryname");
                    txtCatName.Text = catName.ToUpper();

                    Grid.SetColumn(txtCatName, 0);
                    Grid.SetRow(txtCatName, 0);
                    gd_heading.Children.Add(txtCatName);

                    TextBlock txtShowMore = new TextBlock();
                    txtShowMore.Margin = new Thickness(0, 0, 20, 0);
                    txtShowMore.VerticalAlignment = VerticalAlignment.Center;
                    txtShowMore.HorizontalAlignment = HorizontalAlignment.Right;
                    txtShowMore.FontSize = 14;
                    //txtShowMore.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#8F8983"));
                    txtShowMore.Style = (Style)FindResource("style_mainwindow_textblock_showmore");
                    txtShowMore.Text = "More >";
                    txtShowMore.PreviewMouseUp += showmore_previewmousedown;
                    catTag[0] = increasedCount.ToString();
                    catTag[1] = catFilterVal;
                    catTag[2] = catName.ToString();
                    txtShowMore.Tag = catTag;

                    if (dsGameList.Tables[1].Rows.Count > 0)
                    {
                        txtShowMore.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtShowMore.Visibility = Visibility.Collapsed;
                    }

                    Grid.SetColumn(txtShowMore, 1);
                    Grid.SetRow(txtShowMore, 0);
                    gd_heading.Children.Add(txtShowMore);

                    objSpcontainer.Children.Add(gd_heading);

                    int count = 0;

                    StackPanel sp_catgame = new StackPanel();
                    sp_catgame.Orientation = Orientation.Vertical;
                    sp_catgame.Margin = new Thickness(0, 20, 0, 0);

                    StackPanel sp_catgame_inner = new StackPanel();
                    sp_catgame_inner.Orientation = Orientation.Horizontal;

                    foreach (DataRow drr in dsGameList.Tables[0].Rows)
                    {
                        count = count + 1;

                        StackPanel sp_game = new StackPanel();
                        sp_game.Width = 200;
                        sp_game.Margin = new Thickness(10, 10, 2, 2);

                        Image img_game = new Image();
                        img_game.Height = 230;
                        img_game.Width = 180;
                        try
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(drr["ImgFIle"].ToString());
                            bitmap.EndInit();
                            img_game.Source = bitmap;
                        }
                        catch (Exception)
                        {
                            img_game.Source = null;
                        }

                        TextBlock txtgamename = new TextBlock();
                        //txtgamename.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#999"));
                        txtgamename.Style = (Style)FindResource("style_mainwindow_textblock_gamename");
                        txtgamename.FontSize = 12;
                        txtgamename.HorizontalAlignment = HorizontalAlignment.Center;
                        txtgamename.Margin = new Thickness(10);
                        txtgamename.TextAlignment = TextAlignment.Left;
                        txtgamename.TextWrapping = TextWrapping.Wrap;
                        txtgamename.Text = drr["GameName"].ToString();

                        sp_game.Children.Add(img_game);
                        sp_game.Children.Add(txtgamename);


                        sp_catgame_inner.Children.Add(sp_game);

                        if (count >= 5)
                        {
                            count = 0;
                            sp_catgame.Children.Add(sp_catgame_inner);
                            objSpcontainer.Children.Add(sp_catgame);

                            sp_catgame_inner = new StackPanel();
                            sp_catgame_inner.Orientation = Orientation.Horizontal;

                            sp_catgame = new StackPanel();
                            sp_catgame.Orientation = Orientation.Vertical;
                            sp_catgame.Margin = new Thickness(0, 20, 0, 0);
                        }
                    }
                    if (count != 0)
                    {
                        sp_catgame.Children.Add(sp_catgame_inner);
                    }
                    objSpcontainer.Children.Add(sp_catgame);
                }
            }
            catch (Exception ex)
            {
                objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
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
                ShowContent("games");
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

        private void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var objdb = new gamelistDB();
                dtCategory = objdb.Get_Game_CatList();
            }
            catch (Exception ex)
            {
                objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
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
                else if(name  == "games")
                {
                    bg.DoWork += new DoWorkEventHandler(bg_DoWork);
                    bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
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

        private void sp_fliter_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sp_fliter_items.Visibility == Visibility.Visible)
                    sp_fliter_items.Visibility = Visibility.Collapsed;
                else
                    sp_fliter_items.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }


        private void txtsearchgame_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                seachtext_filter = txtsearchgame.Text;
                listopt = "CAT";
                sp_gamelist.Children.Clear();
                if (ArrFilter != null && ArrFilter.Count > 0)
                {
                    for (int i = 0; i < ArrFilter.Count; i++)
                    {
                        cat_filter = ArrFilter[i].ToString();
                        LoadGameList(ArrCatName[i].ToString());
                    }
                }
                else
                {
                    if (!bg.IsBusy)
                    {
                        //objloading.Show();
                        bg.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
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

