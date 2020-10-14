using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BP2Projekt.Models
{
    public class IgraModel : INotifyPropertyChanged
    {
        public int id;
        public string zanr;

        public int ID_Igra
        {
            get => id;
            set
            {
                if (id == value)
                    return;

                id = value;
                NotifyPropertyChanged();
            }
        }

        public string Zanr
        {
            get => zanr;
            set
            {
                if (zanr == value)
                    return;

                zanr = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
