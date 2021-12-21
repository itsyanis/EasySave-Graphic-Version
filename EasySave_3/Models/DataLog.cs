using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Linq;

namespace EasySave_3
{
    // DataLog derives from Data

    class DataLog : Data
    {
        // Properties, Setters & Getters
        public long Size { get; private set; }
        public string Timestamp { get; private set; }
        public TimeSpan TransferTime { get; private set; }
        public TimeSpan CryptedTime { get; private set; }

        public string CopyType { get; private set; }

        static readonly object _object = new object();



        // Constructor
        public DataLog(string BackupName, string SourcePath, string DestinationPath, string Timestamp, long Size, TimeSpan TransferTime, string CopyType, TimeSpan CryptedTime)
        {
            this.BackupName = BackupName;
            this.SourcePath = SourcePath;
            this.DestinationPath = DestinationPath;
            this.Timestamp = Timestamp;

            this.Size = Size;
            this.TransferTime = TransferTime;
            this.CopyType = CopyType;
            this.CryptedTime = CryptedTime;
        }

        //Write on the JSON file for Log
        public void WriteOnFileLogJson(string Path)
        {
            Monitor.Enter(_object);

            List<DataLog> listJson = new List<DataLog>();
            string json = File.ReadAllText(Path);
            var definition = new { File = listJson };


            if (json != "")
            {
                var jsonLog = JsonConvert.DeserializeAnonymousType(json, definition);
                jsonLog.File.Add(this);
                var Log = new { File = jsonLog.File };
                json = JsonConvert.SerializeObject(Log, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(Path, json);
            }
            else
            {
                listJson.Add(this);
                var Log = new { File = listJson };
                json = JsonConvert.SerializeObject(Log, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(Path, json);
            }

            Monitor.Exit(_object);
        }

        //Write on the XML file for Log
        public void WriteOnFileXml(string Path, string BackupName, string SourcePath, string DestinationPath, string Timestamp, long Size, TimeSpan TransferTime, TimeSpan EncryptionTime)
        {
            Console.Write(TransferTime.ToString());
            Console.Write(EncryptionTime.ToString());
            XDocument xmlLog = XDocument.Load(Path);
            XElement element = new XElement("File");
            element.Add(new XElement("Size", Size));
            element.Add(new XElement("Timestamp", Timestamp));
            element.Add(new XElement("TransferTime", TransferTime.ToString()));
            element.Add(new XElement("CryptedTime", EncryptionTime.ToString()));
            element.Add(new XElement("BackupName", BackupName));
            element.Add(new XElement("SourcePath", SourcePath));
            element.Add(new XElement("DestinationPath", DestinationPath));

            try
            {
                xmlLog.Root.Add(element);
                xmlLog.Save(Path);
            }
            catch (Exception e)
            {
            }
        }
    }
}
