using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTableti.Models;

namespace WebTableti.Controllers
{
    public class RemontController : Controller
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["dbNautilusConnectionString"].ConnectionString;
        private dbNautilusEntities1 context = new dbNautilusEntities1();


        // GET: Remont
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                Podaci podaci = new Podaci();
                podaci.AzurirajMasine();

                return View(context.RemontMach.ToList());
            }

            return RedirectToAction("Login");
        }

        public ActionResult DodajRemont()
        {
            if (Session["User"] != null)
            {
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult DodajRemont(RemontMach tremont)
        {
            if (Session["User"] != null)
            {
                string DBrojMatricole = "D_" + tremont.MachineId;
                string LBrojMatricole = "L_" + tremont.MachineId;
                string GBrojMatricole = "G_" + tremont.MachineId;                


                if (tremont.MachineId.Length < 5)
                {
                    TempData["duljinaStringa"] = "Matricola mora sadržavati 5 znakova!";
                    return RedirectToAction("DodajRemont");
                }               

                else if (string.IsNullOrWhiteSpace(DBrojMatricole) && 
                         string.IsNullOrWhiteSpace(LBrojMatricole) &&
                         string.IsNullOrWhiteSpace(GBrojMatricole)) 
                {
                    TempData["ErrorMatricola"] = "Ne postoji mašina sa matricolom " + tremont.MachineId + "!";
                    return RedirectToAction("DodajRemont");
                }

                var id_machineD = context.MACHINES_SETUP.Where(a => a.MachineId == DBrojMatricole).FirstOrDefault();
                var id_machineL = context.MACHINES_SETUP.Where(a => a.MachineId == LBrojMatricole).FirstOrDefault();
                var id_machineG = context.MACHINES_SETUP.Where(a => a.MachineId == GBrojMatricole).FirstOrDefault();

                using (var context = new dbNautilusEntities1())
                {
                    if (id_machineD != null)
                    {
                        int brojMasine = id_machineD.MachCode;
                        string matricola = id_machineD.MachineId;

                        var remont = new RemontMach()
                        {
                            DatumOd = tremont.DatumOd,
                            Opis = tremont.Opis,
                            MachCode = brojMasine,
                            MachineId = matricola,
                            Korisnik = Session["User"].ToString()
                        };

                        context.RemontMach.Add(remont);
                        context.SaveChanges();

                        TempData["Uspjeh"] = "Dodan je novi remont za mašinu " + brojMasine + " sa matricolom " + matricola + ".";
                        return RedirectToAction("DodajRemont");
                    }
                    else if (id_machineL != null)
                    {
                        int brojMasine = id_machineL.MachCode;
                        string matricola = id_machineL.MachineId;

                        var remont = new RemontMach()
                        {
                            //DatumOd = DateTime.Now,
                            DatumOd = tremont.DatumOd,
                            Opis = tremont.Opis,
                            MachCode = brojMasine,
                            MachineId = matricola,
                            Korisnik = Session["User"].ToString()
                        };

                        context.RemontMach.Add(remont);
                        context.SaveChanges();

                        TempData["Uspjeh"] = "Dodan je novi remont za mašinu " + brojMasine + " sa matricolom " + matricola + ".";
                        return RedirectToAction("DodajRemont");
                    }
                    else if (id_machineG != null)
                    {
                        int brojMasine = id_machineG.MachCode;
                        string matricola = id_machineG.MachineId;

                        var remont = new RemontMach()
                        {
                            //DatumOd = DateTime.Now,
                            DatumOd = tremont.DatumOd,
                            Opis = tremont.Opis,
                            MachCode = brojMasine,
                            MachineId = matricola,
                            Korisnik = Session["User"].ToString()
                        };

                        context.RemontMach.Add(remont);
                        context.SaveChanges();

                        TempData["Uspjeh"] = "Dodan je novi remont za mašinu " + brojMasine + " sa matricolom " + matricola + ".";
                        return RedirectToAction("DodajRemont");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Ne postoji mašina sa matricolom " + tremont.MachineId + "!";
                        return RedirectToAction("DodajRemont");
                    }
                }
               
            }

            return RedirectToAction("Login");

        }


        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logiranje(string username, string password)
        {
            string user = "";
            string pass = "";

            user = username;
            pass = password;

            Response.Write("Imena " + user + " " + pass);

            if ((user == "admin" && pass == "admin") || (user == "rodiz" && pass == "zoran123!")
                || (user == "kosn" && pass == "nikola123!") || (user == "ajhlerm" && pass == "marko123!"))
            {
                Session["User"] = user;
                Session["Password"] = pass;

                return RedirectToAction("Index");
            }


            @TempData["ErrorMessage"] = "Krivi username ili password!";
            return RedirectToAction("Login");

        }


        public ActionResult EditRemont(int id)
        {
            if (Session["User"] != null)
            {
                var data = context.RemontMach.Where(x => (x.IdRemonta == id)).FirstOrDefault();

                return View(data);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        [HttpPost]
        public ActionResult EditRemont(RemontMach remontMach)
        {
            if (ModelState.IsValid)
            {
                using (context)
                {
                    var data = context.RemontMach.Where(x => (x.IdRemonta == remontMach.IdRemonta)).FirstOrDefault();

                    if (data != null)
                    {
                        //data.DatumPov =  remontMach.DatumPov;
                        data.Opis = remontMach.Opis;
                    }
                    context.SaveChanges();

                    TempData["uspjehUpdate"] = "Uspješno ste ažurirali Remont sa matricolom " + data.MachineId + "!";
                }

                return RedirectToAction("Index");
            }

            return View();

        }


        public ActionResult DeleteRemont(int id)
        {
            using (context)
            {
                RemontMach remont = context.RemontMach.Where(x => x.IdRemonta == id).FirstOrDefault<RemontMach>();
                context.RemontMach.Remove(remont);
                context.SaveChanges();

                TempData["obrisano"] = "Uspješno ste obrisali Remont sa brojem matricole: " + remont.MachineId + "!";
            }

            return RedirectToAction("Index", "Remont");
        }


        public ActionResult Logout()
        {
            if (Session["User"] != null)
            {
                Session["User"] = "";
                Session["Password"] = "";

                RedirectToAction("Login");

            }

            return View("Login");
        }

    }
}