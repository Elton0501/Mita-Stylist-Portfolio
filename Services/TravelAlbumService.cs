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
    public class TravelAlbumService
    {
        #region singleton
        public static TravelAlbumService Instance
        {
            get
            {
                if (instance == null) instance = new TravelAlbumService();
                return instance;
            }
        }
        private static TravelAlbumService instance { get; set; }

        public TravelAlbumService()
        {
        }
        #endregion
        //public TravelAlbum GetSingleTravelAlbum(int Id)
        //{
        //    using (var context = new ApplicationDbContext())
        //    {
        //        var data = context.TravelAlbum.Include(x => x.TravelAlbumDesc).FirstOrDefault(x => x.Id == Id);
        //        var descData = data.TravelAlbumDesc;
        //        var imageDesc = context.TravelAlbumImage.ToList();
        //        descData = descData.Select(x =>
        //        {
        //            x.TravelAlbumImages = imageDesc.Where(y => y.TravelAlbumDescId == x.Id).ToList();
        //            return x;
        //        }).ToList();
        //        data.TravelAlbumDesc = descData.OrderBy(x=>x.ColumnNo).ToList();
        //        return data;
        //    }
        //}
        public IEnumerable<TravelAlbum> GetTravelAlbum()
        {
            using (var context = new ApplicationDbContext())
            {
                var data = context.TravelAlbum.OrderBy(x => x.CreatedOn).Include(x => x.TravelAlbumImages).ToList();
                return data;
            }
        }
        //public IEnumerable<TravelAlbumDesc> GetTravelAlbumDescImg(int Id)
        //{
        //    using (var context = new ApplicationDbContext())
        //    {
        //        var data = context.TravelAlbumDescs.Include(x=>x.TravelAlbumImages).Include(x=>x.TravelAlbum).Where(x=>x.TravelAlbumId == Id).OrderBy(x => x.ColumnNo).ToList();
        //        return data;
        //    }
        //}
        public List<TravelAlbumImage> GetTravelAlbumImages(int Id)
        {
            using (var context = new ApplicationDbContext())
            {
                var data = context.TravelAlbumImage.ToList();
                return data;
            }
        }
        public List<TravelAlbumImage> GetSingleTravelAlbumImages(int Id)
        {
            using (var context = new ApplicationDbContext())
            {
                var data = context.TravelAlbumImage.Where(x => x.TravelAlbumId == Id).ToList();
                return data;
            }
        }
        public string SaveTravelAlbum(TravelAlbum travel)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                travel.CreatedOn = HelperService.Instance.getCurrentIST();
                travel.IsActive = true;
                //Save multiple images
                string[] PimgList = travel.PImageName.Split(',');
                travel.TravelAlbumImages = PimgList.Select(x => new TravelAlbumImage()
                {
                    Image = x,
                    Default = x.Trim() == travel.imgdefault ? true : false,
                    CreatedOn = DateTime.Now
                }).ToList();

                context.TravelAlbum.Add(travel);
                context.SaveChanges();
                result = "true";
            }
            return result;
        }
        //public string SaveTravelAlbumDesc(TravelAlbumDesc travelAlbumDesc)
        //{
        //    var result = "false";
        //    using (var context = new ApplicationDbContext())
        //    {
        //        travelAlbumDesc.CreatedOn = HelperService.Instance.getCurrentIST();
        //        travelAlbumDesc.IsActive = true;
        //        string[] PimgList = travelAlbumDesc.imageString.Split(',');
        //        if (PimgList != null)
        //        {
        //            travelAlbumDesc.TravelAlbumImages = PimgList.Select(x => new TravelAlbumImage()
        //            {
        //                Image = x,
        //                CreatedOn = HelperService.Instance.getCurrentIST(),
        //                IsActive = true,
        //            }).ToList();
        //        }
        //        context.TravelAlbumDescs.Add(travelAlbumDesc);
        //        context.SaveChanges();
        //        result = "true";
        //    }
        //    return result;
        //}
        public string EditTravelAlbum(TravelAlbum travel)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                context.Entry(travel).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                result = "true";
            }
            return result;
        }
        //public string EditTravelAlbumDesc(TravelAlbumDesc travelAlbumDesc)
        //{
        //    var result = "false";
        //    using (var context = new ApplicationDbContext())
        //    {
        //        var travelAlbumImage = context.TravelAlbumImage.ToList();
        //        if (travelAlbumImage != null)
        //        {
        //            context.TravelAlbumImage.RemoveRange(travelAlbumImage);
        //            context.SaveChanges();
        //        }
        //        string[] PimgList = null;
        //        string imgdelstring = travelAlbumDesc.OldPImageName;

        //        travelAlbumDesc.CreatedOn = HelperService.Instance.getCurrentIST();
        //        travelAlbumDesc.IsActive = true;
        //        if (travelAlbumDesc.imageString != null && travelAlbumDesc.imageString.Length > 0)
        //        {
        //            PimgList = travelAlbumDesc.imageString.Split(',');
        //            var imgdelList = new List<ImageModel>();
        //            if (imgdelstring != null && imgdelstring != "")
        //            {
        //                foreach (var img in PimgList)
        //                {
        //                    imgdelstring = imgdelstring.Replace(img, "");
        //                }
        //            }

        //        }
        //        if (imgdelstring != null)
        //        {
        //            while (imgdelstring.Contains(",,"))
        //            {
        //                imgdelstring = imgdelstring.Replace(",,", ",");
        //            }
        //            if (imgdelstring.StartsWith(","))
        //            {
        //                imgdelstring = imgdelstring.Remove(0, 1);
        //            }
        //            if (imgdelstring.EndsWith(","))
        //            {
        //                imgdelstring = imgdelstring.Remove(imgdelstring.Length - 1, 1);
        //            }
        //            string[] imgdelarr = imgdelstring.Split(',');
        //            foreach (var dimg in imgdelarr)
        //            {
        //                DeleteImg(dimg);
        //            }
        //        }
        //        //if (travelAlbumDesc.imageString != null && travelAlbumDesc.imageString.Length > 0)
        //        //{
        //        //    var plistImg = new List<TravelAlbumImage>();
        //        //    foreach (var pimg in PimgList)
        //        //    {
        //        //        var singleproductImg = new TravelAlbumImage();
        //        //        singleproductImg.Image = pimg;
        //        //        singleproductImg.TravelAlbumDescId = travelAlbumDesc.Id;
        //        //        singleproductImg.CreatedOn = HelperService.Instance.getCurrentIST();
        //        //        singleproductImg.IsActive = true;
        //        //        plistImg.Add(singleproductImg);
        //        //    }
        //        //    context.TravelAlbumImage.AddRange(plistImg);
        //        //}
        //        //context.Entry(travelAlbumDesc).State = EntityState.Modified;
        //        //context.SaveChanges();
        //        result = "true";
        //    }
        //    return result;
        //}
        public void DeleteImg(string img)
        {
            try
            {
                if (img.Length > 0)
                {
                    string removeimagepath = System.Web.Hosting.HostingEnvironment.MapPath(img);
                    if (System.IO.File.Exists(removeimagepath))
                    {
                        System.IO.File.Delete(removeimagepath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string DeleteTravelAlbum(int Id)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                var data = context.TravelAlbum.FirstOrDefault(x => x.Id == Id);
                context.TravelAlbum.Remove(data);
                context.SaveChanges();
                result = "true";
            }
            return result;
        }
        public TravelAlbum GetTravelAlbum(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var data = context.TravelAlbum.Include(x => x.TravelAlbumImages).FirstOrDefault(x => x.Id == id);
                return data;
            }
        }

        //public TravelAlbumDesc GetsingleTADescImg(int id)
        //{
        //    using (var context = new ApplicationDbContext())
        //    {
        //        var data = context.TravelAlbumDescs.Include(x=>x.TravelAlbumImages).Include(x=>x.TravelAlbum).FirstOrDefault(x => x.Id == id);
        //        return data;
        //    }
        //}

        //public string deleteTravelAlbumDescImg(int id)
        //{
        //    var result = "false";
        //    using(var context = new ApplicationDbContext())
        //    {
        //        var travelAlbumImage = context.TravelAlbumImage.ToList();
        //        foreach (var dimg in travelAlbumImage)
        //        {
        //            DeleteImg(dimg.Image);
        //        }
        //        context.TravelAlbumImage.RemoveRange(travelAlbumImage);
        //        var data = context.TravelAlbumDescs.FirstOrDefault(x => x.Id == id);
        //        context.TravelAlbumDescs.Remove(data);
        //        context.SaveChanges();
        //        result = "true";
        //    }
        //    return result;
        //}
        //public List<AlbumImages> GetAlbumImages(int albumId)
        //{
        //    using (var context = new ApplicationDbContext())
        //    {
        //        var data = context.AlbumImages.Include(x => x.Album).Where(x => x.AlbumId == albumId).OrderByDescending(x => x.Id).ToList();
        //        return data;
        //    }
        //}
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
    }
}
