using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BDHotel.Models;

public partial class Position
{
    [Key]
    [Column("PositionID")]
    public int PositionId { get; set; }

    [StringLength(255)]
    public string PositionName { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Salary { get; set; }

    public string? Responsibilities { get; set; }

    public string? Requirements { get; set; }

    [InverseProperty("Position")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
