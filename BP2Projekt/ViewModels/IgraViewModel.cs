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
    class IgraViewModel : BazniDialogViewModel
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
                SetProperty(ref proizvodac, value);

                if (value == null)
                    return;

                Igra.FK_Proizvodac = value.ID_Proizvodac;
                Igra.Proizvodac = value.Naziv;
            }
        }

        public ObservableCollection<ProizvodacModel> ListaProizvodaci { get; set; }
        public ObservableCollection<IgraModel> ListaIgara { get; private set; }
        public int ID_Igra { get; private set; }

        public IgraViewModel()
        {
            _dodajIliOsvjeziCommand = new DelegateCommand(DodajIliOsvjezi);
            ListaProizvodaci = new ObservableCollection<ProizvodacModel>();
        }

        private void UcitajIgru(int ID_igra)
        {
            if (ID_igra == -1)
                return;

            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                var selectSQL = new SQLiteCommand(@"SELECT * FROM Igra I JOIN Proizvodac P ON P.ID_proizvodac = I.FK_proizvodac WHERE ID_igra=@Id ", con);
                selectSQL.Parameters.AddWithValue("@Id", ID_igra);

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    reader.Read();

                    Igra = new IgraModel()
                    {
                        ID_Igra = ID_igra,
                        Naziv = reader["NazivIgre"].ToString(),
                        MaxIgraca = Convert.ToInt32(reader["MaxIgraca"]),
                        Zanr = reader["Zanr"].ToString(),
                        FK_Proizvodac = Convert.ToInt32(reader["FK_proizvodac"])
                    };

                    Proizvodac = new ProizvodacModel()
                    {
                        ID_Proizvodac = Igra.FK_Proizvodac,
                        Naziv = reader["NazivProizvodaca"].ToString()
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno učitavanje uloge, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void UcitajProizvodace()
        {
            try
            {
                using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
                {
                    con.Open();

                    var selectSQL = new SQLiteCommand(@"SELECT ID_proizvodac, NazivProizvodaca FROM Proizvodac", con);

                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    ListaProizvodaci.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaProizvodaci.Add(new ProizvodacModel()
                        {
                            ID_Proizvodac = Convert.ToInt32(s["ID_proizvodac"].ToString()),
                            Naziv = s["NazivProizvodaca"].ToString()
                        });
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neuspješno učitavanje proizvodača, greška: {ex.Message}");
            }
        }

        private void DodajIliOsvjezi()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                string insert;
                SQLiteCommand insertSQL;

                if (Igra.ID_Igra == -1)
                    insert = @"INSERT INTO Igra (NazivIgre, FK_proizvodac, Zanr, MaxIgraca) VALUES (@Naziv, @FK_Proizvodac, @Zanr, @MaxIgraca)";
                else
                    insert = @"UPDATE Igra SET NazivIgre=@Naziv, Zanr=@Zanr, FK_proizvodac=@FK_Proizvodac, MaxIgraca=@MaxIgraca WHERE ID_igra=@Id";

                insertSQL = new SQLiteCommand(insert, con);
                insertSQL.Parameters.AddWithValue("@Id", ID_Igra);
                insertSQL.Parameters.AddWithValue("@Naziv", Igra.Naziv);
                insertSQL.Parameters.AddWithValue("@FK_Proizvodac", Igra.FK_Proizvodac);
                insertSQL.Parameters.AddWithValue("@Zanr", Igra.Zanr);
                insertSQL.Parameters.AddWithValue("@MaxIgraca", Igra.MaxIgraca);

                try
                {
                    insertSQL.ExecuteNonQuery();
                    MessageBox.Show("Igra dodana u bazu!", "Dodano!");

                    if (ID_Igra == -1)
                        ListaIgara.Add(Igra);
                    else
                        ListaIgara[ListaIgara.IndexOf(ListaIgara.FirstOrDefault(o => o.ID_Igra == ID_Igra))] = Igra;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno ubacivanje igre u bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            ListaIgara = parameters.GetValue<ObservableCollection<IgraModel>>("listaIgara");
            ID_Igra = parameters.GetValue<int>("idIgra");

            Igra = new IgraModel() { ID_Igra = -1 };

            UcitajProizvodace();
            UcitajIgru(ID_Igra);

            Proizvodac = ListaProizvodaci.FirstOrDefault(p => p.ID_Proizvodac == Igra.FK_Proizvodac);
        }
    }
}
