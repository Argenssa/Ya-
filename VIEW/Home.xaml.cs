using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {

        readonly int user_id = MainWindow.User_Id;
        readonly int track_kol = 50;
        int page_number = 1;
        private readonly MediaPlayer mediaPlayer = new();
        public Home()
        {
          
            LoadSongs(page_number);
            LoadRecentSongs(user_id);
           
            InitializeComponent();
        }
        readonly List<Songss> songs = new();
        readonly List<Songss> Recent_songs = new();
        readonly List<Playlist> complitations = new();
        List<int> likedTracks = new List<int>();

        private void LoadSongs(int pageNumber)
        {
            
            string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
            NpgsqlConnection conn = new(connect);
            conn.Open();

            using (NpgsqlCommand cmd = new("SELECT * FROM public.\"songs_for_player\"(@size, @number)", conn))
            {
                cmd.Parameters.AddWithValue("@size", track_kol);
                cmd.Parameters.AddWithValue("@number", pageNumber);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    Songss s = new Songss
                    {
                        Id = reader.GetInt32(0),
                        SongName = reader.GetString(1),
                        GenreName = reader.GetString(2),
                        SongWay = reader.GetString(3),
                        SongImg = reader.GetString(4),
                        Autor = reader.GetString(5)
                    };
                    
                    songs.Add(s);
                }
               
                reader.Close();
            }
            
            conn.Close();
            
        }

        private void LoadRecentSongs(int pageNumber)
        {
            Recent_songs.Clear();
            string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
            NpgsqlConnection conn = new(connect);
            conn.Open();

            using (NpgsqlCommand cmd = new("SELECT * FROM public.\"songs_for_recently\"(@id)", conn))
            {
                cmd.Parameters.AddWithValue("@id", user_id);
                

                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Songss s = new Songss
                    {
                        Id = reader.GetInt32(0),
                        SongName = reader.GetString(1),
                        GenreName = reader.GetString(2),
                        SongWay = reader.GetString(3),
                        SongImg = reader.GetString(4),
                        Autor = reader.GetString(5)
                    };

                    Recent_songs.Add(s);
                }

                reader.Close();
            }
            Debug.WriteLine(Recent_songs[0].SongName);
            conn.Close();
           
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset + e.ViewportHeight == e.ExtentHeight)
            {
                page_number++;
                LoadSongs(page_number);
                UpdateTracks();
               ShowRecentTracks();
            }
            else if (e.VerticalOffset == 0)
            {
                if (page_number > 1)
                {
                    page_number--;
                    LoadSongs(page_number);
                    UpdateTracks();
                    ShowRecentTracks();
                }
            }
        }

        private void ScrollViewer_ScrollChanged2(object sender, ScrollChangedEventArgs e)
        {
            if (e.HorizontalOffset + e.ViewportWidth + 300 >= e.ExtentWidth)
            {
                
                
                UpdateTracks();
                ShowRecentTracks();
            }
            else if (e.HorizontalOffset == 0)
            {
                if (page_number > 1)
                {
                  
                    UpdateTracks();
                    ShowRecentTracks();
                }
            }
        }
        private void UpdateTracks()
        {            
                Tracks.Children.Clear();
           
            string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
            NpgsqlConnection conn = new NpgsqlConnection(connect);
            conn.Open();
            using (NpgsqlCommand cmd = new NpgsqlCommand(" SELECT * FROM public.\"list_of_playlists\"(@id)", conn))
            {
                cmd.Parameters.AddWithValue("@id", user_id);

                NpgsqlDataReader reader = cmd.ExecuteReader();
                complitations.Clear();
                while (reader.Read())
                {
                    Playlist s = new Playlist
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),

                    };


                    complitations.Add(s);
                }
                reader.Close();
            }
            likedTracks.Clear();

            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM get_liked_tracks(@user_id)", conn))
            {
                command.Parameters.AddWithValue("@user_id", user_id);
                command.CommandType = CommandType.Text;

              

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int trackId = reader.GetInt32(0);
                        likedTracks.Add(trackId);
                    }
                }

            }
                    using (NpgsqlCommand cmd = new NpgsqlCommand(" SELECT * FROM public.\"songs_for_player\"(@size,@number)", conn))
            {
                cmd.Parameters.AddWithValue("@size", track_kol);
                cmd.Parameters.AddWithValue("@number", page_number);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Songss s = new Songss
                    {
                        Id = reader.GetInt32(0),
                        SongName = reader.GetString(1),
                        GenreName = reader.GetString(2),
                        SongWay = reader.GetString(3),
                        SongImg = reader.GetString(4),
                        Autor = reader.GetString(5)
                    };
                    songs.Add(s);
                }

                // Создаем список элементов MenuItem
                List<MenuItem> list = new List<MenuItem>();
                list.Clear();
                for (int i = 0; i < complitations.Count; i++)
                {
                    MenuItem menuItem = new MenuItem
                    {
                        Header = complitations[i].Name,
                        Tag = complitations[i].Id
                    };
                    menuItem.Click += MenuItem_Click;
                    list.Add(menuItem);
                }

                // Добавляем все элементы в контекстное меню
                ContextMenu contextMenu = new ContextMenu();

                foreach (var menuItem in list)
                {
                    contextMenu.Items.Add(menuItem);
                }

                contextMenu.Background = new SolidColorBrush(Colors.Black);
                contextMenu.Foreground = new SolidColorBrush(Colors.White);
                Style baseStyle = (Style)FindResource(typeof(DataGridColumnHeader));
                Style style = new Style(typeof(DataGridColumnHeader), baseStyle);
                style.Setters.Add(new Setter(DataGridColumnHeader.WidthProperty, 0.0));
                contextMenu.Resources.Add(typeof(DataGridColumnHeader), style);



                for (int i = 0; i < songs.Count; i++)
                {
                    Border br = new Border
                    {
                        CornerRadius = new CornerRadius(20),
                        Width = 930,
                        Height = 60,
                        BorderBrush = new SolidColorBrush(Colors.White),
                        BorderThickness = new Thickness(1, 1, 1, 1),
                        Margin = new Thickness(0, 1, 0, 1)
                    };
                    Grid gr1 = new Grid();
                    ColumnDefinition c1 = new ColumnDefinition
                    {
                        Width = new GridLength(100)
                    };
                    ColumnDefinition c2 = new ColumnDefinition
                    {
                        Width = new GridLength(708)
                    };
                    ColumnDefinition c3 = new ColumnDefinition
                    {
                        Width = new GridLength(120)
                    };
                    gr1.ColumnDefinitions.Add(c1);
                    gr1.ColumnDefinitions.Add(c2);
                    gr1.ColumnDefinitions.Add(c3);

                    Ellipse el = new Ellipse
                    {
                        Width = 40,
                        Height = 40,
                        Margin = new Thickness(8, 0, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left
                    };

                    //*********Image Ellipse**************//
                    BitmapImage image = new BitmapImage(new Uri(songs[i].SongImg, UriKind.Relative));
                    ImageBrush brush = new ImageBrush(image);
                    el.Fill = brush;
                    gr1.Children.Add(el);
                    Grid.SetColumn(el, 0);
                    //*********Autor and song Name**************//
                    StackPanel st = new StackPanel();
                    gr1.Children.Add(st);
                    Grid.SetColumn(st, 1);
                    Grid gr2 = new Grid();
                    RowDefinition r1 = new RowDefinition
                    {
                        Height = new GridLength(30)
                    };
                    RowDefinition r2 = new RowDefinition
                    {
                        Height = new GridLength(30)
                    };
                    gr2.RowDefinitions.Add(r1);
                    gr2.RowDefinitions.Add(r2);
                    TextBlock t1 = new TextBlock();
                    TextBlock t2 = new TextBlock();
                    t1.Text = songs[i].SongName;
                    t2.Text = songs[i].Autor;
                    t1.Margin = new Thickness(0, 2, 0, 0);
                    t2.Margin = new Thickness(0, 2, 0, 0);
                    t1.Foreground = new SolidColorBrush(Colors.White);
                    t1.FontSize = 22;
                    t2.Foreground = new SolidColorBrush(Colors.White);
                    t2.FontSize = 16;
                    gr2.Children.Add(t1);
                    gr2.Children.Add(t2);
                    Grid.SetRow(t1, 0);
                    Grid.SetRow(t2, 1);
                    st.Children.Add(gr2);
                    //************Icons**********//
                    StackPanel stack = new StackPanel();
                    gr1.Children.Add(stack);
                    Grid.SetColumn(stack, 2);
                    Grid gr3 = new Grid();
                    stack.Children.Add(gr3);
                    ColumnDefinition column = new ColumnDefinition
                    {
                        Width = new GridLength(40)
                    };
                    ColumnDefinition column2 = new ColumnDefinition
                    {
                        Width = new GridLength(40)
                    };
                    ColumnDefinition column3 = new ColumnDefinition
                    {
                        Width = new GridLength(40)
                    };
                    gr3.ColumnDefinitions.Add(column);
                    gr3.ColumnDefinitions.Add(column2);
                    gr3.ColumnDefinitions.Add(column3);

                    Ellipse ellipse = new Ellipse
                    {
                        Margin = new Thickness(0, 15, 0, 0),
                        Width = 30,
                        Height = 30,
                        Fill = new ImageBrush(new BitmapImage(new Uri("C:\\coursProj4sem\\Ya!\\Icons\\icons8-play-30.png", UriKind.Relative))),
                        Tag = songs[i].Id
                    };
                    ellipse.MouseLeftButtonDown += Img_MouseLeftButtonDown;

                    bool containts = likedTracks.Contains(songs[i].Id);
                    Ellipse ellipse2 = new Ellipse();
                    ellipse2.Margin = new Thickness(0, 15, 0, 0);
                    ellipse2.Width = 30;
                    ellipse2.Tag = songs[i].Id;
                    ellipse2.Height = 30;
                    ellipse2.MouseLeftButtonDown += Img2_MouseLeftButtonDown;
                    if (containts)
                    {
                        ellipse2.Fill = new ImageBrush(new BitmapImage(new Uri("C:\\coursProj4sem\\Ya!\\Icons\\icons8-музыка-в-сердце-64.png", UriKind.Relative)));                 
                        
                    }
                    else
                    {
                        ellipse2.Fill = new ImageBrush(new BitmapImage(new Uri("C:\\coursProj4sem\\Ya!\\Icons\\icons8-heart-plus-50.png", UriKind.Relative)));
                    }
                  

                    Ellipse ellipse3 = new Ellipse
                    {
                        Margin = new Thickness(0, 15, 0, 0),
                        Width = 30,
                        Height = 30,
                        Fill = new ImageBrush(new BitmapImage(new Uri("C:\\coursProj4sem\\Ya!\\Icons\\icons8-playlist-64.png", UriKind.Relative))),
                        Tag = songs[i].Id,
                        ContextMenu = contextMenu
                    };
                    ellipse3.MouseRightButtonDown += Img3_MouseRightButtonDown;

                    gr3.Children.Add(ellipse);
                    gr3.Children.Add(ellipse2);
                    gr3.Children.Add(ellipse3);
                    Grid.SetColumn(ellipse, 0);
                    Grid.SetColumn(ellipse2, 1);
                    Grid.SetColumn(ellipse3, 2);
                    br.Child = gr1;
                    Tracks.Children.Add(br);
                }
                reader.Close();
            }
            conn.Close();
        }

        private void LoadSongs()
        {
            Tracks.Children.Clear();


            page_number++;
        }

        private void LoadRecentSongs()
        {
            Recent_Tracks.Children.Clear();


            page_number++;
        }


        static void AddToPlaylist(int user_id, int playliist_id, int track_id)
        {
            string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
            NpgsqlConnection conn = new(connect);
            conn.Open();

            using (NpgsqlCommand cmd = new NpgsqlCommand(" SELECT * FROM public.\"add_to_playlist\"(@id,@pl,@tr)", conn))
            {
                cmd.Parameters.AddWithValue("@id", user_id);
                cmd.Parameters.AddWithValue("@pl", playliist_id);
                cmd.Parameters.AddWithValue("@tr", track_id);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        int tagg;

        


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = (MenuItem)sender;
            int playlistId = (int)clickedItem.Tag;

            // Вызываем вашу функцию с передачей параметров
            AddToPlaylist(user_id, playlistId, tagg);
        }

        // обработчик события клика по элементу img3
        private void Img3_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse img = (Ellipse)sender;
            tagg = (int)img.Tag;

            // Сохраняем ID трека в переменную currentTrackId

            // Отображаем контекстное меню для элемента img3

            ((FrameworkElement)sender).ContextMenu.IsOpen = true;
        }
        private void Img2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");

            NpgsqlConnection conn = new NpgsqlConnection(connect);
            conn.Open();
            Ellipse img = (Ellipse)sender;
            int tag = (int)img.Tag;
            using NpgsqlCommand cmd = new NpgsqlCommand(" SELECT public.\"add_to_liked\"(@user,@track)", conn);
            cmd.Parameters.AddWithValue("@user", user_id);
            cmd.Parameters.AddWithValue("@track", tag);
            cmd.ExecuteNonQuery();
            LoadSongs(page_number);
            UpdateTracks();
            ShowRecentTracks();
        }
        //play song
        private void Img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            int index = (int)ellipse.Tag;

            if (ellipse.Fill is ImageBrush imgBrush && imgBrush.ImageSource is BitmapImage bitmapImage)
            {
                if (bitmapImage.UriSource.OriginalString == "C:\\coursProj4sem\\Ya!\\Icons\\icons8-play-30.png")
                {
                    for (int i = 0; i < songs.Count; i++)
                    {
                        if (index == songs[i].Id)
                        {
                            mediaPlayer.Open(new Uri(songs[i].SongWay));
                            mediaPlayer.Play();
                            string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
                            NpgsqlConnection iConnect = new NpgsqlConnection(connect);
                            iConnect.Open();
                            using (NpgsqlConnection conn = new NpgsqlConnection(connect))
                            {
                                conn.Open();
                                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT public.\"add_recent_song\"(@track_id,@song_id)", conn))
                                {
                                    cmd.Parameters.Add("@track_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = index;
                                    cmd.Parameters.Add("@song_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = user_id;
                                   


                                    cmd.ExecuteNonQuery();
                                }
                                conn.Close();
                                LoadRecentSongs(user_id);
                                ShowRecentTracks();
                            }
                            ellipse.Fill = new ImageBrush(new BitmapImage(new Uri("C:\\coursProj4sem\\Ya!\\Icons\\icons8-pause-button-30.png", UriKind.Relative)));
                          
                            break;
                           
                        }
                    }
                    
                }
                else if (bitmapImage.UriSource.OriginalString == "C:\\coursProj4sem\\Ya!\\Icons\\icons8-pause-button-30.png")
                {
                    mediaPlayer.Pause();
                    ellipse.Fill = new ImageBrush(new BitmapImage(new Uri("C:\\coursProj4sem\\Ya!\\Icons\\icons8-play-30.png", UriKind.Relative)));
                }
            }
        }

        private void Img3_MouseRightButtonDown2(object sender, MouseButtonEventArgs e)
        {
            Ellipse img = (Ellipse)sender;
            string tag = (string)img.Tag;

            // Разделяем tag на id и index
            var tagParts = tag.Split('_');
            int id = int.Parse(tagParts[0]);


            ((FrameworkElement)sender).ContextMenu.IsOpen = true;
        }

        private void Img2_MouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");

            NpgsqlConnection conn = new NpgsqlConnection(connect);
            conn.Open();
            Ellipse img = (Ellipse)sender;
            string tag = (string)img.Tag;

            var tagParts = tag.Split('_');
            int id = int.Parse(tagParts[0]);
            using NpgsqlCommand cmd = new NpgsqlCommand(" SELECT public.\"add_to_liked\"(@user,@track)", conn);
            cmd.Parameters.AddWithValue("@user", user_id);
            cmd.Parameters.AddWithValue("@track", id);
            cmd.ExecuteNonQuery();
            LoadSongs(page_number);
            UpdateTracks();
            LoadRecentSongs(user_id);
            ShowRecentTracks();
        }

        private void Img_MouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            Ellipse img = (Ellipse)sender;
            string tag = (string)img.Tag;

            var tagParts = tag.Split('_');
            int id = int.Parse(tagParts[0]);

            if (img.Fill is ImageBrush imgBrush && imgBrush.ImageSource is BitmapImage bitmapImage)
            {
                if (bitmapImage.UriSource.OriginalString == "C:\\coursProj4sem\\Ya!\\Icons\\icons8-play-30.png")
                {
                    for (int i = 0; i < songs.Count; i++)
                    {
                        if (id == songs[i].Id)
                        {
                            mediaPlayer.Open(new Uri(songs[i].SongWay));
                            mediaPlayer.Play();
                       
                            img.Fill = new ImageBrush(new BitmapImage(new Uri("C:\\coursProj4sem\\Ya!\\Icons\\icons8-pause-button-30.png", UriKind.Relative)));

                            break;

                        }
                    }

                }
                else if (bitmapImage.UriSource.OriginalString == "C:\\coursProj4sem\\Ya!\\Icons\\icons8-pause-button-30.png")
                {
                    mediaPlayer.Pause();
                    img.Fill = new ImageBrush(new BitmapImage(new Uri("C:\\coursProj4sem\\Ya!\\Icons\\icons8-play-30.png", UriKind.Relative)));
                }
            }
        }


        private void ShowRecentTracks()
        {         
            Recent_Tracks.Children.Clear();
            string connect = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", 5432, "postgres", "SuperSasha2101", "MusicService");
            NpgsqlConnection conn = new NpgsqlConnection(connect);
            conn.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM get_liked_tracks(@user_id)", conn))
            {
                command.Parameters.AddWithValue("@user_id", user_id);
                command.CommandType = CommandType.Text;



                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int trackId = reader.GetInt32(0);
                        likedTracks.Add(trackId);
                    }
                }

            }

            ScrollViewer scrollViewer = new ScrollViewer
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Width = 930,
                MaxHeight = 100
            };

            Grid grid = new Grid
            {
                MinWidth = 930
            };

            UniformGrid stackPanel = new UniformGrid
            {
                Rows = 1,
                MinWidth = 930
               
            };


            // Создаем список элементов MenuItem
            List<MenuItem> list = new List<MenuItem>();
                list.Clear();
                for (int i = 0; i < complitations.Count; i++)
                {
                MenuItem menuItem = new MenuItem
                {
                    Header = complitations[i].Name,
                    Tag = complitations[i].Id + "_"+ i
                    };
                    menuItem.Click += MenuItem_Click;
                    list.Add(menuItem);
                }
                // Добавляем все элементы в контекстное меню
                ContextMenu contextMenu = new ContextMenu();
                foreach (var menuItem in list)
                {
                    contextMenu.Items.Add(menuItem);
                }
                contextMenu.Background = new SolidColorBrush(Colors.Black);
                contextMenu.Foreground = new SolidColorBrush(Colors.White);
                Style baseStyle = (Style)FindResource(typeof(DataGridColumnHeader));
                Style style = new Style(typeof(DataGridColumnHeader), baseStyle);
                style.Setters.Add(new Setter(DataGridColumnHeader.WidthProperty, 0.0));
                contextMenu.Resources.Add(typeof(DataGridColumnHeader), style);
                for (int i = 0; i < Recent_songs.Count; i++)
                {
                    Border br = new Border
                    {
                        CornerRadius = new CornerRadius(20),
                        MaxWidth = 475,
                        MaxHeight = 60,
                        BorderBrush = new SolidColorBrush(Colors.White),
                        BorderThickness = new Thickness(1, 1, 1, 1),
                        Margin = new Thickness(0, 1, 0, 1)
                    };
                    Grid gr1 = new Grid();
                    ColumnDefinition c1 = new ColumnDefinition
                    {
                        Width = new GridLength(100)
                    };
                    ColumnDefinition c2 = new ColumnDefinition
                    {
                        Width = new GridLength(243)
                    };
                    ColumnDefinition c3 = new ColumnDefinition
                    {
                        Width = new GridLength(120)
                    };
                    gr1.ColumnDefinitions.Add(c1);
                    gr1.ColumnDefinitions.Add(c2);
                    gr1.ColumnDefinitions.Add(c3);

                    Ellipse el = new Ellipse
                    {
                        Width = 40,
                        Height = 40,
                        Margin = new Thickness(8, 0, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left
                    };

                    BitmapImage image = new BitmapImage(new Uri(Recent_songs[i].SongImg, UriKind.Relative));
                    ImageBrush brush = new ImageBrush(image);
                    el.Fill = brush;
                    gr1.Children.Add(el);
                    Grid.SetColumn(el, 0);
                   
                    StackPanel st = new StackPanel();
                    gr1.Children.Add(st);
                    Grid.SetColumn(st, 1);
                    Grid gr2 = new Grid();
                    RowDefinition r1 = new RowDefinition
                    {
                        Height = new GridLength(30)
                    };
                    RowDefinition r2 = new RowDefinition
                    {
                        Height = new GridLength(30)
                    };
                    gr2.RowDefinitions.Add(r1);
                    gr2.RowDefinitions.Add(r2);
                    TextBlock t1 = new TextBlock();
                    TextBlock t2 = new TextBlock();
                    t1.Text = Recent_songs[i].SongName;
                    t2.Text = Recent_songs[i].Autor;
                    t1.Margin = new Thickness(0, 2, 0, 0);
                    t2.Margin = new Thickness(0, 2, 0, 0);
                    t1.Foreground = new SolidColorBrush(Colors.White);
                    t1.FontSize = 22;
                    t2.Foreground = new SolidColorBrush(Colors.White);
                    t2.FontSize = 16;
                    gr2.Children.Add(t1);
                    gr2.Children.Add(t2);
                    Grid.SetRow(t1, 0);
                    Grid.SetRow(t2, 1);
                    st.Children.Add(gr2);
                   
                    StackPanel stack = new StackPanel();
                    gr1.Children.Add(stack);
                    Grid.SetColumn(stack, 2);
                    Grid gr3 = new Grid();
                    stack.Children.Add(gr3);
                    ColumnDefinition column = new ColumnDefinition
                    {
                        Width = new GridLength(40)
                    };
                    ColumnDefinition column2 = new ColumnDefinition
                    {
                        Width = new GridLength(40)
                    };
                    ColumnDefinition column3 = new ColumnDefinition
                    {
                        Width = new GridLength(40)
                    };
                    gr3.ColumnDefinitions.Add(column);
                    gr3.ColumnDefinitions.Add(column2);
                    gr3.ColumnDefinitions.Add(column3);

                    Ellipse ellipse = new Ellipse
                    {
                        Margin = new Thickness(0, 15, 0, 0),
                        Width = 30,
                        Height = 30,
                        Fill = new ImageBrush(new BitmapImage(new Uri("C:\\coursProj4sem\\Ya!\\Icons\\icons8-play-30.png", UriKind.Relative))),
                        Tag = Recent_songs[i].Id+"_"+i
                    };
                    ellipse.MouseLeftButtonDown += Img_MouseLeftButtonDown2;

                bool containts = likedTracks.Contains(Recent_songs[i].Id);
                Ellipse ellipse2 = new Ellipse();
                ellipse2.Margin = new Thickness(0, 15, 0, 0);
                ellipse2.Width = 30;
                ellipse2.Tag = Recent_songs[i].Id+"_"+1;
                ellipse2.Height = 30;
                ellipse2.MouseLeftButtonDown += Img2_MouseLeftButtonDown2;
                if (containts)
                {
                    ellipse2.Fill = new ImageBrush(new BitmapImage(new Uri("C:\\coursProj4sem\\Ya!\\Icons\\icons8-музыка-в-сердце-64.png", UriKind.Relative)));

                }
                else
                {
                    ellipse2.Fill = new ImageBrush(new BitmapImage(new Uri("C:\\coursProj4sem\\Ya!\\Icons\\icons8-heart-plus-50.png", UriKind.Relative)));
                }

                Ellipse ellipse3 = new Ellipse
                    {
                        Margin = new Thickness(0, 15, 0, 0),
                        Width = 30,
                        Height = 30,
                        Fill = new ImageBrush(new BitmapImage(new Uri("C:\\coursProj4sem\\Ya!\\Icons\\icons8-playlist-64.png", UriKind.Relative))),
                        Tag = Recent_songs[i].Id + "_" + i,
                        ContextMenu = contextMenu
                    };
                    ellipse3.MouseRightButtonDown += Img3_MouseRightButtonDown2;

                    gr3.Children.Add(ellipse);
                    gr3.Children.Add(ellipse2);
                    gr3.Children.Add(ellipse3);
                    Grid.SetColumn(ellipse, 0);
                    Grid.SetColumn(ellipse2, 1);
                    Grid.SetColumn(ellipse3, 2);
                    br.Child = gr1;

                stackPanel.Children.Add(br);


            
            }
            scrollViewer.Content = stackPanel;
            grid.Children.Add(scrollViewer);
            Recent_Tracks.Children.Add(grid);
        }
        
        }

    }

