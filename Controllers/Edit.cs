using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Motel_birkenhein.Data;
using Motel_birkenhein.Migrations;
using Motel_birkenhein.Models;
using Newtonsoft.Json;

namespace Motel_birkenhein.Controllers
{
    public class Edit : Controller
    {
        MotelBirkenheinData db;
        private IWebHostEnvironment Environment;
        private readonly UserManager<Models.User> _userManager;
        private readonly SignInManager<Models.User> _signInManager;
        public Edit(UserManager<Models.User> userManager, SignInManager<Models.User> signInManager, MotelBirkenheinData context, IWebHostEnvironment _environment)
        {
            db = context;
            Environment = _environment;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Tarif()
        {
            var hotel = db.Hotel.ToList();
            ViewBag.Hotel = hotel;

            var Tarif = db.Tarif.ToList();
            ViewBag.Tarif = Tarif;  

            return View();
        }


        [HttpPost]
        public IActionResult Tarif_Edit(Tarif tarif)
        {
            db.Update(tarif);
            db.SaveChanges();

            return RedirectToAction("Tarif");
        }
        public IActionResult Reservierung()
        {
            ViewBag.Hotel = db.Hotel.ToList();
            ViewBag.Zimmer = db.Zimmer.ToList();
            ViewBag.Bad = db.Bett.ToList();
            var bed = db.Bett.Select(x=>x.BettName).ToList();

            ViewBag.Gast = db.Gast.ToList();

            ViewBag.Firma = db.Firma.ToList();

            ViewBag.BadJson = JsonConvert.SerializeObject(bed);

            return View();
        }

        [HttpPost]
        public JsonResult HotelSelect([FromBody] HotelRequest request)
        {
           
            var H = db.Hotel.FirstOrDefault(x => x.Name == request.Name);

            var zimmer = db.Zimmer.Where(x => x.XHotelId == H.Id).ToList();

            var notRezerviert = zimmer.Where(x => x.XReservierungId == null).ToList();

            var model = notRezerviert.Select(x => new
            {
                Zimmer = x.Zimmernummer

            }).ToList();

            return Json(model);
        }

        [HttpPost]
        public JsonResult Gast(Models.Gast gast)
        {
            db.Update(gast); 
            db.SaveChanges();

            var model = db.Gast.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Adresse = x.Adresse,
                StAG = x.StAG,
                Geschlecht = x.Geschlecht,
                Telefon = x.Telefon,
                prvat_or_firma =x.prvat_or_firma,
                Kunden_Nr = x.Kunden_Nr

            }).ToList();

 

            return Json(model);
        }
        public class HotelRequest
        {
            public string Name { get; set; }
        }
        [HttpPost]
        public JsonResult Tarif([FromBody] HotelRequest request)
        {
           var H = db.Hotel.FirstOrDefault(x=>x.Name == request.Name);

            var Tarif = db.Tarif.Where(x => x.XHotelId == H.Id).ToList();

            var model = Tarif.Select(x=> new
            {
                Name = x.Name,
                price = x.price

            }).ToList();

            return Json(model);
        }

        [HttpPost]
        public JsonResult SelectBad([FromBody] ValueModel model)
        {
            var zimmerList = db.Zimmer
                     .Include(x => x.Bett)
                     .Where(x => x.Zimmernummer == model.Value).ToList();


            var bed = zimmerList.SelectMany(x=>x.Bett).Where(x=>x.XReservierungId==null).Select(z => new
            {        
                BettId = z.Id,
                BettName = z.BettName
            }).ToList();

            return Json(bed);
        }
        public class ValueModel
        {
            public string Value { get; set; }
        }

        [HttpPost]
        public JsonResult FirmaInfo([FromBody] Firma model)
        {
           var firma = db.Firma.FirstOrDefault(x=>x.FirmeName == model.FirmeName);
            if (firma == null)
            {
                return Json(null);
            }
            return Json(firma);
        }
        [HttpPost]
        public JsonResult GastInfo([FromBody] Models.Gast model)
        {
            var gast = db.Gast.FirstOrDefault(x => x.Name == model.Name);
            
            if (gast == null)
            {
                return Json(null); 
            }
            return Json(gast);
        }
        

