using System.Data;
using HalloDocMVC.DataContext;
using HalloDocMVC.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HallodocMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly HalloDocDbContext _context;
        public LoginController(HalloDocDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View("../Home/loginPage");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validate(string Email, string Passwordhash)
        {
            NpgsqlConnection connection = new NpgsqlConnection("Server=localhost;Database=HalloDocDB;User Id=postgres;Password=Aayushi03;Include Error Detail=True");
            string Query = "SELECT * FROM  aspnetusers where email=@Email and passwordhash=@Passwordhash";
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(Query, connection);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Passwordhash", Passwordhash);
            NpgsqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            int numRows = dataTable.Rows.Count;
            if (numRows > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    HttpContext.Session.SetString("UserName", row["username"].ToString());
                    HttpContext.Session.SetString("UserID", row["Id"].ToString());
                }
                return RedirectToAction("Index", "PatientDashboard");
            }
            else
            {
                ViewData["error"] = "Invalid Id Pass";
                return View("../Home/loginPage");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }

}

