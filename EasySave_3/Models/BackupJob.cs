using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace EasySave_3
{
    // BackupJob derives from Data
    public class BackupJob : Data
    {
        // Properties, Setters & Getters
        public string Type { get; set; }

        // Constructor
        public BackupJob() { }
        public BackupJob(string BackupNameEntry, string SourcePathEntry, string DestinationPathEntry, string TypeEntry)
        {
            BackupName = BackupNameEntry;
            SourcePath = SourcePathEntry;
            DestinationPath = DestinationPathEntry;
            Type = TypeEntry;
        }

        // This method will return a specific backupJob 
        public string GetSpecificJob(string BackupName)
        {
            string[] All_Lines = File.ReadAllLines(ConfigurationManager.AppSettings.Get("BackupFile"));    // get all content of backupFile 

            foreach (string line in All_Lines)                                   // Loop line by line 
            {
                var backupJob = (JObject)JsonConvert.DeserializeObject(line);         // Deserialize the each line 

                string name = backupJob["BackupName"].Value<string>();               // Extract the backup name from each line

                if (name == BackupName)                                             // Compare if the name is the same with the backupName introduced by user 
                {
                    return line;
                }
            }
            return null;
        }


        // This method will return all backupJob 
        public static List<BackupJob> ShowAllJobs()
        {
            string BackupFile = @"C:\EasySave\Backup.json";   // Get the Backup file path
            List<BackupJob> BackupList = new List<BackupJob>();

            if (new FileInfo(BackupFile).Length != 0)                 // Check if file is not empty
            {
                string[] All_Lines = File.ReadAllLines(BackupFile);    // get all content of backupFile 

                foreach (string line in All_Lines)
                {
                    BackupJob backupJob = JsonConvert.DeserializeObject<BackupJob>(line);         // Deserialize the each line 
                    BackupList.Add(backupJob);
                }
            }
            else     //In case of error
            {
            }

            return BackupList;
        }

        public void DeleteSpecificJob(string BackupName)
        {
            string[] All_Lines = File.ReadAllLines(ConfigurationManager.AppSettings.Get("BackupFile"));    // get all content of backupFile 

            File.Delete(ConfigurationManager.AppSettings.Get("BackupFile"));

            using (StreamWriter Backup = new StreamWriter(ConfigurationManager.AppSettings.Get("BackupFile")))
            {
                foreach (string line in All_Lines)
                {
                    var backupJob = (JObject)JsonConvert.DeserializeObject(line);         // Deserialize the each line 

                    string name = backupJob["BackupName"].Value<string>();               // Extract the backup name from each line

                    if (name != BackupName)                                             // Compare if the name is the same with the backupName introduced by user 
                    {
                        Backup.WriteLine(line);
                    }
                }
            }
        }
    }
}
