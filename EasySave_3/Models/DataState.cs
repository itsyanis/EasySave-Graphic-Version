using System;


namespace EasySave_3
{
    // DataState derives from Data
    class DataState : Data

    {
        // Properties, Setters & Getters
        public bool State { get; private set; }
        public int TotalFilesToCopy { get; private set; }
        public long TotalFilesSize { get; private set; }
        public int NbFilesLeftToDo { get; private set; }
        public string Timestamp { get; set; }

        public int Progression { get; private set; }


        // Constructor
        public DataState(string BackupName, string SourcePath, string DestinationPath, bool State, int TotalFilesToCopy, long TotalFilesSize, int NbFilesLeftToDo, string Timestamp, int Progression)
        {
            this.BackupName = BackupName;
            this.SourcePath = SourcePath;
            this.DestinationPath = DestinationPath;
            this.State = State;
            this.Timestamp = Timestamp;
            this.TotalFilesSize = TotalFilesSize;
            this.Progression = 100;

            if (State == true)
            {
                this.TotalFilesToCopy = TotalFilesToCopy;
                this.NbFilesLeftToDo = NbFilesLeftToDo;
                this.Progression = Progression;
            }
        }

    }
}
