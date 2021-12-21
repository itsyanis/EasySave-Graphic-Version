using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave_3.ViewModels
{
    public class BackupJobViewModel : ViewModelBase
    {
        //New backup job model used by ViewModels
        //Makes sure the new object gets the corresponding value
        private readonly BackupJob _backupJob;
        public string BackupName => _backupJob.BackupName;
        public string SourcePath => _backupJob.SourcePath;
        public string DestinationPath => _backupJob.DestinationPath;
        public string Type => _backupJob.Type;
        public bool SomeItemSelected { get; set; }

        public BackupJobViewModel(BackupJob backupJob)
        {
            _backupJob = backupJob;
        }
    }
}
