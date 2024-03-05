using System;
using System.Collections;
using HalloDocMVC.DataContext;
using HalloDocMVC.DataModels;
using HalloDocMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;


namespace HalloDocMVC.Controllers
{
    public class FamilyRequestController : Controller
    {
        private readonly HalloDocDbContext _context;

        public FamilyRequestController(HalloDocDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ViewDataFamilyRequest viewDataFamilyRequest)
        {
            var Request = new Request
            {
                Requesttypeid = 3 , /* these details are added to requestclient table to refer to patient via client*/
                Status = 1,
                Firstname = viewDataFamilyRequest.FF_FirstName,
                Lastname = viewDataFamilyRequest.FF_LastName,
                Email = viewDataFamilyRequest.FF_Email,
                Relationname = viewDataFamilyRequest.FF_RelationWithPatient,
                Phonenumber = viewDataFamilyRequest.FF_PhoneNumber,
                Createddate = DateTime.Now,
                Isurgentemailsent = new BitArray(1)

            };
            _context.Requests.Add(Request);/*To add details to DB*/
            await _context.SaveChangesAsync();/*To save details to synchronisation*/

            var Requestclient = new Requestclient
            {
                Request = Request, /* these details are added to request table*/
                Requestid = Request.Requestid,
                Notes = viewDataFamilyRequest.Symptoms,
                Firstname = viewDataFamilyRequest.FirstName,
                Lastname = viewDataFamilyRequest.LastName,
                Phonenumber = viewDataFamilyRequest.PhoneNumber,
                Email = viewDataFamilyRequest.Email,
                State = viewDataFamilyRequest.State,
                City = viewDataFamilyRequest.City,
                Zipcode = viewDataFamilyRequest.ZipCode

            };
            _context.Requestclients.Add(Requestclient);
            await _context.SaveChangesAsync();

            if (viewDataFamilyRequest.UploadFile != null)
            {
                string FilePath = "wwwroot\\Upload";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileNameWithPath = Path.Combine(path, viewDataFamilyRequest.UploadFile.FileName);
                viewDataFamilyRequest.UploadImage = "~" + FilePath.Replace("wwwroot\\", "/") + "/" + viewDataFamilyRequest.UploadFile.FileName;

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    viewDataFamilyRequest.UploadFile.CopyTo(stream);
                }

                var requestwisefile = new Requestwisefile()
                {
                    Requestid = Request.Requestid,
                    Filename = viewDataFamilyRequest.UploadFile.FileName,
                    Createddate = DateTime.Now,
                };
                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
            }

            return View("../Request/SubmitRequestPage");
        }
        
        public IActionResult FamilyRequest()
        {
            return View("../Request/FamilyRequestPage");
        }
    }
}