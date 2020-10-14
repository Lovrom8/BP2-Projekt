using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BP2Projekt.Models
{
    class IgracModel : INotifyPropertyChanged
    {
        public int id;
        public string nick;
        public string drzava;

        public int ID_igrac
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

        public string Nick
        {
            get => nick;
            set
            {
                if (nick == value)
                    return;

                nick = value;
                NotifyPropertyChanged();
            }
        }

        public string Drzava
        {
            get => drzava;
            set
            {
                if (drzava == value)
                    return;

                drzava = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
