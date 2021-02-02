namespace GameProj
{
    public class ImageDetail : BaseViewModel
    {
        private bool isLoaded;
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
       
    }
}


