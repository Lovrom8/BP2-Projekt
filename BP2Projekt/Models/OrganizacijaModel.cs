using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BP2Projekt.Models
{
    public class OrganizacijaModel : INotifyPropertyChanged
    {
        private int id;
        private string naziv;
        private string osnovana;
        private string drzava;

        public int ID_org
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

        public string Osnovana
        {
            get => osnovana;
            set
            {
                if (osnovana == value)
                    return;

                osnovana = value;
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
