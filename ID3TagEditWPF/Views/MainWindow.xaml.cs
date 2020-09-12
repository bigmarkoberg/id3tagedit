using ID3TagEditWPF.Models;
using Microsoft.Win32;
using System;
using System.CodeDom;
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

namespace ID3TagEditWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column.Header.ToString().ToLower() != "albumcover")
            {
                return;
            }

            e.Cancel = true;
            var open = new OpenFileDialog();
            var b = open.ShowDialog();
            if (b == null || !b.HasValue || !b.Value)
            {
                return;
            }

            try
            {
                ImageSource imageSource = new BitmapImage(new Uri(open.FileName));
                var image1 = new Image();
                image1.Source = imageSource;                
                var a = e.Row.Item as AudioTagItem;
                a.AlbumCover = image1;
            }
            catch (Exception)
            {
            }
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().ToLower() != "albumcover")
            {
                return;
            }

            var col1 = new DataGridTemplateColumn();
            col1.Header = e.Column.Header;
            col1.Width = DataGridLength.SizeToCells;
            
            FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Image));
            Binding b1 = new Binding(e.PropertyName);
            b1.Mode = BindingMode.TwoWay;
            factory1.SetValue(Image.SourceProperty, b1);
            DataTemplate cellTemplate1 = new DataTemplate();
            cellTemplate1.VisualTree = factory1;
            col1.CellTemplate = cellTemplate1;
            e.Column = col1;

        }
    }
}
