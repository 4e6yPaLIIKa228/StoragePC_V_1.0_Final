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
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using Microsoft.Win32;

namespace YchetPer
{
    /// <summary>
    /// Логика взаимодействия для AddTechnic.xaml
    /// </summary>
    public partial class AddTechnic : Window
    {
        DataTable dt1 = new DataTable("NumberKabs");
        public AddTechnic()
        {
            InitializeComponent();
            CbFill();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            CbFill();
            this.Close();

        }
        public void CbFill()  //Данные для комбобоксов 
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query1 = $@"SELECT * FROM Types"; // Типы
                    string query2 = $@"SELECT * FROM Conditions"; // Состояние
                    string query3 = $@"SELECT * FROM NumberKabs"; // Кабинеты
                    //string query4 = $@"SELECT  Devices.Title FROM Devices"; // Названия
                    string query5 = $@"SELECT * FROM Titles"; // Устройства
                    //----------------------------------------------
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    SQLiteCommand cmd3 = new SQLiteCommand(query3, connection);
                    //SQLiteCommand cmd4 = new SQLiteCommand(query4, connection);
                    SQLiteCommand cmd5 = new SQLiteCommand(query5, connection);
                    //----------------------------------------------
                    SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                    SQLiteDataAdapter SDA2 = new SQLiteDataAdapter(cmd2);
                    SQLiteDataAdapter SDA3 = new SQLiteDataAdapter(cmd3);
                    //SQLiteDataAdapter SDA4 = new SQLiteDataAdapter(cmd4);
                    SQLiteDataAdapter SDA5 = new SQLiteDataAdapter(cmd5);
                    //----------------------------------------------
                    DataTable dt1 = new DataTable("Types");
                    DataTable dt2 = new DataTable("Conditions");
                    DataTable dt3 = new DataTable("NumberKabs");
                    DataTable dt4 = new DataTable("Devices");
                    DataTable dt5 = new DataTable("Titles");
                    //----------------------------------------------
                    SDA1.Fill(dt1);
                    SDA2.Fill(dt2);
                    SDA3.Fill(dt3);
                    //SDA4.Fill(dt4);
                    SDA5.Fill(dt5);
                    //----------------------------------------------

                    CbClass.ItemsSource = dt1.DefaultView;
                    CbClass.DisplayMemberPath = "Class";
                    CbClass.SelectedValuePath = "ID";
                    //----------------------------------------------
                    CbCondition.ItemsSource = dt2.DefaultView;
                    CbCondition.DisplayMemberPath = "Condition";
                    CbCondition.SelectedValuePath = "ID";
                    //----------------------------------------------
                    CbNumKab.ItemsSource = dt3.DefaultView;
                    CbNumKab.DisplayMemberPath = "NumKab";
                    CbNumKab.SelectedValuePath = "ID";
                    //----------------------------------------------
                    CbTitle.ItemsSource = dt5.DefaultView;
                    CbTitle.DisplayMemberPath = "Title";
                    CbTitle.SelectedValuePath = "ID";
                    //////----------------------------------------------
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

            private void BtnAdd_Click(object sender, RoutedEventArgs e) //Добавление
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                connection.Open();
                if (String.IsNullOrEmpty(TbNumber.Text) || String.IsNullOrEmpty(CbClass.Text) || CbNumKab.SelectedIndex == -1 || CbCondition.SelectedIndex == -1 || CbTitle.SelectedIndex ==-1)
                {
                    MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                int id, id2, id3, id4;
                bool resultClass = int.TryParse(CbClass.SelectedValue.ToString(), out id);
                bool resultKab = int.TryParse(CbNumKab.SelectedValue.ToString(), out id2);
                bool resultCon = int.TryParse(CbCondition.SelectedValue.ToString(), out id3);
                bool resultTitl = int.TryParse(CbTitle.SelectedValue.ToString(), out id4);
                var UserAdd = Saver.ID;
                var numkab = TbNumber.Text;
                var number = TbNumber.Text;
                var idtype = CbClass.Text;
                var idcon = CbCondition.Text;
                var startWork = StartWork.Text;

                    string query = $@"INSERT INTO Devices(IDType,IDKabuneta,IDTitle,Number,IDCondition,StartWork,IDAddUser) values ('{id}',{id2},'{id4}','{number}','{id3}','{startWork}',{UserAdd});";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Информация добавленна");
                        this.Close();
                    }

                    catch(System.Data.SQLite.SQLiteException)
                    {
                        MessageBox.Show("Такой номер занят!" );
                        TbNumber.Clear();
                    }
                }
            }
        }

        private void BtnAddKab_Click(object sender, RoutedEventArgs e)
        {
            Eddit Edd = new Eddit();
            Edd.Owner = this;
            bool? result = Edd.ShowDialog();
            switch (result)
            {
                default:
                CbFill();
                break;
            }
        }

        private void BtnDellKab_Click(object sender, RoutedEventArgs e)
        {
            DeleteKab();
            CbFill();
        }
        public void DeleteKab()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {


                if (CbNumKab.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите какой кабинет нужно удалить!!!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    
                    int IdKab;
                    bool NumKab = int.TryParse(CbNumKab.SelectedValue.ToString(), out IdKab);
                    try
                    {
                        string query1 = $@"DELETE FROM NumberKabs WHERE id =  '{IdKab}'";
                        connection.Open();
                        SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                        DataTable DT = new DataTable("NumberKabs");
                        cmd1.ExecuteNonQuery();
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }

                }
            }
        }

        private void BtnAddTitl_Click(object sender, RoutedEventArgs e)
        {
            EdditTitle EddTitl = new EdditTitle();
            EddTitl.Owner = this;
            bool? result = EddTitl.ShowDialog();
            switch (result)
            {
                default:
                    CbFill();
                    break;
            }
        }

        public void DeleteTitl()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {


                if (CbTitle.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите какой кабинет нужно удалить!!!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {

                    int IdTil;
                    bool NumTitl = int.TryParse(CbTitle.SelectedValue.ToString(), out IdTil);
                    try
                    {
                        string query1 = $@"DELETE FROM NumberKabs WHERE id =  '{IdTil}'";
                        connection.Open();
                        SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                        DataTable DT = new DataTable("Titles");
                        cmd1.ExecuteNonQuery();
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }

                }
            }
        }
            
        private void BtnDellTilt_Click(object sender, RoutedEventArgs e)
        {
            DeleteTitl();
            CbFill();
        }

    }
}
