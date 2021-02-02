using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace GameProj
{
    /// <summary>
    /// Interaction logic for LeadersUserControl.xaml
    /// </summary>
    /// 
    

    public partial class LeadersUserControl : UserControl
    {
        string cat_filter = string.Empty;
        string seachtext_filter = string.Empty;
        string listopt = string.Empty;

        DataTable dtCategory = new DataTable();
        DataSet dsGameList = new DataSet();
        DataSet dsRecentlyPlayed = new DataSet();
        DataSet dsNewList = new DataSet();
        string gameCode = string.Empty;
        BackgroundWorker bg = new BackgroundWorker();

        ArrayList ArrFilter = new ArrayList();
        ArrayList ArrCatName = new ArrayList();

        Common objcommon = new Common();

        IList<UserViewModel> usersVM = new List<UserViewModel>();
        IList<GamesList> gamesList;

        DataAccess dataAccess = new DataAccess();
        IList<BindedTableInfo> bindedTableInfoList;
        List<Hyperlink> lstHyperlink = new List<Hyperlink>();

        public LeadersUserControl()
        {
            try
            {
                InitializeComponent();
                LoadUser();
                lbUsers.ItemsSource = usersVM;

                gamesList = dataAccess.LoadGamesList();
                cbGames.ItemsSource = gamesList;
                cbGames.DisplayMemberPath = "GameName";
                cbGames.SelectedValuePath = "GameCode";
                cbGames.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }


        StackPanel stackPanelTabs = new StackPanel();
        string strRegion = "";
        string gameMode = "";

        private void cbRegions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // var gameName = (sender as ComboBox).SelectedItem as string;
            strRegion = cbRegions.SelectedValue == null ? "" : cbRegions.SelectedValue.ToString();
            gameCode = string.Empty;

            foreach (var item in lstHyperlink)
            {
                if (item.Tag as string != "All")
                {
                    item.TextDecorations = null;
                }
                else
                {
                    item.TextDecorations = TextDecorations.Underline;
                }
            }

            var list = dataAccess.GetTableData(gameCode, strRegion, string.IsNullOrEmpty(gameMode) ? "All" : gameMode);
            if (list.Count > 0)
            {
                InitializeBannerGrid(list.First());
                InitializeTableGrid(list);
            }
            else
            {
                dg.ItemsSource = null;
                dg.Columns.Clear();

                bannerGrid.DataContext = null;
                bannerGrid.Children.Clear();
                bannerGrid.RowDefinitions.Clear();
                bannerGrid.ColumnDefinitions.Clear();

                Points.Text = null;
                PlayerName.Text = null;
                Center.Text = null;

                //MessageBox.Show("No Data to show for selected value of Game, Region and  Game Mode.");
            }
        }


        private void cbGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gameName = ((sender as ComboBox).SelectedItem as GamesList).GameName as string;
            //change banner background image.
            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();

            var directory = Path.GetDirectoryName(Application.ResourceAssembly.Location);

            if (File.Exists(directory + "\\images\\games\\" + gameName.ToLower().Replace(" ", "") + ".png"))
            {
                image.Source = new BitmapImage(new Uri(directory + @"\images\games\" + gameName.ToLower().Replace(" ", "") + ".png"));
            }
            else
            {
                image.Source = new BitmapImage(new Uri(directory + @"\images\games\fortnite.png"));
            }

            myBrush.ImageSource = image.Source;
            grid_bannerGrid.Background = myBrush;

            //game banner logo image

            if (File.Exists(directory + @"\images\gamelogos\" + gameName.ToLower().Replace(" ", "") + "_logo.png"))
            {
                var uri = new Uri(directory + @"\images\gamelogos\" + gameName.ToLower().Replace(" ", "") + "_logo.png");
                var bitmap = new BitmapImage(uri);
                gameLogo.Source = bitmap;
            }
            else
            {
                gameLogo.Source = new BitmapImage(new Uri(directory + @"\images\gamelogos\callofduty_logo.png"));
            }


            //load combobox region.
            gameCode = (sender as ComboBox).SelectedValue as string;
            var selectedGame = gamesList.FirstOrDefault(x => x.GameCode == gameCode);

            //combo box regions loading
            if (selectedGame.Regions != null)
            {
                cbRegions.ItemsSource = selectedGame.Regions;
                cbRegions.SelectedIndex = 0;
                cbRegions.Visibility = Visibility.Visible;
            }
            else
            {
                cbRegions.Visibility = Visibility.Hidden;
            }

            //initializing the banner and  details grid.              
            bindedTableInfoList = dataAccess.GetTableInfos(gameCode);
            lstHyperlink.Clear();
            if (selectedGame.GameModes != null && !string.IsNullOrEmpty(selectedGame.GameModes[0]))
            {
                stackPanelTabs.Children.Clear();
                stackPanelTabs.Orientation = Orientation.Horizontal;
                stackPanelTabs.Margin = new Thickness(30, 10, 0, 5);
                Grid.SetRow(stackPanelTabs, 2);
                foreach (var item in selectedGame.GameModes)
                {
                    Run run = new Run(item);
                    var hyperLink = new Hyperlink(run);
                    Color color = (Color)ColorConverter.ConvertFromString("#58FFFF");
                    hyperLink.Foreground = new SolidColorBrush(color);
                    hyperLink.Click += HyperLink_Click;
                    hyperLink.Tag = item;
                    if (item != "All")
                    {
                        hyperLink.TextDecorations = null;
                    }

                    lstHyperlink.Add(hyperLink);

                    var textBlock = new TextBlock(hyperLink);
                    textBlock.Margin = new Thickness(0, 0, 10, 0);
                    stackPanelTabs.Children.Add(textBlock);
                }

                if (!grid_gameList.Children.Contains(stackPanelTabs))
                {
                    grid_gameList.Children.Add(stackPanelTabs);
                }
            }
            else
            {
                stackPanelTabs.Children.Clear();
            }

            gameMode = string.Empty;
            //var list = dataAccess.GetTableData(gameCode, cbRegions.SelectedValue == null ? "" : cbRegions.SelectedValue.ToString() , tabGameMode.SelectedValue == null ? "" : tabGameMode.SelectedValue.ToString());
            var list = dataAccess.GetTableData(gameCode, cbRegions.SelectedValue == null ? "" : cbRegions.SelectedValue.ToString(), string.IsNullOrEmpty(gameMode) ? "All" : gameMode);

            if (list.Count > 0)
            {
                InitializeBannerGrid(list.First());
                InitializeTableGrid(list);
            }
            else
            {
                dg.ItemsSource = null;
                dg.Columns.Clear();

                bannerGrid.DataContext = null;
                bannerGrid.Children.Clear();
                bannerGrid.RowDefinitions.Clear();
                bannerGrid.ColumnDefinitions.Clear();

                Points.Text = null;
                PlayerName.Text = null;
                Center.Text = null;

                //MessageBox.Show("No Data to show for selected value of Game, Region and  Game Mode.");
            }
        }

        private void HyperLink_Click(object sender, RoutedEventArgs e)
        {
            gameMode = (sender as Hyperlink).Tag as string;


            foreach (var item in lstHyperlink)
            {
                item.TextDecorations = null;
            }

            (sender as Hyperlink).TextDecorations = TextDecorations.Underline;

            var list = dataAccess.GetTableData(gameCode, strRegion, string.IsNullOrEmpty(gameMode) ? "All" : gameMode);
            if (list.Count > 0)
            {
                InitializeBannerGrid(list.First());
                InitializeTableGrid(list);
            }
            else
            {
                dg.ItemsSource = null;
                dg.Columns.Clear();

                bannerGrid.DataContext = null;
                bannerGrid.Children.Clear();
                bannerGrid.RowDefinitions.Clear();
                bannerGrid.ColumnDefinitions.Clear();

                Points.Text = null;
                PlayerName.Text = null;
                Center.Text = null;

                //MessageBox.Show("No Data to show for selected value of Game, Region and  Game Mode.");
            }
        }

        private void HandleEvents(string gameCode, string Region, string gameMode)
        {
            var list = dataAccess.GetTableData(gameCode, cbRegions.SelectedValue == null ? "" : cbRegions.SelectedValue.ToString(), gameMode);
            if (list.Count > 0)
            {
                InitializeBannerGrid(list.First());
                InitializeTableGrid(list);
            }
            else
            {
                dg.ItemsSource = null;
                dg.Columns.Clear();

                bannerGrid.DataContext = null;
                bannerGrid.Children.Clear();
                bannerGrid.RowDefinitions.Clear();
                bannerGrid.ColumnDefinitions.Clear();

                Points.Text = null;
                PlayerName.Text = null;
                Center.Text = null;

                MessageBox.Show("No Data to show for selected value of Game, Region and  Game Mode.");
            }
        }

        private void InitializeTableGrid(IList<DetailViewModel> tableData)
        {
            dg.ItemsSource = tableData;
            var index = 0;
            dg.Columns.Clear();
            if (bindedTableInfoList.Count > 0)
            {
                foreach (var item in bindedTableInfoList)
                {
                    index++;
                    if (item.ColShowIn == "Both" || item.ColShowIn == "Table")
                    {
                        if (item.ColType == "Image")
                        {
                            DataGridTemplateColumn imgColumn = new DataGridTemplateColumn();
                            imgColumn.Header = item.ColDesc;

                            FrameworkElementFactory imageFactory = new FrameworkElementFactory(typeof(Image));

                            imageFactory.SetBinding(Image.SourceProperty, new Binding("Col" + index));

                            DataTemplate dataTemplate = new DataTemplate();
                            dataTemplate.VisualTree = imageFactory;

                            imgColumn.CellTemplate = dataTemplate;

                            dg.Columns.Add(imgColumn);
                        }
                        else
                        {
                            DataGridTemplateColumn txtBlockColumn = new DataGridTemplateColumn();
                            txtBlockColumn.Header = item.ColDesc;
                            txtBlockColumn.MinWidth = 50;
                            txtBlockColumn.IsReadOnly = true;
                            if (item.ColDesc == "Rank" || item.ColDesc == "Player Name" || item.ColDesc == "Center Name")
                            {
                                if (item.ColDesc == "Center Name")
                                {
                                    txtBlockColumn.CellStyle = FindResource("DataGridCellBoldKey") as Style;
                                }
                                else
                                    txtBlockColumn.CellStyle = FindResource("DataGridCellBigFontKey") as Style;
                            }
                            else
                                txtBlockColumn.CellStyle = FindResource("DataGridCellKey") as Style;


                            FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
                            textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("Col" + index));

                            DataTemplate dataTemplate = new DataTemplate();

                            dataTemplate.VisualTree = textBlockFactory;

                            txtBlockColumn.CellTemplate = dataTemplate;

                            dg.Columns.Add(txtBlockColumn);
                        }
                    }
                }
            }
        }

        private void InitializeBannerGrid(DetailViewModel detailViewModel)
        {
            try
            {
                PlayerName.Text = detailViewModel.Col3;
                Center.Text = detailViewModel.Col4;
                Points.Text = detailViewModel.Col11;
                var uri = new Uri(detailViewModel.Col2);
                var bitmap = new BitmapImage(uri);
                PlayerImage.Source = bitmap;

                if (bindedTableInfoList.Count > 0)
                {
                    var validDataForBanner = bindedTableInfoList.Where(x => x.ColShowIn == "Both" || x.ColShowIn == "Deatil");
                    var noOfRows = validDataForBanner.Where(x => x.DetPos.Contains(',')).Select(x => Convert.ToInt32(x.DetPos.Split(',').First())).Max();
                    var noOfColumns = validDataForBanner.Where(x => x.DetPos.Contains(',')).Select(x => Convert.ToInt32(x.DetPos.Split(',').Last())).Max();
                    bannerGrid.Children.Clear();
                    bannerGrid.RowDefinitions.Clear();
                    bannerGrid.ColumnDefinitions.Clear();
                    for (var i = 0; i < noOfRows; i++)
                    {
                        bannerGrid.RowDefinitions.Add(new RowDefinition());
                    }
                    for (var i = 0; i < noOfColumns; i++)
                    {
                        bannerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                    foreach (var item in validDataForBanner.Where(x => x.DetPos.Contains(',')))
                    {
                        var index = bindedTableInfoList.IndexOf(item);
                        var stackPanel = new StackPanel();
                        stackPanel.Orientation = Orientation.Vertical;
                        TextBlock textBlockHeader = new TextBlock();
                        textBlockHeader.Foreground = new SolidColorBrush(Colors.White);
                        textBlockHeader.Text = item.ColDesc;

                        stackPanel.Children.Add(textBlockHeader);
                        TextBlock textBlockValue = new TextBlock();
                        textBlockValue.Text = GetPropValue(detailViewModel, "Col" + (index + 1));
                        if (item.ColDesc == "Wins")
                        {
                            textBlockValue.Foreground = new SolidColorBrush(Colors.Yellow);
                        }
                        else if (item.ColDesc == "Losses")
                        {
                            textBlockValue.Foreground = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            textBlockValue.Foreground = new SolidColorBrush(Colors.White);
                        }

                        textBlockValue.FontSize = 18;

                        stackPanel.Children.Add(textBlockValue);
                        Grid.SetRow(stackPanel, Convert.ToInt32(item.DetPos.Split(',').First()) - 1);
                        Grid.SetColumn(stackPanel, Convert.ToInt32(item.DetPos.Split(',').Last()) - 1);
                        bannerGrid.Children.Add(stackPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while initializing the Banner details for game. Error : " + ex.Message);
            }
        }

        private string GetPropValue(object src, string propName)
        {
            return Convert.ToString(src.GetType().GetProperty(propName).GetValue(src, null));
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = dg.SelectedItem as DetailViewModel;
            if (row != null)
                InitializeBannerGrid(row);
            else
            {
                if (dg.Items != null && dg.Items.Count > 0)
                    InitializeBannerGrid(dg.Items[0] as DetailViewModel);
            }
        }

        private void dg_Sorting(object sender, DataGridSortingEventArgs e)
        {

        }

        private void txtsearchgame_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchBy = (sender as TextBox).Text;
            LoadUser(searchBy);
        }

        private void LoadUser(string searchBy = null)
        {
            try
            {
                if (!usersVM.Any())
                {
                    usersVM = dataAccess.GetUserInfos();
                }
                var filteredList = string.IsNullOrEmpty(searchBy) ? usersVM : usersVM.Where(x => x.UserName.StartsWith(searchBy, StringComparison.InvariantCultureIgnoreCase));
                var userList = new List<UserViewModel>();
                foreach (var item in filteredList)
                {
                    userList.Add(item);
                }
                lbUsers.ItemsSource = userList;

                //var dt = new DataTable();
                //var objdb = new gamelistDB();

                //dt = objdb.Get_User_Details("VINAY");
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //   // Properties.Settings.Default.UserName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dt.Rows[0]["USerName"].ToString().ToLower());
                //    txtUserName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dt.Rows[0]["USerName"].ToString().ToLower());
                //    txtAmount.Text = dt.Rows[0]["BalAmt"].ToString();
                //    txtBalPoints.Text = dt.Rows[0]["BalPts"].ToString();
                //    txtHours.Text = dt.Rows[0]["Balhrs"].ToString() + " HRs";
                //    txtUrcount.Text = dt.Rows[0]["UrCount"].ToString();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading  Users. Error :" + ex.Message);
                //objcommon.WritErrorLog("Gamelist.xaml", "ErrorLog.txt", ex.StackTrace, Properties.Settings.Default.UserName);
            }
        }      
        
    }
}
