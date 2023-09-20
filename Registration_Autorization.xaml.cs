using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Ya_.Classes_for_Bd;

namespace Ya_
{
    /// <summary>
    /// Логика взаимодействия для Registration_Autorization.xaml
    /// </summary>
    public partial class Registration_Autorization : Window
    {
        public Registration_Autorization()
        {
            InitializeComponent();
            CloseApp.Click += CloseApp_Click;
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void openRegWin(object sender, RoutedEventArgs e)
        {
            RegistationWin registationWin = new RegistationWin();
            registationWin.Show();
            this.Close();
        }
        private void ValidateEmailTextBlock(TextBox textBlock)
        {
            string email = textBlock.Text.Trim();
            if (!string.IsNullOrEmpty(email))
            {
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    textBlock.Text = "Некорректный адрес электронной почты";
                    textBlock.Foreground = Brushes.Red;
                    return;
                }
            }
            textBlock.Text = email;
            textBlock.Foreground = Brushes.Black;
        }
        private void EmailTextBlock_TextChanged(object sender, RoutedEventArgs e)
        {
            ValidateEmailTextBlock((TextBox)sender);
        }
        private void Autorizationn(object sender, RoutedEventArgs e)
        {
            string connectionString = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
            string email = txtEmail.Text;
            string password = txtPassword.Password;


            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand iQuery = new NpgsqlCommand("Select * from public.\"USERS\" ", connection)
                {
                    Connection = connection
                };
                NpgsqlDataAdapter iAdapter = new NpgsqlDataAdapter(iQuery);

                DataSet iDataSet = new DataSet();
                iAdapter.Fill(iDataSet, "USERS");

                int lstCount = iDataSet.Tables["USERS"].Rows.Count;//lstCount holds the total count of the list from database

                int i = 0;//used as counter
                List<Users> items = new List<Users>();
                while (lstCount > i)
                {

                    items.Add(new Users()
                    {

                        Id = Convert.ToInt32(iDataSet.Tables["USERS"].Rows[i]["Id"]),
                        Role_id = Convert.ToInt32(iDataSet.Tables["USERS"].Rows[i]["ROLE_ID"]),
                        Email = iDataSet.Tables["USERS"].Rows[i]["EMAIL"].ToString(),
                        Password = (byte[])iDataSet.Tables["USERS"].Rows[i]["PASSWORD"],

                    });



                    i++;
                }
                int count = 0;

                for (int j = 0; j < items.Count; j++)
                {
                    byte[] decryptedPassword = ProtectedData.Unprotect(items[j].Password, null, DataProtectionScope.CurrentUser);
                    string decryptedPasswordString = Encoding.Unicode.GetString(decryptedPassword);
                    if (email == items[j].Email && password == decryptedPasswordString)
                    {
                        MainWindow mw = new MainWindow();
                        AdminPage adminPage = new AdminPage();
                        if (items[j].Role_id != 1)
                        {
                            mw.Show();
                            this.Close();
                            break;
                        }
                        else
                        {
                            adminPage.Show();
                            this.Close();

                            break;
                        }

                    }
                    else
                    {
                        count++;
                    }

                }
                if (count == items.Count)
                {
                    MessageBox.Show("Неверный логин или пороль");
                }



                connection.Close();

            }
        }

        private void clear(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == "Некорректный адрес электронной почты")
                txtEmail.Text = string.Empty;
        }
    }
}

