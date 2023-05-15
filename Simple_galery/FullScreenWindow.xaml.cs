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

namespace Simple_galery
{
    /// <summary>
    /// Логика взаимодействия для FullScreenWindow.xaml
    /// </summary>
    public partial class FullScreenWindow : Window
    {
        private List<string> list = new List<string>();
        public int Id { get; set; }
        public FullScreenWindow(List<string> list, int id, string mainWindowTheme)
        {
            InitializeComponent();

            this.list = list;
            Id = id;

            ConfigWindow();
            SetThemes(mainWindowTheme);

            imgMain.Source = new BitmapImage(new Uri(list[id]));
        }

        private void ConfigWindow()
        {
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Mouse.OverrideCursor = Cursors.None;
        }

        // прокрутка клавиатурой
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Mouse.OverrideCursor = Cursors.Arrow;
                    Close();
                    break;
                case Key.Right:
                    if (Id == list.Count - 1)
                        return;
                    imgMain.Source = new BitmapImage(new Uri(list[++Id]));
                    break;
                case Key.Left:
                    if (Id == 0)
                        return;
                    imgMain.Source = new BitmapImage(new Uri(list[--Id]));
                    break;
            }
        }
        // прокрутка колесом мыши
        private void imgMain_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (Id == list.Count - 1)
                    return;
                imgMain.Source = new BitmapImage(new Uri(list[++Id]));
            }
            else if (e.Delta < 0)
            {
                if (Id == 0)
                    return;
                imgMain.Source = new BitmapImage(new Uri(list[--Id]));
            }
        }

        private void SetThemes(string dictionary)
        {
            Uri uri = new Uri(dictionary, UriKind.Relative);

            ResourceDictionary? dic = Application.LoadComponent(uri) as ResourceDictionary;
            if (dic != null)
            {
                Resources.Clear();
                Resources.MergedDictionaries.Add(dic);
            }
        }
    }
}

