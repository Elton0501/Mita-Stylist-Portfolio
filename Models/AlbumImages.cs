using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class AlbumImages
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public bool isFeatured { get; set; }
        public bool isLandscape { get; set; }
        public int AlbumId { get; set; }
        [ForeignKey("AlbumId")]
        public Album Album { get; set; }
    }
}
