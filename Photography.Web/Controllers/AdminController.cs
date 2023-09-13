using DataBase;
using DataModels;
using Microsoft.Ajax.Utilities;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Web.Hosting;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Photography.Web.Controllers
{
    public class AdminController : Controller
    {
        ImageController imageController = new ImageController();
        // GET: Admin
        public ActionResult Index()
        {
            var model = new AdminIndexModel();
            model.Categories = AlbumService.Instance.GetCategories();
            model.TravelAlbums = TravelAlbumService.Instance.GetTravelAlbum();
            //new
            using (var context = new ApplicationDbContext())
            {
                var albumlist = context.Albums.Include(x => x.Category).ToList();
                var WebVisitCount = context.WebVisitCounts.ToList();
                model.WebVisitCount = WebVisitCount.Count();
                var visitpagecount = context.PageVisitCounts.ToList();
                var albumdata = albumlist.Select(x =>
                {
                    x.VisitCount = HelperService.Instance.VisitPageCount(x.Name);
                    return x;
                }).OrderByDescending(x => x.VisitCount).ToList();
                model.Albums = albumdata.Count() > 5 ? albumdata.Take(5).ToList() : albumdata;
                var Contacts = context.Contacts.OrderByDescending(x => x.Id).ToList();
                model.Contacts = Contacts.Count() > 5 ? Contacts.Take(5).ToList() : Contacts;
            }
            HelperService.Instance.RemoveOldUserVist();
            //
            return View(model);
        }
        #region Category
        public ActionResult CategoryList()
        {
            using (var context = new ApplicationDbContext())
            {
                var categories = context.Categories.ToList();
                return View(categories);
            }
        }
        [HttpPost]
        public ActionResult CategoryStatus(int catId, bool status)
        {
            var result = false;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categories = context.Categories.FirstOrDefault(x => x.Id == catId);
                    categories.IsActive = status;
                    context.Entry(categories).State = EntityState.Modified;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result);
        }
        public ActionResult CategoryAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CategoryAdd(Category category)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    category.CreatedOn = HelperService.Instance.getCurrentIST();
                    category.IsActive = true;
                    context.Categories.Add(category);
                    context.SaveChanges();
                    result = "true";
                }
            }
            catch (Exception ex)
            {
                return View(category);
                throw ex;
            }
            return Json(result);
        }
        public ActionResult CategoryEdit(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var category = context.Categories.FirstOrDefault(x => x.Id == id);
                    return View(category);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult CategoryEdit(Category category)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(category).State = EntityState.Modified;
                    context.SaveChanges();
                }
                result = "true";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result);
        }
        #endregion 
        #region Banner
        public ActionResult BannerList()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var data = context.HomeBanner.ToList();
                    return View(data);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult BannerStatus(int banId, bool status)
        {
            var result = false;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categories = context.HomeBanner.FirstOrDefault(x => x.Id == banId);
                    categories.IsActive = status;
                    context.Entry(categories).State = EntityState.Modified;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json(result);
        }
        public ActionResult AddBanner()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult AddBanner(HomeBanner homeBanner)
        {
            var result = "false";
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    homeBanner.CreatedOn = HelperService.Instance.getCurrentIST();
                    homeBanner.IsActive = true;
                    context.HomeBanner.Add(homeBanner);
                    context.SaveChanges();
                    result = "true";
                }
                return Json(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult EditBanner(int Id)
        {
            using (var context = new ApplicationDbContext())
            {
                var data = context.HomeBanner.FirstOrDefault(x => x.Id == Id);
                return View(data);
            }
        }
        [HttpPost]
        public ActionResult EditBanner(HomeBanner homeBanner)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                context.Entry(homeBanner).State = EntityState.Modified;
                context.SaveChanges();
                result = "true";
            }
            return Json(result);
        }
        [HttpPost]
        public ActionResult DeleteBanner(int Id)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                var data = context.HomeBanner.FirstOrDefault(x => x.Id == Id);
                context.HomeBanner.Remove(data);
                context.SaveChanges();
                result = "true";
            }
            return Json(result);
        }
        #endregion
        #region Clients
        public ActionResult ClientsList()
        {
            try
            {
                var data = ClientService.Instance.GetClients();
                return View(data);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult ClientStatus(int banId, bool status)
        {
            var result = false;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var categories = context.Client.FirstOrDefault(x => x.Id == banId);
                    categories.IsActive = status;
                    context.Entry(categories).State = EntityState.Modified;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json(result);
        }
        public ActionResult ClientAdd()
        {
            var data = ClientService.Instance.GetClients();
            var pNoSelectList = new List<SelectListItem>();
            var pNo = new List<int>();
            List<int> peNo = data.Select(x => x.PreferNo).ToList();

            for (int i = 0; i <= 50; i++)
            {
                pNo.Add(i + 1);
            }
            // Remove common elements from list1
            pNo = pNo.Except(peNo).ToList();
            for (int i = 0; i < pNo.Count; i++)
            {
                var catData = new SelectListItem() { Text = pNo[i].ToString(), Value = pNo[i].ToString() };
                pNoSelectList.Add(catData);
            }
            ViewBag.perNo = pNoSelectList;
            return View();
        }
        [HttpPost]
        public ActionResult ClientAdd(Client client)
        {
            try
            {
                var result = ClientService.Instance.SaveClient(client);
                return Json(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult ClientEdit(int Id)
        {
            var data = ClientService.Instance.GetClients();
            var returndata = data.FirstOrDefault(x => x.Id == Id);
            var pNoSelectList = new List<SelectListItem>();
            var pNo = new List<int>();
            List<int> peNo = data.Where(x => x.PreferNo != returndata.PreferNo).Select(x => x.PreferNo).ToList();
            for (int i = 0; i <= 50; i++)
            {
                pNo.Add(i + 1);
            }
            // Remove common elements from list1
            pNo = pNo.Except(peNo).ToList();
            for (int i = 0; i < pNo.Count; i++)
            {
                var catData = new SelectListItem() { Text = pNo[i].ToString(), Value = pNo[i].ToString() };
                pNoSelectList.Add(catData);
            }
            ViewBag.perNo = pNoSelectList;
            return View(returndata);
        }
        [HttpPost]
        public ActionResult ClientEdit(Client client)
        {
            try
            {
                var result = ClientService.Instance.EditClient(client);
                return Json(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteClient(int Id)
        {
            try
            {
                var result = ClientService.Instance.DeleteClient(Id);
                return Json(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Album
        public ActionResult AlbumList(int? Id)
        {
            var data = AlbumService.Instance.GetAlbums();
            if (Id.HasValue)
            {
                data = data.Where(x => x.CatId == Id).ToList();
            }           
            return View(data);
        }
        [HttpPost]
        public ActionResult AlbumStatus(int AlbumId, bool status)
        {
            var result = false;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var album = context.Albums.FirstOrDefault(x => x.Id == AlbumId);
                    album.IsActive = status;
                    context.Entry(album).State = EntityState.Modified;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result);
        }
        [HttpPost]
        public ActionResult AlbumIsBigBanner(int AlbumId, bool status)
        {
            var result = false;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var album = context.Albums.FirstOrDefault(x => x.Id == AlbumId);
                    album.isBigBanner = status;
                    context.Entry(album).State = EntityState.Modified;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result);
        }
        public ActionResult AlbumAdd()
        {
            var catSelectList = new List<SelectListItem>();
            using (var context = new ApplicationDbContext())
            {
                var catidVogue = context.Categories.FirstOrDefault(x => x.Name == "VOGUE INDIA DUBAI X TOURISM");
                var catidBride = context.Categories.FirstOrDefault(x => x.Name == "BRIDAL STYLING");
                var catidTravel = context.Categories.FirstOrDefault(x => x.Name == "TRAVEL & PHOTOGRAPHY JOURNAL");
                var dubai = 0;
                if (catidVogue != null)
                {
                    dubai = context.Albums.Where(x => x.CatId == catidVogue.Id && x.IsActive == true).Count();
                }
                var bridal = 0;
                if (catidBride != null)
                {
                    bridal = context.Albums.Where(x => x.CatId == catidBride.Id && x.IsActive == true).Count();
                }
                var travel = 0;
                if (catidTravel != null)
                {
                    dubai = context.Albums.Where(x => x.CatId == catidTravel.Id && x.IsActive == true).Count();
                }
                var categories = context.Categories.Where(x => x.IsActive == true).ToList();
                if (dubai >= 1)
                {
                    categories.Remove(categories.FirstOrDefault(s => s.Name == "VOGUE INDIA DUBAI X TOURISM"));
                }
                if (bridal >= 1)
                {
                    categories.Remove(categories.FirstOrDefault(s => s.Name == "BRIDAL STYLING"));
                }
                if (travel >= 1)
                {
                    categories.Remove(categories.FirstOrDefault(s => s.Name == "TRAVEL & PHOTOGRAPHY JOURNAL"));
                }
                catSelectList = categories.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
            }
            ViewBag.catList = catSelectList;
            return View();
        }
        [HttpPost]
        public ActionResult AlbumAdd(Album album)
        {
            try
            {
                var result = "false";
                using (var context = new ApplicationDbContext())
                {
                    album.CreatedOn = HelperService.Instance.getCurrentIST();
                    album.IsActive = true;
                    var data = GetDimension(album.ThumbnailImage);
                    album.isLandscape = data.Width > data.Height ? true : false;
                    context.Albums.Add(album);
                    context.SaveChanges();
                    result = "true";
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                return View(album);
                throw ex;
            }
        }
        public ActionResult AlbumEdit(int id)
        {
            try
            {
                var catSelectList = new List<SelectListItem>();
                using (var context = new ApplicationDbContext())
                {
                    var categories = context.Categories.Where(x => x.IsActive == true).ToList();
                    catSelectList = categories.Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                }
                ViewBag.catList = catSelectList;
                var album = AlbumService.Instance.GetAlbum(id);
                return View(album);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult AlbumEdit(Album album)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var data = GetDimension(album.ThumbnailImage);
                    album.isLandscape = data.Width > data.Height ? true : false;
                    context.Entry(album).State = EntityState.Modified;
                    context.SaveChanges();
                }
                result = "true";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result);
        }
        #endregion
        #region AlbumImage
        public ActionResult AlbumImageList(int AlbumId)
        {
            ViewBag.AlbumID = AlbumId;
            var dataalbum = AlbumService.Instance.GetAlbum(AlbumId);
            ViewBag.AlbumName = dataalbum.Name;
            //  var data = AlbumService.Instance.GetAlbumImages(AlbumId);
            return View();
        }
        [HttpPost]
        public ActionResult AlbumImageStatus(int AlbumId,int AlbumImageId, bool status)
        {
            var result = "false";
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var albumImages = context.AlbumImages.Where(x=>x.AlbumId == AlbumId && x.isFeatured == true).ToList();
                    var AlbumName = AlbumService.Instance.GetCategoryName(AlbumId);
                    if (AlbumName == "VOGUE INDIA DUBAI X TOURISM" && status == true)
                    {
                        if(albumImages.Count < 3)
                        {
                            result = StatusChange(AlbumImageId, status);
                        }
                        else
                        {
                            result = "invalid";
                        }
                    }
                    
                    else if (AlbumName == "TRAVEL & PHOTOGRAPHY JOURNAL" && status == true)
                    {
                        if(albumImages.Count < 4)
                        {
                            result = StatusChange(AlbumImageId, status);
                        }
                        else
                        {
                            result = "invalid";
                        }
                    }
                    else
                    {
                        result = StatusChange(AlbumImageId, status);
                    }
                    
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result);
        }

        public string StatusChange(int AlbumImageId, bool status)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                var albumImage = context.AlbumImages.FirstOrDefault(x => x.Id == AlbumImageId);
                albumImage.isFeatured = status;
                context.Entry(albumImage).State = EntityState.Modified;
                context.SaveChanges();
                result = "true";
            }
            return result;
        }
        public ActionResult _AlbumImageList(int AlbumId)
        {
            ViewBag.AlbumID = AlbumId;
            var dataalbum = AlbumService.Instance.GetAlbum(AlbumId);
            ViewBag.AlbumName = dataalbum.Name;
            var data = AlbumService.Instance.GetAlbumImages(AlbumId);
            return View(data);
        }
        [HttpPost]
        public ActionResult AddAlbumImages(List<ImageModel> stringData, string AlbumId)
        {
            //var result = AlbumService.Instance.SaveAlbumImages(stringData, AlbumId);
            using (var context = new ApplicationDbContext())
            {
                var model = new List<AlbumImages>();
                foreach (var item in stringData)
                {
                    var singlemodel = new AlbumImages();
                    singlemodel.AlbumId = Convert.ToInt32(AlbumId);
                    singlemodel.Image = item.Image;

                    //Dimension
                    var data = GetDimension(item.Image);
                    singlemodel.isLandscape = data.Width > data.Height ? true : false;
                    model.Add(singlemodel);
                }
                context.AlbumImages.AddRange(model);
                context.SaveChanges();
            }
            return View();
        }
        [HttpPost]
        public string AlbumImgaeDescEdit(string Value, string Id)
        {
            var result = "false";
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    int id = Convert.ToInt32(Id);
                    var data = context.AlbumImages.FirstOrDefault(x => x.Id == id);
                    data.Description = Value;
                    context.Entry(data).State = EntityState.Modified;
                    context.SaveChanges();
                    result = "true";
                }
            }
            catch (Exception)
            {
                result = "false";
            }
            return result;
        }
        [HttpPost]
        public string AlbumImageDelete(int AlbumImageId)
        {
            var result = "false";
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var data = context.AlbumImages.FirstOrDefault(x => x.Id == AlbumImageId);
                    context.AlbumImages.Remove(data);
                    context.SaveChanges();
                    result = "true";
                }
            }
            catch (Exception)
            {
                result = "false";
            }
            return result;
        }
        #endregion
        #region TravelAlbum
        public ActionResult TravelAlbumList()
        {
            try
            {
                var data = TravelAlbumService.Instance.GetTravelAlbum();
                return View(data);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult TravelAlbumStatus(int TravelAlbumId, bool status)
        {
            var result = false;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var album = context.TravelAlbum.FirstOrDefault(x => x.Id == TravelAlbumId);
                    album.IsActive = status;
                    context.Entry(album).State = EntityState.Modified;
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result);
        }
        //[HttpPost]
        //public ActionResult TravelAlbumIsBigBanner(int AlbumId, bool status)
        //{
        //    var result = false;
        //    try
        //    {
        //        using (var context = new ApplicationDbContext())
        //        {
        //            var album = context.TravelAlbum.FirstOrDefault(x => x.Id == AlbumId);
        //            album.isBigBanner = status;
        //            context.Entry(album).State = EntityState.Modified;
        //            context.SaveChanges();
        //            result = true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return Json(result);
        //}
        public ActionResult TravelAlbumAdd()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TravelAlbumAdd(TravelAlbum album)
        {
            try
            {
                var result = TravelAlbumService.Instance.SaveTravelAlbum(album);
                return Json(result);
            }
            catch (Exception ex)
            {
                return View(album);
                throw ex;
            }
        }
        public ActionResult TravelAlbumEdit(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    string imgdata = "";
                    var album = TravelAlbumService.Instance.GetTravelAlbum(id);
                    for (int i = 0; i < album.TravelAlbumImages.Count(); i++)
                    {
                        if (i < (album.TravelAlbumImages.Count() - 1))
                        {
                            imgdata = imgdata + album.TravelAlbumImages[i].Image + ",";
                        }
                        else
                        {
                            imgdata = imgdata + album.TravelAlbumImages[i].Image;
                        }
                    }
                    var defaultimg = context.TravelAlbumImage.FirstOrDefault(x => x.TravelAlbumId == id && x.Default == true);
                    ViewBag.DefaultImage = defaultimg != null ? defaultimg.Image : null;
                    ViewBag.PimgString = imgdata;
                    return View(album);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TravelAlbumEdit(TravelAlbum tAlbum)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var TravelalbumImage = context.TravelAlbumImage.Where(x => x.TravelAlbumId == tAlbum.Id).ToList();
                    context.TravelAlbumImage.RemoveRange(TravelalbumImage);
                    context.SaveChanges();
                    string[] PimgList = null;
                    string imgdelstring = tAlbum.OldPImageName;
                    if (tAlbum.PImageName != null && tAlbum.PImageName.Length > 0)
                    {
                        PimgList = tAlbum.PImageName.Split(',');
                        var imgdelList = new List<ImageModel>();
                        foreach (var img in PimgList)
                        {
                            imgdelstring = imgdelstring.Replace(img, "");
                        }
                    }
                    if (imgdelstring != null)
                    {
                        while (imgdelstring.Contains(",,"))
                        {
                            imgdelstring = imgdelstring.Replace(",,", ",");
                        }
                        if (imgdelstring.StartsWith(","))
                        {
                            imgdelstring = imgdelstring.Remove(0, 1);
                        }
                        if (imgdelstring.EndsWith(","))
                        {
                            imgdelstring = imgdelstring.Remove(imgdelstring.Length - 1, 1);
                        }
                        string[] imgdelarr = imgdelstring.Split(',');
                        foreach (var dimg in imgdelarr)
                        {
                            ImageController Image = new ImageController();
                            Image.DeleteImg(dimg);
                        }
                    }
                    if (tAlbum.PImageName != null && tAlbum.PImageName.Length > 0)
                    {
                        var plistImg = new List<TravelAlbumImage>();
                        foreach (var pimg in PimgList)
                        {
                            var singleproductImg = new TravelAlbumImage();
                            singleproductImg.Image = pimg;
                            singleproductImg.TravelAlbumId = tAlbum.Id;
                            singleproductImg.Default = pimg == tAlbum.imgdefault ? true : false;
                            singleproductImg.CreatedOn = DateTime.Now;
                            plistImg.Add(singleproductImg);
                        }
                        context.TravelAlbumImage.AddRange(plistImg);
                    }
                    context.Entry(tAlbum).State = EntityState.Modified;
                    context.SaveChanges();
                }
                result = "true";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result);
        }
        #endregion
        #region TravelAlbumImages
        public ActionResult getTravelAlbumImages(int TravelAlbumId)
        {
            try
            {
                var data = TravelAlbumService.Instance.GetSingleTravelAlbumImages(TravelAlbumId);
                var list = new List<string>();
                for (int i = 0; i < data.Count(); i++)
                {
                    list.Add(data[i].Image);
                }
                var array = list.ToArray();
                return Json(array, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Key
        public ActionResult WebsiteInformation()
        {
            using (var context = new ApplicationDbContext())
            {
                var keys = context.Keys.ToList();
                return View(keys);
            }
        }
        public ActionResult InfoAdd()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult InfoAdd(Key Key)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    Key.CreatedOn = HelperService.Instance.getCurrentIST();
                    context.Keys.Add(Key);
                    context.SaveChanges();
                    result = "true";
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult InfoEdit(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var key = context.Keys.FirstOrDefault(x => x.Id == id);
                    return PartialView(key);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult InfoEdit(Key key)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Entry(key).State = EntityState.Modified;
                    context.SaveChanges();
                    result = "true";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result);
        }
        public bool InfoDelete(int id)
        {
            bool result = false;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var delete = context.Keys.Find(id);
                    context.Keys.Remove(delete);
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
                throw;
            }
            return result;
        }
        #endregion
        #region Newsletter
        public ActionResult Newsletter()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Newsletter(string Sub, string Head, string Body)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var testmail = context.Keys.Where(x => x.Name == "testemail").ToList();
                    if (testmail.Count > 0)
                    {
                        foreach (var email in testmail)
                        {
                            newslettermail(Sub, Head, Body.ToString(), email.Description);
                        }
                    }
                    result = "true";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult NewsletterFinal(string Sub, string Head, string Body)
        {
            string result;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var maillist = context.Newsletters.ToList();
                    if (maillist.Count > 0)
                    {
                        foreach (var email in maillist)
                        {
                            newslettermail(Sub, Head, Body.ToString(), email.Email);
                        }
                    }
                    result = "true";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result);
        }
        public void newslettermail(string Sub, string Head, string Body, string email)
        {
            try
            {

                var mail = ConfigurationManager.AppSettings["email"];
                var pass = ConfigurationManager.AppSettings["pass"];
                var port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                var stp = ConfigurationManager.AppSettings["smtp"];
                var ssl = Convert.ToBoolean(ConfigurationManager.AppSettings["ssl"]);

                string FilePath = HostingEnvironment.MapPath("~/EmailTemplate/") + "NewsLTemp" + ".cshtml";
                StreamReader reader = new StreamReader(FilePath);
                string textMail = reader.ReadToEnd();
                reader.Close();

                textMail = textMail.Replace("[head]", Head);
                textMail = textMail.Replace("[content]", Body);


                MailMessage message = new MailMessage();
                message.From = new MailAddress(mail);
                message.To.Add(email);
                message.Subject = Sub;
                message.Body = textMail;

                message.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(stp, port);
                smtp.Credentials = new NetworkCredential(mail, pass);
                smtp.EnableSsl = ssl;
                smtp.Send(message);
                smtp.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region VisitCount
        public ActionResult PageVisitCount()
        {
            using (var context = new ApplicationDbContext())
            {
                var albumlist = context.Albums.Include(x => x.Category).Include(x => x.AlbumImages).ToList();
                var visitpagecount = context.PageVisitCounts.ToList();
                var albumdata = albumlist.Select(x =>
                {
                    x.VisitCount = HelperService.Instance.VisitPageCount(x.Name);
                    return x;
                }).OrderByDescending(x => x.VisitCount).ToList();
                return View(albumdata);
            }
        }
        #endregion
        #region Contact
        public ActionResult Contact()
        {
            using (var context = new ApplicationDbContext())
            {
                var Contacts = context.Contacts.OrderByDescending(x => x.Id).ToList();
                return View(Contacts);
            }
        }
        #endregion
        #region Charts
        public ActionResult pieChart()
        {
            var result = new List<WebCharts>();
            var WebCount = new List<WVCountPerDay>();
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var visit = context.WebVisitCounts.ToList();
                    WebCount = visit.Select(x => new WVCountPerDay()
                    {
                        day = x.VisitDateTime.ToString("d ddd"),
                        Count = visit.Where(z => z.VisitDateTime.Date == x.VisitDateTime.Date).ToList().Count()
                    }).ToList();
                    result = WebCount.Select(x => new WebCharts()
                    {
                        Key = x.day.ToString(),
                        Value = x.Count
                    }).DistinctBy(x => x.Key).OrderByDescending(x => x.Value).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult barChart(string First, string Second, string name)
        {
            DateTime first = First != null && First != string.Empty ? Convert.ToDateTime(First) : DateTime.Now.AddDays(-7);
            DateTime second = Second != null && Second != string.Empty ? Convert.ToDateTime(Second).AddDays(1) : DateTime.Now;
            var result = new List<WebCharts>();
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var pageVisit = context.PageVisitCounts.Where(x => x.VisitDateTime < second && x.VisitDateTime >= first && x.VisitPage == name).ToList();
                    result = pageVisit.Select(x => new WebCharts()
                    {
                        Key = x.VisitDateTime.Date.ToString("MMM-dd"),
                        Value = pageVisit.Where(y => y.VisitDateTime.Date == x.VisitDateTime.Date).Count()
                    }).DistinctBy(x => x.Key).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ImageDimension
        public ImageDim GetDimension(string URL)
        {
            var data = new ImageDim();
            FileStream fs = new FileStream(Server.MapPath(URL), FileMode.Open, FileAccess.Read, FileShare.Read);
            System.Drawing.Image dimg = System.Drawing.Image.FromStream(fs);
            data.Width = Convert.ToInt32(dimg.Width);
            data.Height = Convert.ToInt32(dimg.Height);
            return data;

        }
        #endregion
    }

}

public class ImageDim
{
    public int Height { get; set; }
    public int Width { get; set; }

}
