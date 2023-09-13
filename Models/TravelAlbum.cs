using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class TravelAlbum : Base
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AlbumDate { get; set; }
        //[AllowHtml]
        //public string AlbumDesc { get; set; }
        public bool isFeatured { get; set; }
        // public List<TravelAlbumDesc> TravelAlbumDesc { get; set; }
        // [NotMapped]

        public string imgdefault { get; set; }
        [NotMapped]
        public string PImageName { get; set; }
        [NotMapped]
        public string OldPImageName { get; set; }
        [NotMapped]
        public string DefaultImage { get; set; }
        public List<TravelAlbumImage> TravelAlbumImages { get; set; }
    }
}
