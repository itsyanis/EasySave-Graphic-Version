using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave_3
{
    class Settings
    {
        public string Language { get; set; }
        public List<string> Extensions { get; set; }
        public List<string> Softwares { get; set; }

        public List<string> FilePriority { get; set; }

        public long MaxFileSize { get; set; }
        public string LogFileType { get; set; }

    }


}
