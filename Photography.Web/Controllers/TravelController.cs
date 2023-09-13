using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Photography.Web.Controllers
{
    public class TravelController : Controller
    {
        // GET: Travel
        public ActionResult Index(int Id)
        {
            //var data = TravelAlbumService.Instance.GetSingleTravelAlbum(Id);
            //return View(data);
            return View();
        }
    }
}