using Prism.Mvvm;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using TagLib;

namespace ID3TagEditWPF.Models
{
    public class AudioTagItem : BindableBase, IDisposable
    {
        private TagLib.File file;
        private bool disposedValue;

        public AudioTagItem(string filename) : this(TagLib.File.Create(filename))
        {
        }

        public AudioTagItem(TagLib.File tagFile)
        {
            this.file = tagFile;
        }

        public string Title
        {
            get
            {
                return this.file?.Tag?.Title;
            }
            set
            {
                if (this.file != null && this.file.Tag != null)
                {
                    this.file.Tag.Title = value;
                    this.file?.Save();
                    this.RaisePropertyChanged();
                }
            }
        }

        public string Artist
        {
            get
            {
                return this.file?.Tag?.FirstPerformer;
            }
            set
            {
                if (this.file != null && this.file.Tag != null)
                {
                    this.file.Tag.Performers = new string[] { value };
                    this.file.Save();
                    this.RaisePropertyChanged();
                }
            }
        }

        public string Album
        {
            get
            {
                return this.file?.Tag?.Album;
            }
            set
            {
                if (this.file != null && this.file.Tag != null)
                {
                    this.file.Tag.Album = value;
                    this.file.Save();
                    this.RaisePropertyChanged();
                }
            }
        }

        public uint Track
        {
            get
            {
                if (this.file != null && this.file.Tag != null)
                {
                    return this.file.Tag.Track;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (this.file != null && this.file.Tag != null)
                {
                    this.file.Tag.Track = value;
                    this.file.Save();
                    this.RaisePropertyChanged();
                }
            }
        }

        public uint Year
        {
            get
            {
                if (this.file != null && this.file.Tag != null)
                {
                    return this.file.Tag.Year;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (this.file != null && this.file.Tag != null)
                {
                    this.file.Tag.Year = value;
                    this.file.Save();
                    this.RaisePropertyChanged();
                }
            }
        }

        public System.Windows.Controls.Image AlbumCover
        {
            get
            {
                var pics = this.file?.Tag?.Pictures;
                if (pics == null || pics.Length <= 0)
                {
                    return null;
                }

                try
                {

                    var bin = (byte[])(file.Tag.Pictures[0].Data.Data);
                    var mem = new MemoryStream();
                    new MemoryStream(bin).CopyTo(mem);

                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = mem;
                    imageSource.EndInit();

                    var image = new System.Windows.Controls.Image();
                    image.Source = imageSource;

                    return image;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                if (this.file != null && this.file.Tag != null)
                {
                    var bmpsource = value.Source as BitmapImage;
                    byte[] data;
                    using (var mem = new MemoryStream())
                    {
                        var encoder = new BmpBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bmpsource));
                        encoder.Save(mem);
                        data = mem.ToArray();
                    }
                    ByteVector bv = new ByteVector(data);
                    var pic = new Picture(bv);
                    this.file.Tag.Pictures = new IPicture[] { pic };
                    this.file.Save();
                    this.RaisePropertyChanged();
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                this.file?.Dispose();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~AudioTagItem()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
