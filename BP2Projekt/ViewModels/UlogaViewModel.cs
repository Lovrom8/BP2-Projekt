using BP2Projekt.Models;
using MvvmHelpers;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BP2Projekt.ViewModels
{
    class UlogaViewModel : BaseViewModel
    {
        private readonly DelegateCommand _dodajIliOsvjeziCommand;
        public ICommand DodajIliOsvjeziCommand => _dodajIliOsvjeziCommand;

        private UlogaModel uloga;
        private IgraModel igra;

        public IgraModel Igra 
        { 
            get => igra;
            set
            {
                igra = value;
                Uloga.ID_Igra = value.ID_Igra;
                Uloga.IgraNaziv = value.Naziv;
                OnPropertyChanged("Igra");
            }
        }
        public ObservableCollection<IgraModel> ListaIgre { get; set; }
        public UlogaModel Uloga 
        {
            get => uloga;
            set
            {
                uloga = value;
                OnPropertyChanged("Uloga");
            }
        }

        public UlogaViewModel(int UlogaID)
        {
            _dodajIliOsvjeziCommand = new DelegateCommand(DodajIliOsvjezi);
            Uloga = new UlogaModel();
            Uloga.ID_Uloga = UlogaID;
            Igra = new IgraModel();

            ListaIgre = new ObservableCollection<IgraModel>();

            UcitajUlogu(UlogaID);
            UcitajIgre();
        }

        private void UcitajUlogu(int ID)
        {
            if (ID == -1)
                return;

            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                var selectSQL = new SQLiteCommand(@"SELECT * FROM Uloga U JOIN Igra I ON I.ID_igra = U.FK_igra WHERE ID_uloga=@Id ", con);
                selectSQL.Parameters.AddWithValue("@Id", ID);

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    reader.Read();
                    Uloga.ID_Uloga = ID;
                    Uloga.Naziv = reader["NazivUloge"].ToString();
                    Uloga.ID_Igra = Convert.ToInt32(reader["FK_igra"]);
                    Uloga.IgraNaziv = reader["Naziv"].ToString();
                    
                    Igra.ID_Igra = Uloga.ID_Igra;
                    Igra.Naziv = Uloga.IgraNaziv;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void UcitajIgre()
        {
            try
            {
                using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
                {
                    con.Open();

                    var selectSQL = new SQLiteCommand(@"SELECT ID_igra, Naziv FROM Igra", con);

                    var reader = selectSQL.ExecuteReader();

                    reader.Read();
                    if (!reader.HasRows)
                        return;

                    ListaIgre.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaIgre.Add(new IgraModel()
                        {
                            ID_Igra = Convert.ToInt32(s["ID_igra"].ToString()),
                            Naziv = s["Naziv"].ToString()
                        });
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neuspješna promjena timova, greška: {ex.Message}");
            }
        }

        private void DodajIliOsvjezi()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                var insertSQL = new SQLiteCommand(@"INSERT OR REPLACE INTO Uloga (ID_uloga, NazivUloge, FK_igra) VALUES (@Id, @Naziv, @FK_igra)", con);

                insertSQL.Parameters.AddWithValue("@Id", Uloga.ID_Uloga);
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
