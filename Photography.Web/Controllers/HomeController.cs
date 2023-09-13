using DataBase;
using DataModels;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Script.Serialization;

namespace Photography.Web.Controllers
{
    public class HomeController : Controller
    {
        HelperController helperController = new HelperController();
        public ActionResult Index()
        {
            //For User Visit
            var userSession = HttpContext.Session["UserSession"];
            var data = new HomeViewModel();
            using (var context = new ApplicationDbContext())
            {
                //For User Visit
                if (userSession == null)
                {
                    var UserSessionId = Guid.NewGuid();
                    HttpContext.Session["UserSession"] = UserSessionId;
                    var model = new WebVisitCount();
                    model.SessionID = UserSessionId.ToString();
                    model.VisitDateTime = HelperService.Instance.getCurrentIST();
                    context.WebVisitCounts.Add(model);
                    context.SaveChanges();
                }
                //
                data.HomeBanner = context.HomeBanner.FirstOrDefault(x=>x.IsActive == true);
                data.Client = context.Client.Where(x => x.IsActive == true).ToList();
                //data.travelAlbumsSmall = context.TravelAlbum.Include(x => x.TravelAlbumDesc).Where(x => x.isFeatured == true && x.IsActive == true && x.isBigBanner != true).ToList();
                //data.travelAlbumsLarge = context.TravelAlbum.Include(x => x.TravelAlbumDesc).FirstOrDefault(x => x.isFeatured == true && x.IsActive == true && x.isBigBanner == true);
            }
            return View(data);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(Contact model)
        {
            try
            {
                var result = "* All fields are required to fill.";
                if (model.Email != string.Empty && model.Email != null && model.FirstName != null && model.FirstName != string.Empty && model.LastName != null && model.LastName != string.Empty && 
                    model.Message != null && model.Message != string.Empty)
                {
                    using (var context = new ApplicationDbContext())
                    {
                        model.CreatedOn = DateTime.Now;
                        context.Contacts.Add(model);
                        context.SaveChanges();
                    }
                    result = "true";
                    if (result == "true")
                    {
                        string message = "<p>Thank you for contact us</p>";
                        string subject = "Contact form";
                        string Head = "Contact form";
                        helperController.templateEmail(model.Email, subject, Head, message);

                        string message1 = model.Message;
                        string subject1 = "Contact form from" + model.FirstName;
                        string Head1 = "Contact form";
                        helperController.templateEmail(ConfigurationManager.AppSettings["email"].ToString(), subject1, Head1, message1);
                    }
                }
                return Json(result);
            }
            catch (Exception)
            {
                return Redirect("~/Not_found.html");
            }
        }
        [HttpPost]
        public ActionResult AddsubscribeForm(Newsletter model)
        {
            try
            {
                var result = "* Email is required to fill.";
                if (model.Email != string.Empty && model.Email != null)
                {
                    using (var context = new ApplicationDbContext())
                    {
                        var Existdata = context.Newsletters.FirstOrDefault(x => x.Email == model.Email);
                        if (Existdata == null)
                        {
                            model.CreatedOn = HelperService.Instance.getCurrentIST();
                            context.Newsletters.Add(model);
                            context.SaveChanges();
                            result = "true";
                            if (result == "true")
                            {
                                string message = "<p>Thank you for newsletter subscribtion</p>";
                                string subject = "Newsletter Subscribe";
                                string Head = "Newsletter Subscribe";
                                helperController.templateEmail(model.Email, subject, Head, message);
                            }
                        }
                        else
                        {
                            result = "* You have already register.";
                        }
                    }
                }
                return Json(result);
            }
            catch (Exception)
            {
                return Redirect("~/Not_found.html");
            }
        }
        [HttpPost]
        public ActionResult SliderData(string SearchType)
        {
            try
            {
                string newdata = "";
                using (var context = new ApplicationDbContext())
                {
                    var serializer = new JavaScriptSerializer();
                    
                    List<Album> albums = new List<Album>();
                    Album album = new Album();
                    var data = context.Albums.Include(x=>x.Category).Where(x => x.IsActive == true && x.isFeatured == true).ToList();
                    if (data != null && data.Count() > 0)
                    {
                        if (SearchType == "VOGUE INDIA DUBAI X TOURISM" || SearchType == "BRIDAL STYLING" || SearchType == "TRAVEL & PHOTOGRAPHY JOURNAL")
                        {
                            album = data.Where(x => x.Category.Name.ToLower() == SearchType.ToLower()).FirstOrDefault();
                            var albumImgs = context.AlbumImages.Where(x => x.AlbumId == album.Id && x.isFeatured == true).ToList();
                            List<string> AlbumList= new List<string>();
                            for (int i = 0; i < albumImgs.Count; i++)
                            {
                                AlbumList.Add(albumImgs[i].Image);
                            }

                            album.Category = null;
                            album.AlbumImages = null;
                            album.Imgs= AlbumList;

                            //album = albums.Select(x => {
                            //    x.Category = null;
                            //    x.Imgs = AlbumList;
                            //    return x;
                            //}).FirstOrDefault();
                            newdata = serializer.Serialize(album);
                            
                        }
                        else
                        {
                            albums = data.Where(x => x.Category.Name.ToLower() == SearchType.ToLower()).ToList();
                            albums = albums.Select(x => {
                                x.Category = null;
                                return x;
                            }).ToList();
                           
                            newdata = serializer.Serialize(albums);
                        }
                    }

                    return Json(newdata);  
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        webApiController webApiController = new webApiController();

        public ActionResult Test()
        {
            return View();
        }

        public JsonResult TestResult()
        {
            try
            {
                int RId = 1;
                var menu = webApiController.GetTestResult();
                return Json(menu, JsonRequestBehavior.AllowGet);
                //return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}