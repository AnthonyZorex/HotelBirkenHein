using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Motel_birkenhein.Data;
using Motel_birkenhein.Migrations;
using Motel_birkenhein.Models;
using Motel_birkenhein.Send_Data;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Resources;

namespace Motel_birkenhein.Controllers
{
    public class HomeController : Controller
    {
        MotelBirkenheinData db;
        private IWebHostEnvironment Environment;
        private readonly UserManager<Models.User> _userManager;
        private readonly SignInManager<Models.User> _signInManager;
        private readonly ILogger<HomeController> _logger;
        public HomeController(UserManager<Models.User> userManager, ILogger<HomeController> logger, SignInManager<Models.User> signInManager, MotelBirkenheinData context, IWebHostEnvironment _environment)
        {
            db = context;
            Environment = _environment;
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
        }
     
        public IActionResult Index()
        {

            var ReserviertChek = db.Reservierung.Where(x => x.Rcontinue == true).ToList();

            foreach (var item in ReserviertChek)
            {
                if (item.Reserviertbis < DateTime.Today)
                {
                    float money = 0;

                    if (item.Muss_bezahlen > 0 || item.Rcontinue == false)
                    {

                        item.Rcontinue = false;
                        db.Update(item);
                        db.SaveChanges();
                    }
                    else
                    {

                        var hotel = db.Hotel.FirstOrDefault(x => x.Name == item.Name);

                        var zm = db.Zimmer.Include(x=>x.Bett).Where(x=>x.Zimmernummer == item.Zimmernummer).ToList();

                        var bet = zm.SelectMany(x=>x.Bett).Where(x=>x.XReservierungId!=null).Count();

                        var betCount = zm.SelectMany(x => x.Bett).Count();

                        var tarif = db.Tarif.Where(x => x.XHotelId == hotel.Id).ToList();

                        item.Rcontinue = true;

                     

                        var startDate = item.Reserviertbis.Value;

                        var endDate = startDate.AddMonths(1);


                        for (var date = startDate; date < endDate; date = date.AddDays(1))
                        {
                            var dayOfWeek = date.DayOfWeek;
                           
                            if (betCount == bet)
                            {
                                var cost = tarif.FirstOrDefault(x => x.Name == "Zimmer");
                                money += cost.price ?? 0;
                            }
                            else
                            {
                                var cost = tarif.FirstOrDefault(x => x.Name == "Bett");
                                var costWikend = tarif.FirstOrDefault(x => x.Name == "Wochenende");

                                if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                                {
                                    money += cost.price ?? 0;
                                }
                                else
                                {
                                    money += costWikend.price ?? 0;
                                }
                            }
                            
                        }
                        item.Muss_bezahlen = money;

                        item.Reserviertbis = endDate;

                        db.Update(item);
                        db.SaveChanges();
                    }
                }

            }


            var  Hotel = db.Hotel.Include(x=>x.Zimmer).ThenInclude(x=>x.Bett).Where(x => x.Name == "Motel Birkenhain").ToList();

            var reservierung = db.Reservierung.Select(x=>x).ToList();   
           
            var firstHotel = db.Hotel.Include(x => x.Zimmer).ThenInclude(x => x.Bett).FirstOrDefault(x=>x.Name == "Motel Birkenhain");

            var allHotel = db.Hotel.Include(x => x.Zimmer).ThenInclude(x => x.Bett).Where(x => x.Name != "Motel Birkenhain").ToList();

            ViewBag.allHotel = allHotel;

            ViewBag.FirstHotel = firstHotel;

            var zimmerGrouped = allHotel
              .SelectMany(h => h.Zimmer, (hotel, zimmer) => new { hotel.Name, zimmer })
              .GroupBy(x => x.Name)
              .Select(g => new
              {
                  HotelName = g.Key,
                  ReservierteZimmer = g.Count(x => x.zimmer.XReservierungId == null)
              })
              .ToList();


            var bettenGrouped = allHotel
             .SelectMany(h => h.Zimmer, (hotel, zimmer) => new { hotel.Name, zimmer })
             .SelectMany(x => x.zimmer.Bett, (x, bett) => new { x.Name, bett })
             .GroupBy(x => x.Name)
             .Select(g => new
             {
                 HotelName = g.Key,
                 ReservierteBetten = g.Count(x => x.bett.XReservierungId == null)
             })
             .ToList();

            ViewBag.AllZimmer = zimmerGrouped;
            ViewBag.AllBett = bettenGrouped;

            var first_hotel_reservierung = reservierung.Where(x=>x.Name == firstHotel.Name).ToList();

            var zahlen = first_hotel_reservierung.Sum(x=>x.Zahlen) ?? 0; 
            var muszahlen = first_hotel_reservierung.Sum(x => x.Muss_bezahlen) ?? 0;

            ViewBag.Zahlen = zahlen;    
            ViewBag.Muszahlen = muszahlen;


            var notizen = first_hotel_reservierung.Select(x => x.Bemerkung).Count();

            ViewBag.Notieren = notizen;

            var countZimmer = Hotel.SelectMany(x => x.Zimmer).Count();

            var rezervierenZimmer = Hotel.SelectMany(x => x.Zimmer).Where(x => x.XReservierungId != null).Count();
            
            ViewBag.RezervierungZimmer = rezervierenZimmer;
            ViewBag.ZimmerCount = countZimmer;

            var freiZimmer = countZimmer - rezervierenZimmer;

            ViewBag.FreiZimmer = freiZimmer;

            var countBett = Hotel.SelectMany(x=>x.Zimmer).SelectMany(x=>x.Bett).Count();

            var reservierungBett = Hotel.SelectMany(x => x.Zimmer).SelectMany(x => x.Bett).Where(x=>x.XReservierungId!=null).Count();

            ViewBag.ReservierungBett = reservierungBett;

            var freiBett = countBett - reservierungBett;

            ViewBag.BettFrei = freiBett;

            ViewBag.BettCount = countBett;

            return View();
        }


