using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Album : Base
    {
        public int Id { get; set; }
        public string ThumbnailImage { get; set; }
        public string Name { get; set; }
        public bool isFeatured { get; set; }
        public bool isBigBanner { get; set; }
        public bool isLandscape { get; set; }
        public int? CatId { get; set; }
        [ForeignKey("CatId")]
        public Category Category { get; set; }
        public List<AlbumImages> AlbumImages { get; set; }
        [NotMapped]
        public string Images { get; set; }
        [NotMapped]
        public List<string> Imgs { get; set; }
        [NotMapped]
        public string VisitCount { get; set; }
    }
}
