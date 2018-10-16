using System;
using System.Collections.Generic;

namespace MusicLibraryBrowser
{
    public partial class WorkVersion
    {
        public int WorkVersionId { get; set; }
        public int WorkId { get; set; }
        public string WorkVersionName { get; set; }
        public bool Lossless { get; set; }
        public int? ImageId { get; set; }

        public Work Work { get; set; }
    }
}
