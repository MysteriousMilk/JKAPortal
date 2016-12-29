using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKAPortal.Components.Data
{
    public class Song
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public int TrackNumber { get; set; }
        public string FileName { get; set; }
    }

    public class Album
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public byte[] Artwork { get; set; }
    }

    public class Artist
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }
}
