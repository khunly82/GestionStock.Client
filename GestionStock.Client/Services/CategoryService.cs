using GestionStock.Client.Models;
using System.Net.Http.Json;

namespace GestionStock.Client.Services
{
    public class CategoryService(HttpClient httpClient)
    {
        public async Task<List<Category>> Get()
        {
            return (
                await httpClient.GetFromJsonAsync<List<Category>>(
                    "/api/Category"
            )) ?? throw new HttpRequestException();
        }
    }
}
