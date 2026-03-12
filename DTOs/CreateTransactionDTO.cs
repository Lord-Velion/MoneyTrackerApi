namespace MoneyTrackerApi.DTOs
{
    public class CreateTransactionDTO
    {

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public DateTime? Date { get; set; }

        public bool IsIncome { get; set; }

        public int AccountId { get; set; }

        public int CategoryId { get; set; }
    }
}
