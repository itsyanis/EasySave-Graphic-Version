using EasySave_3.Stores;
using EasySave_3.Commands;
using EasySave_3.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;


namespace EasySave_3.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {


        //Get the extension name given by the user in the textbox in the view
        private string _extensionName;
        public string ExtensionName
        {
            get { return _extensionName; }
            set
            {
                _extensionName = value;
                _errorsViewModel.ClearErrors(nameof(ExtensionName));    //Remove error
                if (_extensionList.Contains(_extensionName))            //Check if already in the list
                {
                    _errorsViewModel.AddError(nameof(ExtensionName), strings.SVMExtensionError);   //Add error
                }
                OnPropertyChanged(nameof(ExtensionName));   //Update the property state
            }
        }

        //Get the software name given by the user in the textbox in the view
        private string _appJobName;
        public string AppJobName
        {
            get { return _appJobName; }
            set
            {
                _appJobName = value;
                _errorsViewModel.ClearErrors(nameof(AppJobName));   //Remove the error
                if (_appJobList.Contains(_appJobName))  //Check if already in the list
                {
                    _errorsViewModel.AddError(nameof(AppJobName), strings.SVMAppJobError);    //Add error
                }
                OnPropertyChanged(nameof(AppJobName));  //Update the property state
            }
        }

        //Get the max size file given by the user in the textbox in the view
        private string _maxFileSize;
        public string MaxFileSize
        {
            get { return _maxFileSize; }
            set
            {
                _maxFileSize = value;
                _errorsViewModel.ClearErrors(nameof(MaxFileSize));  //Remove the error
                if (!IsDigitOnly(_maxFileSize)){    //Check if it's only digits
                    _errorsViewModel.AddError(nameof(MaxFileSize), strings.SVMMaxFileSizeError);   //Add error
                }
                OnPropertyChanged(nameof(MaxFileSize)); //Update the property state
            }
        }

        //Get the name of the extension for the file priority in the textbox in the view
        private string _filePriorityName;
        public string FilePriorityName
        {
            get { return _filePriorityName; }
            set
            {
                _filePriorityName = value;
                _errorsViewModel.ClearErrors(nameof(FilePriorityName)); //Remove error
                if (_filePrioList.Contains(_filePriorityName))  //Check if already in list
                {
                    _errorsViewModel.AddError(nameof(FilePriorityName), strings.SVMFilePriorityError);  //Add error
                }
                OnPropertyChanged(nameof(FilePriorityName));    //Update property state
            }
        }


        private readonly ObservableCollection<string> _appJobList; //Collection containing the jobapps
        public IEnumerable<string> AppJob => _appJobList;   //Interface connected to the view

        private readonly ObservableCollection<string> _extensionList;   //Collection containing the extensions
        public IEnumerable<string> Extension=> _extensionList;  //Interface connected to the view

        private readonly ObservableCollection<string> _filePrioList;   //Collection containing the extensions
        public IEnumerable<string> FilePriority => _filePrioList;  //Interface connected to the view

        //All the commands used in the view
        public ICommand ReturnCommand { get; }
        public ICommand LanguageFrench { get; }
        public ICommand LanguageEnglish { get; }
        public ICommand AddAppJob { get; }
        public ICommand DeleteAppJob { get; }
        public ICommand AddExtension { get; }
        public ICommand DeleteExtension { get; }
        public ICommand LogJson { get; }
        public ICommand LogXml { get; }
        public ICommand SetMaxFileSize { get; }
        public ICommand AddFilePriority { get; }
        public ICommand DeleteFilePriority { get; }


        private readonly ErrorsViewModel _errorsViewModel;  //View Model for the errors
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;    //Event to update the errors
        public bool CanCreate => !_errorsViewModel.HasErrors;   //bool to know if we can execute the command or not

        //Constructor
        public SettingsViewModel(NavigationStore navigationStore)
        {
            _appJobList = new ObservableCollection<string>();   //Initialize the collection which will contain the apps job
            _extensionList = new ObservableCollection<string>(); //Initialize the collection which will contain the extensions
            _filePrioList = new ObservableCollection<string>(); //Initialize the collection which will contain the files for the priority

            _errorsViewModel = new ErrorsViewModel();       //Initialize the viewmodel for the errors
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;    //Updated the event

            GetAppJobList();                //Get the apps job in the settings file to display them
            GetExtensionList();             //Get the extensions in the settings file to display them
            GetMaxFileSize();               //Get the max file size in the settings file to display it
            GetFilePriority();              //Get the files in the settings file to display them

            ReturnCommand = new NavigateHomeCommand(navigationStore);   //Return to main menu

            LanguageFrench = new SetLanguageCommand(navigationStore, "fr"); //Change language to french
            LanguageEnglish = new SetLanguageCommand(navigationStore, "en");    //Change language to english

            AddExtension = new AddExtensionCommand(this, navigationStore);  //Add an extension
            AddAppJob = new AddAppJobCommand(this, navigationStore);        //Add an app job
            AddFilePriority = new AddFilePriorityCommand(this, navigationStore); //Add a file priority

            DeleteExtension = new DeleteExtensionCommand(navigationStore);  //Delete selected extension
            DeleteAppJob = new DeleteAppJobCommand(navigationStore);        //Detele selected app job
            DeleteFilePriority = new DeleteFilePriorityCommand(navigationStore); //Delete selected file priority

            LogJson = new SetTypeLogCommand(navigationStore, "json");   //Change the type of the Log file to json
            LogXml = new SetTypeLogCommand(navigationStore, "xml");     //Change the type of the Log file to xml

            SetMaxFileSize = new SetMaxFileSizeCommad(this, navigationStore);   //Set the max file size
        }

        //read settings file to get list app job
        private void GetAppJobList()
        {
            List<string> AppJobList = FileDirectoryProcessing.ReadSettings("Softwares");    //Function to read the settings file with the parameter to get the right infos

            //Add the jobapps in the collection
            foreach(string appJob in AppJobList)
            {
                _appJobList.Add(appJob);
            }
        }

        //read settings file to get list extension
        private void GetExtensionList()
        {
            List<string> ExtensionList = FileDirectoryProcessing.ReadSettings("Extensions");    //Function to read the settings file with the parameter to get the right infos

            //add the extension in the collection
            foreach (string extension in ExtensionList)
            {
                _extensionList.Add(extension);
            }
        }

        //read the settings file to get the list of file priority
        private void GetFilePriority()
        {
            List<string> FilePrioList = FileDirectoryProcessing.ReadSettings("FilePriority");
            //add the file priority in the collection
            foreach(string file in FilePrioList)
            {
                _filePrioList.Add(file);
            }
        }

        //read the settings file to get the max file size
        private void GetMaxFileSize()
        {
            string MaxFileSize = FileDirectoryProcessing.GetFileSize();
            _maxFileSize = MaxFileSize;
        }

        //Check if there is only digits in the string
        private bool IsDigitOnly(string str)
        {
            foreach(char c in str)
            {
                if (c < '0' || c > '9') return false;
            }
            return true;
        }

        //Get the errors
        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }

        //Update the error state
        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }

    }
}
