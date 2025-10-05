using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BDHotel.Models;

[Index("ClientId", Name = "IX_Bookings_ClientID")]
[Index("EmployeeId", Name = "IX_Bookings_EmployeeID")]
[Index("RoomId", Name = "IX_Bookings_RoomID")]
public partial class Booking
{
    [Key]
    [Column("BookingID")]
    public int BookingId { get; set; }

    [Column("RoomID")]
    public int RoomId { get; set; }

    [Column("ClientID")]
    public int ClientId { get; set; }

    [Column("EmployeeID")]
    public int EmployeeId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CheckInDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CheckOutDate { get; set; }

    [InverseProperty("Booking")]
    public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();

    [ForeignKey("ClientId")]
    [InverseProperty("Bookings")]
    public virtual Client Client { get; set; } = null!;

    [ForeignKey("EmployeeId")]
    [InverseProperty("Bookings")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("RoomId")]
    [InverseProperty("Bookings")]
    public virtual Room Room { get; set; } = null!;
}
