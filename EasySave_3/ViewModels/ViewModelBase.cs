using System.ComponentModel;

namespace EasySave_3.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;   //Event to know if a property has changed

        protected void OnPropertyChanged(string propertyName)   //Function called to check if the property has changed
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
