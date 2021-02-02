using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProj
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        object loadedObject;
        public object LoadedObject
        {
            get
            {
                return loadedObject;
            }
            set
            {
                loadedObject = value;
                OnPropertyChanged("LoadedObject");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
