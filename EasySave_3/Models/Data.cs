using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace EasySave_3
{
    public class Data
    {
        // Properties, Setters & Getters
        public string BackupName { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }

        static readonly object _object = new object();


        // Default Constructor  
        public Data()
        {
            this.BackupName = BackupName;
            this.SourcePath = SourcePath;
            this.DestinationPath = DestinationPath;
        }


        // Constructor
        public Data(string BackupNameEntry, string SourcePathEntry, string DestinationPathEntry)
        {
            this.BackupName = BackupNameEntry;
            this.SourcePath = SourcePathEntry;
            this.DestinationPath = DestinationPathEntry;
        }

        // This method will create the 'File' if it hasn't been created, and replenish it with the informations
        public void WriteOnFile(string Path, object Informations)
        {
            string JsonInformations = JsonConvert.SerializeObject(Informations);     // Convert DataLog informations to JSON 

              Monitor.Enter(_object);

            try
            {
               
                if (File.Exists(Path) == true)                                              // Check if 'File' exist
                {
                    using (StreamWriter logFile = File.AppendText(Path))                    // If the 'File' exist just append the JSON informations
                    {
                        logFile.WriteLine(JsonInformations);
                    }
                }
                else
                {
                    using (StreamWriter logFile = File.CreateText(Path))                     // If the 'File' doesn't exist we create it and put ON Json informations
                    {
                        logFile.WriteLine(JsonInformations);
                    }
                }
            }
            catch (Exception e)
            {
            }
           
            Monitor.Exit(_object);
        }

    }
}

