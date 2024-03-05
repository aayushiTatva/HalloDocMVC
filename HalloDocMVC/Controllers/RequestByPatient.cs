using HalloDocMVC.DataContext;
using HalloDocMVC.DataModels;
using HalloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Globalization;

namespace HallodocMVC.Controllers
{
    public class RequestByPatient : Controller
    {
        private readonly HalloDocDbContext _context;

        public RequestByPatient(HalloDocDbContext context)
        {
            _context = context;
        }
        public IActionResult RequestForMe()
        {
            var ViewDataPatientRequest = _context.Users
                               .Where(r => Convert.ToString(r.Aspnetuserid) == (CV.UserID()))
                               .Select(r => new ViewDataPatientRequest
                               {
                                   FirstName = r.Firstname,
                                   LastName = r.Lastname,
                                   Email = r.Email,
                                   PhoneNumber = r.Mobile,
                                   BirthDate = new DateTime((int)r.Intyear, DateTime.ParseExact(r.Strmonth, "MMMM", new CultureInfo("en-US")).Month, (int)r.Intdate)
                               })
                               .FirstOrDefault();
            return View(ViewDataPatientRequest);
        }
        public IActionResult RequestForSomeoneelse()
        {
            return View("../RequestByPatient/RequestSomeoneElseByPatient");
        }

        //PostSomeoneElseAsync
        public async Task<IActionResult> PostMe(ViewDataPatientRequest viewpatientrequestforme)
        {
            //if (ModelState.IsValid)
            //{

            var Request = new Request();
            var Requestclient = new Requestclient();

            Request.Requesttypeid = 2;
            var isexist = _context.Users.FirstOrDefault(x => x.Email == viewpatientrequestforme.Email);
            Request.Userid = isexist.Userid;
            Request.Firstname = isexist.Firstname;
            Request.Lastname = isexist.Lastname;
            Request.Email = isexist.Email;
            Request.Phonenumber = isexist.Mobile;
            Request.Isurgentemailsent = new BitArray(1);
            Request.Createddate = DateTime.Now;
            _context.Requests.Add(Request);
            await _context.SaveChangesAsync();

            Requestclient.Requestid = Request.Requestid;
            Requestclient.Firstname = viewpatientrequestforme.FirstName;
            Requestclient.Address = viewpatientrequestforme.Street;
            Requestclient.Lastname = viewpatientrequestforme.LastName;
            Requestclient.Email = viewpatientrequestforme.Email;
            Requestclient.Phonenumber = viewpatientrequestforme.PhoneNumber;
           

            _context.Requestclients.Add(Requestclient);
            await _context.SaveChangesAsync();


            if (viewpatientrequestforme.UploadFile != null)
            {
                string FilePath = "wwwroot\\Upload";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileNameWithPath = Path.Combine(path, viewpatientrequestforme.UploadFile.FileName);
                viewpatientrequestforme.UploadImage = viewpatientrequestforme.UploadFile.FileName;

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    viewpatientrequestforme.UploadFile.CopyTo(stream);
                }

                var requestwisefile = new Requestwisefile
                {
                    Requestid = Request.Requestid,
                    Filename = viewpatientrequestforme.UploadImage,
                    Createddate = DateTime.Now,
                };
                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
            }
            //}
            //else
            //{
            //    return View("../RequestByPatient/SubmitForMe", viewpatientrequestforme);
            //}
            return RedirectToAction("Index", "PatientDashboard");
        }


        //PostSomeoneElseAsync
        public async Task<IActionResult> PostSomeoneElse(ViewDataPatientRequest viewpatientrequestforelse)
        {
            //if (ModelState.IsValid)
            //{
            var Request = new Request();
            var Requestclient = new Requestclient();
            var isexist = _context.Users.FirstOrDefault(x => Convert.ToString(x.Aspnetuserid) == (CV.UserID()));
            Request.Requesttypeid = 2;
            //Request.Userid = isexist.Userid;
            Request.Firstname = isexist.Firstname;
            Request.Lastname = isexist.Lastname;
            Request.Email = isexist.Email;
            Request.Phonenumber = isexist.Mobile;
            Request.Relationname = viewpatientrequestforelse.Relation;
            Request.Isurgentemailsent = new BitArray(1);
            Request.Createddate = DateTime.Now;
            _context.Requests.Add(Request);
            await _context.SaveChangesAsync();

            Requestclient.Requestid = Request.Requestid;
            Requestclient.Firstname = viewpatientrequestforelse.FirstName;
            Requestclient.Address = viewpatientrequestforelse.Street;
            Requestclient.Lastname = viewpatientrequestforelse.LastName;
            Requestclient.Email = viewpatientrequestforelse.Email;
            Requestclient.Phonenumber = viewpatientrequestforelse.PhoneNumber;

            _context.Requestclients.Add(Requestclient);
            await _context.SaveChangesAsync();


            if (viewpatientrequestforelse.UploadFile != null)
            {
                string FilePath = "wwwroot\\Upload";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileNameWithPath = Path.Combine(path, viewpatientrequestforelse.UploadFile.FileName);
                viewpatientrequestforelse.UploadImage = viewpatientrequestforelse.UploadFile.FileName;

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    viewpatientrequestforelse.UploadFile.CopyTo(stream);
                }

                var requestwisefile = new Requestwisefile
                {
                    Requestid = Request.Requestid,
                    Filename = viewpatientrequestforelse.UploadImage,
                    Createddate = DateTime.Now,
                };
                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
            }
            //}
            //else
            //{
            //    return View("../Views/RequestByPatient/SubmitForSomeoneelse", viewpatientrequestforme);
            //}
            return RedirectToAction("Index", "PatientDashboard");
        }
    }
}
