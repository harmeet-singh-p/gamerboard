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
        DataAccess dataAccess;

        private List<NewsViewModel> _newsVM;

        private IList<ChallengeModel> _challengeModel;

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

        public IList<ChallengeModel> Challenges
        {
            get
            {
                return _challengeModel;
            }
            set
            {
                _challengeModel = value;
                OnPropertyRaised("Challenges");
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
            dataAccess = new DataAccess();
            var fileArray = Directory.GetFiles(@".\images\slideshow\");
            ImageArray = fileArray.Select(x => new ImageDetail { FileName = x, IsLoaded = false }).ToList();

            LoadNextAdv = new CommandHandler(btnNext_Click);
            LoadPrevAdv = new CommandHandler(btnPrevious_Click);
            SeeMoreNews = new CommandHandler(btnSeeMore_Click);

            PlaySlideShow(ctr);
            LoadNews(1, 5);
            LoadChallenges(1, 5);
           // lbNews.ItemsSource = newsVM;
        }

        private void LoadChallenges(int from, int to)
        {
            Challenges = dataAccess.GetChallenges("0170", 1, 5);
        }

        private void LoadNews(int from, int to)
        {            
            NewsVM = dataAccess.LoadNews(from, to).ToList();
        }

        private void btnSeeMore_Click()
        {
            LoadNews(5, 10);
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


