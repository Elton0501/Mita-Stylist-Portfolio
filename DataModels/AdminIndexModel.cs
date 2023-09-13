using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class AdminIndexModel
    {
        public int WebVisitCount { get; set; }
        public List<Album> Albums { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<Category> Categories { get; set; }
        public IEnumerable<TravelAlbum> TravelAlbums { get; set; }

    }
}
