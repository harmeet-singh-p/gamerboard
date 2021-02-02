using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.IO;
using Path = System.IO.Path;

namespace GameProj
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    /// 
    public class ImageDetail : INotifyPropertyChanged
    {
        bool isLoaded;
        public string FileName { get; set; }

        public bool IsLoaded
        {
            get
            {
                return isLoaded;
            }
            set
            {
                isLoaded = value;
                OnPropertyRaised("IsLoaded");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }

    public partial class MainPage : Page
    {
        List<NewsViewModel> newsVM = new List<NewsViewModel>();
        int ctr = 0;
        List<ImageDetail> imageArray;


        public MainPage()
        {
            InitializeComponent();
            ctr = 1;
            var fileArray = Directory.GetFiles(@".\images\slideshow\", "*.jpeg");
            imageArray = fileArray.Select(x => new ImageDetail { FileName = x, IsLoaded = false }).ToList();
            listBoxDots.ItemsSource = imageArray;
            PlaySlideShow(ctr);
            LoadNews(1,5);

            lbNews.ItemsSource = newsVM;
        }     


        private void LoadNews(int from, int to)
        {
            DataAccess dataAccess = new DataAccess();
            newsVM = dataAccess.LoadNews(from,to).ToList();            
        }

        private void btnTopLeftSeeMore_Click(object sender, RoutedEventArgs e)
        {
            LoadNews(1, 5);
        }


        private void PlaySlideShow(int ctr)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            var item = imageArray.ElementAt(ctr - 1);
            foreach (var im in imageArray)
            {
                im.IsLoaded = false;
            }
            item.IsLoaded = true;
            image.UriSource = new Uri(item.FileName, UriKind.Relative);

            image.EndInit();
            myImage.Source = image;
            myImage.Stretch = Stretch.Uniform;

        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            ctr--;
            if (ctr < 1)
            {
                ctr = imageArray.Count - 1;
            }
            PlaySlideShow(ctr);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            ctr++;
            if (ctr > imageArray.Count)
            {
                ctr = 1;
            }
            PlaySlideShow(ctr);
        }

    }
}
