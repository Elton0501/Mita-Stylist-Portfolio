using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class HomeViewModel
    {
        public HomeBanner HomeBanner { get; set; }
        public List<Client> Client { get; set; }
        public List<TravelAlbum> travelAlbumsSmall { get; set; }
        public TravelAlbum travelAlbumsLarge { get; set; }
    }
}
