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

        public ObservableCollection<OrganizacijaModel> ListaOrganizacije { get; set; } // PAZI: mora biti property!
        public ObservableCollection<UlogaModel> ListaUloge { get; set; }
        public ObservableCollection<IgraModel> ListaIgre { get; set; }

        private IgraModel _igra;

        public SudionikModel Sudionik { get; set; }
        public UlogaModel Uloga { get; set; }
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

        public SudionikViewModel(SudionikModel sudionici)
        {
            _dodajSudionikaCmd = new DelegateCommand(DodajSudionika);
            Sudionik = new SudionikModel();
            Uloga = new UlogaModel();
            Organizacija = new OrganizacijaModel();
            RadioModel = new IgracTrenerRadioModel();

            ListaOrganizacije = new ObservableCollection<OrganizacijaModel>();
            ListaUloge = new ObservableCollection<UlogaModel>();
            ListaIgre = new ObservableCollection<IgraModel>();

            sudionici.Nick = "lel2";

            PopuniOrganizacije();
            PopuniIgre();
           // PopuniInfo(nick);
        }

        private void PopuniIgre()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Igra", con);

                con.Open();

                try
                {
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
                            Naziv = s["Naziv"].ToString(),
                            Zanr = s["Zanr"].ToString()
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

        private void OsvjeziUloge(int ID_Igra) // Efikasno? Niti malo, ali eto
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Uloga WHERE FK_igra=@ID_Igra", con);
                selectSQL.Parameters.AddWithValue("@ID_Igra", ID_Igra);

                con.Open();

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
                            Naziv = s["NazivUloge"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno citanje iz baze, greška: {ex.Message}");
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
                            ID_Organizacija = Convert.ToInt32(s["ID_org"].ToString()),
                            Naziv = s["NazivOrganizacije"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno citanje iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniInfo(string nick)
        {
            if (nick == string.Empty)
                return;

            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT S.*, T.Naziv AS NazivTima, O.*, I.ID_igra FROM Sudionik S 
                                                    JOIN Tim T ON S.FK_tim = T.ID_tim
                                                    JOIN Organizacija O ON O.ID_org = T.FK_organizacija 
                                                    JOIN Igra I ON I.ID_igra = T.FK_igra
                                                    WHERE Nadimak=@Nick 
                                                    ", con);
                selectSQL.Parameters.AddWithValue("@Nick", nick);

                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    reader.Read();
                    if (!reader.HasRows)
                        return;

                    Sudionik.ID_Sudionik = Convert.ToInt32(reader["ID_sudionik"].ToString());
                    Sudionik.ID_Uloga = 0;

                    Sudionik.ID_Tim = Convert.ToInt32(reader["FK_tim"].ToString());
                    Sudionik.Drzava = reader["Drzava"].ToString();
                    Sudionik.Nick = reader["Nadimak"].ToString();
                    Sudionik.TimNaziv = reader["NazivTima"].ToString();

                    Organizacija = ListaOrganizacije.FirstOrDefault(org => org.ID_Organizacija == Convert.ToInt32(reader["ID_org"]));
                    Igra = ListaIgre.FirstOrDefault(igra => igra.ID_Igra == Convert.ToInt32(reader["ID_igra"]));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno citanje iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }


        private void DodajSudionika()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                long timID = -1;
                int brojTimova = 0;

                var selectSQL = new SQLiteCommand(@"SELECT ID_tim, COUNT(*) FROM Tim WHERE FK_igra=@FK_igra AND FK_organizacija=@FK_org", con); // Provjeri ima li 
                selectSQL.Parameters.AddWithValue("FK_igra", Igra.ID_Igra);
                selectSQL.Parameters.AddWithValue("FK_org", Organizacija.ID_Organizacija);

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    reader.Read();
                    if (!reader.HasRows)
                        return;

                    brojTimova = Convert.ToInt32(reader["COUNT(*)"].ToString());

                    if (brojTimova != 0)
                        timID = Convert.ToInt32(reader["ID_tim"].ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno čitanje iz baze, greška: {ex.Message}");
                    return;
                }

                if (brojTimova == 0) // Ako nema, ubaci novi tim i spremi zadnje iskorišten primarni ključ
                {
                    SQLiteTransaction transaction = con.BeginTransaction();

                    var insertTimSQL = new SQLiteCommand(@"INSERT INTO Tim (Naziv, FK_organizacija, FK_igra) VALUES (@Naziv, @FK_org, @FK_igra)", con);
                    insertTimSQL.Parameters.AddWithValue("@Naziv", Organizacija.Naziv);
                    insertTimSQL.Parameters.AddWithValue("@FK_org", Organizacija.ID_Organizacija);
                    insertTimSQL.Parameters.AddWithValue("@FK_igra", Igra.ID_Igra);

                    try
                    {
                        insertTimSQL.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Neuspješno dodavanje tima u bazu, greška: {ex.Message}");
                        return;
                    }

                    timID = con.LastInsertRowId;

                    transaction.Commit();
                }

                string upit = string.Empty;
                if (RadioModel.JeIgrac) // Ubaci igrača 
                    upit = @"INSERT OR REPLACE INTO Igrac (ID_igrac, Nick, Drzava, FK_tim, FK_uloga) VALUES (@Id, @Nick, @Drzava, @FK_tim, @FK_uloga)";
                else if (RadioModel.JeTrener)
                    upit = @"INSERT OR REPLACE INTO Trener (ID_trener, Nick, Drzava, FK_tim) VALUES (@Id, @Nick, @Osnovana, @Drzava)";

                var insertSQL = new SQLiteCommand(upit, con);

                insertSQL.Parameters.AddWithValue("@Id", Sudionik.ID_Sudionik);
                insertSQL.Parameters.AddWithValue("@Nick", Sudionik.Nick);
                insertSQL.Parameters.AddWithValue("@FK_tim", timID);
                insertSQL.Parameters.AddWithValue("@Drzava", Sudionik.Drzava);
                insertSQL.Parameters.AddWithValue("@FK_uloga", Uloga.ID_Uloga);

                try
                {
                    insertSQL.ExecuteNonQuery();
                    MessageBox.Show("Sudionik dodan u bazu!", "Dodano!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno dodavanje sudionika u bazu! {Environment.NewLine} Greška: {ex.Message}");
                }
                con.Close();
            }
        }
    }
}
