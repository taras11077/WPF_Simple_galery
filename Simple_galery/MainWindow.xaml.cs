using FileProcessor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

namespace Simple_galery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Border? activeBorder = null;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private List<string> list = new List<string>();
        private Finder finder;
        private int lastLoadedIndex = 0;
        private int extraLoad = 3;
       
        public ImageSource ImagePreview
        {
            get { return (ImageSource)GetValue(ImagePreviewProperty); }
            set { SetValue(ImagePreviewProperty, value); }
        }
                
        public static readonly DependencyProperty ImagePreviewProperty =
            DependencyProperty.Register("ImagePreview", typeof(ImageSource), typeof(MainWindow), new PropertyMetadata(null));



        public MainWindow()
        {
            InitializeComponent();

            fbd = new System.Windows.Forms.FolderBrowserDialog();
            finder = new Finder();
            //ImagePreview = new Image();
            DataContext = this;

            ConfigWindow();

            string[] filePatterns =
            {
                "*.JPG",
                "*.jpeg",
                "*.jpg",
                "*.png",
                "*.gif",
            };

            finder.FileMasks = filePatterns;
        }

        private void ConfigWindow()
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                finder.FindFiles(fbd.SelectedPath);

                finder.Container.Files.ForEach(f =>
                {
                    list.Add(f.FullName);
                });
            }
            UpdateStack();
        }
        private void UpdateStack()
        {
            for (int i = lastLoadedIndex; i < lastLoadedIndex + extraLoad && i < list.Count; i++)
            {
                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    image.Source = new BitmapImage(new Uri(list[i]));
                    image.MouseWheel += ScrollViewer_MouseWheel;

                    Border border = new Border()
                    {
                        Child = image,

                        MinWidth = 150,
                        BorderThickness = new Thickness(3, 3, 3, 3),
                        Margin = new Thickness(10, 15, 10, 15),
                    };

                    border.MouseDown += Border_MouseDown;
                    border.MouseEnter += Border_MouseEnter;
                    border.MouseLeave += Border_MouseLeave;
                    //border.MouseWheel += ScrollViewer_MouseWheel;

                    stack.Children.Add(border);
            };

            lastLoadedIndex += extraLoad;
        }
        private void SelectImage(object sender)
        {
            Border? border = sender as Border;
            if (border != null)
            {
                if (activeBorder != null)
                    activeBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);

                activeBorder = border;
                border.BorderBrush = new SolidColorBrush(Colors.ForestGreen);

                if (border.Child is Image image)
                    ImagePreview = image.Source;

                if ((bool)rbtnPreviewImage.IsChecked) // установка SelectImage фоном главного изображения
                    panelPreview.Background = new ImageBrush(ImagePreview);

                if (stack.Children.IndexOf(activeBorder) == lastLoadedIndex-1) // запуск подгрузки, если SelectImage достигло последнего загруженного фото
                    UpdateStack();
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                SelectImage(sender);
            }
        }
        private void menuFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (activeBorder == null)
                return;

            FullScreenWindow fsWin = new FullScreenWindow(list, stack.Children.IndexOf(activeBorder));
            fsWin.ShowDialog();

            SelectImage(stack.Children[fsWin.Id]);
        }
        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }


        // драг-н-дроп
        private void MainGrid_PreviewDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] files = (string[])(e.Data.GetData(DataFormats.FileDrop, true));
            }
        }
        private void MainGrid_PreviewDrop(object sender, DragEventArgs e)
        {
            string[] pathes = (string[])e.Data.GetData(DataFormats.FileDrop, true);

            foreach (string path in pathes)
            {
                if (Directory.Exists(path))
                    ExtractFilesFromDir(path);

                else if (File.Exists(path))
                    list.Add(path);
            }
            UpdateStack();
        }
        private void ExtractFilesFromDir(string path)
        {
            try
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                    list.Add(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // прокрутка предпросмотра колесом мыши
        private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
                scrollViewer.LineRight();
            else if (e.Delta > 0)
                scrollViewer.LineLeft();

            e.Handled = false;
        }

        // выделение фото предпросмотра(размер) при наведении мыши
        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
                border.Margin = new Thickness(10, 15, 10, 15);
        }
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
                border.Margin = new Thickness(10, 3, 10, 3);
        }

        // обработчики radioButtons изменения фона
        private void rbtnPreviewImage_Checked(object sender, RoutedEventArgs e)
        {
            if (ImagePreview != null)
                panelPreview.Background = new ImageBrush(ImagePreview);
        }
        private void rbtnNone_Checked(object sender, RoutedEventArgs e)
        {
            if (panelPreview !=null && panelPreview.Background is ImageBrush)
                panelPreview.Background = null;
        }

        
    }
}

