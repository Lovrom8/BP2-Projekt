using BP2Projekt.Baza;
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
    public class SudionikViewModel : BazniDialogViewModel
    {
        private readonly DelegateCommand _dodajSudionikaCmd;
        public ICommand DodajSudionikaCommand => _dodajSudionikaCmd;

        public ObservableCollection<OrganizacijaModel> ListaOrganizacije { get; set; } // PAZI: mora biti property!
        public ObservableCollection<UlogaModel> ListaUloge { get; set; }
        public ObservableCollection<IgraModel> ListaIgre { get; set; }

        private IgraModel _igra;
        public IgraModel Igra
        {
            get => _igra;
            set
            {
                if (value == null)
                    return;

                SetProperty(ref _igra, value);
                OsvjeziUloge(value.ID_Igra);
            }
        }
        public SudionikModel Sudionik { get; set; }
        public UlogaModel Uloga { get; set; }
        public OrganizacijaModel Organizacija { get; set; }

        public IgracTrenerRadioModel RadioModel { get; set; }
        public ObservableCollection<SudionikModel> ListaSudionika { get; set; }
        public int ID_Sudionik { get; private set; }

        public SudionikViewModel()
        {
            _dodajSudionikaCmd = new DelegateCommand(DodajSudionika);
            Sudionik = new SudionikModel();
            Uloga = new UlogaModel();
            Organizacija = new OrganizacijaModel();
            RadioModel = new IgracTrenerRadioModel();

            ListaOrganizacije = new ObservableCollection<OrganizacijaModel>();
            ListaUloge = new ObservableCollection<UlogaModel>();
            ListaIgre = new ObservableCollection<IgraModel>();
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

                    if (!reader.HasRows)
                        return;

                    ListaIgre.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaIgre.Add(new IgraModel()
                        {
                            ID_Igra = Convert.ToInt32(s["ID_igra"].ToString()),
                            Naziv = s["NazivIgre"].ToString(),
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
                var selectSQL = new SQLiteCommand(@"SELECT ID_org, NazivOrganizacije FROM Organizacija", con);
                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

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
                    MessageBox.Show($"Neuspješno citanje organizacija baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniInfo(int ID_Igrac)
        {
            if (ID_Igrac == -1)
                return;

            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT S.*, T.NazivTima AS NazivTima, U.*, O.*, I.ID_igra FROM Sudionik S 
                                                    JOIN Tim T ON S.FK_tim = T.ID_tim
                                                    JOIN Organizacija O ON O.ID_org = T.FK_organizacija 
                                                    JOIN Igra I ON I.ID_igra = T.FK_igra
                                                    LEFT JOIN Igrac IG ON IG.FK_sudionik = @Id
                                                    LEFT JOIN Uloga U ON U.ID_uloga = IG.FK_uloga
                                                    LEFT JOIN Trener TR ON TR.FK_sudionik = @Id
                                                    WHERE ID_sudionik = @Id", con);
                selectSQL.Parameters.AddWithValue("@Id", ID_Igrac);

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
                    Sudionik.Starost = Convert.ToInt32(reader["Starost"]);
                    Sudionik.Ime = reader["Ime"].ToString();
                    Sudionik.Prezime = reader["Prezime"].ToString();

                    Organizacija = ListaOrganizacije.FirstOrDefault(org => org.ID_Organizacija == Convert.ToInt32(reader["ID_org"]));
                    Igra = ListaIgre.FirstOrDefault(igra => igra.ID_Igra == Convert.ToInt32(reader["ID_igra"]));

                    if (reader["ID_uloga"] != null)
                        Uloga = ListaUloge.FirstOrDefault(uloga => uloga.ID_Uloga == Convert.ToInt32(reader["ID_uloga"]));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno citanje trenutnog igraca baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void DodajSudionika()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                /*long timID = -1;
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
                }*/

                string upit = string.Empty;
                SQLiteCommand insertSQL;

                if (ID_Sudionik == -1)
                    upit = @"INSERT INTO Sudionik (Ime, Prezime, Starost, Nadimak, FK_tim, Aktivan, Drzava) VALUES (@Ime, @Prezime, @Starost, @Nadimak, @FK_tim, @Aktivan, @Drzava)";
                else
                    upit = @"UPDATE Sudionik WHERE FK_sudionik=@Id SET Ime=@Ime, Prezime=@Prezime, Starost=@Starost, Nadimak=@Nadimak, FK_tim=@FK_tim, Aktivan=@Aktivan, Drzava=@Drzava";

                insertSQL = new SQLiteCommand(upit, con);
                insertSQL.Parameters.AddWithValue("@Id", Sudionik.ID_Sudionik);
                insertSQL.Parameters.AddWithValue("@Ime", Sudionik.Ime);
                insertSQL.Parameters.AddWithValue("@Prezime", Sudionik.Prezime);
                insertSQL.Parameters.AddWithValue("@Starost", Sudionik.Starost);
                insertSQL.Parameters.AddWithValue("@Nadimak", Sudionik.Nick);
                insertSQL.Parameters.AddWithValue("@FK_tim", Sudionik.FK_Tim);
                insertSQL.Parameters.AddWithValue("@Aktivan", Sudionik.Aktivan);
                insertSQL.Parameters.AddWithValue("@Drzava", Sudionik.Drzava);

                try
                {
                    insertSQL.ExecuteNonQuery();
                    MessageBox.Show("Sudionik dodan u bazu!", "Dodano!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno dodavanje sudionika u bazu! {Environment.NewLine} Greška: {ex.Message}");
                }

                if (RadioModel.JeIgrac)
                {
                    if (ID_Sudionik == -1)
                        upit = @"INSERT INTO Igrac (FK_sudionik, FK_uloga, Igrac_od) VALUES (@Id, @FK_uloga, @Igrac_od)";
                    else
                        upit = @"UPDATE Igrac WHERE FK_sudionik=@Id SET FK_uloga = @FK_uloga, Igrac_od=@Igrac_od";
                }
                else if (RadioModel.JeTrener)
                {
                    if (ID_Sudionik == -1)
                        upit = @"INSERT INTO Trener (FK_sudionik, Trener_od) VALUES (@Id, @Trener_od)";
                    else
                        upit = @"UPDATE Trener WHERE FK_sudionik=@Id SET Trener_od=@Trener_od";
                }

                insertSQL = new SQLiteCommand(upit, con);
                insertSQL.Parameters.AddWithValue("@Id", Sudionik.ID_Sudionik);
                insertSQL.Parameters.AddWithValue("@FK_uloga", Uloga.ID_Uloga);
                insertSQL.Parameters.AddWithValue("@Igrac_od", Sudionik.IgracOd);
                insertSQL.Parameters.AddWithValue("@Trener_od", Sudionik.TrenerOd);

                try
                {
                    insertSQL.ExecuteNonQuery();

                    if (ID_Sudionik == -1)
                        ListaSudionika.Add(Sudionik);
                    else
                        ListaSudionika[ListaSudionika.IndexOf(ListaSudionika.FirstOrDefault(s => s.ID_Sudionik == ID_Sudionik))] = Sudionik;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno dodavanje sudionika u bazu! {Environment.NewLine} Greška: {ex.Message}");
                }

                con.Close();
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            ListaSudionika = parameters.GetValue<ObservableCollection<SudionikModel>>("listaSudionika");
            ID_Sudionik = parameters.GetValue<int>("idSudionik");

            Sudionik = new SudionikModel() { ID_Sudionik = -1 };

            PopuniOrganizacije();
            PopuniIgre();
            PopuniInfo(ID_Sudionik);
        }
    }
}
