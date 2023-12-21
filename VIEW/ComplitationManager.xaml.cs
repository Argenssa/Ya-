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
                   
                        
                        if (title == string.Empty || genre == string.Empty || kol == 0)
                        {
                            throw new Exception("Введеите данные коректно");
                        }
                        conn.Open();
                        using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT public.\"create_competition\"(@name,@genre,@kol)", conn))
                        {
                            cmd.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = title;
                            cmd.Parameters.Add("@genre", NpgsqlTypes.NpgsqlDbType.Varchar).Value = genre;
                            cmd.Parameters.Add("@kol", NpgsqlTypes.NpgsqlDbType.Integer).Value = kol;


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
