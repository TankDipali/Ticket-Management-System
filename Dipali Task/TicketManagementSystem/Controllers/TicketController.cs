using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class TicketController : Controller
{
    private readonly ApplicationDbContext _context;

    public TicketController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var role = HttpContext.Session.GetString("Role");
        if (role == "Admin")
            return View(_context.Tickets.Include(t => t.CreatedBy).Include(t => t.AssignedTo).ToList());

        int userId = int.Parse(HttpContext.Session.GetString("UserId"));
        return View(_context.Tickets.Where(t => t.CreatedByUserId == userId).ToList());
    }

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Ticket ticket)
    {
        ticket.CreatedByUserId = int.Parse(HttpContext.Session.GetString("UserId"));
        ticket.Status = "Open";
        _context.Tickets.Add(ticket);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var ticket = _context.Tickets.Find(id);
        return ticket != null ? View(ticket) : NotFound();
    }

    [HttpPost]
    public IActionResult Edit(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Assign(int id)
    {
        var ticket = _context.Tickets.Include(t => t.CreatedBy).FirstOrDefault(t => t.TicketId == id);
        ViewBag.Users = _context.Users.Where(u => u.Role == "Admin").ToList();
        return View(ticket);
    }

    [HttpPost]
    public IActionResult Assign(int ticketId, int assignedToUserId)
    {
        var ticket = _context.Tickets.Find(ticketId);
        if (ticket != null)
        {
            ticket.AssignedToUserId = assignedToUserId;
            _context.SaveChanges();
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
