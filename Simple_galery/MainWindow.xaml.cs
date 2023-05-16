using FileProcessor;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;
using DataFormats = System.Windows.DataFormats;
using DragEventArgs = System.Windows.DragEventArgs;
using Image = System.Windows.Controls.Image;
using MessageBox = System.Windows.MessageBox;


// ========Приложение Галерея.===========
//
// Переключаемые темы Главного окна и Окна полноэкранного режима.
// Переключаемый фон Главного окна просмотра.
// Загрузка файлов через файловий диалог или drag-and-drop.
// Порционная загрузка. Количество разово загружаемых фото определяется переменной "extraLoad". Догрузка автоматическая, по мере просмотра.
// Обработка изображений: поворот на 90 градусов, зеркальное отражение, черно-белое изображение.
// Сохранение изменений и возврат к первоначальному состоянию.
// Слайд-шоу.
// Удаление файла изображения из файловой системы в корзину.


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
        private int extraLoad = 5;
        private string mainWindowTheme = string.Empty;
        private string pathOriginals = string.Empty;

        private ObjectAnimationUsingKeyFrames objectAnimation = new();
            
        public ImageSource? ImagePreview
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
            DataContext = this;

            ConfigWindow();
            SetThemes("DarkTheme.xaml");

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
            for (int i = lastLoadedIndex; i < lastLoadedIndex + extraLoad && i < list.Count; i++) // загрузка заданного количества изображений с фиксацией индекса последнешо загруженного
            {
                Image image = new Image();

                BitmapImage bitmapImage = new BitmapImage();
                FileStream fs = File.OpenRead(list[i]);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = fs;
                bitmapImage.EndInit();
                fs.Close();
                fs.Dispose();

                image.Source = bitmapImage;

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
                border.MouseWheel += ScrollViewer_MouseWheel;

                stack.Children.Add(border); 
            };

            lastLoadedIndex += extraLoad;
        }
        private void SelectImage(object sender)
        {
            Border? border = sender as Border;
            if (border == null)
                return;
                        
                if (activeBorder != null)
                    activeBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);

                activeBorder = border;
                border.BorderBrush = new SolidColorBrush(Colors.IndianRed);

                if (border.Child is Image image)
                    ImagePreview = image.Source;
                    
                if ((bool)rbtnPreviewImage.IsChecked!) // установка SelectImage фоном главного изображения
                    panelPreview.Background = new ImageBrush(ImagePreview);

                if (stack.Children.IndexOf(activeBorder) >= lastLoadedIndex-1) // запуск подгрузки, если SelectImage достигло последнего загруженного фото
                    UpdateStack(); 
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

            FullScreenWindow fsWin = new FullScreenWindow(list, stack.Children.IndexOf(activeBorder), mainWindowTheme);
            fsWin.ShowDialog();

            // подгрузка стека до соответствия индекса стека значению Id Фуллскрина 
            while(fsWin.Id >= lastLoadedIndex - 1)
                UpdateStack();
            
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
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        // прокрутка предпросмотра колесом мыши
        private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                scrollViewer.LineRight();

            else if (e.Delta < 0)
                scrollViewer.LineLeft();

            e.Handled = false;
        }

        // выделение фото предпросмотра(размер) при наведении мыши
        private void Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Border border)
                border.Margin = new Thickness(10, 15, 10, 15);
        }
        private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
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

        // поворот изображения 
        private void btnRotate_Click(object sender, RoutedEventArgs e)
        {
            if (ImagePreview == null)
                return;

            TransformedBitmap tb = new TransformedBitmap();

            tb.BeginInit();

            tb.Source = ImagePreview as BitmapSource;
            RotateTransform transform = new RotateTransform(90);
            tb.Transform = transform;

            tb.EndInit();

            ImagePreview = tb;
        }

        // зеркальное отображение
        private void btnMirror_Click(object sender, RoutedEventArgs e)
        {
            if (ImagePreview == null)
                return;

            TransformedBitmap tb = new TransformedBitmap();

            tb.BeginInit();
            tb.Source = ImagePreview as BitmapSource;
            ScaleTransform transform = new ScaleTransform(-1, 1, 0.5, 0.5);
            tb.Transform = transform;
            tb.EndInit();

            ImagePreview = tb;
        }

        // преобразование в черно-белое изображение
        private void btnSepia_Click(object sender, RoutedEventArgs e)
        {
            if (ImagePreview == null)
                return;
            
            FormatConvertedBitmap newFormat = new FormatConvertedBitmap();

            newFormat.BeginInit();
            newFormat.Source = ImagePreview as BitmapSource;
            newFormat.DestinationFormat = PixelFormats.Gray32Float;
            newFormat.EndInit();

            ImagePreview = newFormat; 
        }

        // удаление изображения из списка
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ImagePreview == null)
                return;
            
            ImagePreview = null;

            FileSystem.DeleteFile(list[stack.Children.IndexOf(activeBorder)], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

            list.RemoveAt(stack.Children.IndexOf(activeBorder));

            stack.Children.Remove(activeBorder);
            panelPreview.Background = null;
            activeBorder = null;
            lastLoadedIndex--;
        }

        // установка темы
        private void menuLightTheme_Click(object sender, RoutedEventArgs e)
        {
            SetThemes("LightTheme.xaml");
        }

        private void menuDarkTheme_Click(object sender, RoutedEventArgs e)
        {
            SetThemes("DarkTheme.xaml"); 
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

            mainWindowTheme = dictionary;
        }

        // слайд-шоу
        private void btnSlideShow_Click(object sender, RoutedEventArgs e)
        {
            if (stack.Children == null)
                return;

            panelPreview.Background = null;

            // анимация
            objectAnimation.Duration = new Duration(TimeSpan.FromSeconds(stack.Children.Count));

            objectAnimation.Completed += ObjectAnimation_Completed!;

            for (int i = 0; i < stack.Children.Count; i++)
            {
                Border? border = stack.Children[i] as Border;
                Image? image = border?.Child as Image;

                objectAnimation.KeyFrames.Add(new DiscreteObjectKeyFrame() 
                {
                  Value = image?.Source,
                  KeyTime = KeyTime.Paced
                });
            }

            imgPreview.BeginAnimation(Image.SourceProperty, objectAnimation);
        }

        // установка главного изображения после окончания анимации
        private void ObjectAnimation_Completed(object sender, EventArgs e)
        {
            imgPreview.BeginAnimation(Image.SourceProperty, null);

            if (activeBorder != null)
                SelectImage(activeBorder);
            else 
                ImagePreview = null;

            objectAnimation = new();
        }
        // остановка анимации
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if(objectAnimation!=null)
                imgPreview.BeginAnimation(Image.SourceProperty, null);
        }

        // сохранение изменений
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ImagePreview == null)
                return;

            //поиск пути изображения, отображаемого на главном екране
            string path = list[stack.Children.IndexOf(activeBorder)];
                        
            // создание папки для хранения оригиналов изображений
            pathOriginals = CreateDirectory(path);

            // указание пути для файла оригинала изображения
            string newFileName = GenerateNewName(path, pathOriginals);

            // сохранение оригинального изображения в файл в отдельную папку (резервний файл для отката изменений)
            if (newFileName !=string.Empty & !File.Exists(newFileName))
            {
                var encoderOriginal = new PngBitmapEncoder();
                encoderOriginal.Frames.Add(BitmapFrame.Create((activeBorder?.Child as Image)?.Source as BitmapImage));
                using (FileStream fs = new FileStream(newFileName, FileMode.Create))
                    encoderOriginal.Save(fs);
            }
         
            //сохранение измененного главного изображения в файл по старому пути (сохранение изменений)
            var encoderModified = new PngBitmapEncoder();
            encoderModified.Frames.Add(BitmapFrame.Create((BitmapSource)ImagePreview));
            using (FileStream fs = new FileStream(path, FileMode.Create))
                encoderModified.Save(fs);

            // передача измененного главного изображения в бордер
            (activeBorder?.Child as Image)!.Source = ImagePreview;
        }

        private string CreateDirectory(string path)
        {
            if (path == null)
                return string.Empty;

            try
            {
                FileInfo fileInfo = new FileInfo(path);
                string? dirPath = fileInfo.DirectoryName;

                DirectoryInfo dir = new DirectoryInfo(dirPath!);
                dir.CreateSubdirectory(@"Originals");

                return Path.Combine(dir.FullName, @"Originals");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return string.Empty;
            }
        }
        private string GenerateNewName(string fileName, string dirName)
        {
            //поиск индекса последней точки
            int dotIndex = fileName.LastIndexOf('.');
            int slashIndex = fileName.LastIndexOf('\\');
            string ext = fileName.Substring(dotIndex + 1, fileName.Length - dotIndex - 1);

            // формирование нового пути (добавление к имени файла (original))
            string temp1 = fileName.Remove(dotIndex + 1) + "(original)" + "." + ext;
            string temp2 = temp1.Remove(0, slashIndex);
            string newFileName = temp2.Insert(0, dirName);

            return newFileName;
        } 

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            if(activeBorder == null )
                  return;
           
            string path = list[stack.Children.IndexOf(activeBorder)];   // путь текущей версии изображения
            string originalFile = GenerateNewName(path, pathOriginals); // путь оригинальной версии изображения  

            try
            {
                // удаление текущего изображения сохраненного по старому пути
                if (File.Exists(path) & File.Exists(originalFile))
                    File.Delete(path);
                else
                    return;

                // перезапись оригинального изображения по старому пути
                if (File.Exists(originalFile) & !File.Exists(path))
                {
                    File.Move(originalFile, path);
                    File.Delete(originalFile);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            // загрузка оригинального изображения в активний бордер и главное окно
            Image image = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            FileStream fs = File.OpenRead(path);

            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = fs;
            bitmapImage.EndInit();
            fs.Close();
            fs.Dispose();

            image.Source = bitmapImage;
            
            activeBorder.Child = image;
            ImagePreview = image.Source;
        }
    }
}






