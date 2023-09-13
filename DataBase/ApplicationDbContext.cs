using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("MitaDasDb")
        {

        }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Key> Keys { get; set; }
        public virtual DbSet<Newsletter> Newsletters { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<HomeBanner> HomeBanner { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<AlbumImages> AlbumImages { get; set; }
        public virtual DbSet<TravelAlbum> TravelAlbum { get; set; }
        public virtual DbSet<TravelAlbumImage> TravelAlbumImage { get; set; }
        public virtual DbSet<PageVisitCount> PageVisitCounts { get; set; }
        public virtual DbSet<WebVisitCount> WebVisitCounts { get; set; }

    }
}
