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
using System.Windows.Shapes;

namespace Ya_
{
    /// <summary>
    /// Логика взаимодействия для AddPlaylist.xaml
    /// </summary>
    public partial class AddPlaylist : Window
    {
        int user_id = MainWindow.User_Id;
        public AddPlaylist()
        {
            InitializeComponent();
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string name = IdTextBox.Text;
                if (name == string.Empty)
                {
                    throw new Exception(message: "Введите название");
                }

                string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
                NpgsqlConnection iConnect = new NpgsqlConnection(connect);
                iConnect.Open();
                using (NpgsqlConnection conn = new NpgsqlConnection(connect))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT public.\"create_playlist\"(@id,@name)", conn))
                    {
                        cmd.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Integer).Value = user_id;
                        cmd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = name;





                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                // сохранить информацию о песне здесь
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }
    }
}
