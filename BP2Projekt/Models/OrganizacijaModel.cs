using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BP2Projekt.Models
{
    public class OrganizacijaModel : INotifyPropertyChanged
    {
        private int id;
        private string naziv;
        private string osnovana;
        private string drzava;
        private int brojTimova;

        public int ID_Organizacija
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

        public int BrojTimova
        {
            get => brojTimova;
            set
            {
                if (brojTimova == value)
                    return;

                brojTimova = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
