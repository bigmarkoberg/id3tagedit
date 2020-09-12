using ID3TagEditWPF.Models;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace ID3TagEditWPF.ViewModels
{
    public class MainModel : BindableBase
    {

        public MainModel()
        {
            this.Items = new ObservableCollection<AudioTagItem>();
            this.AddCommand = new DelegateCommand(this.Add, this.CanAdd);
        }

        public DelegateCommand AddCommand { get; }
        private bool CanAdd()
        {
            return true;
        }

        private void Add()
        {
            var open = new OpenFileDialog();
            open.Multiselect = true;
            var b = open.ShowDialog();
            if (b == null || !b.HasValue || !b.Value)
            {
                return;
            }

            var files = open.FileNames;
            Array.Sort<string>(files);
            foreach (var file in files)
            {
                this.Items.Add(new AudioTagItem(file));
            }
        }

        public ObservableCollection<AudioTagItem> Items { get; }

    }
}
