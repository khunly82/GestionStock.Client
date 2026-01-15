using GestionStock.Client.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace GestionStock.Client.Services
{
    public class ProductService(HttpClient httpClient)
    {
        public async Task Add(ProductForm form, IBrowserFile? image)
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent(form.Name), "Name" },
                { new StringContent(form.Quantity.ToString()), "Stock" },
                { new StringContent(form.Price.ToString()), "Price" },
            };
            if(form.Description != null)
            {
                content.Add(new StringContent(form.Description), "Description");
            }

            foreach(int c in form.Categories)
            {
                content.Add(new StringContent(c.ToString()), "Categories");
            }

            if(image != null)
            {
                using var stream = image.OpenReadStream();
                content.Add(new StreamContent(stream), "Image", "image.png");
                await httpClient.PostAsync("/api/product", content);
                return;
            }
            
            await httpClient.PostAsync("/api/product", content);
        }
    }
}
