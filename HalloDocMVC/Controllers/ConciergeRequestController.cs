using System.Collections;
using HalloDocMVC.DataContext;
using HalloDocMVC.DataModels;
using HalloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace HalloDocMVC.Controllers
{
    public class ConciergeRequestController : Controller
    {
        private readonly HalloDocDbContext _context;

        public ConciergeRequestController(HalloDocDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ViewDataConciergeRequest viewDataConciregeRequest)
        {
            var Concierge = new Concierge();
            var Request = new Request();
            var Requestclient = new Requestclient();
            var Requestconcierge = new Requestconcierge();

            Concierge.Conciergename = viewDataConciregeRequest.CON_FirstName + " " + viewDataConciregeRequest.CON_LastName;
            Concierge.Street = viewDataConciregeRequest.CON_Street;
            Concierge.City = viewDataConciregeRequest.CON_City;
            Concierge.State = viewDataConciregeRequest.CON_State;
            Concierge.Zipcode = viewDataConciregeRequest.CON_Zipcode;
            Concierge.Regionid = 1;
            Concierge.Createddate = DateTime.Now;
            _context.Concierges.Add(Concierge);
            await _context.SaveChangesAsync();
            int id1 = Concierge.Conciergeid;

            Request.Requesttypeid = 4;
            Request.Status = 1;
            Request.Firstname = viewDataConciregeRequest.FirstName;
            Request.Lastname = viewDataConciregeRequest.LastName;
            Request.Email = viewDataConciregeRequest.Email;
            Request.Phonenumber = viewDataConciregeRequest.PhoneNumber;
            Request.Isurgentemailsent = new BitArray(1);
            Request.Createddate = DateTime.Now;
            _context.Requests.Add(Request);
            await _context.SaveChangesAsync();
            int id2 = Request.Requestid;

            Requestclient.Requestid = Request.Requestid;
            Requestclient.Firstname = viewDataConciregeRequest.FirstName;
            Requestclient.Lastname = viewDataConciregeRequest.LastName;
            Requestclient.Email = viewDataConciregeRequest.Email;
            Requestclient.Phonenumber = viewDataConciregeRequest.PhoneNumber;
            _context.Requestclients.Add(Requestclient);
            await _context.SaveChangesAsync();

            Requestconcierge.Requestid = id2;
            Requestconcierge.Conciergeid = id1;

            _context.Requestconcierges.Add(Requestconcierge);
            await _context.SaveChangesAsync();

            if (viewDataConciregeRequest.UploadFile != null)
            {
                string FilePath = "wwwroot\\Upload";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileNameWithPath = Path.Combine(path, viewDataConciregeRequest.UploadFile.FileName);
                viewDataConciregeRequest.UploadImage = "~" + FilePath.Replace("wwwroot\\", "/") + "/" + viewDataConciregeRequest.UploadFile.FileName;

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    viewDataConciregeRequest.UploadFile.CopyTo(stream);
                }

                var requestwisefile = new Requestwisefile
                {
                    Requestid = Request.Requestid,
                    Filename = viewDataConciregeRequest.UploadFile.FileName,
                    Createddate = DateTime.Now,
                };
                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
            }


            return View("../Request/SubmitRequestPage");
        }
        public IActionResult ConciergeRequest()
        {
            return View("../Request/ConciergeRequestPage");
        }
    }
}
