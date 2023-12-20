using Npgsql;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using Ya_.Classes_for_Bd;

namespace Ya_.VIEW
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : UserControl
    {

        readonly int user_id = MainWindow.User_Id;
        public Profile()
        {
            InitializeComponent();
            LoadComplitations();
        }
        List<Playlist> complitations = new List<Playlist>();
        private void LoadComplitations()
        {
            Playlistss.Children.Clear();
            complitations.Clear();
            string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");

            NpgsqlConnection conn = new NpgsqlConnection(connect);
            conn.Open();

            using (NpgsqlCommand cmd = new NpgsqlCommand(" SELECT * FROM public.\"list_of_playlists\"(@id)", conn))
            {
                cmd.Parameters.AddWithValue("@id", user_id);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Playlist s = new Playlist
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),

                    };


                    complitations.Add(s);
                }


                for (int i = 0; i < complitations.Count; i++)
                {
                    Border br = new Border();
                    //  br.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B3838"));
                    br.Width = 250;
                    br.Height = 300;
                    br.Margin = new Thickness(20, 10, 20, 15);
                    Grid grid = new Grid();
                    br.Child = grid;
                    RowDefinition c1 = new RowDefinition
                    {
                        Height = new GridLength(250)
                    };
                    RowDefinition c2 = new RowDefinition
                    {
                        Height = new GridLength(50)
                    };
                    grid.RowDefinitions.Add(c1);
                    grid.RowDefinitions.Add(c2);
                    Rectangle rect = new Rectangle
                    {
                        Width = 230,
                        Height = 230,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };

                    //*********Image Ellipse**************//
                    Random random = new Random(i);
                    Color color1 = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
                    Color color2 = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));

                    // создание градиентной заливки
                    GradientStopCollection gradientStops = new GradientStopCollection();
                    gradientStops.Add(new GradientStop(color1, 0));
                    gradientStops.Add(new GradientStop(color2, 1));
                    LinearGradientBrush gradientBrush = new LinearGradientBrush(gradientStops, new Point(0, 0), new Point(1, 1));
                    rect.Fill = gradientBrush;
                    grid.Children.Add(rect);
                    Grid.SetRow(rect, 0);
                    Playlistss.Children.Add(br);
                    TextBlock t1 = new TextBlock();
                    t1.Text = complitations[i].Name;
                    t1.Margin = new Thickness(0, 5, 0, 0);
                    t1.Foreground = new SolidColorBrush(Colors.White);
                    t1.FontSize = 18;
                    t1.HorizontalAlignment = HorizontalAlignment.Center;
                    t1.VerticalAlignment = VerticalAlignment.Center;
                    grid.Children.Add(t1);
                    br.Tag = complitations[i].Id;
                    br.PreviewMouseLeftButtonDown += Br_PreviewMouseLeftButtonDown;
                    Grid.SetRow(t1, 1);
                }
                reader.Close();
            }
            conn.Close();
        }
        private void Br_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            int tag = (int)border.Tag;
            ModalPlaylist modal = new ModalPlaylist(tag);
             // Изменено здесь
            modal.ShowDialog();
        }

        private void Modal_Closing(object? sender, CancelEventArgs e) // Изменено здесь
        {
            LoadComplitations();
        }


        private void OpemModal(object sender, MouseButtonEventArgs e)
        {
            AddPlaylist add = new AddPlaylist();
            add.Closing += Modal_Closing;
            add.Closing += Modal_Closing;
            add.ShowDialog();
          
        }

    }
}
