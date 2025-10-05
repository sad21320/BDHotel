using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BDHotel.Models;

public partial class Client
{
    [Key]
    [Column("ClientID")]
    public int ClientId { get; set; }

    [StringLength(255)]
    public string FullName { get; set; } = null!;

    [StringLength(255)]
    public string? PassportData { get; set; }

    [InverseProperty("Client")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
