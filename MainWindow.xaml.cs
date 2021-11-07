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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DisplayData();
            CbFill();
            DGAllEmp.Columns[0].IsReadOnly = true;
            DGAllEmp.Columns[1].IsReadOnly = true;
            DGAllEmp.Columns[2].IsReadOnly = true;
            DGAllEmp.Columns[3].IsReadOnly = true;
            DGAllEmp.Columns[4].IsReadOnly = true;
            BtnEdd.IsEnabled = false;
        }
        void LoadImage()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT  Devices.Photo";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                   
                }
                catch (Exception exc) { MessageBox.Show(exc.Message); }
            }
        }
            

        public void DisplayData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query = $@"SELECT Devices.ID, Types.Class, Titles.Title, Devices.Number, Conditions.Condition ,NumberKabs.NumKab ,Devices.StartWork,Users.Login
                                        FROM Devices JOIN  Types
                                        ON Devices.IDType = Types.ID
                                        JOIN  Conditions
                                        ON Devices.IDCondition = Conditions.ID
                                        JOIN  NumberKabs
                                        ON Devices.IDKabuneta = NumberKabs.ID
                                        JOIN Titles
                                        ON Devices.IDTitle = Titles.ID
										JOIN Users
										ON Devices.IDAddUser = Users.ID;";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    DataTable DT = new DataTable("Devices");
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    SDA.Fill(DT);
                    DGAllEmp.ItemsSource = DT.DefaultView;
                    Login.Text = $"Ваш логин: " + Saver.Login;
                    

                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
        }
        public void Delete()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {

                    foreach (var item in DGAllEmp.SelectedItems.Cast<DataRowView>())
                    {
                        string query1 = $@"DELETE FROM Devices WHERE ID = " + item["ID"];
                        connection.Open();

                        SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                        DataTable DT = new DataTable("Devices");
                        cmd1.ExecuteNonQuery();
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
        }
        private void BtnUpd_Click(object sender, RoutedEventArgs e)
        {
          
            DisplayData();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Delete();
            DisplayData();
         
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTechnic AddTec = new AddTechnic();
            AddTec.Owner = this;
            AddTec.ShowDialog();
            DisplayData();
           


        }

        private void DGAllEmp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CbFill();
            BtnEdd.IsEnabled = true;
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                if (DGAllEmp.SelectedItems.Count > 0)
                {
                    DataRowView row = (DataRowView)DGAllEmp.SelectedItems[0];
                    CbClass.Text = row["Class"].ToString();
                    CbTitle.Text = row["Title"].ToString();
                    CbNumKab.Text = row["NumKab"].ToString();
                    TbNumber.Text = row["Number"].ToString();
                    CbCondition.Text = row["Condition"].ToString();
                    StartWork.Text = row["StartWork"].ToString();
                    TbID.Text = row["ID"].ToString();
                  
                }
                else
                {
                    MessageBox.Show("Please Select Any Employee From List...");
                }


            }
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
                    string query5 = $@"SELECT * FROM Titles"; // Устройства
                    //----------------------------------------------
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    SQLiteCommand cmd3 = new SQLiteCommand(query3, connection);
                    SQLiteCommand cmd5 = new SQLiteCommand(query5, connection);
                    //----------------------------------------------
                    SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                    SQLiteDataAdapter SDA2 = new SQLiteDataAdapter(cmd2);
                    SQLiteDataAdapter SDA3 = new SQLiteDataAdapter(cmd3);
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnEdd_Click(object sender, RoutedEventArgs e) //Изменение
        {
            if (TbID.Text == null)
            {
                MessageBox.Show("Выберите в таблице строку изменения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                BtnEdd.IsEnabled = false;
            }
            else
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    int id, id2, id3,id4;
                    bool resultClass = int.TryParse(CbClass.SelectedValue.ToString(), out id);
                    bool resultKab = int.TryParse(CbNumKab.SelectedValue.ToString(), out id2);
                    bool resultCon = int.TryParse(CbCondition.SelectedValue.ToString(), out id3);
                    bool resultTitl = int.TryParse(CbTitle.SelectedValue.ToString(), out id4);
                   
                    var numkab = TbNumber.Text;
                    var number = TbNumber.Text;
                    var idtype = CbClass.Text;
                   
                    var idcon = CbCondition.Text;
                    var startWork = StartWork.Text;
                    var ID = TbID.Text;
                   
                    connection.Open();

                    
                    string query = $@"UPDATE Devices SET IDType=@IDType, IDKabuneta=@IDKabuneta, IDTitle=@IDTitle, Number=@Number, IDCondition=@IDCondition, StartWork=@StartWork WHERE ID=@ID;";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    try
                    {
                        cmd.Parameters.AddWithValue("@IDType", id);
                        cmd.Parameters.AddWithValue("@IDKabuneta", id2);
                        cmd.Parameters.AddWithValue("@IDTitle", id4);
                        cmd.Parameters.AddWithValue("@Number", number);
                        cmd.Parameters.AddWithValue("@IDCondition", id3);
                        cmd.Parameters.AddWithValue("@StartWork", startWork);
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Данные изменены");
                        DisplayData();
                       

                    }

                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Excel.Worksheet sheet1 = (Excel.Worksheet)workbook.Sheets[1];
            for (int j = 0; j < DGAllEmp.Columns.Count; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = DGAllEmp.Columns[j].Header;
            }
            for (int i = 0; i < DGAllEmp.Columns.Count; i++)
            {
                for (int j = 0; j < DGAllEmp.Items.Count; j++)
                {
                    TextBlock b = DGAllEmp.Columns[i].GetCellContent(DGAllEmp.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
        }

        private static string RndStr(int len)//геннератор пароля(временного)
        {
            string s = "", symb = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
            Random rnd = new Random();

            for (int i = 0; i < len; i++)
                s += symb[rnd.Next(0, symb.Length)];
            return s;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Authoriz aut = new Authoriz();
            aut.Show();
            this.Close();
        }
    }
}

