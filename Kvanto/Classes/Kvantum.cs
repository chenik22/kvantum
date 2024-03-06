using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kvanto.Classes
{
    public class Kvantum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Kvantum(int Id, string Name, string Description)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
        }
        // Прогрузка квантумов(предметов)
        static public void LoadKvantum(MainWindow _mainWindow)
        {
            try
            {
                _mainWindow.KvantumList.Clear();
                MySqlConnection mySqlConnection = new MySqlConnection(WorkingBD.GetConnectionString());
                mySqlConnection.Open();
                MySqlDataReader users_query = WorkingBD.Query("SELECT * FROM kvanto.kvantum;", mySqlConnection);
                while (users_query.Read())
                {
                    _mainWindow.KvantumList.Add(new Kvantum(Convert.ToInt32(users_query.GetValue(0)),
                    users_query.GetValue(1).ToString(),
                    users_query.GetValue(2).ToString()
                    ));
                }
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                _mainWindow.ErrMessOn(ex.Message, "Ой! Таблица " + "kvanto" + " не была загружена");

            }
        }
    }

}
