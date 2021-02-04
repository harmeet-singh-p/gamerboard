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

        private List<MPGViewModel> _mPGamesVM;

        public List<MPGViewModel> MPGamesVM
        {
            get
            {
                return _mPGamesVM;
            }
            set
            {
                _mPGamesVM = value;
                OnPropertyRaised("MPGamesVM");
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

        public ICommand SeeMoreMPGames { get; set; }

        private Visibility _isSeeMoreNews;
        public Visibility IsSeeMoreNews {
            get
            {
                return _isSeeMoreNews;
            }
            set
            {
                _isSeeMoreNews = value;
                OnPropertyRaised("IsSeeMoreNews");
            }
        }
        
        private Visibility _isSeeMoreMPG;
        public Visibility IsSeeMoreMPG {
            get
            {
                return _isSeeMoreMPG;
            }
            set
            {
                _isSeeMoreMPG = value;
                OnPropertyRaised("IsSeeMoreMPG");
            }
        }


        int ctr = 1;

        int fromNews;
        int toNews;
        int fromMPG;
        int toMPG;
       
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

            var fileArray = Directory.GetFiles(@".\images\slideshow\");
            ImageArray = fileArray.Select(x => new ImageDetail { FileName = x, IsLoaded = false }).ToList();

            LoadNextAdv = new CommandHandler(btnNext_Click);
            LoadPrevAdv = new CommandHandler(btnPrevious_Click);
            SeeMoreNews = new CommandHandler(btnSeeMoreNews_Click);
            SeeMoreMPGames = new CommandHandler(btnSeeMoreMPGames_Click);

            fromNews = 1;
            toNews = 5;

            fromMPG = 1;
            toMPG = 6;

            PlaySlideShow(ctr);
            LoadNews(fromNews ,toNews);
            LoadMPG(fromMPG, toMPG);

            // lbNews.ItemsSource = newsVM;
        }

        private void LoadMPG(int fromMPG, int toMPG)
        {
            DataAccess dataAccess = new DataAccess();
            MPGamesVM = dataAccess.LoadMPGames("","", "MPLAY", fromMPG, toMPG).ToList();
        }

        private void LoadNews(int from, int to)
        {
            DataAccess dataAccess = new DataAccess();
            NewsVM = dataAccess.LoadNews(from, to).ToList();
        }

        private void btnSeeMoreMPGames_Click()
        {
            //fromMPG += 6;
            toMPG += 6;

            LoadMPG(fromMPG, toMPG);

            if (MPGamesVM.Count < toMPG)
            {
                IsSeeMoreMPG = Visibility.Hidden;
            }
        }

        private void btnSeeMoreNews_Click()
        {
            //fromNews += 5;
            toNews += 5;

            LoadNews(fromNews,toNews);

            if (NewsVM.Count < toNews)
            {
                IsSeeMoreNews = Visibility.Hidden;
            }            
        }


        private void PlaySlideShow(int ctr)
        {           
            var item = ImageArray.ElementAt(ctr - 1);
            foreach (var im in ImageArray)
            {
                im.IsLoaded = false;
            }
            item.IsLoaded = true;          
            ImagePath = item.FileName;            
        }

        private void btnPrevious_Click()
        {
            ctr--;
            if (ctr < 1)
            {
                ctr = ImageArray.Count;
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


