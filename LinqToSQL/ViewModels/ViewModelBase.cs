// Importing necessary namespaces
using System.ComponentModel;           // Provides interfaces and classes to support the component model, including INotifyPropertyChanged
using System.Runtime.CompilerServices; // Provides information about the caller of a method, allowing access to the calling member name

// Namespace declaration
namespace LinqToSQL.ViewModels
{
    // Base class for view models that implements property change notifications
    public class ViewModelBase : INotifyPropertyChanged
    {
        // Event to notify the UI when a property value has changed
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to raise the PropertyChanged event, allowing bound UI elements to update when a property changes
        // [CallerMemberName] attribute automatically supplies the name of the calling property, if none is provided
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // If PropertyChanged is not null, raise the event with the name of the property that changed
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Helper method to set the value of a property and raise the PropertyChanged event if the value changes
        // Uses generics to handle properties of any type
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            // Check if the new value is equal to the existing field value; if so, return false (no change needed)
            if (Equals(field, value)) return false;

            // Update the field with the new value
            field = value;

            // Raise the PropertyChanged event to notify the UI of the change
            OnPropertyChanged(propertyName);

            // Return true to indicate the value was changed
            return true;
        }
    }
}
