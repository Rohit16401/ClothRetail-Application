using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RetailUI.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            Debug.WriteLine($"Property {name} has changed.");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
