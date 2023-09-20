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
    /// Логика взаимодействия для RegistationWin.xaml
    /// </summary>
    public partial class RegistationWin : Window
    {
        public RegistationWin()
        {
            InitializeComponent();
            CloseApp.Click += CloseApp_Click;
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        bool em;
        bool pas;
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
                else
                {
                    em = true;
                }

            }
            textBlock.Text = email;
            textBlock.Foreground = Brushes.Black;
        }
        private void EmailTextBlock_TextChanged(object sender, RoutedEventArgs e)
        {
            ValidateEmailTextBlock((TextBox)sender);
        }
        int id;

        private void Back_to_autorization(object sender, RoutedEventArgs e)
        {
            Registration_Autorization registration_Autorization = new Registration_Autorization();
            registration_Autorization.Show();
            this.Close();
        }

        private void Create_Acc(object sender, RoutedEventArgs e)
        {

            if (em && pas)
            {

                string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
                NpgsqlConnection iConnect = new NpgsqlConnection(connect);
                iConnect.Open();
                string email = txtEmail.Text;

                string password = txtPassword.Password;

                byte[] encrypted = ProtectedData.Protect(Encoding.Unicode.GetBytes(password), null, DataProtectionScope.CurrentUser);
                using (NpgsqlConnection conn = new NpgsqlConnection(connect))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT public.\"add_user\"(2, @Password,@Email)", conn))
                    {
                        cmd.Parameters.Add("@Email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = email;
                        cmd.Parameters.Add("@Password", NpgsqlTypes.NpgsqlDbType.Bytea).Value = encrypted;

                        id = (int)cmd.ExecuteScalar();

                    }
                    conn.Close();
                }
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(em.ToString() + " " + pas.ToString());
            }
        }

        private void PasswordBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (IsValidPassword(((PasswordBox)sender).Password))
            {
                pas = true;
                // пароль соответствует правилам
                // добавьте здесь дополнительный код, если необходимо
            }
            else
            {
                MessageBox.Show("Пароль должен содержать не менее 8 символов.\r\n\r\nПароль должен содержать хотя бы одну заглавную букву.\r\n\r\nПароль должен содержать хотя бы одну строчную букву.\r\n\r\nПароль должен содержать хотя бы одну цифру.\r\n\r\nПароль должен содержать хотя бы один специальный символ");
                // пароль не соответствует правилам
                // добавьте здесь код для уведомления пользователя об ошибке, если необходимо
            }
        }
        private bool IsValidPassword(string password)
        {
            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
            return regex.IsMatch(password);
        }

        private void clear(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == "Некорректный адрес электронной почты")
                txtEmail.Text = string.Empty;
        }
    }
}
