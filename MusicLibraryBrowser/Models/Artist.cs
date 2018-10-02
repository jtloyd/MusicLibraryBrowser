using System;
using System.Collections.Generic;

namespace MusicLibraryBrowser
{
    public partial class Artist
    {
        public Artist()
        {
            Work = new HashSet<Work>();
        }

        public int ArtistId { get; set; }
        public int GenreId { get; set; }
        public string ArtistName { get; set; }

        public Genre Genre { get; set; }
        public ICollection<Work> Work { get; set; }
    }
}
