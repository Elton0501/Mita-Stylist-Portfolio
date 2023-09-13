using DataBase;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DataModels;

namespace Services
{
    public class AlbumService
    {
        #region singleton
        public static AlbumService Instance
        {
            get
            {
                if (instance == null) instance = new AlbumService();
                return instance;
            }
        }
        private static AlbumService instance { get; set; }

        public AlbumService()
        {
        }
        #endregion
        public List<Category> GetCategories()
        {
            using (var context = new ApplicationDbContext())
            {
                var data = context.Categories.Include(x => x.Albums).OrderByDescending(x => x.CreatedOn).ToList();
                return data;
            }
        }
        public IEnumerable<Album> GetAlbums()
        {
            using (var context = new ApplicationDbContext())
            {
                var data = context.Albums.Include(x=>x.Category).OrderByDescending(x => x.CreatedOn).ToList();
                return data;
            }
        }
        public string SaveAlbums(Album album)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                album.CreatedOn = HelperService.Instance.getCurrentIST();
                album.IsActive = true;
                context.Albums.Add(album);
                context.SaveChanges();
                result = "true";
            }
            return result;
        }
        public string EditAlbum(Album album)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                context.Entry(album).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                result = "true";
            }
            return result;
        }
        public string DeleteAlbum(int Id)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                var data = context.Albums.FirstOrDefault(x => x.Id == Id);
                context.Albums.Remove(data);
                context.SaveChanges();
                result = "true";
            }
            return result;
        }
        public Album GetAlbum(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var data = context.Albums.FirstOrDefault(x => x.Id == id);
                return data;
            }
        }
        public List<AlbumImages> GetAlbumImages(int albumId)
        {
            using (var context = new ApplicationDbContext())
            {
                var data = context.AlbumImages.Include(x=>x.Album).Where(x => x.AlbumId == albumId).OrderByDescending(x=>x.Id).ToList();
                return data;
            }
        }
        //public string SaveAlbumImages(List<ImageModel> stringData, string albumId)
        //{
        //    var result = "false";
        //    using (var context = new ApplicationDbContext())
        //    {
        //        var model = new List<AlbumImages>();
        //        foreach (var item in stringData)
        //        {
        //            var singlemodel = new AlbumImages();
        //            singlemodel.AlbumId = Convert.ToInt32(albumId);
        //            singlemodel.Image = item.Image;
        //            model.Add(singlemodel);
        //        }
        //        context.AlbumImages.AddRange(model);
        //        context.SaveChanges();
        //        result = "true";
        //        return result;
        //    }
        //}

        public string GetCategoryName(int albumId)
        {
            using (var context = new ApplicationDbContext())
            {
                var CatId = context.Albums.Where(x => x.Id == albumId).FirstOrDefault().CatId;
                var CatName = context.Categories.Where(x => x.Id == CatId).FirstOrDefault().Name;
                return CatName;
            }
                
        }

        public Category GetCategoryById(int Id)
        {
            using (var context = new ApplicationDbContext())
            {
                var data = context.Categories.Include(x => x.Albums).Where(x => x.Id == Id)
                    .OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                return data;
            }
        }

        public Category GetCategoryByName(string catName)
        {
            using (var context = new ApplicationDbContext())
            {
                var data = context.Categories.Include(x => x.Albums).Where(x => x.Name.ToLower() == catName.ToLower())
                    .FirstOrDefault();
                return data;
            }
        }
        public Album GetAlbumByName(string albumName)
        {
            using (var context = new ApplicationDbContext())
            {
                var data = context.Albums.Include(x => x.AlbumImages).Where(x => x.Name.ToLower() == albumName.ToLower())
                    .FirstOrDefault();
                return data;
            }
        }
    }
}
