using LibraryManagementSystem.admin.CommandModels;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.admin.QueryModel;
using Microsoft.EntityFrameworkCore;

public class BorrowTransactionService : IBorrowTransactionService
{
    private readonly Sql12792576Context _context;

    public BorrowTransactionService(Sql12792576Context context)
    {
        _context = context;
    }

    public async Task<List<BorrowTransactionQueryModel>> GetAllAsync()
    {
        var transactions = await _context.BorrowTransactions
            .Include(bt => bt.Book)
            .Include(bt => bt.User)
            .ToListAsync();

        bool hasChanges = false;
        var today = DateOnly.FromDateTime(DateTime.Today);

        foreach (var transaction in transactions)
        {
            if (transaction.ReturnDate == null &&
                transaction.DueDate.HasValue &&
                today > transaction.DueDate.Value &&
                (transaction.FineAmount == null || transaction.FineAmount == 0))
            {
                var overdueDays = (today.ToDateTime(TimeOnly.MinValue) - transaction.DueDate.Value.ToDateTime(TimeOnly.MinValue)).Days;
                transaction.FineAmount = overdueDays * 20;
                hasChanges = true;
            }
        }

        if (hasChanges)
        {
            await _context.SaveChangesAsync();
        }

        return transactions.Select(bt => new BorrowTransactionQueryModel
        {
            TransactionId = bt.TransactionId,
            BookTitle = bt.Book?.Title ?? "",
            UserEmail = bt.User?.Email ?? "",
            BorrowDate = bt.BorrowDate ?? default,
            DueDate = bt.DueDate ?? default,
            ReturnDate = bt.ReturnDate,
            FineAmount = bt.FineAmount,
            Status = bt.Status
        }).ToList();
    }

    public async Task<BorrowTransactionQueryModel?> GetByIdAsync(int id)
    {
        var bt = await _context.BorrowTransactions
            .Include(x => x.Book)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TransactionId == id);

        if (bt == null) return null;

        var today = DateOnly.FromDateTime(DateTime.Today);

        if (bt.ReturnDate == null &&
            bt.DueDate.HasValue &&
            today > bt.DueDate.Value &&
            (bt.FineAmount == null || bt.FineAmount == 0))
        {
            var overdueDays = (today.ToDateTime(TimeOnly.MinValue) - bt.DueDate.Value.ToDateTime(TimeOnly.MinValue)).Days;
            bt.FineAmount = overdueDays * 20;
            await _context.SaveChangesAsync();
        }

        return new BorrowTransactionQueryModel
        {
            TransactionId = bt.TransactionId,
            BookTitle = bt.Book?.Title ?? "",
            UserEmail = bt.User?.Email ?? "",
            BorrowDate = bt.BorrowDate ?? default,
            DueDate = bt.DueDate ?? default,
            ReturnDate = bt.ReturnDate,
            FineAmount = bt.FineAmount,
            Status = bt.Status
        };
    }

    public async Task<bool> BorrowAsync(BorrowTransactionCommandModel model)
    {
        var book = await _context.Books.FindAsync(model.BookId);
        if (book == null || book.AvailableCopies <= 0)
            return false;

        book.AvailableCopies--;

        var transaction = new BorrowTransaction
        {
            UserId = model.UserId,
            BookId = model.BookId,
            BorrowDate = model.BorrowDate,
            DueDate = model.DueDate,
            Status = "borrowed"
        };

        _context.BorrowTransactions.Add(transaction);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ReturnAsync(BorrowTransactionReturnModel model)
    {
        var transaction = await _context.BorrowTransactions
            .Include(x => x.Book)
            .FirstOrDefaultAsync(x => x.TransactionId == model.TransactionId);

        if (transaction == null || transaction.ReturnDate != null)
            return false;

        transaction.ReturnDate = model.ReturnDate;
        transaction.Status = "returned";

        transaction.Book.AvailableCopies++;

        await _context.SaveChangesAsync();
        return true;
    }

}
