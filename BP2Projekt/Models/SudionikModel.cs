using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BP2Projekt
{
    public class SudionikModel : INotifyPropertyChanged
    {
        private int ID_sudionik;
        private int ID_tim;
        private int ID_uloga;
        private string drzava;
        private string nick;

        public int ID_Sudionik
        {
            get => ID_sudionik;
            set
            {
                if (ID_sudionik == value)
                    return;
                ID_sudionik = value;

                NotifyPropertyChanged();
            }
        }
        public int ID_Tim
        {
            get => ID_tim;
            set
            {
                if (ID_tim == value)
                    return;

                ID_tim = value;

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
        public int ID_Uloga
        {
            get => ID_uloga;
            set
            {
                if (ID_uloga == value)
                    return;
                ID_uloga = value;

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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    class IgracModel : SudionikModel
    {

    }

    class TrenerModel : SudionikModel
    {

    }
}
