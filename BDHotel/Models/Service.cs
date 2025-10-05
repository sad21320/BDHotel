using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BDHotel.Models;

public partial class Service
{
    [Key]
    [Column("ServiceID")]
    public int ServiceId { get; set; }

    [StringLength(255)]
    public string ServiceName { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [InverseProperty("Service")]
    public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
}
