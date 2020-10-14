using BP2Projekt.Baza;
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
    public class SudionikViewModel : BaseViewModel
    {
        private readonly DelegateCommand _dodajSudionikaCmd;
        public ICommand DodajSudionikaCommand => _dodajSudionikaCmd;

        private ObservableCollection<OrganizacijaModel> _listaOrganizacije;
        private ObservableCollection<UlogaModel> _listaUloge;

        private UlogaModel _uloga;
        private IgraModel _igra;

        public ObservableCollection<OrganizacijaModel> ListaOrganizacije { get; set; }
        public ObservableCollection<UlogaModel> ListaUloge { get; set; }

        public SudionikModel Sudionik { get; set; }
        public UlogaModel Uloga
        {
            get => _uloga;
            set
            {
                _uloga = value;
               // Osvjezi(value.ID_Uloga);
            }
        }
        public OrganizacijaModel Organizacija { get; set; }
        public IgraModel Igra
        {
            get => _igra;
            set
            {
                _igra = value;
                OsvjeziUloge(value.ID_Igra);
            }
        }
        public IgracTrenerRadioModel RadioModel { get; set; }

        public SudionikViewModel(string nick)
        {
            _dodajSudionikaCmd = new DelegateCommand(DodajSudionika);
            Sudionik = new SudionikModel();
            Uloga = new UlogaModel();
            Organizacija = new OrganizacijaModel();
            RadioModel = new IgracTrenerRadioModel();

            ListaOrganizacije = new ObservableCollection<OrganizacijaModel>();
            ListaUloge = new ObservableCollection<UlogaModel>();

            PopuniInfo(nick);
            PopuniOrganizacije();
        }
        private void OsvjeziUloge(int ID_Igra) // Efikasno? Niti malo, ali eto
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Uloge WHERE FK_igra=@ID_Igra", con);
                selectSQL.Parameters.AddWithValue("@ID_Igra", ID_Igra);

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    reader.Read();

                    if (!reader.HasRows)
                        return;

                    ListaUloge.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaUloge.Add(new UlogaModel()
                        {
                            ID_Uloga = Convert.ToInt32(s["ID_uloga"].ToString()),
                            Naziv = s["Naziv"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniOrganizacije()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Organizacija", con);
                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    reader.Read();

                    if (!reader.HasRows)
                        return;

                    ListaOrganizacije.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaOrganizacije.Add(new OrganizacijaModel()
                        {
                            ID_org = Convert.ToInt32(s["ID_org"].ToString()),
                            Naziv = s["Naziv"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniInfo(string nick)
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Igrac WHERE Nick=@Nick UNION SELECT * FROM Trener WHERE Nick=@Nick LIMIT 1", con);
                selectSQL.Parameters.AddWithValue("@Nick", nick);

                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    reader.Read();

                    if (!reader.HasRows)
                        return;

                    if (SQLUtil.ColumnExists(reader, "ID_igrac"))
                    {
                        Sudionik.ID_Sudionik = Convert.ToInt32(reader["ID_igrac"].ToString());
                        Sudionik.ID_Uloga = Convert.ToInt32(reader["FK_uloga"].ToString());
                    }

                    else
                    {
                        Sudionik.ID_Sudionik = Convert.ToInt32(reader["ID_trener"].ToString());
                        Sudionik.ID_Uloga = Convert.ToInt32(reader["FK_uloga"].ToString());
                    }

                    Sudionik.ID_Tim = Convert.ToInt32(reader["FK_tim"].ToString());
                    Sudionik.Drzava = reader["Drzava"].ToString();
                    Sudionik.Nick = reader["Nick"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void DodajSudionika()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                var insertSQL = new SQLiteCommand(@"INSERT OR REPLACE INTO Igraci (ID_org, Naziv, Osnovana, Drzava) VALUES (@Id, @Naziv, @Osnovana, @Drzava)", con);

                /*insertSQL.Parameters.AddWithValue("@Id", Organizacija.ID_org);
                insertSQL.Parameters.AddWithValue("@Naziv", Organizacija.Naziv);
                insertSQL.Parameters.AddWithValue("@Osnovana", Organizacija.Osnovana);
                insertSQL.Parameters.AddWithValue("@Drzava", Organizacija.Drzava);*/

                try
                {
                    insertSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }

            MessageBox.Show("Organizacija dodana u bazu!", "Dodano!");
        }
    }
}
