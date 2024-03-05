using System.Collections;
using HallodocMVC.Controllers;
using HalloDocMVC.DataContext;
using HalloDocMVC.DataModels;
using HalloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;


namespace HalloDocMVC.Controllers
{
    [CheckAccess]
    public class PatientDashboardController : Controller
    {
        private readonly HalloDocDbContext _context;
        public PatientDashboardController(HalloDocDbContext context)
        {
            _context = context;
        }

        public enum Status
        {
            Unassigned = 1,
            Accepted,
            Cancelled,
            Reserving,
            MDEnRoute,
            MDOnSite,
            FollowUp,
            Closed,
            Locked,
            Declined,
            Consult,
            Clear,
            CancelledByProvider,
            CCUploadedByClient,
            CCApprovedByAdmin
        }
        public async Task<IActionResult> Index()
        {
            if (CV.UserID() != null)
            {
                var UserIDForRequest = _context.Users.Where(r => r.Aspnetuserid == CV.UserID()).FirstOrDefault(); /*to get details of entered username via userid*/
                /*UserIDForRequest is the name of variable */
                if (UserIDForRequest != null)
                {
                    List<Request> Request = _context.Requests.Where(r => r.Userid == UserIDForRequest.Userid).ToList();
                    List<int> ids = new List<int>();

                    foreach (var request in Request)
                    {

                        var doc = _context.Requestwisefiles.Where(r => r.Requestid == request.Requestid).FirstOrDefault();
                        if (doc != null)
                        {
                            ids.Add(request.Requestid);
                        }
                    }
                    ViewBag.docidlist = ids;
                    ViewBag.listofrequest = Request;
                }
                return View("../Dashboard/PatientDashboard");
            }
            else
            {
                return View("../Dashboard/PatientDashboard");
            }

        }

        public IActionResult ViewDocuments(int? id)
        {
            List<Request> Request = _context.Requests.Where(r => r.Requestid == id).ToList();
            ViewBag.requestinfo = Request;
            List<Requestwisefile> DocList = _context.Requestwisefiles.Where(r => r.Requestid == id).ToList();
            ViewBag.DocList = DocList;
            return View("../Dashboard/ViewDocuments");
        }
        public IActionResult UploadDoc(int Requestid, IFormFile? UploadFile)
        {
            string UploadImage;
            if (UploadFile != null)
            {
                string FilePath = "wwwroot\\Upload";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileNameWithPath = Path.Combine(path, UploadFile.FileName);
                UploadImage = UploadFile.FileName;
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    UploadFile.CopyTo(stream);
                }
                var requestwisefile = new Requestwisefile
                {
                    Requestid = Requestid,
                    Filename = UploadFile.FileName,
                    Createddate = DateTime.Now,
                };
                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
            }
            return View("../Dashboard/ViewDocuments", new { id = Requestid });
        }

        /*public IActionResult UserProfile()
        {
            return View("../Profile/UserProfile");
        }*/

        public IActionResult RequestSomeoneElseByPatient()
        {
            return View("../Dashboard/RequestSomeoneElseByPatient");
        }

        public IActionResult RequestForMe()
        {
            return View("../Dashboard/RequestForMe");
        }
    }
}
