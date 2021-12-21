using EasySave_3.Stores;
using EasySave_3.ViewModels;
using System;
using System.Text;
using System.Windows;

namespace EasySave_3.Commands
{
    public class SetTypeLogCommand : CommandBase
    {
        NavigationStore _navigationStore;
        private string _logType;

        public SetTypeLogCommand(NavigationStore navigationStore, string logType)
        {
            _navigationStore = navigationStore;
            _logType = logType;
        }

        public override void Execute(object parameter)
        {
            FileDirectoryProcessing.ChangeLogFileType(_logType);
            switch (_logType)
            {
                case "json":
                    MessageBox.Show(Properties.strings.STLCBoxJson);
                    break;
                case "xml":
                    MessageBox.Show(Properties.strings.STLCBoxXml);
                    break;
            }
            _navigationStore.CurrentViewModel = new SettingsViewModel(_navigationStore);
        }
    }
}
