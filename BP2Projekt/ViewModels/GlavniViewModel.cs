using BP2Projekt.Models;
using BP2Projekt.Util;
using MvvmHelpers;
using Prism.Commands;
using Prism.Mvvm;
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
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BP2Projekt.ViewModels
{
    class GlavniViewModel : BindableBase
    {
        private IDialogService _dialogService { get; }

        private int _odabraniTab;
        public int OdabraniTab
        {
            get => _odabraniTab;
            set => SetProperty(ref _odabraniTab, value);
        }

        private readonly DelegateCommand _otvoriSudionikaCmd;
        private readonly DelegateCommand _otvoriTimoveCmd;
        private readonly DelegateCommand _otvoriOrganizatoraCmd;
        private readonly DelegateCommand _otvoriOrganizacijuCmd;
        private readonly DelegateCommand _otvoriLiguCmd;
        private readonly DelegateCommand _otvoriProizvodacaCmd;
        private readonly DelegateCommand _otvoriUloguCmd;
        private readonly DelegateCommand _otvoriMecCmd;
        private readonly DelegateCommand _otvoriIgruCmd;

        public ICommand OtvoriSudionikaCommand => _otvoriSudionikaCmd;
        public ICommand OtvoriTimCommand => _otvoriTimoveCmd;
        public ICommand OtvoriOrganizatoraCommand => _otvoriOrganizatoraCmd;
        public ICommand OtvoriOrganizacijuCommand => _otvoriOrganizacijuCmd;
        public ICommand OtvoriLiguCommand => _otvoriLiguCmd;
        public ICommand OtvoriProizvodacaCommand => _otvoriProizvodacaCmd;
        public ICommand OtvoriUloguCommand => _otvoriUloguCmd;
        public ICommand OtvoriMecCommand => _otvoriMecCmd;
        public ICommand OtvoriIgruCommand => _otvoriIgruCmd;

        public ObservableCollection<MecModel> ListaMecevi { get; set; }
        public ObservableCollection<LigaModel> ListaLige { get; set; }
        public ObservableCollection<SudionikModel> ListaSudionici { get; set; }
        public ObservableCollection<IgraModel> ListaIgre { get; set; }
        public ObservableCollection<TimModel> ListaTimovi { get; set; }
        public ObservableCollection<OrganizacijaModel> ListaOrganizacija { get; set; }
        public ObservableCollection<OrganizatorModel> ListaOrganizatora { get; set; }
        public ObservableCollection<UlogaModel> ListaUloga { get; set; }
        public ObservableCollection<ProizvodacModel> ListaProizvodaca { get; set; }

        private MecModel odabraniMec;
        public MecModel OdabraniMec
        {
            get => odabraniMec; 
            set
            {
                SetProperty(ref odabraniMec, value);
            }
        }

        public LigaModel OdabranaLiga { get; set; }
        public SudionikModel OdabraniSudionik { get; set; }
        public IgraModel OdabranaIgra { get; set; }
        public TimModel OdabraniTim { get; set; }
        public OrganizacijaModel OdabranaOrganizacija { get; set; }
        public OrganizatorModel OdabraniOrganizator { get; set; }
        public UlogaModel OdabranaUloga { get; set; }
        public ProizvodacModel OdabraniProizvodac { get; set; }

        private DelegateCommand _urediCmd;
        private DelegateCommand _obrisiCmd;
        public ICommand UrediCommand => _urediCmd;
        public ICommand ObrisiCommand => _obrisiCmd;

        public GlavniViewModel(IDialogService dialogService)
        {
            _otvoriSudionikaCmd = new DelegateCommand(OtvoriSudionika);
            _otvoriTimoveCmd = new DelegateCommand(OtvoriTim);
            _otvoriOrganizatoraCmd = new DelegateCommand(OtvoriOrganizatora);
            _otvoriOrganizacijuCmd = new DelegateCommand(OtvoriOrganizaciju);
            _otvoriLiguCmd = new DelegateCommand(OtvoriLigu);
            _otvoriIgruCmd = new DelegateCommand(OtvoriIgru);
            _otvoriUloguCmd = new DelegateCommand(OtvoriUlogu);
            _otvoriMecCmd = new DelegateCommand(OtvoriMec);
            _otvoriProizvodacaCmd = new DelegateCommand(OtvoriProizvodaca);

            ListaMecevi = new ObservableCollection<MecModel>();
            ListaLige = new ObservableCollection<LigaModel>();
            ListaSudionici = new ObservableCollection<SudionikModel>();
            ListaIgre = new ObservableCollection<IgraModel>();
            ListaOrganizacija = new ObservableCollection<OrganizacijaModel>();
            ListaTimovi = new ObservableCollection<TimModel>();
            ListaOrganizatora = new ObservableCollection<OrganizatorModel>();
            ListaUloga = new ObservableCollection<UlogaModel>();
            ListaProizvodaca = new ObservableCollection<ProizvodacModel>();

            PopuniMečeve();
            PopuniLige();
            PopuniSudionike();
            PopuniIgre();
            PopuniOrganizacije();
            PopuniOrganizatore();
            PopuniUloge();
            PopuniProizvodaca();
            PopuniTimove();

            _dialogService = dialogService;
            _obrisiCmd = new DelegateCommand(ObrisiRedak);
            _urediCmd = new DelegateCommand(UrediTablicu);
        }

        #region Otvaranje prozorčića
        private void UrediTablicu()
        {
            switch (OdabraniTab)
            {
                case 0:
                    OtvoriMeceve(OdabraniMec.ID_Mec);
                    break;
                case 1:
                    OtvoriLige(OdabranaLiga.ID_Liga);
                    break;
                case 2:
                    OtvoriSudionike(OdabraniSudionik.ID_Sudionik);
                    break;
                case 3:
                    OtvoriIgre(OdabranaIgra.ID_Igra);
                    break;
                case 4:
                    OtvoriOrganizacije(OdabranaOrganizacija.ID_Organizacija);
                    break;
                case 5:
                    OtvoriOrganizatore(OdabraniOrganizator.ID_Organizator);
                    break;
                case 6:
                    OtvoriProizvodace(OdabraniProizvodac.ID_Proizvodac);
                    break;
                case 7:
                    OtvoriTimove(OdabraniTim.ID_Tim);
                    break;
                case 8:
                    OtvoriUloge(OdabranaUloga.ID_Uloga);
                    break; ;
            }
        }

        private void OtvoriSudionika() => OtvoriSudionike(-1);
        private void OtvoriOrganizatora() => OtvoriOrganizatore(-1);
        private void OtvoriOrganizaciju() => OtvoriOrganizacije(-1);
        private void OtvoriLigu() => OtvoriLige(-1);
        private void OtvoriIgru() => OtvoriIgre(-1);
        private void OtvoriUlogu() => OtvoriUloge(-1);
        private void OtvoriMec() => OtvoriMeceve(-1);
        private void OtvoriProizvodaca() => OtvoriProizvodace(-1);
        private void OtvoriTim() => OtvoriTimove(-1);

        private void OtvoriTimove(int timID)
        {
            _dialogService.ShowDialog("TimProzor", new DialogParameters
            {
                { "listaTimovi", ListaTimovi},
                { "idTim", timID}
            }, r => { });
        }

        private void OtvoriLige(int ligaID)
        {
            _dialogService.ShowDialog("LigaProzor", new DialogParameters
            {
                { "listaLiga", ListaLige},
                { "idLiga", ligaID}
            }, r => { });
        }

        private void OtvoriOrganizacije(int orgID)
        {
            _dialogService.ShowDialog("OrganizacijaProzor", new DialogParameters
            {
                { "listaOrganizacija", ListaOrganizacija},
                { "idOrg", orgID}
            }, r => { });
        }

        private void OtvoriSudionike(int sudionikID)
        {
            _dialogService.ShowDialog("SudionikProzor", new DialogParameters
            {
                { "listaSudionik", ListaSudionici},
                { "idSudionik", sudionikID}
            }, r => { });
        }

        private void OtvoriOrganizatore(int organizatorID)
        {
            _dialogService.ShowDialog("OrganizatorProzor", new DialogParameters
            {
                { "listaOrganizator", ListaOrganizatora},
                { "idOrganizator", organizatorID}
            }, r => { });
        }

        private void OtvoriIgre(int igraID)
        {
            _dialogService.ShowDialog("IgraProzor", new DialogParameters
            {
                { "listaIgara", ListaIgre},
                { "idIgra", igraID}
            }, r => { });
        }

        private void OtvoriProizvodace(int proizvodacID)
        {
            _dialogService.ShowDialog("ProizvodacProzor", new DialogParameters
            {
                { "listaProizvodaca", ListaProizvodaca},
                { "idProizvodac", proizvodacID}
            }, r => { });
        }

        private void OtvoriUloge(int ulogaID)
        {
            _dialogService.ShowDialog("UlogaProzor", new DialogParameters
            {
                { "listaUloga", ListaUloga},
                { "idUloga", ulogaID}
            }, r => { });
        }

        private void OtvoriMeceve(int mecID)
        {
            _dialogService.ShowDialog("MecProzor", new DialogParameters
            {
                { "listaMeceva", ListaMecevi},
                { "idMec", mecID}
            }, r => { });
        }
        #endregion
        #region Popunjavanje podataka
        private void PopuniMečeve()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT M.*, T1.NazivTima AS T1_Naziv, T2.NazivTima AS T2_Naziv FROM Mec M
                                                    JOIN Tim AS T1 ON T1.ID_tim = M.FK_TimA
                                                    JOIN Tim AS T2 ON T2.ID_tim = M.FK_TimB", con);

                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    //reader.Read(); PAZI - read tu bi preskočilo prvi red!
                    if (!reader.HasRows)
                        return;

                    ListaMecevi.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaMecevi.Add(new MecModel()
                        {
                            ID_Mec = Convert.ToInt32(s["ID_mec"]),
                            FK_TimA = Convert.ToInt32(s["FK_timA"]),
                            FK_TimB = Convert.ToInt32(s["FK_timB"]),
                            RezultatA = Convert.ToInt32(s["RezultatA"]),
                            RezultatB = Convert.ToInt32(s["RezultatB"]),
                            FK_Pobjednik = Convert.ToInt32(s["FK_Pobjednik"]),
                            TimA = s["T1_Naziv"].ToString(),
                            TimB = s["T2_Naziv"].ToString()
                        });
                    }

                    foreach (var mec in ListaMecevi)
                    {
                        if (mec.FK_TimA == mec.FK_Pobjednik)
                            mec.Pobjednik = mec.TimA;
                        else
                            mec.Pobjednik = mec.TimB;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno čitanje mečeva iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniLige()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT L.*, O.NazivOrganizatora AS OrganizatorNaziv FROM Liga L
                                                    JOIN Organizator O ON L.FK_organizator = O.ID_organizator
                                                    ", con);

                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    ListaLige.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaLige.Add(new LigaModel()
                        {
                            ID_Liga = Convert.ToInt32(s["ID_liga"].ToString()),
                            Naziv = s["NazivLige"].ToString(),
                            FK_Organizator = Convert.ToInt32(s["FK_organizator"].ToString()),
                            Organizator = s["OrganizatorNaziv"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno čitanje liga iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniSudionike()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT S.*, T.NazivTima AS NazivTima, U.NazivUloge FROM Sudionik S 
                                                    LEFT OUTER JOIN Uloga U ON U.ID_uloga = (SELECT FK_uloga FROM Igrac WHERE FK_sudionik = S.ID_sudionik)
                                                    JOIN Tim T ON T.ID_tim = S.FK_Tim
                                                    ", con);

                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    ListaSudionici.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaSudionici.Add(new SudionikModel()
                        {
                            ID_Sudionik = Convert.ToInt32(s["ID_sudionik"]),
                            ID_Tim = Convert.ToInt32(s["FK_Tim"]),
                            Nick = s["Nadimak"].ToString(),
                            Drzava = s["Drzava"].ToString(),
                            UlogaNaziv = s["NazivUloge"].ToString(),
                            TimNaziv = s["NazivTima"].ToString(),
                            Aktivan = Convert.ToBoolean(s["Aktivan"])
                        });
                    }

                    foreach (var sudionik in ListaSudionici)
                    {
                        if (sudionik.UlogaNaziv == "")
                            sudionik.UlogaNaziv = "Trener";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniIgre()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT I.ID_igra, I.Zanr, I.NazivIgre, I.MaxIgraca, I.FK_proizvodac, 
                                                    P.NazivProizvodaca FROM Igra I 
                                                    JOIN Proizvodac P ON P.ID_proizvodac = I.FK_proizvodac 
                                                    ", con);

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
                            ID_Igra = Convert.ToInt32(s["ID_igra"]),
                            Zanr = s["Zanr"].ToString(),
                            Naziv = s["NazivIgre"].ToString(),
                            MaxIgraca = Convert.ToInt32(s["MaxIgraca"]),
                            FK_Proizvodac = Convert.ToInt32(s["FK_proizvodac"]),
                            Proizvodac = s["NazivProizvodaca"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno čitanje igara iz baze, greška: {ex.Message}");
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

                    if (!reader.HasRows)
                        return;

                    ListaOrganizacija.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaOrganizacija.Add(new OrganizacijaModel()
                        {
                            ID_Organizacija = Convert.ToInt32(s["ID_org"]),
                            Naziv = s["NazivOrganizacije"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno čitanje organizacija iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniOrganizatore()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Organizator", con);

                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    ListaOrganizatora.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaOrganizatora.Add(new OrganizatorModel()
                        {
                            ID_Organizator = Convert.ToInt32(s["ID_organizator"]),
                            Osnovan = s["Osnovan"].ToString(),
                            Naziv = s["NazivOrganizatora"].ToString(),
                            Drzava = s["Drzava"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno čitanje organizatora iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniUloge()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Uloga U JOIN Igra I ON U.FK_igra = I.ID_igra", con);

                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    ListaUloga.Clear();
                    var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaUloga.Add(new UlogaModel()
                        {
                            ID_Uloga = Convert.ToInt32(s["ID_uloga"]),
                            Naziv = s["NazivUloge"].ToString(),
                            IgraNaziv = s["NazivIgre"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno čitanje uloga iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniProizvodaca()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Proizvodac", con);

                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    ListaProizvodaca.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaProizvodaca.Add(new ProizvodacModel()
                        {
                            ID_Proizvodac = Convert.ToInt32(s["ID_proizvodac"]),
                            Naziv = s["NazivProizvodaca"].ToString(),
                            Drzava = s["Drzava"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno čitanje proizvođača iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniTimove()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Tim", con);

                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    ListaTimovi.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaTimovi.Add(new TimModel()
                        {
                            ID_Tim = Convert.ToInt32(s["ID_tim"]),
                            Naziv = s["NazivTima"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno čitanje timova iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }
        #endregion

        #region Brisanje
        private void ObrisiRedak()
        {
            switch (OdabraniTab)
            {
                case 0:
                    ObrisiOdabraniMec();
                    break;
                case 1:
                    ObrisiOdabranuLigu();
                    break;
                case 2:
                    ObrisiOdabranogSudionika();
                    break;
                case 3:
                    ObrisiOdabranuIgru();
                    break;
                case 4:
                    ObrisiOdabranuOrganizaciju();
                    break;
                case 5:
                    ObrisiOdabranogOrganizatora();
                    break;
                case 6:
                    ObrisiOdabranogProizvodaca();
                    break;
                case 7:
                    ObrisiOdabraniTim();
                    break;
                case 8:
                    ObrisiOdabranuUlogu();
                    break; ;
            }
        }

        private void ObrisiOdabraniTim()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var deleteSQL = new SQLiteCommand(@"DELETE FROM Tim WHERE ID_tim = @Id", con);
                deleteSQL.Parameters.AddWithValue("Id", OdabraniTim.ID_Tim);

                con.Open();

                try
                {
                    deleteSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno brisanje tima iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void ObrisiOdabraniMec()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var deleteSQL = new SQLiteCommand(@"DELETE FROM Mec WHERE ID_mec = @Id", con);
                deleteSQL.Parameters.AddWithValue("Id", OdabraniMec.ID_Mec);

                con.Open();

                try
                {
                    deleteSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno brisanje mec iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void ObrisiOdabranuOrganizaciju()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var deleteSQL = new SQLiteCommand(@"DELETE FROM Organizacija WHERE ID_org = @Id", con);
                deleteSQL.Parameters.AddWithValue("Id", OdabranaOrganizacija.ID_Organizacija);

                con.Open();

                try
                {
                    deleteSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno brisanje organizacije iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void ObrisiOdabranogOrganizatora()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var deleteSQL = new SQLiteCommand(@"DELETE FROM Organizator WHERE ID_organizator = @Id", con);
                deleteSQL.Parameters.AddWithValue("Id", OdabraniOrganizator.ID_Organizator);

                con.Open();

                try
                {
                    deleteSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno brisanje organizatora iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void ObrisiOdabranuLigu()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var deleteSQL = new SQLiteCommand(@"DELETE FROM Liga WHERE ID_liga = @Id", con);
                deleteSQL.Parameters.AddWithValue("Id", OdabranaLiga.ID_Liga);

                con.Open();

                try
                {
                    deleteSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno brisanje lige iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void ObrisiOdabranuIgru()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var deleteSQL = new SQLiteCommand(@"DELETE FROM Igra WHERE ID_igra = @Id", con);
                deleteSQL.Parameters.AddWithValue("Id", OdabranaIgra.ID_Igra);

                con.Open();

                try
                {
                    deleteSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno brisanje lige iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void ObrisiOdabranogProizvodaca()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var deleteSQL = new SQLiteCommand(@"DELETE FROM Proizvodac WHERE ID_proizvodac = @Id", con);
                deleteSQL.Parameters.AddWithValue("Id", OdabraniProizvodac.ID_Proizvodac);

                con.Open();

                try
                {
                    deleteSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno brisanje proizvodaca iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void ObrisiOdabranogSudionika()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var deleteSQL = new SQLiteCommand(@"DELETE FROM Sudionik WHERE ID_sudionik = @Id", con);
                deleteSQL.Parameters.AddWithValue("Id", OdabraniSudionik.ID_Sudionik);

                con.Open();

                try
                {
                    deleteSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno brisanje sudionika iz baze, greška: {ex.Message}");
                }

                con.Close();
            }

        }

        private void ObrisiOdabranuUlogu()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var deleteSQL = new SQLiteCommand(@"DELETE FROM Uloga WHERE ID_uloga = @Id", con);
                deleteSQL.Parameters.AddWithValue("Id", OdabranaUloga.ID_Uloga);

                con.Open();

                try
                {
                    deleteSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno brisanje uloge iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }
        #endregion

        #region Pretraživanje
        #endregion
    }
}
