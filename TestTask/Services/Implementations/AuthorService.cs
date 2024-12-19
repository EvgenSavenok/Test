using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class AuthorService : IAuthorService
{
    private readonly ApplicationDbContext _context;
    public AuthorService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Author> GetAuthor()
    {
        var author = await _context.Authors
            .SelectMany(a => a.Books.Select(b => new { Author = a, TitleLength = b.Title.Length }))
            .OrderByDescending(x => x.TitleLength)
            .ThenBy(x => x.Author.Id)
            .Select(x => x.Author)
            .FirstOrDefaultAsync();
        return author;
    }

    public async Task<List<Author>> GetAuthors()
    {
        var authors = await _context.Authors
            .Where(a => a.Books.Count(b => b.PublishDate > new DateTime(2015, 1, 1)) % 2 == 0)
            .ToListAsync();
        return authors;
    }
}
