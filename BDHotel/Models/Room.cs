using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BDHotel.Models;

public partial class Room
{
    [Key]
    [Column("RoomID")]
    public int RoomId { get; set; }

    [StringLength(255)]
    public string RoomName { get; set; } = null!;

    public int? Capacity { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [InverseProperty("Room")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
