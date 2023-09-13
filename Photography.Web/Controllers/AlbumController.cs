using DataBase;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Photography.Web.Controllers
{
    public class AlbumController : Controller
    {
        public ActionResult Album(string CatName)
        {
            var category = AlbumService.Instance.GetCategoryByName(CatName);
            var userSession = HttpContext.Session[category.Name];
            if (userSession == null)
            {
                using (var context = new ApplicationDbContext())
                {
                    var UserSessionId = Guid.NewGuid();
                    HttpContext.Session[category.Name] = UserSessionId;
                    var model = new PageVisitCount();
                    model.SessionID = UserSessionId.ToString();
                    model.VisitPage = category.Name;
                    model.VisitDateTime = HelperService.Instance.getCurrentIST();
                    context.PageVisitCounts.Add(model);
                    context.SaveChanges();
                }
            }
            return View(category);
        }

        public ActionResult AlbumPhotos(string AlbumName)
        {
            var data = AlbumService.Instance.GetAlbumByName(AlbumName);
            var userSession = HttpContext.Session[data.Name];
            if (userSession == null)
            {
                using (var context = new ApplicationDbContext())
                {
                    var UserSessionId = Guid.NewGuid();
                    HttpContext.Session[data.Name] = UserSessionId;
                    var model = new PageVisitCount();
                    model.SessionID = UserSessionId.ToString();
                    model.VisitPage = data.Name;
                    model.VisitDateTime = HelperService.Instance.getCurrentIST();
                    context.PageVisitCounts.Add(model);
                    context.SaveChanges();
                }
            }
            return View(data);
        }

        public ActionResult CustomDesign()
        {
            return View();
        }
        
        public ActionResult TravelJournal(string AlbumName)
        {
            var data = AlbumService.Instance.GetAlbumByName(AlbumName);
            return View(data);
        }
    }
}