        [HttpPost]
        public JsonResult Firma(Firma firma)
        {
            db.Update(firma);
            db.SaveChanges();

            var model = db.Firma.Select(z => new
             {
                Id = z.Id,
                FirmeName = z.FirmeName,
                Name = z.Name,
                Adresse = z.Adresse,
                HRB = z.HRB,
                Telefon = z.Telefon,
                Bemekung = z.Bemekung,
                Kunden_Nr = z.Kunden_Nr,

            }).ToList();


            return Json(model);
        }

        [HttpPost]
        public JsonResult Hotel (Models.Hotel hotel)
        {
            db.Update(hotel);
            db.SaveChanges();

            var model = db.Hotel.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Adresse = x.Adresse,
                Kontaktperson = x.Kontaktperson,
                Telefon = x.Telefon,
                Private = x.Private,

            }).ToList();

            return Json(model);
        }
        [HttpPost]
        public JsonResult Zimmer(Zimmer zimmer)
        {
            db.Update(zimmer);
            db.SaveChanges();

            var model = db.Zimmer.Select(x => new
            {
                Id = x.Id,
                Zimmernummer = x.Zimmernummer,
                XReservierungId = x.XReservierungId,
                XHotelId = x.XHotelId,

            }).ToList();


            return Json(model);
        }
        public async Task<IActionResult> DeleteTarif(Tarif tarif)
        {
            var tr = db.Tarif.FirstOrDefault(x => x.Id == tarif.Id);
            db.Remove(tarif);
            db.SaveChanges();

            return RedirectToAction("Tarif");
        }
        public async Task<IActionResult>Tarif_Save(Tarif tarif)
        {
            var tr = db.Tarif.FirstOrDefault(x=>x.Id == tarif.Id);
            tr.price = tarif.price;
            tr.Name = tarif.Name;

            db.Update(tr);
            db.SaveChanges();
            return RedirectToAction("Tarif");
        }
        [HttpPost]
        public async Task<IActionResult> Bett(Bett bett)
        {
            db.Update(bett);
            db.SaveChanges();

            var Bett = db.Bett.Select(x => x).ToList();

            ViewBag.Bett = Bett;

            return Json(new { message = "Zimmer Erstelen" });
        }


        [HttpPost]
        public IActionResult ReservierungSave(Reservierung reservierung)
        {

            db.Update(reservierung);
            db.SaveChanges();

            var hotel = db.Hotel.Include(x => x.Zimmer).ThenInclude(x=>x.Bett).Where(x => x.Name == reservierung.Name).ToList();

            var Zymmer = hotel.SelectMany(x => x.Zimmer).FirstOrDefault(x => x.Zimmernummer == reservierung.Zimmernummer);

            Zymmer.XReservierungId = reservierung.Id;
            
            db.Update(Zymmer);
            db.SaveChanges();


            var allbet = reservierung.AllBett.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(b => b.Trim()).ToList();

            var bett = Zymmer.Bett.Where(x => allbet.Contains(x.BettName)).ToList();

            foreach(var list in bett)
            {
                list.XReservierungId = reservierung.Id;
                list.Gast = reservierung.NameGast;
                db.Update(list);
                db.SaveChanges();
            }

            var Gast = db.Gast.Where(x => x.Name == reservierung.NameGast).ToList();
            
            if(Gast.Count > 0)
            {
                foreach (var list in Gast)
                {
                    list.XReservierungId = reservierung.Id;
                    db.Update(list);
                    db.SaveChanges();
                }                
            }
           
            var Firma = db.Firma.Where(x=>x.FirmeName == reservierung.Rechnungsempf).ToList();

            if (Firma.Count > 0) 
            {
                foreach (var list in Firma)
                {
                    list.XReservierungId = reservierung.Id;
                    db.Update(list);
                    db.SaveChanges();
                }
           
            }
          

            return RedirectToAction("Reservierung");
        }

    }
}
