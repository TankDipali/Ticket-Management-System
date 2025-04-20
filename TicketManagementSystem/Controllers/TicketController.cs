using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List Tickets
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == "Admin")
            {
                return View(_context.Tickets
                    .Include(t => t.CreatedBy)
                    .Include(t => t.AssignedTo)
                    .ToList());
            }

            if (!int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            return View(_context.Tickets
                .Where(t => t.CreatedByUserId == userId)
                .ToList());
        }

        
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Ticket ticket)
        {
            if (!int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrWhiteSpace(ticket.Title) || string.IsNullOrWhiteSpace(ticket.Description))
            {
                ViewBag.Message = "Title and Description are required.";
                return View(ticket);
            }

            ticket.CreatedByUserId = userId;
            ticket.Status = "Open";

            try
            {
                _context.Tickets.Add(ticket);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Update Error: {ex.InnerException?.Message}");
                ViewBag.Message = "An error occurred while saving the ticket.";
                return View(ticket);
            }
        }

       
        public IActionResult Edit(int id)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket); 
        }


       
        [HttpPost]
        public IActionResult Edit(Ticket ticket)
        {
            if (ticket == null || ticket.TicketId == 0)
            {
                return NotFound();
            }

            var existingTicket = _context.Tickets.Find(ticket.TicketId);
            if (existingTicket == null)
            {
                return NotFound();
            }

           
            existingTicket.Title = ticket.Title;
            existingTicket.Description = ticket.Description;
            existingTicket.Priority = ticket.Priority;
            existingTicket.Status = ticket.Status;

            try
            {
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Update Error: {ex.InnerException?.Message}");
                ViewBag.Message = "An error occurred while updating the ticket.";
                return View(ticket);
            }
        }


        
        public IActionResult Assign(int id)
        {
            var ticket = _context.Tickets
                .Include(t => t.CreatedBy)
                .FirstOrDefault(t => t.TicketId == id);

            if (ticket == null) return NotFound();

            ViewBag.Users = _context.Users.Where(u => u.Role == "Admin").ToList();
            return View(ticket);
        }

        
        [HttpPost]
        public IActionResult Assign(int ticketId, int assignedToUserId)
        {
            var ticket = _context.Tickets.Find(ticketId);
            var assignedUser = _context.Users.Find(assignedToUserId);

            if (ticket == null || assignedUser == null)
            {
                ViewBag.Message = "Invalid ticket or user.";
                return RedirectToAction("Index");
            }

            ticket.AssignedToUserId = assignedToUserId;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database Update Error: {ex.InnerException?.Message}");
                ViewBag.Message = "An error occurred while assigning the ticket.";
            }

            return RedirectToAction("Index");
        }

       
        public IActionResult GenerateReport()
        {
            var resolvedTickets = _context.Tickets.Where(t => t.Status == "Resolved").ToList();

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, ms);
                document.Open();
                document.Add(new Paragraph("Resolved Tickets Report"));

                foreach (var ticket in resolvedTickets)
                {
                    document.Add(new Paragraph($"Ticket ID: {ticket.TicketId}, Title: {ticket.Title}, Status: {ticket.Status}"));
                }

                document.Close();
                return File(ms.ToArray(), "application/pdf", "ResolvedTickets.pdf");
            }
        }
    }
}
