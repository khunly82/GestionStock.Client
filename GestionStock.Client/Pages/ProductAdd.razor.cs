using GestionStock.Client.Models;
using GestionStock.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace GestionStock.Client.Pages
{
    public partial class ProductAdd
    {
        [Inject]
        public required CategoryService CategoryService { get; set; }

        [Inject]
        public required ProductService ProductService { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        private ProductForm _form = new ProductForm() { Name = "" };
        private List<Category>? _categories;
        private string? _preview;

        private IBrowserFile? _image;

        protected async override Task OnInitializedAsync()
        {
            _categories = await CategoryService.Get();
            await base.OnInitializedAsync();
        }

        private async void OnSubmit()
        {
            // Envoi vers l'api
            try
            {
                await ProductService.Add(_form, _image);
                // rediriger
                NavigationManager.NavigateTo("/");
            }
            catch (HttpRequestException)
            {
                // afficher un message d'erreur
            }
        } 
        private async void FileChangedHandler(IBrowserFile file)
        {
            using Stream s = file.OpenReadStream(long.MaxValue);
            MemoryStream ms = new MemoryStream();
            await s.CopyToAsync(ms);
            ms.Position = 0;
            byte[] bytes = ms.ToArray();
            _preview = "data:image/png;base64," + Convert.ToBase64String(bytes);
            StateHasChanged();
            _image = file;
        }
    }
}
