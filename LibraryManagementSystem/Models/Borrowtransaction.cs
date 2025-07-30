using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Models;

public partial class BorrowTransaction
{
    public int TransactionId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public DateOnly? BorrowDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public decimal? FineAmount { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }

    public string Status { get; set; } = "borrowed";
}
