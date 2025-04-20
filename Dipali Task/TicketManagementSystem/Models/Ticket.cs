using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Ticket
{
    [Key]
    public int TicketId { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string Priority { get; set; } 

    [Required]
    public string Status { get; set; } = "Open"; 

    public int? AssignedToUserId { get; set; } 

    [ForeignKey("AssignedToUserId")]
    public virtual User AssignedTo { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public int CreatedByUserId { get; set; }

    [ForeignKey("CreatedByUserId")]
    public virtual User CreatedBy { get; set; }
}
