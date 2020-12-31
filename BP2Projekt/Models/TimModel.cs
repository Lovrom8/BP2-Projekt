using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BP2Projekt.Models
{
    class TimModel : INotifyPropertyChanged
    {
        private int id;
        private ObservableCollection<IgracModel> igraci;
        private string naziv;
        private string igra;
        private string organizacija;

        public int ID_Tim
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

        public string Naziv
        {
            get => naziv;
            set
            {
                if (naziv == value)
                    return;

                naziv = value;
                NotifyPropertyChanged();
            }
        }

        public string Igra
        {
            get => igra;
            set
            {
                if (igra == value)
                    return;

                igra = value;
                NotifyPropertyChanged();
            }
        }
        public string Organizacija
        {
            get => organizacija;
            set
            {
                if (organizacija == value)
                    return;

                organizacija = value;
                NotifyPropertyChanged();
            }
        }


        public ObservableCollection<IgracModel> Igraci
        {
            get => igraci;
            set
            {
                if (igraci == value)
                    return;

                igraci = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
