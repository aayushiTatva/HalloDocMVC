using System.Collections;
using HalloDocMVC.DataContext;
using HalloDocMVC.DataModels;
using HalloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace HalloDocMVC.Controllers
{
    public class PatientRequestController : Controller
    {
        private readonly HalloDocDbContext _context;

        public PatientRequestController(HalloDocDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CheckEmailAsync(string email)
        {
            string message;
            var aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == email);
            if (aspnetuser == null)
            {
                message = "False";
            }
            else
            {
                message = "Success";
            }
            return Json(new
            {
                isAspnetuser = aspnetuser == null
            });
        }
        public async Task<IActionResult> Create(ViewDataPatientRequest viewDataPatientRequest)
        {
            var Aspnetuser = new Aspnetuser();
            var User = new User();
            var Request = new Request();
            var Requestclient = new Requestclient();
            var isexist = _context.Users.FirstOrDefault(x => x.Email == viewDataPatientRequest.Email);
            if (isexist == null)
            {
                // Aspnetuser
                Guid g = Guid.NewGuid();
                Aspnetuser.Id = g.ToString();
                Aspnetuser.Username = viewDataPatientRequest.Email;
                Aspnetuser.Passwordhash = viewDataPatientRequest.PassWord;
                Aspnetuser.CreatedDate = DateTime.Now;
                Aspnetuser.Email = viewDataPatientRequest.Email;
                _context.Aspnetusers.Add(Aspnetuser);
                await _context.SaveChangesAsync();

                User.Aspnetuserid = Aspnetuser.Id;
                User.Firstname = viewDataPatientRequest.FirstName;
                User.Lastname = viewDataPatientRequest.LastName;
                User.Email = viewDataPatientRequest.Email;
                User.Mobile = viewDataPatientRequest.PhoneNumber;
                User.Intdate = viewDataPatientRequest.BirthDate.Day;
                User.Strmonth = viewDataPatientRequest.BirthDate.ToString("MMMM");
                User.Intyear = viewDataPatientRequest.BirthDate.Year;
                User.Street = viewDataPatientRequest.Street;
                User.State = viewDataPatientRequest.State;
                User.City = viewDataPatientRequest.City;/*
                User.Zipcode = viewDataPatientRequest.ZipCode;*/
                User.Createdby = Aspnetuser.Id;
                User.Createddate = DateTime.Now;
                _context.Users.Add(User);
                await _context.SaveChangesAsync();
            }

                Request.Requesttypeid = 2;
                Request.Status = 1;

            if (isexist == null)
            {
                Request.Userid = User.Userid;
            }
            else
            {
                Request.Userid = isexist.Userid;
            }
            Request.Firstname = viewDataPatientRequest.FirstName;
            Request.Lastname = viewDataPatientRequest.LastName;
            Request.Email = viewDataPatientRequest.Email;
            Request.Phonenumber = viewDataPatientRequest.PhoneNumber;
            Request.Isurgentemailsent = new BitArray(1);
            Request.Createddate = DateTime.Now;
            _context.Requests.Add(Request);
            await _context.SaveChangesAsync();

            Requestclient.Requestid = Request.Requestid;
            Requestclient.Firstname = viewDataPatientRequest.FirstName;
            Requestclient.Address = viewDataPatientRequest.Street;
            Requestclient.Lastname = viewDataPatientRequest.LastName;
            Requestclient.Email = viewDataPatientRequest.Email;
            Requestclient.Phonenumber = viewDataPatientRequest.PhoneNumber;
            Requestclient.Notes = viewDataPatientRequest.Symptoms;
            Requestclient.Intdate = viewDataPatientRequest.BirthDate.Day;
            Requestclient.Strmonth = viewDataPatientRequest.BirthDate.ToString("MMMM");
            Requestclient.Intyear = viewDataPatientRequest.BirthDate.Year;
            Requestclient.Street = viewDataPatientRequest.Street;
            Requestclient.State = viewDataPatientRequest.State;
            Requestclient.City = viewDataPatientRequest.City;

            _context.Requestclients.Add(Requestclient);
            await _context.SaveChangesAsync();

            if (viewDataPatientRequest.UploadFile != null)
            {
                string FilePath = "wwwroot\\Upload";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileNameWithPath = Path.Combine(path, viewDataPatientRequest.UploadFile.FileName);
                viewDataPatientRequest.UploadImage = "~" + FilePath.Replace("wwwroot\\", "/") + "/" + viewDataPatientRequest.UploadFile.FileName;

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    viewDataPatientRequest.UploadFile.CopyTo(stream);
                }

                var requestwisefile = new Requestwisefile
                {
                    Requestid = Request.Requestid,
                    Filename = viewDataPatientRequest.UploadFile.FileName,
                    Createddate = DateTime.Now,
                };
                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
            }

            return View("../Request/SubmitRequestPage"); /*which page is to be returned after saving the details in DB*/
        }
        public IActionResult PatientRequest()/*this methos is called when clicked on Patient request in submit request page to call Patient Request page*/
        {
            return View("../Request/PatientRequestPage");
        }
        public IActionResult RequestSomeoneElseByPatient()
        {
            return View("../Dashboard/RequestSomeoneElseByPatient");
        }
    }
}
/*combines file path with mvc upload path*/
/*checks if the path value stored in path variable recently exists or not if not then creates such path*/
/*combines path with file name of uploaded file*/
/* used to store the file in DBdone with input stream*/
/*using statement ensures that the stream object is disposed after the code block is executed, which releases any resources used by the file*/
/*fileNameWithPath variable is a string that represents the full path of the local file and FileMode.Create enum value specifies that the file should be created if it does not exist, or overwritten if it does exist*/
/*viewpatientcreaterequest.UploadFile property is an object that represents the uploaded file, which has an InputStream property that returns a Stream object that can be read from. The CopyTo method copies the bytes from the source stream (viewpatientcreaterequest.UploadFile.InputStream) to the destination stream (stream) */