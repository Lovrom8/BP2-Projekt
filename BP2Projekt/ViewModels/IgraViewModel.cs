using BP2Projekt.Models;
using MvvmHelpers;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BP2Projekt.ViewModels
{
    //TODO: zavrsi
    class IgraViewModel : BaseViewModel
    {
        private readonly DelegateCommand _dodajIliOsvjeziCommand;
        public ICommand DodajIliOsvjeziCommand => _dodajIliOsvjeziCommand;

        public IgraModel Igra { get; set; }

        private ProizvodacModel proizvodac;

        public ProizvodacModel Proizvodac
        {
            get => proizvodac;
            set
            {
                proizvodac = value;
                Igra.FK_Proizvodac = value.ID;
                OnPropertyChanged("Proizvodac");
            }
        }

        public ObservableCollection<ProizvodacModel> ListaProizvodaci { get; set; }

        public IgraViewModel(int ID_igra)
        {
            _dodajIliOsvjeziCommand = new DelegateCommand(DodajIliOsvjezi);
            ListaProizvodaci = new ObservableCollection<ProizvodacModel>();

            Igra = new IgraModel() { ID_Igra = -1 };

            UcitajIgru(ID_igra);
            UcitajProizvodace();
        }

        private void UcitajIgru(int ID_igra)
        {

        }

        private void UcitajProizvodace()
        {

        }


        private void DodajIliOsvjezi()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();


                SQLiteCommand insertSQL = new SQLiteCommand(@"INSERT OR REPLACE INTO Uloga (ID_uloga, NazivUloge, FK_igra) VALUES (@Id, @Naziv, @FK_igra)", con);

                insertSQL.Parameters.AddWithValue("@Id", Proizvodac.ID);
                insertSQL.Parameters.AddWithValue("@Naziv", Uloga.Naziv);
                insertSQL.Parameters.AddWithValue("@FK_Igra", Uloga.ID_Igra);

                try
                {
                    insertSQL.ExecuteNonQuery();
                    MessageBox.Show("Uloga dodana u bazu!", "Dodano!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }
    }
}
