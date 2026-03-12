using System;
using System.Collections.Generic;

namespace MoneyTrackerApi.Models;

public partial class Account
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}