using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Services
{
    public interface IDocumentService
    {
        Task<Document> CreateAsync(Document doc);
        Task<Document> GetAsync(int documentId);
        Task<IEnumerable<Document>> GetAllAsync();
        Task<Document> UpdateAsync(Document doc);
        Task DeleteAsync(int documentId);
    }
}