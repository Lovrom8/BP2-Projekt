using BP2Projekt.Models;
using BP2Projekt.Util;
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
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BP2Projekt.ViewModels
{
    class GlavniViewModel : BaseViewModel
    {
        private readonly DelegateCommand _otvoriSudionikeCmd;
        private readonly DelegateCommand _otvoriTimoveCmd;
        private readonly DelegateCommand _otvoriOrganizatoraCmd;

        public ICommand OtvoriSudionikeCommand => _otvoriSudionikeCmd;
        public ICommand OtvoriTimoveCommand => _otvoriTimoveCmd;
        public ICommand OtvoriOrganizatoraCommand => _otvoriOrganizatoraCmd;

        public ObservableCollection<MecModel> ListaMecevi { get; set; }
        public ObservableCollection<LigaModel> ListaLige { get; set; }

        public GlavniViewModel()
        {
            _otvoriSudionikeCmd = new DelegateCommand(OtvoriSudionike);
            _otvoriTimoveCmd = new DelegateCommand(OtvoriIgrace);
            _otvoriOrganizatoraCmd = new DelegateCommand(OtvoriOrganizatora);

            ListaMecevi = new ObservableCollection<MecModel>();
            ListaLige = new ObservableCollection<LigaModel>();

            PopuniMečeve();
            PopuniLige();
            PopuniSudionike();
        }

        private void OtvoriSudionike() => ProzorManager.Prikazi("ProzorSudionici");
        private void OtvoriIgrace() => ProzorManager.Prikazi("ProzorTim");
        private void OtvoriOrganizatora() => ProzorManager.Prikazi("ProzorOrganizator");

        private void PopuniMečeve()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Mec", con);

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
                            ID_Mec = Convert.ToInt32(s["ID_mec"].ToString()),
                            FK_TimA = Convert.ToInt32(s["FK_timA"].ToString()),
                            FK_TimB = Convert.ToInt32(s["FK_timB"].ToString()),
                            RezultatA = Convert.ToInt32(s["RezultatA"].ToString()),
                            RezultatB = Convert.ToInt32(s["RezultatB"].ToString()),
                            FK_Pobjednik = Convert.ToInt32(s["FK_Pobjednik"].ToString())
                        });
                    }

                    foreach (var mec in ListaMecevi)
                    {
                        selectSQL = new SQLiteCommand(@"SELECT T1.Naziv AS T1_Naziv, T2.Naziv AS T2_Naziv
                                                        FROM Tim T1 JOIN Tim AS T2 ON T2.ID_tim = @TimB_ID
                                                        WHERE T1.ID_tim = @TimA_ID", con);
                        selectSQL.Parameters.AddWithValue("@TimA_ID", mec.FK_TimA);
                        selectSQL.Parameters.AddWithValue("@TimB_ID", mec.FK_TimB);

                        reader = selectSQL.ExecuteReader();

                        if (!reader.HasRows)
                            return;

                        reader.Read();
                        mec.TimA = reader["T1_Naziv"].ToString();
                        mec.TimB = reader["T2_Naziv"].ToString();

                        if (mec.FK_TimA == mec.FK_Pobjednik)
                            mec.Pobjednik = mec.TimA;
                        else
                            mec.Pobjednik = mec.TimB;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniLige()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Liga", con);

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
                            ID = Convert.ToInt32(s["ID_liga"].ToString()),
                            Naziv = s["Naziv"].ToString(),
                            FK_Organizator = Convert.ToInt32(s["FK_organizator"].ToString())
                        });
                    }

                    foreach (var liga in ListaLige)
                    {
                        selectSQL = new SQLiteCommand(@"SELECT Naziv FROM Organizator O WHERE @FK_organizator = O.ID_organizator", con);
                        selectSQL.Parameters.AddWithValue("@FK_organizator", liga.FK_Organizator);

                        reader = selectSQL.ExecuteReader();

                        if (!reader.HasRows)
                            return;

                        reader.Read();
                        liga.Organizator = reader["Naziv"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void PopuniSudionike()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Liga", con);

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
                            ID = Convert.ToInt32(s["ID_liga"].ToString()),
                            Naziv = s["Naziv"].ToString(),
                            FK_Organizator = Convert.ToInt32(s["FK_organizator"].ToString())
                        });
                    }

                    foreach (var liga in ListaLige)
                    {
                        selectSQL = new SQLiteCommand(@"SELECT Naziv FROM Organizator O WHERE @FK_organizator = O.ID_organizator", con);
                        selectSQL.Parameters.AddWithValue("@FK_organizator", liga.FK_Organizator);

                        reader = selectSQL.ExecuteReader();

                        if (!reader.HasRows)
                            return;

                        reader.Read();
                        liga.Organizator = reader["Naziv"].ToString();
                    }
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
