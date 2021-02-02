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
    public class HomeViewModel : BaseViewModel
    {

        private List<NewsViewModel> _newsVM;

        public  List<NewsViewModel> NewsVM
        {
            get { 
                return _newsVM;
            }
            set 
            {
                _newsVM = value;
                OnPropertyRaised("NewsVM");
            }
        }

        private string _imagePath;

        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                _imagePath = value;
                OnPropertyRaised("ImagePath");
            }
        }

        public ICommand LoadNextAdv { get; set; }

        public ICommand LoadPrevAdv { get; set; }

        public ICommand SeeMoreNews { get; set; }


        int ctr = 1;
       
        private List<ImageDetail> _imageArray;

        public List<ImageDetail> ImageArray { 
            get
            {
                return _imageArray;
            }
            set
            {
                _imageArray = value;
                OnPropertyRaised("ImageArray");
            }
        }

        public HomeViewModel()
        {          

            var fileArray = Directory.GetFiles(@".\images\slideshow\", "*.png");
            ImageArray = fileArray.Select(x => new ImageDetail { FileName = x, IsLoaded = false }).ToList();

            LoadNextAdv = new CommandHandler(btnNext_Click);
            LoadPrevAdv = new CommandHandler(btnPrevious_Click);
            SeeMoreNews = new CommandHandler(btnTopLeftSeeMore_Click);

            PlaySlideShow(ctr);
            LoadNews(1, 5);

           // lbNews.ItemsSource = newsVM;
        }


        private void LoadNews(int from, int to)
        {
            DataAccess dataAccess = new DataAccess();
            NewsVM = dataAccess.LoadNews(from, to).ToList();
        }

        private void btnTopLeftSeeMore_Click()
        {
            LoadNews(5, 10);
        }


        private void PlaySlideShow(int ctr)
        {
            //BitmapImage image = new BitmapImage();
            //image.BeginInit();
            var item = ImageArray.ElementAt(ctr - 1);
            //foreach (var im in imageArray)
            //{
            //    im.IsLoaded = false;
            //}
            //item.IsLoaded = true;
            //image.UriSource = new Uri(item.FileName, UriKind.Relative);

            //image.EndInit();
            ImagePath = item.FileName;
            //myImage.Stretch = Stretch.Uniform;
        }

        private void btnPrevious_Click()
        {
            ctr--;
            if (ctr < 1)
            {
                ctr = ImageArray.Count - 1;
            }
            PlaySlideShow(ctr);
        }

        private void btnNext_Click()
        {
            ctr++;
            if (ctr > ImageArray.Count)
            {
                ctr = 1;
            }
            PlaySlideShow(ctr);
        }
}
}


