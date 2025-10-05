using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BDHotel.Models;

[Index("BookingId", Name = "IX_BookingServices_BookingID")]
[Index("ServiceId", Name = "IX_BookingServices_ServiceID")]
public partial class BookingService
{
    [Key]
    [Column("BookingServiceID")]
    public int BookingServiceId { get; set; }

    [Column("BookingID")]
    public int BookingId { get; set; }

    [Column("ServiceID")]
    public int ServiceId { get; set; }

    [ForeignKey("BookingId")]
    [InverseProperty("BookingServices")]
    public virtual Booking Booking { get; set; } = null!;

    [ForeignKey("ServiceId")]
    [InverseProperty("BookingServices")]
    public virtual Service Service { get; set; } = null!;
}
