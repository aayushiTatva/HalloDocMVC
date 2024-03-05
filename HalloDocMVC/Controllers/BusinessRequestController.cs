using System.Collections;
using HalloDocMVC.DataContext;
using HalloDocMVC.DataModels;
using HalloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;


namespace HalloDocMVC.Controllers
    {
        public class BusinessRequestController : Controller
        {
            private readonly HalloDocDbContext _context;

            public BusinessRequestController(HalloDocDbContext context)
            {
                _context = context;
            }
            public IActionResult Index()
            {
                return View();
            }


            [HttpPost]
            public async Task<IActionResult> Create(ViewDataBusinessRequest viewDataBusinessRequest)
            {
                var Request = new Request
                {
                    Requesttypeid = 4,
                    Status = 1,
                    Firstname = viewDataBusinessRequest.BP_FirstName,
                    Lastname = viewDataBusinessRequest.BP_LastName,
                    Email = viewDataBusinessRequest.BP_Email,
                    Phonenumber = viewDataBusinessRequest.BP_PhoneNumber,
                    Createddate = DateTime.Now,
                    Isurgentemailsent = new BitArray(1)

                };
                _context.Requests.Add(Request);/*To add details to DB*/
                await _context.SaveChangesAsync();/*To save details to synchronisation*/

                var Requestclient = new Requestclient
                {
                    Request = Request,
                    Requestid = Request.Requestid,
                    Notes = viewDataBusinessRequest.Symptoms,
                    Firstname = viewDataBusinessRequest.FirstName,
                    Lastname = viewDataBusinessRequest.LastName,
                    Phonenumber = viewDataBusinessRequest.PhoneNumber,
                    Email = viewDataBusinessRequest.Email,
                    State = viewDataBusinessRequest.State,
                    City = viewDataBusinessRequest.City,
                    Zipcode = viewDataBusinessRequest.ZipCode

                };
                _context.Requestclients.Add(Requestclient);
                await _context.SaveChangesAsync();

                if (viewDataBusinessRequest.UploadFile != null)
                {
                    string FilePath = "wwwroot\\Upload";
                    string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileNameWithPath = Path.Combine(path, viewDataBusinessRequest.UploadFile.FileName);
                viewDataBusinessRequest.UploadImage = "~" + FilePath.Replace("wwwroot\\", "/") + "/" + viewDataBusinessRequest.UploadFile.FileName;

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                    viewDataBusinessRequest.UploadFile.CopyTo(stream);
                    }

                    var requestwisefile = new Requestwisefile
                    {
                        Requestid = Request.Requestid,
                        Filename = viewDataBusinessRequest.UploadFile.FileName,
                        Createddate = DateTime.Now,
                    };
                    _context.Requestwisefiles.Add(requestwisefile);
                    _context.SaveChanges();
                }
            return View("../Request/SubmitRequestPage");
            }
            public IActionResult BusinessRequest()
            {
                return View("../Request/BusinessRequestPage");
            }
        }
    }
