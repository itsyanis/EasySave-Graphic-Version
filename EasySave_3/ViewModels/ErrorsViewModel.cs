using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EasySave_3.ViewModels
{
    public class ErrorsViewModel : INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>(); //Dictionnary that contains the name of the error and it's message

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;    //Event to update the error state
        public bool HasErrors => _propertyErrors.Any();     //bool to know if the dictionnary has anything

        //Get the errors
        public IEnumerable GetErrors(string propertyName)
        {
            return _propertyErrors.GetValueOrDefault(propertyName, null);
        }

        //Add an error in the dictionnary _propertyErrors
        public void AddError(string propertyName, string errorMessage)  //Parameters are the key and the message
        {
            if (!_propertyErrors.ContainsKey(propertyName)) //Check if the error already exists in the dictionnary
            {
                _propertyErrors.Add(propertyName, new List<string>());  //Add the key to the dictionnary
            }
            _propertyErrors[propertyName].Add(errorMessage);    //Add the message to the corresponding key
            OnErrorsChanged(propertyName);      //Update the state of the errors
        }

        //Remove the error in the dictionnary _propertyErrors
        public void ClearErrors(string propertyName)    //Parameter is the key of the error
        {
            if (_propertyErrors.Remove(propertyName))   //If there is an error to be removed, remove it
            {
                OnErrorsChanged(propertyName);      //Update the state of the errors
            }
        }

        //Update the state of the errors
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
