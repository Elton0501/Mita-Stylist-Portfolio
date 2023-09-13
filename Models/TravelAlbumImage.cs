using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TravelAlbumImage : Base
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public bool Default { get; set; }
        public int TravelAlbumId { get; set; }
        [ForeignKey("TravelAlbumId")]
        public TravelAlbum TravelAlbum { get; set; }
    }
}
