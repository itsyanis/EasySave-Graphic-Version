using EasySave_3.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave_3.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _nagivationStore;  //Store the navigation
        public ViewModelBase CurrentViewModel => _nagivationStore.CurrentViewModel; //Current viewmodel loaded
        public MainViewModel(NavigationStore navigationStore)
        {
            _nagivationStore = navigationStore;
            _nagivationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        //Checks if the viewmodel has been changed
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
