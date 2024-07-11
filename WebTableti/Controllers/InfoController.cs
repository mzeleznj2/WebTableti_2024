using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTableti.Controllers
{
    public class InfoController : Controller
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["dbNautilusConnectionString"].ConnectionString;
        private dbNautilusEntities1 context = new dbNautilusEntities1();
        
        public ActionResult Index()
        {        
           return View();
        }

        public ActionResult PrikažiInfo()
        {        
            return View(context.TUBLAInfo.ToList());
        }

        [HttpPost]
        public ActionResult DodajInfo(TUBLAInfo tInfo, HttpPostedFileBase postedFile)
        {
            string path;
            if (postedFile != null)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                string ImageName = System.IO.Path.GetFileName(postedFile.FileName);
                path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                postedFile.SaveAs(path + fileName);
                //ViewBag.ImageUrl = "Uploads/" + fileName;

                using (var context = new dbNautilusEntities1())
                {
                    var Info = new TUBLAInfo()
                    {
                        datum = DateTime.Now,
                        informacija = tInfo.informacija,
                        tip = tInfo.tip,
                        tip2 = path + "" + fileName,
                        imageName = ImageName
                    };

                    context.TUBLAInfo.Add(Info);
                    context.SaveChanges();
                }
            }
            else if (postedFile == null)
            {
                using (var context = new dbNautilusEntities1())
                {
                    var Info = new TUBLAInfo()
                    {
                        datum = DateTime.Now,
                        informacija = tInfo.informacija,
                        tip = tInfo.tip                       
                    };

                    context.TUBLAInfo.Add(Info);
                    context.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Info");
        }

        public ActionResult Delete(int id)
        {
            using (var context = new dbNautilusEntities1())
            {
                TUBLAInfo info = context.TUBLAInfo.Where(x => x.idInfo == id).FirstOrDefault<TUBLAInfo>();
                context.TUBLAInfo.Remove(info);
                context.SaveChanges();
            }

            return RedirectToAction("PrikažiInfo", "Info");
        }

        
    }
}