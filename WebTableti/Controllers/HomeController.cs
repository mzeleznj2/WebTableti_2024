using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebTableti.Models;

namespace WebTableti.Controllers

{
    public class HomeController : Controller
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["dbNautilusConnectionString"].ConnectionString;
        private SqlConnection conn = new SqlConnection();
        private SqlCommand cmd = new SqlCommand();
        private dbNautilusEntities1 context = new dbNautilusEntities1();
        


        public ActionResult Index()
        {
            
                if (context.TUBLAInfo.OrderByDescending(x => x.datum).ToList() != null)
                {
                    using (context)
                        {
                            return View(context.TUBLAInfo.OrderByDescending(x => x.datum).ToList());
                        }                       
                }               
          return View();  
    }

       
        //Grupa1
        public ActionResult About()
        {
            Podaci podaci = new Podaci();
            DataTable dtPodaci = podaci.NapuniGrid();

            //funckija za slanje maila u Campionario
           podaci.SendEmailToCampionario();
                       
            
            List<Brojac> listaBrojac = new List<Brojac>();
            DataTable dtPodaciBrojac = podaci.brojacGrupa1();

            foreach (DataRow dr in dtPodaciBrojac.Rows)
            {

                listaBrojac = dtPodaciBrojac.AsEnumerable().Select
                (x => new Brojac
                {
                    LastStopCode = Convert.ToInt32(x["LastStopCode"]),
                    brojac = Convert.ToInt32(x["Brojac"])                   
                }).ToList();
            }
                                  
            var brojac2 = from r in listaBrojac
                          where r.LastStopCode == 50002
                          select r.brojac;

            var brojac7 = from r in listaBrojac
                          where r.LastStopCode == 50007
                          select r.brojac;

            var brojac8 = from r in listaBrojac
                          where r.LastStopCode == 50008
                          select r.brojac;

            ViewBag.Brojac2 = Convert.ToInt32(brojac2.FirstOrDefault());
            ViewBag.Brojac7 = Convert.ToInt32(brojac7.FirstOrDefault());
            ViewBag.Brojac8 = Convert.ToInt32(brojac8.FirstOrDefault());
            


            return View("About", dtPodaci);

        }
            

        //Grupa2
        public ActionResult About2()
        {
            Podaci podaci = new Podaci();
            DataTable dtPodaci = podaci.NapuniGrid2();

            //funckija za slanje maila u Campionario
           // podaci.SendEmailToCampionario();

            List<Brojac> listaBrojac = new List<Brojac>();
            DataTable dtPodaciBrojac = podaci.brojacGrupa2();

            foreach (DataRow dr in dtPodaciBrojac.Rows)
            {

                listaBrojac = dtPodaciBrojac.AsEnumerable().Select
                (x => new Brojac
                {
                    LastStopCode = Convert.ToInt32(x["LastStopCode"]),
                    brojac = Convert.ToInt32(x["Brojac"])
                }).ToList();
            }

            var brojac2 = from r in listaBrojac
                          where r.LastStopCode == 50002
                          select r.brojac;

            var brojac7 = from r in listaBrojac
                          where r.LastStopCode == 50007
                          select r.brojac;

            var brojac8 = from r in listaBrojac
                          where r.LastStopCode == 50008
                          select r.brojac;

            ViewBag.Brojac2 = Convert.ToInt32(brojac2.FirstOrDefault());
            ViewBag.Brojac7 = Convert.ToInt32(brojac7.FirstOrDefault());
            ViewBag.Brojac8 = Convert.ToInt32(brojac8.FirstOrDefault());
        

            return View("About2", dtPodaci);
        }


        //Grupa3
        public ActionResult About3()
        {
            Podaci podaci = new Podaci();
            DataTable dtPodaci = podaci.NapuniGrid3();

            //funckija za slanje maila u Campionario
           // podaci.SendEmailToCampionario();

            List<Brojac> listaBrojac = new List<Brojac>();
            DataTable dtPodaciBrojac = podaci.brojacGrupa3();

            foreach (DataRow dr in dtPodaciBrojac.Rows)
            {

                listaBrojac = dtPodaciBrojac.AsEnumerable().Select
                (x => new Brojac
                {
                    LastStopCode = Convert.ToInt32(x["LastStopCode"]),
                    brojac = Convert.ToInt32(x["Brojac"])
                }).ToList();
            }

            var brojac2 = from r in listaBrojac
                          where r.LastStopCode == 50002
                          select r.brojac;

            var brojac7 = from r in listaBrojac
                          where r.LastStopCode == 50007
                          select r.brojac;

            var brojac8 = from r in listaBrojac
                          where r.LastStopCode == 50008
                          select r.brojac;

            ViewBag.Brojac2 = Convert.ToInt32(brojac2.FirstOrDefault());
            ViewBag.Brojac7 = Convert.ToInt32(brojac7.FirstOrDefault());
            ViewBag.Brojac8 = Convert.ToInt32(brojac8.FirstOrDefault());


            return View("About3", dtPodaci);
        }


