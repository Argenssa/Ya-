using Microsoft.Win32;
using Npgsql;
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

namespace Ya_.VIEW
{
    /// <summary>
    /// Логика взаимодействия для SongManager.xaml
    /// </summary>
    public partial class SongManager : UserControl
    {
        public SongManager()
        {
            InitializeComponent();
        }
        string SongWay;
        string ImageWay;
        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio files (*.mp3;*.wav)|*.mp3;*.wav|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                SongWay = openFileDialog.FileName;
                // обработать выбранный файл здесь

            }
        }

        private void SelectCoverButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.png)|*.jpg;*.png|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImageWay = openFileDialog.FileName;


                // обработать выбранный файл здесь
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string artist = ArtistTextBox.Text;
            string[] artist_arr = artist.Split(',');


            string genre = GenreTextBox.Text;

            string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
            NpgsqlConnection iConnect = new NpgsqlConnection(connect);
            iConnect.Open();
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connect))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT public.\"add_song\"(@song_name,@autor_name,@genre_name,@song_file,@song_image)", conn))
                    {
                        cmd.Parameters.Add("@song_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = title;
                        cmd.Parameters.AddWithValue("@autor_name", artist_arr);
                        cmd.Parameters.Add("@genre_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = genre;
                        cmd.Parameters.Add("@song_file", NpgsqlTypes.NpgsqlDbType.Varchar).Value = SongWay;
                        cmd.Parameters.Add("@song_image", NpgsqlTypes.NpgsqlDbType.Varchar).Value = ImageWay;

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

      private void CancelButton_Click(object sender, EventArgs e)
        {
            TitleTextBox.Clear();
            ArtistTextBox.Clear();
            GenreTextBox.Clear();
            SongWay = string.Empty;
            ImageWay = string.Empty;
        }
    }
}
