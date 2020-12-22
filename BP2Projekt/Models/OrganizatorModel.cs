using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BP2Projekt.Models
{
    class OrganizatorModel
    {
        private int id;
        private string naziv;
        private string drzava;
        private string osnovan;

        public int ID_Organizator
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

        public string Osnovan
        {
            get => osnovan;
            set
            {
                if (osnovan == value)
                    return;

                osnovan = value;
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