        //STARO
        //public ActionResult DetailsOld(int kod, string linija)
        //{
        //    DataTable dtPovijest = new DataTable();
        //    using (conn = new SqlConnection(connString))
        //    {
        //        conn.Open();
        //        cmd = new SqlCommand("Select * from NF_Tracc_fermi_96ore where MachCode = @code order by DateRec desc", conn);
        //        cmd.Parameters.AddWithValue("@code", kod);
        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        adapter.Fill(dtPovijest);
        //    }

        //    ViewBag.Data = kod;

        //    return View("Details", dtPovijest);
        //}


        //2 view-a u 1
        public ActionResult Details(int kod, string linija)
        {
            Podaci podaci = new Podaci();
            ViewModel model = new ViewModel(); 
            model.podaciDetalji = podaci.sviDetalji(kod);

            
            model.podacidetalji06 = podaci.sviDetalji06(linija.Trim());
            TempData["linija"] = linija.Trim();


            return View(model);
        }


        //Grupa 1
        public ActionResult ChangeStatus(string kod, int zastavica)
        {

            int linija = Convert.ToInt32(TempData["linija"]);
            using (var context = new dbNautilusEntities1())
            {
                if (context.TUBLAFermi.Any(o => o.UniqueID == kod))
                {
                    var status = context.TUBLAFermi.OrderBy(a => a.ID).LastOrDefault(a => a.UniqueID == kod);
                    status.Status = true;
                    context.SaveChanges();
                }
                else
                {
                    var Stato = new TUBLAFermi()
                    {
                        UniqueID = kod,
                        Status = true
                    };
                    context.TUBLAFermi.Add(Stato);
                    context.SaveChanges();
                }
            }
         

            if (zastavica > 0)
            {
                return RedirectToAction("Sortirano1", new { linija = linija });
            }           

            return RedirectToAction("About", "Home");
        }

        //Grupa 2
        public ActionResult ChangeStatus2(string kod, int zastavica)
        {
            int linija = Convert.ToInt32(TempData["linija"]);

            using (var context = new dbNautilusEntities1())
            {
                if (context.TUBLAFermi.Any(o => o.UniqueID == kod))
                {
                    var status = context.TUBLAFermi.OrderBy(a => a.ID).LastOrDefault(a => a.UniqueID == kod);
                    status.Status = true;
                    context.SaveChanges();
                }
                else
                {
                    var Stato = new TUBLAFermi()
                    {
                        UniqueID = kod,
                        Status = true
                    };
                    context.TUBLAFermi.Add(Stato);
                    context.SaveChanges();
                }
            }

            if (zastavica > 0)
            {
                return RedirectToAction("Sortirano2", new { linija = linija });
            }

            return RedirectToAction("About2", "Home");
        }


        //Grupa 3
        public ActionResult ChangeStatus3(string kod, int zastavica)
        {
            int linija = Convert.ToInt32(TempData["linija"]);

            using (var context = new dbNautilusEntities1())
            {
                if (context.TUBLAFermi.Any(o => o.UniqueID == kod))
                {
                    var status = context.TUBLAFermi.OrderBy(a => a.ID).LastOrDefault(a => a.UniqueID == kod);
                    status.Status = true;
                    context.SaveChanges();
                }
                else
                {
                    var Stato = new TUBLAFermi()
                    {
                        UniqueID = kod,
                        Status = true
                    };
                    context.TUBLAFermi.Add(Stato);
                    context.SaveChanges();
                }
            }

            if (zastavica > 0)
            {
                return RedirectToAction("Sortirano3", new { linija = linija });
            }

            return RedirectToAction("About3", "Home");
        }



        //Grupa 1
        public ActionResult OdbaciStatus(string kod, int zastavica) {

            int linija = Convert.ToInt32(TempData["linija"]);

            using (var context = new dbNautilusEntities1())
            {
                var status = context.TUBLAFermi.Where(a => a.UniqueID == kod)
                                               .OrderByDescending(p => p.ID)
                                               .FirstOrDefault();
                context.TUBLAFermi.Remove(status);
                context.SaveChanges();
            }


            if (zastavica > 0)
            {
                return RedirectToAction("Sortirano1", new { linija = linija });
            }

            return RedirectToAction("About", "Home");
        }

        //Grupa 2
        public ActionResult OdbaciStatus2(string kod, int zastavica)
        {
            int linija = Convert.ToInt32(TempData["linija"]);

            using (var context = new dbNautilusEntities1())
            {
                var status = context.TUBLAFermi.Where(a => a.UniqueID == kod).OrderByDescending(p => p.ID).FirstOrDefault();
                context.TUBLAFermi.Remove(status);
                context.SaveChanges();
            }

            if (zastavica > 0)
            {
                return RedirectToAction("Sortirano2", new { linija = linija });
            }

            return RedirectToAction("About2", "Home");
        }


