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
    /// Логика взаимодействия для Statistics.xaml
    /// </summary>
    public partial class Statistics : UserControl
    {
        public Statistics()
        {

            InitializeComponent();
            functions();
        }

        private void functions()
        {
            string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
            NpgsqlConnection conn = new NpgsqlConnection(connect);
            conn.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT public.GetUsersCount()", conn))
            {
                // Выполнение команды и получение результата
                int userCount = (int)command.ExecuteScalar();
                t1.Text += "  " + userCount.ToString();
            }
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT public.GetTotalListens()", conn))
            {
                // Выполнение команды и получение результата
                int totalListens = (int)command.ExecuteScalar();
                t2.Text += "  " + totalListens.ToString();
            }

            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.GetMostListenedTrack()", conn))
            {
                // Выполнение команды и получение результата
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string trackName = reader.GetString(0);
                        string artistName = reader.GetString(1);
                        t3.Text += "  " + trackName + "  " + artistName;
                    }
                }
            }

            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.GetMostLikedTrack()", conn))
            {
                // Выполнение команды и получение результата
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string trackName = reader.GetString(0);
                        string artistName = reader.GetString(1);
                        t4.Text += "  " + trackName + "  " + artistName;

                    }
                }
            }

            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.GetMostFrequentTrack()", conn))
            {
                // Выполнение команды и получение результата
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string trackName = reader.GetString(0);
                        string artistName = reader.GetString(1);
                        t5.Text += "  " + trackName + "  " + artistName;

                    }
                }
            }

            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.GetUserWithMostLikes()", conn))
            {
                // Выполнение команды и получение результата
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string userEmail = reader.GetString(0);
                        t6.Text += "  " + userEmail;

                    }
                }
            }

            using (NpgsqlCommand command = new NpgsqlCommand("SELECT public.GetTotalTrackCount()", conn))
            {
                // Выполнение команды и получение результата
                int totalCount = (int)command.ExecuteScalar();
                t7.Text += "  " + totalCount.ToString();

            }
        }
        }
}
