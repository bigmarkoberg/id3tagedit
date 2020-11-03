using Prism.Mvvm;
using System;
using System.Drawing;
using System.IO;
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

        public string Path
        {
            get
            {
                return this.file?.Name;
            }
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

        public System.Drawing.Image AlbumCoverThumbnail
        {
            get
            {
                const int LARGEST = 75;
                var image = this.AlbumCover;
                var w = image.Width;
                var h = image.Height;

                if (w >= h)
                {
                    // w/h = LARGEST/nh
                    h = LARGEST * h / w;
                    w = LARGEST;
                }
                else
                {
                    // w/h = mw/LARGEST
                    w = LARGEST * w / h;
                    h = LARGEST;
                }

                return image.GetThumbnailImage(w, h, null, IntPtr.Zero);
            }
        }

        public System.Drawing.Image AlbumCover
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
                    return Image.FromStream(mem);
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
                    var mem = new MemoryStream();
                    value.Save(mem, System.Drawing.Imaging.ImageFormat.Bmp);
                    ByteVector bv = new ByteVector(mem.GetBuffer());
                    var pic = new Picture(bv);
                    this.file.Tag.Pictures = new IPicture[] { pic };
                    this.file.Save();
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.AlbumCoverThumbnail));
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
