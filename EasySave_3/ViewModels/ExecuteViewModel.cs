using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EasySave_3.Commands;
using EasySave_3.Stores;

namespace EasySave_3.ViewModels
{
    public class ExecuteViewModel : ViewModelBase
    {
        public IEnumerable<BackupJobViewModel> BackupJobList;
        public ICommand ReturnCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand ResumeCommand { get; }
        public ICommand StopCommand { get; }

        public ExecuteViewModel(NavigationStore navigationStore, ObservableCollection<BackupJobViewModel> listBackup)
        {
            BackupJobList = listBackup;
            PauseCommand = new PauseSaveCommand();
            ResumeCommand = new ResumeSaveCommand();
            StopCommand = new StopSaveCommand(navigationStore);
            ReturnCommand = new NavigateManageBackupCommand(navigationStore);

            Action onCompleted = () =>
            {
                MessageBox.Show(Properties.strings.EVMSaveComplete, Properties.strings.EVMBoxTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                navigationStore.CurrentViewModel = new ManageBackupJobViewModel(navigationStore);
            };

            Thread thread = new Thread(() =>
            {

                try
                {
                    LaunchSave();
                }
                catch (Exception e)
                {
                    Trace.WriteLine("error" + e.ToString());
                }
                finally
                {
                    onCompleted();
                }
            });

            thread.Start();
        }

        public void LaunchSave()
        {
            Save Save = new Save();

            foreach (BackupJobViewModel item in BackupJobList)
            {
                if (item.SomeItemSelected)
                {
                    if (item.Type == "Complete")
                    {
                        Save.CompleteSave(item.BackupName, item.SourcePath, item.DestinationPath);
                    }
                    else
                    {
                        Save.DifferentialSave(item.BackupName, item.SourcePath, item.DestinationPath);
                    }
                }
            }
        }

        public void ExecuteAllBackups()
        {
            Save Save = new Save();
            Save.SaveAlljobs();
        }
    }
}