        [HttpGet]
        public IActionResult Bett_Edit(Guid Id)
        {
            var Bed = db.Bett.FirstOrDefault(x => x.Id == Id);

            var Zimmer = db.Zimmer.FirstOrDefault(x => x.Id == Bed.XZimmerId);

            var hotel = db.Hotel.FirstOrDefault(x => x.Id == Zimmer.XHotelId);

            var reservierung = db.Reservierung.Where(x => x.Id == Bed.XReservierungId).ToList();

            if (Bed?.XReservierungId != null)
            {
                ViewBag.ReserviertInformations = db.Reservierung
                    .Where(x => x.Id == Bed.XReservierungId)
                    .ToList();
            }
            else
            {
                ViewBag.ReserviertInformations = new List<Reservierung>(); 
            }

            if (reservierung.Count()>0)
            {
                var Gast = db.Gast.FirstOrDefault(x => x.XReservierungId == reservierung.First().Id);
                var Firma = db.Firma.FirstOrDefault(x => x.XReservierungId == reservierung.First().Id);
                ViewBag.Gast = Gast;
                ViewBag.Firma = Firma;
              
            }


            ViewBag.AllZimmer = db.Zimmer.Where(x => x.XHotelId == hotel.Id).ToList();

            ViewBag.Zimmer = Zimmer;
            ViewBag.Hotel = hotel;
         

            return View(Bed);
        }

        [HttpGet]
        public IActionResult Reservierung_Edit(Guid Id)
        {
            var reservierung = db.Reservierung.FirstOrDefault(x => x.Id == Id);

            var hotelReservierung = db.Hotel.FirstOrDefault(x => x.Name == reservierung.Name);

            var zimmerReservierung = db.Zimmer.FirstOrDefault(x => x.Zimmernummer == reservierung.Zimmernummer);

            var bedR = db.Bett.Where(x=>x.XReservierungId == reservierung.Id).ToList();

            var gastR = db.Gast.FirstOrDefault(x => x.XReservierungId == reservierung.Id);

            var firmaR = db.Firma.FirstOrDefault(x => x.FirmeName == reservierung.Rechnungsempf);

            ViewBag.ReservierungH = hotelReservierung;
            ViewBag.ReservierungZ = zimmerReservierung;
            ViewBag.Rfirma = firmaR;
            ViewBag.Rgast = gastR;

            ViewBag.Hotel = db.Hotel.ToList();
            ViewBag.Zimmer = db.Zimmer.ToList();
            ViewBag.Bad = db.Bett.ToList();


            var bed = db.Bett.Select(x => x.BettName).ToList();

            ViewBag.Gast = db.Gast.ToList();

            ViewBag.Firma = db.Firma.ToList();

            ViewBag.BadJson = JsonConvert.SerializeObject(bed);
            return View(reservierung);
        }

