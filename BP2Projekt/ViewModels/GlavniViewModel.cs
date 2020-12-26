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
        private readonly DelegateCommand _otvoriSudionikeCmd;
        private readonly DelegateCommand _otvoriTimoveCmd;
        private readonly DelegateCommand _otvoriOrganizatoraCmd;
        private readonly DelegateCommand _otvoriOrganizacijuCmd;

        public ICommand OtvoriSudionikeCommand => _otvoriSudionikeCmd;
        public ICommand OtvoriTimoveCommand => _otvoriTimoveCmd;
        public ICommand OtvoriOrganizatoraCommand => _otvoriOrganizatoraCmd;
        public ICommand OtvoriOrganizacijuCommand => _otvoriOrganizacijuCmd;

        public ObservableCollection<MecModel> ListaMecevi { get; set; }
        public ObservableCollection<LigaModel> ListaLige { get; set; }
        public ObservableCollection<SudionikModel> ListaSudionici { get; set; }
        public ObservableCollection<IgraModel> ListaIgre { get; set; }
        public ObservableCollection<TimModel> ListaTimovi { get; set; }
        public ObservableCollection<OrganizacijaModel> ListaOrganizacije { get; set; }

        private IDialogService _dialogService { get; }

        public GlavniViewModel(IDialogService dialogService)
        {
            _otvoriSudionikeCmd = new DelegateCommand(OtvoriSudionike);
            _otvoriTimoveCmd = new DelegateCommand(OtvoriIgrace);
            _otvoriOrganizatoraCmd = new DelegateCommand(OtvoriOrganizatora);
            _otvoriOrganizacijuCmd = new DelegateCommand(OtvoriOrganizaciju);

            ListaMecevi = new ObservableCollection<MecModel>();
            ListaLige = new ObservableCollection<LigaModel>();
            ListaSudionici = new ObservableCollection<SudionikModel>();
            ListaIgre = new ObservableCollection<IgraModel>();
            ListaOrganizacije = new ObservableCollection<OrganizacijaModel>();

            PopuniMečeve();
            PopuniLige();
            PopuniSudionike();
            PopuniIgre();
            PopuniOrganizacije();

            _dialogService = dialogService;
        }

        private void OtvoriSudionike()
        {
            //ProzorManager.Prikazi("ProzorSudionici");

            SudionikModel noviSudionik = new SudionikModel();
            var childWindow = new WindowSudionik();
            var childWindowViewModel = new SudionikViewModel(noviSudionik);
            childWindow.DataContext = childWindowViewModel;
            childWindow.Show();

            int x = 0;
        }

        private void OtvoriIgrace() => ProzorManager.Prikazi("ProzorTim");
        private void OtvoriOrganizatora() => ProzorManager.Prikazi("ProzorOrganizator");

        private void OtvoriOrganizaciju() => OtvoriOrganizacije(-1);

        private void OtvoriOrganizacije(int orgID)
        {
            _dialogService.ShowDialog("OrganizacijaProzor", new DialogParameters
            {
                { "listaOrganizacija", ListaOrganizacije},
                { "idOrg", orgID}
            }, r => { });
        }

        private void PopuniMečeve()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT M.*, T1.Naziv AS T1_Naziv, T2.Naziv AS T2_Naziv FROM Mec M
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
                    var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                    ListaLige.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaLige.Add(new LigaModel()
                        {
                            ID = Convert.ToInt32(s["ID_liga"].ToString()),
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
                var selectSQL = new SQLiteCommand(@"SELECT S.*, T.Naziv AS NazivTima, U.NazivUloge FROM Sudionik S 
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

                    ListaOrganizacije.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaOrganizacije.Add(new OrganizacijaModel()
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
    }
}
