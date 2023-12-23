using ProEventos.Api.Models;
using System.Text.Json;

namespace ProEventos.Api.Extensions
{
    public static class Pagination
    {
        public static void AddPagination(this HttpResponse response, int currentPage, 
            int itemsPerPage, int totalItems, int totalPages)
        {

            PaginationHeader pagination = new PaginationHeader(currentPage, itemsPerPage, 
                totalItems, totalPages);  

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(pagination, options));

            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
