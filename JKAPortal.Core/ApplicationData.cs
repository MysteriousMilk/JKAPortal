using JKAPortal.API.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Media.Imaging;

namespace JKAPortal.Core
{
    public class ApplicationData : IApplicationData
    {
        public IThumbnail DefaultThumbnail
        {
            get;
            set;
        }

        public List<IThumbnail> MapThumbnails
        {
            get;
            set;
        }

        public ApplicationData()
        {
            DefaultThumbnail = new Thumbnail();
            MapThumbnails = new List<IThumbnail>();

            LoadMapThumbnails();
        }

        private void LoadMapThumbnails()
        {
            string filename = @"C:\Games\Steam\steamapps\common\Jedi Academy\GameData\base\assets0.zip";

            try
            {
                using (ZipArchive zip = ZipFile.Open(filename, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries.Where(e => e.FullName.Contains("levelshots/mp/")))
                    {
                        try
                        {
                            if (entry.FullName.Equals("levelshots/mp/"))
                                continue;

                            using (var stream = entry.Open())
                            {
                                Thumbnail thumb = new Thumbnail();
                                thumb.Name = Path.GetFileNameWithoutExtension(entry.Name);

                                Bitmap img = (Bitmap)Image.FromStream(stream);

                                using (MemoryStream ms = new MemoryStream())
                                {
                                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                    ms.Position = 0;

                                    thumb.Image = new BitmapImage();
                                    thumb.Image.BeginInit();
                                    thumb.Image.DecodePixelWidth = 168;
                                    thumb.Image.CacheOption = BitmapCacheOption.OnLoad;
                                    thumb.Image.UriSource = null;
                                    thumb.Image.StreamSource = ms;
                                    thumb.Image.EndInit();

                                    MapThumbnails.Add(thumb);
                                }

                                img.Dispose();
                            }
                        }
                        catch (Exception)
                        {
                            Trace.WriteLine(string.Format("Failed to load map thumbnail [{0}]", entry.FullName));
                        }
                    }
                }
            }
            catch (Exception)
            {
                Trace.WriteLine(string.Format("Failed to load .PK3 file [{0}]", filename));
            }

            DefaultThumbnail.Name = "default";

            DefaultThumbnail.Image = new BitmapImage();
            DefaultThumbnail.Image.BeginInit();
            DefaultThumbnail.Image.CacheOption = BitmapCacheOption.OnLoad;
            DefaultThumbnail.Image.UriSource = new Uri(
                "pack://application:,,,/JKAPortal.Core;component/Resources/DefaultMapThumb.png",
                UriKind.Absolute
                );
            DefaultThumbnail.Image.EndInit();

            GC.Collect();
        }
    }
}
