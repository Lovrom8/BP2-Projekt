using BP2Projekt.Models;
using MvvmHelpers;
using Prism.Commands;
using Prism.Services.Dialogs;
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
    class UlogaViewModel : BazniDialogViewModel
    {
        private readonly DelegateCommand _dodajIliOsvjeziCommand;
        public ICommand DodajIliOsvjeziCommand => _dodajIliOsvjeziCommand;

        private IgraModel igra;
        public IgraModel Igra
        {
            get => igra;
            set
            {
                if (value == null)
                    return;

                SetProperty(ref igra, value);
                Uloga.ID_Igra = value.ID_Igra;
                Uloga.IgraNaziv = value.Naziv;
            }
        }
        public ObservableCollection<IgraModel> ListaIgre { get; set; }

        private UlogaModel uloga;
        public UlogaModel Uloga
        {
            get => uloga;
            set => SetProperty(ref uloga, value);
        }

        public ObservableCollection<UlogaModel> ListaUloga { get; private set; }
        public int ID_Uloga { get; private set; }

        public UlogaViewModel()
        {
            _dodajIliOsvjeziCommand = new DelegateCommand(DodajIliOsvjezi);
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
                    Uloga.IgraNaziv = reader["NazivIgre"].ToString();

                    Igra.ID_Igra = Uloga.ID_Igra;
                    Igra.Naziv = Uloga.IgraNaziv;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno učitavanje uloge, greška: {ex.Message}");
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

                    var selectSQL = new SQLiteCommand(@"SELECT ID_igra, NazivIgre FROM Igra", con);

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
                            Naziv = s["NazivIgre"].ToString()
                        });
                    }

                    if (Igra != null)
                        Igra = ListaIgre.FirstOrDefault(i => i.ID_Igra == Igra.ID_Igra);

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neuspješno učitavanje igara, greška: {ex.Message}");
            }
        }

        private void DodajIliOsvjezi()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                string insert;
                SQLiteCommand insertSQL;

                if (Uloga.ID_Uloga == -1)
                    insert = @"INSERT INTO Uloga (NazivUloge, FK_igra) VALUES (@Naziv, @FK_Igra)";
                else
                    insert = @"UPDATE Uloga SET NazivUloge=@Naziv, FK_igra=@FK_Igra WHERE ID_uloga=@Id";

                insertSQL = new SQLiteCommand(insert, con);
                insertSQL.Parameters.AddWithValue("@Id", Uloga.ID_Uloga);
                insertSQL.Parameters.AddWithValue("@Naziv", Uloga.Naziv);
                insertSQL.Parameters.AddWithValue("@FK_Igra", Uloga.ID_Igra);

                try
                {
                    insertSQL.ExecuteNonQuery();
                    MessageBox.Show("Uloga dodana u bazu!", "Dodano!");

                    if (Uloga.ID_Uloga == -1)
                        ListaUloga.Add(Uloga);
                    else
                        ListaUloga[ListaUloga.IndexOf(ListaUloga.FirstOrDefault(o => o.ID_Uloga == ID_Uloga))] = Uloga;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            ListaUloga = parameters.GetValue<ObservableCollection<UlogaModel>>("listaUloga");
            ID_Uloga = parameters.GetValue<int>("idUloga");

            ListaIgre = new ObservableCollection<IgraModel>();
            Uloga = new UlogaModel() { ID_Uloga = -1 };
            Igra = new IgraModel();

            UcitajIgre();
            UcitajUlogu(ID_Uloga);
        }
    }
}
