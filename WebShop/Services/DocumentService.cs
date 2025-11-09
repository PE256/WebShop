using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly DataContext _context;

        public DocumentService(DataContext context)
        {
            _context = context;
        }

        public async Task<Document> CreateAsync(Document doc)
        {
            _context.Documents.Add(doc);
            await _context.SaveChangesAsync();
            return doc;
        }

        public async Task DeleteAsync(int documentId)
        {
            var doc = await _context.Documents.FindAsync(documentId);
            if (doc != null)
            {
                _context.Documents.Remove(doc);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            return await _context.Documents.AsNoTracking().ToListAsync();
        }

        public async Task<Document> GetAsync(int documentId)
        {
            return await _context.Documents
                .Include(d => d.Tasks)
                .SingleOrDefaultAsync(d => d.DocumentId == documentId);
        }

        public async Task<Document> UpdateAsync(Document doc)
        {
            _context.Documents.Update(doc);
            await _context.SaveChangesAsync();
            return doc;
        }
    }
}