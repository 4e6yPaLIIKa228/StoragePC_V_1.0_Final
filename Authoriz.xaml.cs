using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using YchetPer.Connection;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Drawing;
using System.Net.Mail;
using System.Net;

namespace YchetPer
{
    /// <summary>
    /// Логика взаимодействия для Authoriz.xaml
    /// </summary>
    public partial class Authoriz : Window
    {
        public Authoriz()
        {
            InitializeComponent();
            TbLogin.MaxLength = 30;
            TbPass.MaxLength = 30;
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TbLogin.Text) || String.IsNullOrEmpty(TbPass.Password))
            {
                MessageBox.Show("Заполните поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else 
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                    try
                    {
                        byte[] Pass;
                        Func f = new Func();
                        Pass = f.GetHashPassword(TbPass.Password);
                        connection.Open();
                    string query = $@"SELECT  COUNT(1) FROM USERS WHERE Login=@Login AND Pass=@Pass";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                     
                        cmd.Parameters.AddWithValue("@Login", TbLogin.Text);
                        cmd.Parameters.AddWithValue("@Pass", Pass);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 1)
                        {
                           
                            string query2 = $@"SELECT ID FROM USERS WHERE Login=@Login";
                            SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                            cmd2.Parameters.AddWithValue("@Login", TbLogin.Text);
                            int countID = Convert.ToInt32(cmd2.ExecuteScalar());
                            Saver.Login = TbLogin.Text;
                            Saver.ID = countID;
                            connection.Close();
                            MessageBox.Show("Добро пожаловать!");
                            MainWindow menu = new MainWindow();
                            menu.Show();
                            this.Close();

                        }
                        else
                        {
                            MessageBox.Show("Неверное имя пользователя или пароль");
                        }
                    }
                      catch (SQLiteException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
            }       
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            Regist Aftoriz = new Regist();
            this.Close();
            Aftoriz.ShowDialog();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
