using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibraryBrowser.ViewModels
{
    public class ArtistViewModel
    {
        public int ArtistId { get; set; }
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public string ArtistName { get; set; }
        public int? ImageId { get; set; }
        public byte[] ImageData{ get; set; }
    }
}
