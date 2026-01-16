namespace GestionStock.Client.Services
{
    public class LoadingStateService
    {
        private bool _isLoading = false;
        public bool IsLoading { 
            get => _isLoading; 
            set 
            {
                _isLoading = value;
                LoadingStateHasChanged?.Invoke();
            } 
        }

        public event Action? LoadingStateHasChanged;
    }
}
