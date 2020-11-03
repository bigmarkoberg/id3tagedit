using ID3TagEditWPF.Models;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace ID3TagEditWPF.ViewModels
{
    public class MainModel : BindableBase
    {

        public MainModel()
        {
            this.Items = new ObservableCollection<AudioTagItem>();
            this.AddCommand = new DelegateCommand(this.Add, this.CanAdd);
            this.AlignTrackNumberCommand = new DelegateCommand<object>(this.AlignTrackNumber, this.CanAlignTrackNumber);
            this.AlignTitleCommand = new DelegateCommand<object>(this.AlignTitle, this.CanAlignTitle);
            this.AlignImageCommand = new DelegateCommand<object>(this.AlignImage, this.CanAlignImage);
        }

        public DelegateCommand AddCommand { get; }
        public DelegateCommand<object> AlignTrackNumberCommand { get; }
        public DelegateCommand<object> AlignTitleCommand { get; }
        public DelegateCommand<object> AlignImageCommand { get; }

        private bool CanAlignImage(object param)
        {
            return true;
        }

        private void AlignImage(object param)
        {
            var list = param as IList;
            if (list == null || list.Count <= 0)
            {
                return;
            }

            var open = new OpenFileDialog();
            var b = open.ShowDialog();
            if (b == null || !b.HasValue || !b.Value)
            {
                return;
            }

            foreach (var item in this.Items)
            {
                var tag = item as AudioTagItem;
                if (tag == null)
                {
                    continue;
                }

                if (!list.Contains(item))
                {
                    continue;
                }

                tag.AlbumCover = System.Drawing.Image.FromFile(open.FileName);
            }
        }

        private bool CanAlignTitle(object param)
        {
            return true;
        }

        private void AlignTitle(object param)
        {
            var list = param as IList;
            if (list == null || list.Count <= 0)
            {
                return;
            }

            foreach (var item in this.Items)
            {
                var tag = item as AudioTagItem;
                if (tag == null)
                {
                    continue;
                }

                if (!list.Contains(item))
                {
                    continue;
                }

                tag.Title = tag.Album + " " + tag.Track;
            }
        }

        private bool CanAlignTrackNumber(object param)
        {
            return true;
        }

        private void AlignTrackNumber(object param)
        {
            var list = param as IList;
            if (list == null || list.Count <= 0)
            {
                return;
            }

            var value = Microsoft.VisualBasic.Interaction.InputBox("Start Value", string.Empty, "1");
            uint start = 1;
            if (!uint.TryParse(value?.ToString(), out start))
            {
                return;
            }

            foreach (var item in this.Items)
            {
                var tag = item as AudioTagItem;
                if (tag == null)
                {
                    continue;
                }

                if (!list.Contains(item)) {
                    continue;
                }

                tag.Track = start;
                start++;
            }
        }

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
