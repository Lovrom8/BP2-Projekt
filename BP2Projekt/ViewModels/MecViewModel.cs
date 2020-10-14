using BP2Projekt.Models;
using MvvmHelpers;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BP2Projekt.ViewModels
{
    class MecViewModel : BaseViewModel
    {
        // private readonly DelegateCommand 

        public MecViewModel(int MecID)
        {
            PopuniInfo(MecID);
        }

        private void PopuniInfo(int MecID)
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                var selectSQL = new SQLiteCommand(@"SELECT * FROM Mec WHERE ID_mec=@Id", con);
                selectSQL.Parameters.AddWithValue("@Id", MecID);

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    reader.Read();

                    if (!reader.HasRows)
                        return;

                    /*  Organizacija.ID_org = ID;
                      Organizacija.Naziv = reader["Naziv"].ToString();
                      Organizacija.Drzava = reader["Drzava"].ToString();
                      Organizacija.Osnovana = reader["Osnovana"].ToString();*/
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
