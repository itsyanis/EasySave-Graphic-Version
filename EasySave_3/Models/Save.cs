using EasySave_3.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EasySave_3
{
    class Save
    {
        private string StateFile = ConfigurationManager.AppSettings.Get("StateFile");         // Get State File Path from App.config
        private string SettingsFile = ConfigurationManager.AppSettings.Get("Settings");       // Get Settings File Path from App.config
        private string CryptoSoft = ConfigurationManager.AppSettings.Get("CryptoSoft");

        private Stopwatch TransferTime = new Stopwatch();                                              // Initialize the StopWatch timer for copy
        private Stopwatch EncryptionTime = new Stopwatch();                                           // Initialize the StopWatch timer for Encryption
        public static bool Stop { get; set; }

        private static Mutex Mutx = new Mutex();

        public static ManualResetEvent Wait_handle = new ManualResetEvent(true);



        public void Pause()
        {
            Wait_handle.Reset();
            Trace.WriteLine("******* Save PAUSED ********");
        }

        public void Resume()
        {
            Wait_handle.Set();
            Trace.WriteLine("******* Save RESUMED *******");
        }

        public void Stop_Save()
        {
            Stop = true;
            Trace.WriteLine("******* Save STOPED *******");
            MessageBox.Show("Save Stoped");
        }


        public void CompleteSave(string BackupJobName, string SourcePath, string DestinationPath)
        {
            FileDirectoryProcessing processing = new FileDirectoryProcessing();
            List<Task> TaskList = new List<Task>();

            if (Directory.Exists(SourcePath) && !processing.IsDirectoryEmpty(SourcePath))                // Check if source path exist and isn't empty
            {
                List<string> Files = Directory.GetFiles(SourcePath).ToList<string>();                   // Get all files contained in the source Path (put them in List)

                int NbrFiles = Directory.GetFiles(SourcePath).Length;                                  // Get number of Files to copy
                int NbFilesLeftToDo = NbrFiles;
                long TotalFileSize = processing.GetDirectorySize(SourcePath);
                int Progression = 100;

                string SettingLine = File.ReadLines(SettingsFile).First();                                 // Get the setting line
                var Setting = (JObject)JsonConvert.DeserializeObject(SettingLine);                        // Deserialize the setting line 

                string Extensions = Setting["Extensions"].ToString();                                     // Get extensions of files to be Crypted defined by user in settings
                string FilesPriority = Setting["FilePriority"].ToString();                               // Get extensions of the priority files defined by user in settings
                long MaxFileSize = Setting["MaxFileSize"].Value<long>();                                // Get the Max File Size defined by user in settings

                
                // Is user defined a priority file, we sort files of sourcePath
                if (FilesPriority.Length != 0)
                {
                    Files = processing.SortFiles(Files, FilesPriority);
                }

              
                // Thread 
                Thread thread = new Thread(() => {

                        foreach (string File in Files)                                                    // Loop file by file 
                        {
                            Wait_handle.WaitOne();

                            // Extract all informations about file (name, size, extention ...)
                            string FileName = Path.GetFileName(File);                                    // Get file Name
                            string FileExtension = Path.GetExtension(File);                             //  Get file extensions
                            long Size = new FileInfo(File).Length;                                     //   Get size

                            string DestinationFile = Path.Combine(DestinationPath, FileName);        // Combine the destination path with the file name  
                            bool State = false;
                            string CopyType = "Parallele";

                            // If Extension File exists on the extension Settings, we encrypt the file
                            if (Extensions.Length != 0 && Extensions.Contains(FileExtension))
                            {
                                EncryptionTime.Reset();
                                Process Process = ProcessManagement.Init_CryptoSoft(File, DestinationFile);

                                // If the File Size > Max filesize defined in Settings, we do sequential save
                                if (Size >= MaxFileSize)
                                {
                                    CopyType = "Sequential";
                                    DataState StateInformations = new DataState(BackupJobName, File, DestinationFile, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), Progression);
                                    SequentialSave(File, DestinationFile, StateInformations, true, Process);             // Encryp File + Copy (Sequential)

                                }
                                else
                                {
                                    DataState StateInformations = new DataState(BackupJobName, File, DestinationFile, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), Progression);
                                    ParallelSave(TaskList, File, DestinationFile, StateInformations, true, Process);   // Encryp File + Copy (Parellele)
                                }
                            }
                            else                                 // Else, we just copy it on destination
                            {
                                if (Size >= MaxFileSize)
                                {
                                    CopyType = "Sequential";
                                    DataState StateInformations = new DataState(BackupJobName, File, DestinationFile, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), Progression);
                                    SequentialSave(File, DestinationFile, StateInformations, false, null);              //  Copy (Sequential)
                                }
                                else
                                {
                                    DataState StateInformations = new DataState(BackupJobName, File, DestinationFile, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), Progression);
                                    ParallelSave(TaskList, File, DestinationFile, StateInformations, false, null);    // Copy (Parellele)
                                }
                            }

                            // Progression = (Files.Length - NbrFiles) / 100;

                            DataLog LogInformations = new DataLog(BackupJobName, File, DestinationFile, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), Size, TransferTime.Elapsed, CopyType, EncryptionTime.Elapsed);
                            string LogPath = FileDirectoryProcessing.GetLogFilePath();

                            if (FileDirectoryProcessing.GetLogFileType() == "json")
                            {
                                LogInformations.WriteOnFileLogJson(LogPath);                  // Put Log informations on Log File
                            }
                            else
                            {
                                LogInformations.WriteOnFileXml(LogPath, BackupJobName, File, DestinationFile, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), Size, TransferTime.Elapsed, EncryptionTime.Elapsed);               // Put Log informations on Log File
                            }


                           /*Socket socket;

                            socket = Client_Socket.SeConnecter();

                            Client_Socket.EcouterReseau(socket, File);
                            Client_Socket.Deconnecter(socket);*/


                            NbFilesLeftToDo--;

                            if (Stop)
                            {
                                Trace.WriteLine("********** JE STOPPP ******");
                                return;
                            }
                        }

                        //Task.WaitAll(TaskList.ToArray());

                        if (Directory.GetDirectories(SourcePath).Length > 0)      // Check if source path contain Sub Directory
                        {
                            // In case the source path contains SubDirectory

                            string[] Directories = Directory.GetDirectories(SourcePath);   // Get all directories contained in the source Path (put them in array)

                            foreach (string Dir in Directories)
                            {
                                // Extract all informations about directory (name, size ...)
                                DirectoryInfo DirInfo = new DirectoryInfo(Dir);
                                string DirectoryName = DirInfo.Name;
                                long DirectorySize = processing.GetDirectorySize(Dir);

                                string DestinationDirectory = Path.Combine(DestinationPath, DirectoryName);

                                TransferTime.Reset();                                                   // Start the transfer time

                                try
                                {
                                    Directory.CreateDirectory(DestinationDirectory);   // Create the directory


                                }
                                catch (Exception e)
                                {

                                }

                                TaskList.Clear();
                                CompleteSave(BackupJobName, Dir, DestinationDirectory);                                 // Recursivity : We call the method for sub sub ... directory and their files.
                            }
                        }
                });


                // 'Pause' the complete Save if a specific software is running
                var Softwares = Setting["Softwares"].ToString();

                Process[] ProcessCollection = Process.GetProcesses();

                foreach (Process P in ProcessCollection)
                {
                    if (Softwares.Contains(P.ProcessName))
                    {
                        while (Process.GetProcessesByName(P.ProcessName).Length > 0)
                        {
                            Trace.WriteLine("Sotware : " + P.ProcessName + "is open, can't save...");
                            Mutx.WaitOne();
                        }

                        Mutx.ReleaseMutex();
                    }
                }

                Wait_handle.WaitOne();
                thread.Start();
                thread.Join();
            }
        }


        public void DifferentialSave(string BackupJobName, string SourcePath, string DestinationPath)
        {
            FileDirectoryProcessing processing = new FileDirectoryProcessing();
            List<Task> TaskList = new List<Task>();

            if (Directory.Exists(SourcePath) == true && !processing.IsDirectoryEmpty(SourcePath))         // Check if source path is a Directory and isn't empty
            {
                int NbrFiles = Directory.GetFiles(SourcePath).Length;                 // Get nbr of Files to copy
                int NbFilesLeftToDo = NbrFiles;
                long TotalFileSize = processing.GetDirectorySize(SourcePath);
                Stopwatch TransferTime = new Stopwatch();                                              // Initialize the StopWatch timer 
                Stopwatch EncryptionTime = new Stopwatch();                                           // Initialize the StopWatch timer for Encryption

                string SettingLine = File.ReadLines(SettingsFile).First();                            // Get the setting line
                var Setting = (JObject)JsonConvert.DeserializeObject(SettingLine);                    // Deserialize the setting line 

                string Extensions = Setting["Extensions"].ToString();
                string FilesPriority = Setting["FilePriority"].ToString();                               // Get extensions of the priority files defined by user in settings
                long MaxFileSize = Setting["MaxFileSize"].Value<long>();                                // Get the Max File Size defined by user in settings
                                                                                                        // Get extensions of files to be Crypted

                if (Directory.GetFiles(SourcePath).Length > 0)                                          // Check if source path contain files
                {
                    if (Directory.GetFiles(DestinationPath).Length > 0)                             // Check if destination path contain files
                    {
                        List<string> SourceFiles = Directory.GetFiles(SourcePath).ToList<string>();                     // Get all source files
                        List<string> DestinationFiles = Directory.GetFiles(DestinationPath).ToList<string>();          // Get all destination files

                        // Is user defined a priority file, we sort files of sourcePath
                        if (FilesPriority.Length != 0)
                        {
                            SourceFiles = processing.SortFiles(SourceFiles, FilesPriority);
                        }

                        Thread thread = new Thread(() => {

                            Wait_handle.WaitOne();

                            foreach (string SourceFile in SourceFiles)
                            {
                                string SourceFileName = Path.GetFileName(SourceFile);                       // Extract name and source file size 
                                long SourceFileLength = new FileInfo(SourceFile).Length;

                                bool Find = false;
                                bool State = false;
                                int Progression = 0;
                                string CopyType = "Parallele";

                                foreach (string DestinationFile in DestinationFiles)
                                {
                                    string DestinationFileName = Path.GetFileName(DestinationFile);         // Extract name and destination file size
                                    long DestinationFilelength = new FileInfo(DestinationFile).Length;

                                    if (string.Compare(SourceFileName, DestinationFileName) == 0)           // Comparing
                                    {
                                        Find = true;                                                // We find one source file into destination

                                        string FileExtension = Path.GetExtension(SourceFile);

                                        if (SourceFileLength != DestinationFilelength)   // compare by sizing 
                                        {
                                            // If Extension File exists on the extension Settings, we encrypt the file
                                            if (Extensions.Length != 0 && Extensions.Contains(FileExtension))
                                            {
                                                Process Process = ProcessManagement.Init_CryptoSoft(SourceFile, DestinationFile);

                                                // If the File Size > Max filesize defined in Settings, we do sequential save
                                                if (SourceFileLength >= MaxFileSize)
                                                {
                                                    CopyType = "Sequential";
                                                    DataState StateInformations = new DataState(BackupJobName, SourceFile, DestinationFile, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), Progression);
                                                    SequentialSave(SourceFile, DestinationFile, StateInformations, true, Process);             // Encryp File + Copy (Sequential)
                                                }
                                                else
                                                {
                                                    DataState StateInformations = new DataState(BackupJobName, SourceFile, DestinationFile, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), Progression);
                                                    ParallelSave(TaskList, SourceFile, DestinationFile, StateInformations, true, Process);   // Encryp File + Copy (Parellele)
                                                }

                                            }
                                            else                                 // Else, we just copy it on destination
                                            {
                                                if (SourceFileLength >= MaxFileSize)
                                                {
                                                    CopyType = "Sequential";
                                                    DataState StateInformations = new DataState(BackupJobName, SourceFile, DestinationFile, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), Progression);
                                                    SequentialSave(SourceFile, DestinationFile, StateInformations, false, null);              //  Copy (Sequential)
                                                }
                                                else
                                                {
                                                    DataState StateInformations = new DataState(BackupJobName, SourceFile, DestinationFile, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), Progression);
                                                    ParallelSave(TaskList, SourceFile, DestinationFile, StateInformations, false, null);    // Copy (Parellele)
                                                }
                                            }

                                            // Progression = (Files.Length - NbrFiles) / 100;
                                            Progression = 100;

                                            DataLog LogInformations = new DataLog(BackupJobName, SourceFile, DestinationFile, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), processing.GetFileSize(SourceFile), TransferTime.Elapsed, CopyType, EncryptionTime.Elapsed);
                                            string LogPath = FileDirectoryProcessing.GetLogFilePath();

                                            if (FileDirectoryProcessing.GetLogFileType() == "json")
                                            {
                                                LogInformations.WriteOnFileLogJson(LogPath);                // Put Log informations on Log File (.json)
                                            }
                                            else
                                            {
                                                LogInformations.WriteOnFileXml(LogPath, BackupJobName, SourceFile, DestinationFile, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), processing.GetFileSize(SourceFile), TransferTime.Elapsed, EncryptionTime.Elapsed);               // Put Log informations on Log File (.xml)
                                            }
                                            NbFilesLeftToDo--;
                                        }
                                    }
                                }

                                if (Find == false)  // Case if we didn't find source file into destination we need to copy it 
                                {
                                    string SrcName = Path.GetFileName(SourceFile);
                                    string destination = Path.Combine(DestinationPath, SrcName);

                                    CopyType = "Sequential";
                                    DataState StateInformations = new DataState(BackupJobName, SourceFile, destination, State, NbrFiles, TotalFileSize, NbFilesLeftToDo, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), Progression);
                                    SequentialSave(SourceFile, destination, StateInformations, false, null);              //  Copy (Sequential)

                                    DataLog LogInformations = new DataLog(BackupJobName, SourceFile, destination, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), processing.GetFileSize(SourceFile), TransferTime.Elapsed, CopyType, EncryptionTime.Elapsed);
                                    string LogPath = FileDirectoryProcessing.GetLogFilePath();

                                    if (FileDirectoryProcessing.GetLogFileType() == "json")
                                    {
                                        LogInformations.WriteOnFileLogJson(LogPath);                // Put Log informations on Log File (.json)
                                    }
                                    else
                                    {
                                        LogInformations.WriteOnFileXml(LogPath, BackupJobName, SourceFile, destination, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), processing.GetFileSize(SourceFile), TransferTime.Elapsed, EncryptionTime.Elapsed);               // Put Log informations on Log File (.xml)
                                    }
                                }
                            }
                        });

                        // 'Pause' the complete Save if a specific software is running
                        var Softwares = Setting["Softwares"].ToString();

                        Process[] ProcessCollection = Process.GetProcesses();

                        foreach (Process P in ProcessCollection)
                        {
                            if (Softwares.Contains(P.ProcessName))
                            {
                                while (Process.GetProcessesByName(P.ProcessName).Length > 0)
                                {
                                    Trace.WriteLine("Sotware : " + P.ProcessName + "is open, can't save...");
                                    Mutx.WaitOne();
                                }

                                Mutx.ReleaseMutex();
                            }
                        }

                        Wait_handle.WaitOne();
                        thread.Start();

                    }
                    else       // Case when destination path doesn't contain files, we copy all source files to destination
                    {
                        string[] SourceFiles = Directory.GetFiles(SourcePath);

                        foreach (string File in SourceFiles)
                        {
                            string SrcName = Path.GetFileName(File);
                            string Destination = Path.Combine(DestinationPath, SrcName);
                            System.IO.File.Copy(File, Destination, true);
                        }
                    }
                }


                if (Directory.GetDirectories(SourcePath).Length > 0)  // Check if the source Path contain sub directory 
                {
                    string[] SourceDirectories = Directory.GetDirectories(SourcePath);

                    foreach (string SubDirectory in SourceDirectories)
                    {
                        DirectoryInfo DirInfo = new DirectoryInfo(SubDirectory);
                        string DirName = DirInfo.Name;
                        string DestinationDir = Path.Combine(DestinationPath, DirName);

                        if (Directory.Exists(DestinationDir) == false)
                        {
                            try
                            {
                                Directory.CreateDirectory(DestinationDir);   // Create the directory 
                            }
                            catch (Exception e)
                            {
                            }
                        }

                        TaskList.Clear();
                        DifferentialSave(BackupJobName, SubDirectory, DestinationDir);                    // Do the recursivity for files and sub directory 
                    }
                }
            }
        }


        public void ParallelSave(List<Task> TaskList, string Source, string Destination, DataState StateInformations, bool Encrypt, Process Process)
        {
            TaskList.Add(Task.Run(() =>
            {
                Wait_handle.WaitOne();
                
                if (Encrypt == true)
                {
                    try
                    {
                        Parallel.Invoke(
                                            () => EncryptionTime.Start(),
                                            () => ProcessManagement.Launch_CryptoSoft(Process)       // Do the copy with true parametre for overwrite// Start the transfer time
                                        );

                        EncryptionTime.Stop();
                        StateInformations.WriteOnFile(this.StateFile, StateInformations);

                        Trace.WriteLine("\n File : " + Source + " ====> Copied");

                    }
                    catch (Exception e)
                    {
                    }
                }
                else
                {
                    try
                    {
                        Parallel.Invoke(
                                            () => TransferTime.Start(),
                                            () => File.Copy(Source, Destination, true),
                                            () => StateInformations.WriteOnFile(this.StateFile, StateInformations)
                                        );

                        TransferTime.Stop();


                    }
                    catch (Exception e)
                    {
                    }
                }
            }));
        }


        public void SequentialSave(string Source, string Destination, DataState StateInformations, bool Encryption, Process Process)
        {

            if (Encryption == true)
            {
                try
                {
                    Parallel.Invoke(
                                            () => EncryptionTime.Start(),
                                            () => ProcessManagement.Launch_CryptoSoft(Process),  // Do the copy with true parametre for overwrite// Start the transfer time
                                            () => StateInformations.WriteOnFile(this.StateFile, StateInformations)
                                        );

                    EncryptionTime.Stop();
                    Trace.WriteLine("\n File : " + Source + " Crypted");
                }
                catch (Exception e)
                {
                }
            }
            else
            {
                try
                {
                    Parallel.Invoke(
                                      () => TransferTime.Start(),
                                      () => File.Copy(Source, Destination, true),  // Do the copy with true parametre for overwrite// Start the transfer time
                                      () => StateInformations.WriteOnFile(this.StateFile, StateInformations)
                                    );

                    TransferTime.Stop();
                    Trace.WriteLine("\n File  : " + Source + " ====> Copied");
                }
                catch (Exception e)
                {
                }
            }

        }


        public void SpecificSave(string BackupJobName)
        {
            BackupJob Backup = new BackupJob();
            string Job = Backup.GetSpecificJob(BackupJobName);               // Get the specific jb from BackupFile

            var data = (JObject)JsonConvert.DeserializeObject(Job);         // Deseriaize the backup informations

            string Type = data["Type"].Value<string>();                    //  Get the Type, Source Path and Destination Path
            string SourcePath = data["SourcePath"].Value<string>();
            string DestinationPath = data["DestinationPath"].Value<string>();

            switch (Type)
            {
                case "Complete":
                    CompleteSave(BackupJobName, SourcePath, DestinationPath);
                    break;

                case "Differential":
                    DifferentialSave(BackupJobName, SourcePath, DestinationPath);
                    break;
            }
        }

        // This method will save all created Backup Jobs
        public void SaveAlljobs()
        {
            string[] All_Lines = File.ReadAllLines(ConfigurationManager.AppSettings.Get("BackupFile"));    // get all content of backupFile 

            foreach (string line in All_Lines)                                   // Loop line by line 
            {
                var backupJob = (JObject)JsonConvert.DeserializeObject(line);         // Deserialize the each line 

                string BackupName = backupJob["BackupName"].Value<string>();               // Extract the backup name from each line
                SpecificSave(BackupName);
            }
        }
    }
}



