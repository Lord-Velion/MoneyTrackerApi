using System;
using System.Collections.Generic;

namespace MoneyTrackerApi.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    public bool IsIncome { get; set; }

    public int AccountId { get; set; }

    public int CategoryId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
