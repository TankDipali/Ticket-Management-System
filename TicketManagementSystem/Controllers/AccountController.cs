using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Cryptography;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

       [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                ViewBag.Message = "User not found!";
                return View();
            }

            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                ViewBag.Message = "Incorrect password!";
                return View();
            }

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("Index", "Ticket");
        }

        public IActionResult Register()
        {
            ViewBag.Role = HttpContext.Session.GetString("Role"); // Get Role for conditional view rendering
            return View();
        }

      
        [HttpPost]
        public IActionResult Register(User user, string password, string role)
        {
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                ViewBag.Message = "Username already exists!";
                return View();
            }

            user.PasswordHash = HashPassword(password, out string salt);
            user.PasswordSalt = salt;

    
            var loggedInUserRole = HttpContext.Session.GetString("Role");
            if (loggedInUserRole == "Admin")
            {
                user.Role = role; 
            }
            else
            {
                user.Role = "User"; 
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

      
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

       
        private string HashPassword(string password, out string saltBase64)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);

            byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 10000, 32);
            saltBase64 = Convert.ToBase64String(salt);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] salt = Convert.FromBase64String(storedSalt);
            byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 10000, 32);
            return Convert.ToBase64String(hash) == storedHash;
        }
    }
}
