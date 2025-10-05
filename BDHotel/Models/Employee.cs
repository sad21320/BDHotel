using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BDHotel.Models;

[Index("PositionId", Name = "IX_Employees_PositionID")]
public partial class Employee
{
    [Key]
    [Column("EmployeeID")]
    public int EmployeeId { get; set; }

    [Column("PositionID")]
    public int PositionId { get; set; }

    [StringLength(255)]
    public string FullName { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(50)]
    public string? Gender { get; set; }

    [StringLength(255)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(255)]
    public string? PassportData { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [ForeignKey("PositionId")]
    [InverseProperty("Employees")]
    public virtual Position Position { get; set; } = null!;
}
