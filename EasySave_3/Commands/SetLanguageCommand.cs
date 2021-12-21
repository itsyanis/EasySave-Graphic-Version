using EasySave_3.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace EasySave_3.Commands
{
    public class SetLanguageCommand : CommandBase
    {
        private NavigationStore _navigateStore;
        private FileDirectoryProcessing _fileDirectoryProcessing;
        private string _language;

        public SetLanguageCommand(NavigationStore navigateStore, string language)
        {
            _fileDirectoryProcessing = new FileDirectoryProcessing();
            _navigateStore = navigateStore;
            _language = language;
        }

        public override void Execute(object parameter)
        {
            FileDirectoryProcessing.SetLanguage(_language);
            switch (_language)
            {
                case "fr":
                    _fileDirectoryProcessing.Language();
                    MessageBox.Show(Properties.strings.SLCBoxText);
                    _navigateStore.CurrentViewModel = new ViewModels.MainMenuViewModel(_navigateStore);
                    break;
                case "en":
                    _fileDirectoryProcessing.Language();
                    MessageBox.Show(Properties.strings.SLCBoxText);
                    _navigateStore.CurrentViewModel = new ViewModels.MainMenuViewModel(_navigateStore);
                    break;
            }
            
            
        }
    }
}
