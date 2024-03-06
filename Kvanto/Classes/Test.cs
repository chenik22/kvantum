using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kvanto.Classes
{
    public class Test
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string Kvantum { get; set; }
        public Test(int Id, string FIO, string Kvantum)
        {
            this.Id = Id;
            this.FIO = FIO;
            this.Kvantum = Kvantum;
        }
        // Прогрузка итога теста
        static public void LoadTest(MainWindow _mainWindow)
        {
            try
            {
                _mainWindow.TestList.Clear();
                MySqlConnection mySqlConnection = new MySqlConnection(WorkingBD.GetConnectionString());
                mySqlConnection.Open();
                MySqlDataReader users_query = WorkingBD.Query("SELECT * FROM kvanto.test;", mySqlConnection);
                while (users_query.Read())
                {
                    _mainWindow.TestList.Add(new Test(Convert.ToInt32(users_query.GetValue(0)),
                    users_query.GetValue(1).ToString(),
                    users_query.GetValue(2).ToString()
                    ));
                }
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                _mainWindow.ErrMessOn(ex.Message, "Ой! Таблица "+ "test" + " не была загружена");
            }
        }
    }
}
