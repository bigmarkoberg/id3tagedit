using ID3TagEditWPF.Models;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

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
            if (e.Column.Header.ToString().ToLower() != "cover")
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
                var a = e.Row.Item as AudioTagItem;
                a.AlbumCover = System.Drawing.Image.FromFile(open.FileName);
            }
            catch (Exception)
            {
            }
        }
    }
}