        [HttpPost]
        public IActionResult ReservierungSave(Reservierung reservierung)
        {
  
            db.Update(reservierung);
            db.SaveChanges();

            var hotel = db.Hotel.Include(x => x.Zimmer).ThenInclude(x => x.Bett).Where(x => x.Name == reservierung.Name).ToList();

            var Zymmer = hotel.SelectMany(x => x.Zimmer).FirstOrDefault(x => x.Zimmernummer == reservierung.Zimmernummer);

            Zymmer.XReservierungId = reservierung.Id;

            db.Update(Zymmer);
            db.SaveChanges();


            var allbet = reservierung.AllBett.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(b => b.Trim()).ToList();

            var bett = Zymmer.Bett.Where(x => allbet.Contains(x.BettName)).ToList();

            foreach (var list in bett)
            {
                list.XReservierungId = reservierung.Id;
                list.Gast = reservierung.NameGast;
                db.Update(list);
                db.SaveChanges();
            }

            var Gast = db.Gast.Where(x => x.Name == reservierung.NameGast).ToList();

            if (Gast.Count > 0)
            {
                foreach (var list in Gast)
                {
                    list.XReservierungId = reservierung.Id;
                    db.Update(list);
                    db.SaveChanges();
                }
            }

            var Firma = db.Firma.Where(x => x.FirmeName == reservierung.Rechnungsempf).ToList();

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

        [HttpPost]
        public IActionResult Betten_Editieren(Bett bett)
        {
            db.Update(bett);
            db.SaveChanges();
            return RedirectToAction("Bett_Edit", new { id = bett.Id });
        }

        [HttpGet]
        public IActionResult Zimmer_Edit (Guid Id)
        {
            var Zimmer = db.Zimmer.Include(x=>x.Bett).FirstOrDefault(x=>x.Id==Id);  

            var hotel = db.Hotel.FirstOrDefault(x=>x.Id==Zimmer.XHotelId);

            var allHotel = db.Hotel.ToList();

            ViewBag.AllHotel = allHotel;
            
            ViewBag.Zimmer = Zimmer;

            ViewBag.Hotel = hotel;

            var reservierung = db.Reservierung.Where(x => x.Id == Zimmer.XReservierungId).ToList();

            var BedCount = Zimmer.Bett.Count();
            var reservierungBett = Zimmer.Bett.Where(x => x.XReservierungId != null).Count();
        
            ViewBag.BedCount = BedCount;
            ViewBag.reservierungBett = reservierungBett;

            return View();
        }

        [HttpPost]
        public IActionResult Zimmer_Editieren(Zimmer zimmer) 
        {
            db.Update(zimmer);
            db.SaveChanges();
            return RedirectToAction("Zimmer_Edit", new { id = zimmer.Id });
        }

        [HttpGet]
        public IActionResult Hotel_Edit(Guid id)
        {
            var hotel = db.Hotel.Include(x=>x.Zimmer).ThenInclude(x=>x.Bett).FirstOrDefault(x => x.Id == id);

            ViewBag.Hotel = hotel;

            var reservierung = db.Reservierung.Where(x=>x.Name==hotel.Name).ToList();

            var ZimmerCount = hotel.Zimmer.Count();
            var BedCount = hotel.Zimmer.SelectMany(x=>x.Bett).Count();
            var rezervierenZimmer = hotel.Zimmer.Where(x => x.XReservierungId != null).Count();
            var reservierungBett = hotel.Zimmer.SelectMany(x => x.Bett).Where(x => x.XReservierungId != null).Count();

            ViewBag.ZimmerCount = ZimmerCount;
            ViewBag.BedCount = BedCount;
            ViewBag.rezervierenZimmer = rezervierenZimmer;
            ViewBag.reservierungBett = reservierungBett;

            return View();
        }
        [HttpGet]
        public IActionResult Kunden_Edit(Guid id)
        {
            var gast = db.Gast.FirstOrDefault(x => x.Id == id); 

            var reservierung = db.Reservierung.Include(x=>x.Zimmer).Include(x=>x.Bett).Where(x => x.Id == gast.XReservierungId).ToList();

            var zimmer = reservierung.SelectMany(x=>x.Zimmer).ToList();
            var bett = zimmer.SelectMany(x=>x.Bett).ToList();

            ViewBag.Gast = gast;
            ViewBag.Zimmer = zimmer;
            ViewBag.reservierung = reservierung;

            return View();
        }
        [HttpGet]
        public IActionResult Firma_Edit(Guid id)
        {
            var firma = db.Firma.FirstOrDefault(x => x.Id == id);

            var reservierung = db.Reservierung.Include(x => x.Zimmer).Include(x => x.Bett).Where(x => x.Id == firma.XReservierungId).ToList();

            var zimmer = reservierung.SelectMany(x => x.Zimmer).ToList();
            var bett = zimmer.SelectMany(x => x.Bett).ToList();

            ViewBag.Gast = firma;
            ViewBag.Zimmer = zimmer;
            ViewBag.reservierung = reservierung;

            return View();
        }


        [HttpPost]
        public IActionResult Hotel_Editiren(Models.Hotel hotel)
        {
            db.Update(hotel);
            db.SaveChanges();

            return RedirectToAction("Hotel_Edit", new { id = hotel.Id });
        }

        public IActionResult Zimmer()
        {
            var reservierung = db.Reservierung.ToList();
            var allbed = db.Bett.ToList();
            var zimmer = db.Zimmer.ToList();
            var gast = db.Gast.ToList();
            var firma = db.Firma.ToList();

            var fullZimmerInfo = from z in zimmer
                                 let r = reservierung.FirstOrDefault(x => x.Id == z.XReservierungId)
                                 join h in db.Hotel on z.XHotelId equals h.Id
                                 let g = r != null ? gast.FirstOrDefault(x => x.XReservierungId == r.Id) : null
                                 let f = r != null ? firma.FirstOrDefault(x => x.XReservierungId == r.Id) : null
                                 let betten = allbed.Where(x => x.XZimmerId == z.Id).ToList()
                                 select new
                                 {
                                     Zimmernummer = z.Zimmernummer,
                                     ZimmerId = z.Id,
                                     Reservierung = r,
                                     NotReservierungBett = z?.Bett.Count() ?? 0,
                                     ReservierungBett = r?.Bett.Count() ?? 0,
                                     ReservierungVon = r?.Reserviertvod,
                                     ReservierungBis = r?.Reserviertbis,
                                     Muszahlen = r?.Muss_bezahlen ?? 0,
                                     GastName = g?.Name ?? f?.FirmeName ?? "Keine",
                                     Betten = betten,
                                     Hotelname = h.Name
                                 };

            var model = fullZimmerInfo.GroupBy(x => x.Zimmernummer).ToList();
           
            ViewBag.ZymmerReservierung = model;

            ViewBag.Hotel = db.Hotel.ToList();

            return View();
        }

        public IActionResult Kunden()
        {
            var reservierung = db.Reservierung.ToList();
            var allbed = db.Bett.ToList();
            var zimmer = db.Zimmer.ToList();
            var gast = db.Gast.ToList();
            var firma = db.Firma.ToList();

            var query = from g in db.Gast
                        join r in db.Reservierung on g.XReservierungId equals r.Id
                        join z in db.Zimmer on r.Id equals z.XReservierungId into zimmerGroup
                        from z in zimmerGroup.DefaultIfEmpty()
                        join b in db.Bett on z.Id equals b.XZimmerId into bettenGroup
                        select new
                        {
                            Id= g.Id,
                            GastName = g.Name,
                            hotel = r.Name,
                            ReservierungVon = r.Reserviertvod,
                            ReservierungBis = r.Reserviertbis,
                            MussBezahlen = r.Muss_bezahlen,
                            Zimmernummer = z != null ? z.Zimmernummer : "Keine Zimmer",
                            Betten = bettenGroup.Where(x=>x.XReservierungId!=null).ToList()
                        };

            var model = query.ToList();

            var allgast = db.Gast.Where(g => !db.Reservierung.Any(r => r.Id == g.XReservierungId)).ToList(); 

            ViewBag.ZymmerReservierung = model;
            ViewBag.AllGast = allgast;

            ViewBag.Hotel = db.Hotel.ToList();
            return View();
        }
        public IActionResult Kunden_Delet(Guid Id)
        {
            var kunde = db.Gast.FirstOrDefault(g => g.Id == Id);
            

            if(kunde.prvat_or_firma != "Firma")
            {
                var reservierung = db.Reservierung.Where(x => x.NameGast == kunde.Name).ToList();

                foreach (var reservation in reservierung)
                {
                    var Zimmer = db.Zimmer.Include(x => x.Bett).Where(x => x.XReservierungId == reservation.Id).ToList();
                   
                    var bed = Zimmer.SelectMany(x=>x.Bett).ToList();

                    foreach(var z in Zimmer)
                    {
                        z.XReservierungId = null;
                        db.Update(z);
                        db.SaveChanges();
                    }
                    foreach(var b in bed)
                    {
                        b.XReservierungId = null;
                        db.Update(b);
                        db.SaveChanges();
                    }

                    db.Remove(reservation);
                    db.SaveChanges();
                }
            }
          
           
            db.Remove(kunde);
            db.SaveChanges();
            return RedirectToAction("Kunden");
        }
        public IActionResult Firma_Delet(Guid Id)
        {
             var firma = db.Firma.FirstOrDefault(g => g.Id == Id);

            var reservierung = db.Reservierung.Where(x => x.Rechnungsempf == firma.FirmeName).ToList();

                foreach (var reservation in reservierung)
                {
                    var Zimmer = db.Zimmer.Include(x => x.Bett).Where(x => x.XReservierungId == reservation.Id).ToList();
                   
                    var bed = Zimmer.SelectMany(x=>x.Bett).ToList();

                    var gast = db.Gast.Where(x=>x.XReservierungId== reservation.Id).ToList();
                    
                    foreach (var z in gast)
                    {
                        z.XReservierungId = null;
                        db.Update(z);
                        db.SaveChanges();
                    }

                    foreach (var z in Zimmer)
                    {
                        z.XReservierungId = null;
                        db.Update(z);
                        db.SaveChanges();
                    }
                    foreach(var b in bed)
                    {
                        b.XReservierungId = null;
                        db.Update(b);
                        db.SaveChanges();
                    }

                    db.Remove(reservation);
                    db.SaveChanges();
                }

            db.Remove(firma);
            db.SaveChanges();

            return RedirectToAction("Firma");
        }
        public IActionResult Betten()
        {
            var reservierung = db.Reservierung.ToList();
            var allbed = db.Bett.ToList();
            var gast = db.Gast.ToList();
            var firma = db.Firma.ToList();
            var zimmer = db.Zimmer.ToList();         
            var hotel = db.Hotel.ToList();

            var fullZimmerInfo = from b in allbed
                                 let z = zimmer.FirstOrDefault(x => x.Id == b.XZimmerId) // 🔗 Bett → Zimmer
                                 let h = z != null ? hotel.FirstOrDefault(x => x.Id == z.XHotelId) : null // 🔗 Zimmer → Hotel
                                 let r = reservierung.FirstOrDefault(x => x.Id == b.XReservierungId)
                                 let g = r != null ? gast.FirstOrDefault(x => x.XReservierungId == r.Id) : null
                                 let f = r != null ? firma.FirstOrDefault(x => x.XReservierungId == r.Id) : null
                                 select new
                                 {
                                     Id = b.Id,
                                     BettName = b.BettName,
                                     HotelName = h?.Name ?? "Unbekannt",
                                     Reservierung = r,
                                     ReservierungVon = r?.Reserviertvod,
                                     ReservierungBis = r?.Reserviertbis,
                                     Muszahlen = r?.Muss_bezahlen ?? 0,
                                     GastName = g?.Name ?? f?.FirmeName ?? "Keine"
                                 };

            var model = fullZimmerInfo.ToList();

            ViewBag.ZymmerReservierung = model;

            ViewBag.Zimmer = zimmer;

            ViewBag.Hotel = db.Hotel.ToList();

            return View();
        }

        public IActionResult Firma()
        {
            var reservierung = db.Reservierung.ToList();
            var allbed = db.Bett.ToList();
            var zimmer = db.Zimmer.ToList();
            var gast = db.Gast.ToList();
            var firma = db.Firma.ToList();

            var query = from f in db.Firma
                        join r in db.Reservierung on f.XReservierungId equals r.Id
                        join z in db.Zimmer on r.Id equals z.XReservierungId into zimmerGroup
                        from z in zimmerGroup.DefaultIfEmpty()
                        join b in db.Bett on z.Id equals b.XZimmerId into bettenGroup
                        select new
                        {
                            Id = f.Id,
                            GastName = f.Name,
                            hotel = r.Name,
                            ReservierungVon = r.Reserviertvod,
                            ReservierungBis = r.Reserviertbis,
                            MussBezahlen = r.Muss_bezahlen,
                            Zimmernummer = z != null ? z.Zimmernummer : "Keine Zimmer",
                            Betten = bettenGroup.Where(x => x.XReservierungId != null).ToList()
                        };

            var model = query.ToList();

            var allfirma = db.Firma.Where(g => !db.Reservierung.Any(r => r.Id == g.XReservierungId)).ToList();

            ViewBag.ZymmerReservierung = model;
            ViewBag.AllGast = allfirma;

            ViewBag.Hotel = db.Hotel.ToList();
            return View();
        }
        public IActionResult Zimmer_Delet(Guid Id)
        {
            var zimmer = db.Zimmer.FirstOrDefault(x => x.Id == Id);

            var reservierung = db.Reservierung.Where(x => x.Id == zimmer.XReservierungId).ToList();

            var bed = db.Bett.Where(x => x.XZimmerId == zimmer.Id).ToList();
           
            zimmer.XReservierungId = null;

            foreach (var list in bed)
            {

                db.Remove(list);
                db.SaveChanges();
            }
            foreach(var list in reservierung)
            {
                var gast = db.Gast.FirstOrDefault(x=>x.XReservierungId== list.Id);
                gast.XReservierungId = null;
               
                var firma = db.Firma.FirstOrDefault(x=>x.XReservierungId == list.Id);
                firma.XReservierungId = null;

                db.Remove(list);
                db.SaveChanges();
            }


            db.Remove(zimmer);
            db.SaveChanges();

            return RedirectToAction("Zimmer");
        }
        public IActionResult Bed_Delet(Guid Id)
        {
            var bed = db.Bett.FirstOrDefault(x => x.Id == Id);

            var reservierung = db.Reservierung.Where(x => x.Id == bed.XReservierungId).ToList();

            foreach (var list in reservierung)
            {
                var gast = db.Gast.FirstOrDefault(x => x.XReservierungId == list.Id);
                gast.XReservierungId = null;

                var firma = db.Firma.FirstOrDefault(x => x.XReservierungId == list.Id);
                firma.XReservierungId = null;

                db.Remove(list);
                db.SaveChanges();
            }

            db.Remove(bed);
            db.SaveChanges();

            return RedirectToAction("Betten");
        }
        public IActionResult Reservierung_Delet(Guid Id)
        {
            var reservierung = db.Reservierung.FirstOrDefault(x => x.Id == Id).Id;

            var zimmer = db.Zimmer.Where(x => x.XReservierungId == reservierung).ToList();

            foreach (var item in zimmer)
            {
                item.XReservierungId = null;
                db.Update(item);
                db.SaveChanges() ;
            }

            var bed = db.Bett.Where(x => x.XReservierungId == reservierung).ToList();

            foreach (var item in bed)
            {
                item.XReservierungId = null;
                db.Update(item);
                db.SaveChanges();
            }

            var firma = db.Firma.Where(x=>x.XReservierungId == reservierung).ToList();

            foreach (var item in firma)
            {
                item.XReservierungId = null;
                db.Update(item);
                db.SaveChanges();
            }

            var guest = db.Gast.Where(x => x.XReservierungId == reservierung).ToList();

            foreach (var item in guest)
            {
                item.XReservierungId = null;
                db.Update(item);
                db.SaveChanges();
            }

            var reservierungItem = db.Reservierung.FirstOrDefault(x => x.Id == Id);

            db.Remove(reservierungItem);
            db.SaveChanges();

            return RedirectToAction("Reservierung");
        }
        public IActionResult Reservierung()
        {
            var reservierung = db.Reservierung.ToList();

            ViewBag.Reservierung = reservierung;    

            ViewBag.Hotel = db.Hotel.ToList();
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
