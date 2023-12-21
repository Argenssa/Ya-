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
    /// Логика взаимодействия для DelWindow.xaml
    /// </summary>
    public partial class DelWindow : UserControl
    {
        public DelWindow()
        {
            InitializeComponent();
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {



            string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
            NpgsqlConnection iConnect = new NpgsqlConnection(connect);
            iConnect.Open();
            using (NpgsqlConnection conn = new NpgsqlConnection(connect))
            {
                try
                {
                    if((TitleTextBox.Text == string.Empty && ArtistTextBox.Text == string.Empty)||GenreTextBox.Text == string.Empty)  { }
                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("SELECT public.\"delete_track\"(@song_name,@autor_name,@id)", conn))
                    {
                        command.Parameters.AddWithValue("@song_name", string.IsNullOrEmpty(TitleTextBox.Text) ? (object)DBNull.Value : TitleTextBox.Text);
                        command.Parameters.AddWithValue("@autor_name", string.IsNullOrEmpty(ArtistTextBox.Text) ? (object)DBNull.Value : ArtistTextBox.Text);
                        command.Parameters.AddWithValue("@id", string.IsNullOrEmpty(GenreTextBox.Text) ? (object)DBNull.Value : int.Parse(GenreTextBox.Text));
                        command.ExecuteNonQuery();
                    }
                    conn.Close();
                    MessageBox.Show("Трек удаляен");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                TitleTextBox.Text = String.Empty;
                ArtistTextBox.Text = String.Empty;
                GenreTextBox.Text = String.Empty;
            }

        }
    }

