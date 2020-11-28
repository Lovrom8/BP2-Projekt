using BP2Projekt.Models;
using BP2Projekt.Util;
using MvvmHelpers;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public GlavniViewModel()
        {
            _otvoriSudionikeCmd = new DelegateCommand(OtvoriSudionike);
            _otvoriTimoveCmd = new DelegateCommand(OtvoriIgrace);
            _otvoriOrganizatoraCmd = new DelegateCommand(OtvoriOrganizatora);

            PopuniMečeve();
        }

        private void OtvoriSudionike() => ProzorManager.Prikazi("ProzorSudionici");
        private void OtvoriIgrace() => ProzorManager.Prikazi("ProzorTimovi");
        private void OtvoriOrganizatora() => ProzorManager.Prikazi("ProzorOrganizator");
    
        private void PopuniMečeve()
        {
           

            return;
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

                    //ListaIgre.Clear();

                    /*foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaIgre.Add(new IgraModel()
                        {
                            ID_Igra = Convert.ToInt32(s["ID_igra"].ToString()),
                            Naziv = s["Naziv"].ToString(),
                            Zanr = s["Zanr"].ToString()
                        });
                    }*/
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }
    }
}
