using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace EasySave_3.Commands
{
    public class StopSaveCommand : CommandBase
    {
        private Stores.NavigationStore _navigationStore;

        public StopSaveCommand(Stores.NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            Save Save = new Save();
            
            Save.Pause();
            MessageBoxResult Result = MessageBox.Show(Properties.strings.SSCBoxText, Properties.strings.SSCBoxTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (Result.ToString())
            {
                case "Yes":
                    Save.Stop_Save();
                    _navigationStore.CurrentViewModel = new ViewModels.ManageBackupJobViewModel(_navigationStore);
                    break;
                case "No":
                    Save.Resume();
                    break;
            }

            
        }
    }
}
