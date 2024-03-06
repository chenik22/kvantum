using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kvanto.Classes
{
    public class Users
    {
        public MainWindow mainWindow;
        public int Id { get; set; }
        public string Login { get; set; }
        public string FIO { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Kvantum { get; set; }
        public string Teacher { get; set; }

        public Users(int Id, string Login, string FIO, string Password, string Role, string Kvantum, string Teacher)
        {
            this.Id = Id;
            this.Login = Login;
            this.FIO = FIO;
            this.Password = Password;
            this.Role = Role;
            this.Kvantum = Kvantum;
            this.Teacher = Teacher;
        }
        // Прогрузка пользователей
        static public void LoadUsers(MainWindow _mainWindow)
        {
            try
            {
                _mainWindow.UsersList.Clear();
                MySqlConnection mySqlConnection = new MySqlConnection(WorkingBD.GetConnectionString());
                mySqlConnection.Open();
                MySqlDataReader users_query = WorkingBD.Query("SELECT * FROM kvanto.users;", mySqlConnection);
                while (users_query.Read())
                {
                    _mainWindow.UsersList.Add(new Users(Convert.ToInt32(users_query.GetValue(0)),
                    users_query.GetValue(1).ToString(),
                    users_query.GetValue(2).ToString(),
                    users_query.GetValue(3).ToString(),
                    users_query.GetValue(4).ToString(),
                    users_query.GetValue(5).ToString(),
                    users_query.GetValue(6).ToString()
                    ));
                }
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                _mainWindow.ErrMessOn(ex.Message, "Ой! Таблица " + "users" + " не была загружена");
            }
        }
        static public void UpdateUser(Users user)
        {
            try
            {
                MySqlConnection mySqlConnection = new MySqlConnection(WorkingBD.GetConnectionString());
                mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand($"UPDATE kvanto.users SET Login = '{user.Login}', FIO = '{user.FIO}', Password = '{user.Password}', Role = '{user.Role}', Kvantum = '{user.Kvantum}', Teacher = '{user.Teacher}' WHERE Id = {user.Id};", mySqlConnection);
                command.ExecuteNonQuery();
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
