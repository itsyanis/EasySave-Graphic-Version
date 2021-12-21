using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace EasySave_3
{
    class FileDirectoryProcessing
    {

        // Properties, Setters & Getters

        public string ConfigDirectory { get; set; }
        public string BackupFile { get; set; }
        public string LogFile { get; set; }
        public string StateFile { get; set; }
        public string Settings { get; set; }

        // Default Constructor
        public FileDirectoryProcessing()
        {
            this.ConfigDirectory = ConfigurationManager.AppSettings.Get("EasySave");   // Get the Directory EsaySave Path from App.config
            this.BackupFile = ConfigurationManager.AppSettings.Get("BackupFile");      // Get the Backup File Path from App.config
            this.LogFile = ConfigurationManager.AppSettings.Get("LogFile");            // Get the Log File Path from App.config
            this.StateFile = ConfigurationManager.AppSettings.Get("StateFile");        // Get State File Path from App.config
            this.Settings = ConfigurationManager.AppSettings.Get("Settings");           // Get Settings File Path from App.config
        }


        // This method will create all configs File (EasySave, State, Log)
        public void GenerateConfigFiles()
        {
            this.CreateDirectory(ConfigDirectory);           // Create the EasySave directory that will contain log,state and backup file
            this.CreateFile(BackupFile);                    //  Create the Backup file
            this.CreateFile(LogFile);                      // Create the log file
            this.CreateFile(StateFile);                   //  Create the state files
            this.CreateSettingsFile(Settings);                     // Create the settings files
        }



        // This method will create a new file
        public void CreateFile(string Path)
        {
            //  Check if file already exists
            if (File.Exists(Path) == false)                  //  Check if file already exists,create the file if not
            {
                try
                {
                    // Try to create the file.
                    using (File.Create(Path)) { }         // Create the file 
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("An error occurred while creating a file, the path does not exist or is invalid ... " + e.ToString());
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }


        // This method will create a new directory
        public void CreateDirectory(string Path)
        {
            try
            {
                // Check if the directory exists.
                if (Directory.Exists(Path) == false)
                {
                    // Try to create the directory.
                    DirectoryInfo dir = Directory.CreateDirectory(Path);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred while creating a directory, the path does not exist or is invalid ...", e.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }



        // This method will check if the directory is empty
        public bool IsDirectoryEmpty(string SourcePath)
        {
            if (Directory.GetFileSystemEntries(SourcePath).Length == 0)
            {
                return true;
            }
            return false;
        }

        public List<string> SortFiles(List<string> Files, string FilesPriority)
        {
            List<string> FilesSorted = new List<string>();

            //Add first Priority Files
            foreach (string File in Files.ToArray())
            {
                string FileExtension = Path.GetExtension(File);

                if (FilesPriority.Contains(FileExtension))
                {
                    FilesSorted.Add(File);
                    Files.Remove(File);
                }
            }

            // Then add non-priority file
            foreach (string File in Files)
            {
                FilesSorted.Add(File);
            }

            return FilesSorted;
        }


        // Create the settings file
        public void CreateSettingsFile(string path)
        {
            if (!File.Exists(path)) //Checks if file exists
            {
                this.CreateFile(Settings);
            }

            if (new FileInfo(path).Length == 0)
            {
                Settings newJson = new Settings
                {
                    Language = "en",                      // Default language
                    Extensions = new List<string>(),
                    Softwares = new List<string>(),
                    MaxFileSize = 0,
                    FilePriority = new List<string>(),
                    LogFileType = "json"
                };


                JsonSerializer serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                string SettingsPath = ConfigurationManager.AppSettings.Get("Settings");

                using StreamWriter sw = new StreamWriter(SettingsPath);
                using JsonWriter writer = new JsonTextWriter(sw);
                serializer.Serialize(writer, newJson);

            }
        }

        //Write the new info on the settings file
        //If more options are added, they need to be included as a parameter
        public static void SetLanguage(string NewLanguage)
        {
            string SettingsPath = ConfigurationManager.AppSettings.Get("Settings");

            string SettingLine = File.ReadLines(SettingsPath).First();                   // Get the setting line

            var Setting = (JObject)JsonConvert.DeserializeObject(SettingLine);         // Deserialize the setting line 
            string Language = Setting["Language"].Value<string>();                    // Get the Language

            SettingLine = SettingLine.ReplaceFirst(Language, NewLanguage);

            File.WriteAllText(SettingsPath, SettingLine);

        }

        public static void SetLogFileType(string NewType)
        {
            string SettingsPath = ConfigurationManager.AppSettings.Get("Settings");

            string SettingLine = File.ReadLines(SettingsPath).First();                   // Get the setting line

            var Setting = (JObject)JsonConvert.DeserializeObject(SettingLine);         // Deserialize the setting line 
            string Type = Setting["LogFileType"].Value<string>();                    // Get the type

            SettingLine = SettingLine.Replace(Type, NewType);

            File.WriteAllText(SettingsPath, SettingLine);
        }


        // Write on Settings File List of extension added by user
        public static void SetExtensions(string NewExtension, bool remove)
        {
            string SettingsPath = ConfigurationManager.AppSettings.Get("Settings");

            string SettingLine = File.ReadLines(SettingsPath).First();                   // Get the setting line

            var Setting = (JObject)JsonConvert.DeserializeObject(SettingLine);         // Deserialize the setting line 

            string Language = Setting["Language"].Value<string>();
            string LogFileType = Setting["LogFileType"].Value<string>();
            long MaxFileSize = Setting["MaxFileSize"].Value<long>();


            List<string> Softwares = new List<string>();
            foreach (string Software in Setting["Softwares"])
            {
                Softwares.Add(Software);
            }

            List<string> FilesPriority = new List<string>();
            foreach (string FPriority in Setting["FilePriority"])
            {
                FilesPriority.Add(FPriority);
            }

            List<string> Extensions = new List<string>();                // Get the Language
            foreach (string Extension in Setting["Extensions"])
            {
                Extensions.Add(Extension);
            }
            switch (remove)
            {
                case false:
                    Extensions.Add(NewExtension);
                    break;
                case true:
                    Extensions.Remove(NewExtension);
                    break;

            }

            Settings newJson = new Settings
            {
                Language = Language,                      // Default language
                Extensions = Extensions,
                Softwares = Softwares,
                MaxFileSize = MaxFileSize,
                FilePriority = FilesPriority,
                LogFileType = LogFileType
            };


            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            using StreamWriter sw = new StreamWriter(SettingsPath);
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, newJson);
        }


        // Write on Settings File List of softwares added by user

        public static void SetSoftwares(string NewSoftware, bool remove)
        {
            string SettingsPath = ConfigurationManager.AppSettings.Get("Settings");

            string SettingLine = File.ReadLines(SettingsPath).First();                   // Get the setting line

            var Setting = (JObject)JsonConvert.DeserializeObject(SettingLine);         // Deserialize the setting line 

            string Language = Setting["Language"].Value<string>();
            string LogFileType = Setting["LogFileType"].Value<string>();
            long MaxFileSize = Setting["MaxFileSize"].Value<long>();

            List<string> Extensions = new List<string>();
            foreach (string Extension in Setting["Extensions"])
            {
                Extensions.Add(Extension);
            }

            List<string> FilesPriority = new List<string>();
            foreach (string FPriority in Setting["FilePriority"])
            {
                FilesPriority.Add(FPriority);
            }

            List<string> Softwares = new List<string>();                // Get the Language
            foreach(string Software in Setting["Softwares"])
            {
                Softwares.Add(Software);
            }
            switch (remove)
            {
                case false:
                    Softwares.Add(NewSoftware);
                    break;
                case true:
                    Softwares.Remove(NewSoftware);
                    break;
            }
            


            Settings newJson = new Settings
            {
                Language = Language,
                Extensions = Extensions,
                Softwares = Softwares,
                FilePriority = FilesPriority,
                MaxFileSize = MaxFileSize,
                LogFileType = LogFileType
            };


            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            using StreamWriter sw = new StreamWriter(SettingsPath);
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, newJson);
        }


        // Write on Settings File List of File extension Priority added by user

        public static void SetFilePriority(string FilePriority, bool remove)
        {
            string SettingsPath = ConfigurationManager.AppSettings.Get("Settings");

            string SettingLine = File.ReadLines(SettingsPath).First();                   // Get the setting line

            var Setting = (JObject)JsonConvert.DeserializeObject(SettingLine);         // Deserialize the setting line 

            string Language = Setting["Language"].Value<string>();
            string LogFileType = Setting["LogFileType"].Value<string>();
            long MaxFileSize = Setting["MaxFileSize"].Value<long>();


            List<string> Extensions = new List<string>();
            foreach (string Extension in Setting["Extensions"])
            {
                Extensions.Add(Extension);
            }

            List<string> Softwares = new List<string>();
            foreach (string Software in Setting["Softwares"])
            {
                Softwares.Add(Software);
            }

            List<string> PriorityFile = new List<string>();                // Get the Language
            foreach (string priorityFile in Setting["FilePriority"])
            {
                PriorityFile.Add(priorityFile);
            }
            switch (remove)
            {
                case false:
                    PriorityFile.Add(FilePriority);
                    break;
                case true:
                    PriorityFile.Remove(FilePriority);
                    break;
            }


            Settings newJson = new Settings
            {
                Language = Language,
                Extensions = Extensions,
                Softwares = Softwares,
                FilePriority = PriorityFile,
                MaxFileSize = MaxFileSize,
                LogFileType = LogFileType
            };

            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            using StreamWriter sw = new StreamWriter(SettingsPath);
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, newJson);
        }


        public static void SetMaxFile(long MaxFileSize)
        {
            string SettingsPath = ConfigurationManager.AppSettings.Get("Settings");

            string SettingLine = File.ReadLines(SettingsPath).First();                   // Get the setting line

            var Setting = (JObject)JsonConvert.DeserializeObject(SettingLine);         // Deserialize the setting line 

            string Language = Setting["Language"].Value<string>();
            string LogFileType = Setting["LogFileType"].Value<string>();

            List<string> Extensions = new List<string>();
            foreach (string Extension in Setting["Extensions"])
            {
                Extensions.Add(Extension);
            }

            List<string> FilesPriority = new List<string>();
            foreach (string FPriority in Setting["FilePriority"])
            {
                FilesPriority.Add(FPriority);
            }

            List<string> Softwares = new List<string>();
            foreach (string Software in Setting["Softwares"])
            {
                Softwares.Add(Software);
            }

            Settings newJson = new Settings
            {
                Language = Language,
                Extensions = Extensions,
                Softwares = Softwares,
                FilePriority = FilesPriority,
                MaxFileSize = MaxFileSize,
                LogFileType = LogFileType
            };


            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            using StreamWriter sw = new StreamWriter(SettingsPath);
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, newJson);

        }


        //Set language for the application
        public void Language()
        {
            string SettingFile = ConfigurationManager.AppSettings.Get("Settings");   // Get the settings file path
            string[] All_Lines = File.ReadAllLines(SettingFile);
            foreach (string line in All_Lines)
            {
                var jsonLangue = (JObject)JsonConvert.DeserializeObject(line);         // Deserialize the each line
                switch (jsonLangue["Language"].ToString())                              //Check which language is set in the file
                {
                    case "en":
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");        //Set the language to english
                        break;
                    case "fr":
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr-FR");        //Set the language to french
                        break;
                }
            }
        }

        public static List<string> ReadSettings(string type)
        {
            string SettingFile = ConfigurationManager.AppSettings.Get("Settings");
            string SettingLine = File.ReadLines(SettingFile).First();
            var Setting = (JObject)JsonConvert.DeserializeObject(SettingLine);

            List<string> TypeToGetList = new List<string>();
            foreach(string TypeGet in Setting[type])
            {
                TypeToGetList.Add(TypeGet);
            }
            return TypeToGetList;
        }

        public static string GetFileSize()
        {
            string SettingFile = ConfigurationManager.AppSettings.Get("Settings");
            string SettingLine = File.ReadLines(SettingFile).First();
            var Setting = (JObject)JsonConvert.DeserializeObject(SettingLine);
            var filesize = Setting["MaxFileSize"];
            return filesize.ToString();
        }

        public static void ChangeLogFileType(string NewType)
        {

            string LogContentJson;
            string LogContentXml;
            switch (NewType)
            {
                case "json":
                    LogContentXml = File.ReadAllText(ConfigurationManager.AppSettings.Get("LogfileXml"));
                    XmlDocument toJson = new XmlDocument();
                    toJson.LoadXml(LogContentXml);
                    XmlElement xmlElement = toJson.DocumentElement;
                    string json = JsonConvert.SerializeXmlNode(xmlElement, Newtonsoft.Json.Formatting.Indented, omitRootObject: true);
                    using (StreamWriter logFile = File.CreateText(@"C:\EasySave\Log.json"))
                    {
                        logFile.Write(json);
                    }
                    break;
                case "xml":
                    LogContentJson = File.ReadAllText(ConfigurationManager.AppSettings.Get("Logfile"));
                    if (File.Exists(@"C:\EasySave\Log.xml") == true)
                    {
                        XmlDocument toXml = (XmlDocument)JsonConvert.DeserializeXmlNode(LogContentJson, "root");
                        using (StreamWriter logFile = File.CreateText(@"C:\EasySave\Log.xml"))
                        {
                            logFile.Write(toXml.OuterXml);
                        }
                    }
                    else
                    {
                        XmlDocument toXml = (XmlDocument)JsonConvert.DeserializeXmlNode(LogContentJson, "root");
                        using (StreamWriter logFile = File.CreateText(@"C:\EasySave\Log.xml"))
                        {
                            logFile.Write(toXml.OuterXml);
                        }
                    }

                    break;
            }

        }
        public static string GetLogFileType()
        {
            string SettingFile = ConfigurationManager.AppSettings.Get("Settings");   // Get the settings file path
            string[] All_Lines = File.ReadAllLines(SettingFile);
            foreach (string line in All_Lines)
            {
                var jsonLangue = (JObject)JsonConvert.DeserializeObject(line);         // Deserialize the each line
                switch (jsonLangue["LogFileType"].ToString())                              //Check which type is set in the file
                {
                    case "json":
                        return "json";

                    case "xml":
                        return "xml";
                }
            }
            return "json";
        }

        public static string GetLogFilePath()
        {
            string LogPath;
            if (FileDirectoryProcessing.GetLogFileType() == "json")
            {
                return LogPath = ConfigurationManager.AppSettings.Get("LogFile");
            }
            else
            {
                return LogPath = ConfigurationManager.AppSettings.Get("LogFileXml");
            }
        }


        // This method will return the Directory Size
        public long GetDirectorySize(string DirectoryPath)
        {
            string[] Files = Directory.GetFiles(DirectoryPath);    // Get all files contained in the directory (put them in array)

            long Size = 0;

            foreach (string File in Files)                        // Loop on the files into Directory 
            {
                FileInfo F = new FileInfo(File);                 // Use FileInfo to get length of each file.
                Size += F.Length;
            }

            return Size;                                        // return the result directory size
        }


        // This method will return the file name
        public string GetFileName(string FilePath)
        {
            return Path.GetFileName(FilePath);
        }


        // This method will return the file size
        public int GetFileSize(string FilePath)
        {
            return FilePath.Length;
        }

        // This method will return the file extension
        public static string GetFileExtension(string FilePath)
        {
            return Path.GetExtension(FilePath);
        }

    }

    public static partial class Bonus
    {
        /// <summary>
        ///     A string extension method that replace first occurence.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>The string with the first occurence of old value replace by new value.</returns>
        public static string ReplaceFirst(this string @this, string oldValue, string newValue)
        {
            int startindex = @this.IndexOf(oldValue);

            if (startindex == -1)
            {
                return @this;
            }

            return @this.Remove(startindex, oldValue.Length).Insert(startindex, newValue);
        }

        /// <summary>
        ///     A string extension method that replace first number of occurences.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="number">Number of.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>The string with the numbers of occurences of old value replace by new value.</returns>
        public static string ReplaceFirst(this string @this, int number, string oldValue, string newValue)
        {
            List<string> list = @this.Split(oldValue).ToList();
            int old = number + 1;
            IEnumerable<string> listStart = list.Take(old);
            IEnumerable<string> listEnd = list.Skip(old);

            return string.Join(newValue, listStart) +
                   (listEnd.Any() ? oldValue : "") +
                   string.Join(oldValue, listEnd);
        }
    }
}