        //Grupa 3
        public ActionResult OdbaciStatus3(string kod, int zastavica)
        {
            int linija = Convert.ToInt32(TempData["linija"]);

            using (var context = new dbNautilusEntities1())
            {
                var status = context.TUBLAFermi.Where(a => a.UniqueID == kod).OrderByDescending(p => p.ID).FirstOrDefault();
                context.TUBLAFermi.Remove(status);
                context.SaveChanges();
            }

            if (zastavica > 0)
            {
                return RedirectToAction("Sortirano3", new { linija = linija });
            }

            return RedirectToAction("About3", "Home");
        }


        //Grupa 1 filtriranje
        public ActionResult Sortirano1(int linija)
        {

            Podaci podaci = new Podaci();
            DataTable dtPodaci = podaci.NapuniGridPoLinijiGrupa1(linija);


            List<Brojac> listaBrojac = new List<Brojac>();
            DataTable dtPodaciBrojac = podaci.brojacGrupa1();

            foreach (DataRow dr in dtPodaciBrojac.Rows)
            {

                listaBrojac = dtPodaciBrojac.AsEnumerable().Select
                (x => new Brojac
                {
                    LastStopCode = Convert.ToInt32(x["LastStopCode"]),
                    brojac = Convert.ToInt32(x["Brojac"])                    
                }).ToList();
            }

            var brojac2 = from r in listaBrojac
                          where r.LastStopCode == 50002                          
                          select r.brojac;

            var brojac7 = from r in listaBrojac
                          where r.LastStopCode == 50007                          
                          select r.brojac;

            var brojac8 = from r in listaBrojac
                          where r.LastStopCode == 50008                          
                          select r.brojac;

            ViewBag.Brojac2 = Convert.ToInt32(brojac2.FirstOrDefault());
            ViewBag.Brojac7 = Convert.ToInt32(brojac7.FirstOrDefault());
            ViewBag.Brojac8 = Convert.ToInt32(brojac8.FirstOrDefault());

            string linijaFull = "LINEA-" + linija.ToString();
            TempData["linija"] = linijaFull;

           
            return View("Sortirano1", dtPodaci);
        }

        //Grupa 2 filtriranje
        public ActionResult Sortirano2(int linija)
        {
            Podaci podaci = new Podaci();
            DataTable dtPodaci = podaci.NapuniGridPoLinijiGrupa2(linija);


            List<Brojac> listaBrojac = new List<Brojac>();
            DataTable dtPodaciBrojac = podaci.brojacGrupa2();

            foreach (DataRow dr in dtPodaciBrojac.Rows)
            {

                listaBrojac = dtPodaciBrojac.AsEnumerable().Select
                (x => new Brojac
                {
                    LastStopCode = Convert.ToInt32(x["LastStopCode"]),
                    brojac = Convert.ToInt32(x["Brojac"])
                }).ToList();
            }

            var brojac2 = from r in listaBrojac
                          where r.LastStopCode == 50002
                          select r.brojac;

            var brojac7 = from r in listaBrojac
                          where r.LastStopCode == 50007
                          select r.brojac;

            var brojac8 = from r in listaBrojac
                          where r.LastStopCode == 50008
                          select r.brojac;

            ViewBag.Brojac2 = Convert.ToInt32(brojac2.FirstOrDefault());
            ViewBag.Brojac7 = Convert.ToInt32(brojac7.FirstOrDefault());
            ViewBag.Brojac8 = Convert.ToInt32(brojac8.FirstOrDefault());

            string linijaFull = "LINEA-" + linija.ToString();
            TempData["linija"] = linijaFull;

            return View("Sortirano2", dtPodaci);
        }

        //Grupa 3 filtriranje
        public ActionResult Sortirano3(int linija)
        {
            Podaci podaci = new Podaci();
            DataTable dtPodaci = podaci.NapuniGridPoLinijiGrupa3(linija);


            List<Brojac> listaBrojac = new List<Brojac>();
            DataTable dtPodaciBrojac = podaci.brojacGrupa3();

            foreach (DataRow dr in dtPodaciBrojac.Rows)
            {

                listaBrojac = dtPodaciBrojac.AsEnumerable().Select
                (x => new Brojac
                {
                    LastStopCode = Convert.ToInt32(x["LastStopCode"]),
                    brojac = Convert.ToInt32(x["Brojac"])
                }).ToList();
            }

            var brojac2 = from r in listaBrojac
                          where r.LastStopCode == 50002
                          select r.brojac;

            var brojac7 = from r in listaBrojac
                          where r.LastStopCode == 50007
                          select r.brojac;

            var brojac8 = from r in listaBrojac
                          where r.LastStopCode == 50008
                          select r.brojac;

            ViewBag.Brojac2 = Convert.ToInt32(brojac2.FirstOrDefault());
            ViewBag.Brojac7 = Convert.ToInt32(brojac7.FirstOrDefault());
            ViewBag.Brojac8 = Convert.ToInt32(brojac8.FirstOrDefault());

            string linijaFull = "LINEA-" + linija.ToString();
            TempData["linija"] = linijaFull;

            return View("Sortirano3", dtPodaci);
        }


        public ActionResult PopisRemont()
        {
            return View(context.RemontMach.ToList());
        }
    }
}