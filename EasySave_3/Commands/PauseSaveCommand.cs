using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace EasySave_3.Commands
{
    public class PauseSaveCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            Save Save = new Save();
            Save.Pause();
        }
    }
}
