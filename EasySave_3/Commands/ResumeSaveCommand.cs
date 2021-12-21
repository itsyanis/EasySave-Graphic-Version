using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace EasySave_3.Commands
{
    public class ResumeSaveCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            Save Save = new Save();
            Save.Resume();
        }
     }
}
