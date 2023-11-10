using Microsoft.Win32;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Ya_.VIEW
{
    /// <summary>
    /// Логика взаимодействия для ComplitationManager.xaml
    /// </summary>
    public partial class ComplitationManager : UserControl
    {
        public ComplitationManager()
        {
            InitializeComponent();
        }
        byte[] ImageWay;
        private void SelectCoverButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.png)|*.jpg;*.png|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                // Read the image file into a byte array
                byte[] imageData = File.ReadAllBytes(imagePath);

                // Save the byte array to a class-level variable
                ImageWay = imageData;

                // обработать выбранный файл здесь
            }
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string title = TitleTextBox.Text;
                string genre = GenreTextBox.Text;
                int kol = Convert.ToInt32(KolTextBox.Text);


                string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
                NpgsqlConnection iConnect = new NpgsqlConnection(connect);
                iConnect.Open();
                using (NpgsqlConnection conn = new NpgsqlConnection(connect))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT public.\"create_competition\"(@name,@genre,@kol,@way)", conn))
                    {
                        cmd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = title;
                        cmd.Parameters.Add("@genre", NpgsqlTypes.NpgsqlDbType.Varchar).Value = genre;
                        cmd.Parameters.Add("@kol", NpgsqlTypes.NpgsqlDbType.Integer).Value = kol;
                        cmd.Parameters.Add("@way", NpgsqlTypes.NpgsqlDbType.Bytea).Value = ImageWay;


                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            TitleTextBox.Clear();
            GenreTextBox.Clear();
            KolTextBox.Clear();
            

        }
    }
}
