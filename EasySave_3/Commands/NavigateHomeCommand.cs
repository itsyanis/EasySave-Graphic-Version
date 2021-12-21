using EasySave_3.Stores;
using EasySave_3.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave_3.Commands
{
    public class NavigateHomeCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public NavigateHomeCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = new MainMenuViewModel(_navigationStore);
        }
    }
}
