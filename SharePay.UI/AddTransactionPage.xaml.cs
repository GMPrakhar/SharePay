using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using SharePay.Models;

namespace SharePay.UI
{
    public partial class AddTransactionPage : ContentPage
    {
        public AddTransactionPage(GroupModel group)
        {
            InitializeComponent();
            BindingContext = new AddTransactionViewModel(group);
        }

        private void OnUserSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (BindingContext as AddTransactionViewModel).OnSelectionChanged((sender as CollectionView).SelectedItems.Select(x => x as UserViewModel));
        }
    }

    public class AddTransactionViewModel : INotifyPropertyChanged
    {
        private GroupModel _group;
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public TransactionCategory Category { get; set; }
        public ObservableCollection<TransactionCategory> Categories { get; set; }
        public ObservableCollection<UserViewModel> Users { get; set; }
        public ObservableCollection<UserViewModel> SelectedUsers { get; set; }
        public ICommand SubmitCommand { get; }

        public AddTransactionViewModel(GroupModel group)
        {
            _group = group;
            Categories = new ObservableCollection<TransactionCategory>(Enum.GetValues(typeof(TransactionCategory)).Cast<TransactionCategory>());
            Users = new ObservableCollection<UserViewModel>();
            SelectedUsers = new ObservableCollection<UserViewModel>();
            SubmitCommand = new Command(async () => await SubmitTransaction());
            LoadUsers();
        }

        public void OnSelectionChanged(IEnumerable<UserViewModel> selectedUsers)
        {
            SelectedUsers.Clear();
            foreach(var user in selectedUsers)
            {
                SelectedUsers.Add(user);
            }
        }

        private async void LoadUsers()
        {
            var users = await FetchUsersFromApi();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private async Task<List<UserViewModel>> FetchUsersFromApi()
        {
            using (var client = new HttpClient())
            {

                var jsonSettings = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };
                return await client.GetFromJsonAsync<List<UserViewModel>>($"{Global.ApiEndpoint}/v1/groups/{_group.Id}/users", jsonSettings);
            }
        }

        private async Task SubmitTransaction()
        {
            var transaction = new TransactionModel
            {
                FromUser = Guid.Parse(Global.User), // Replace with actual user ID
                ToUsers = new HashSet<Guid>(SelectedUsers.Select(u => u.Id)),
                DivisionStrategyPerUserUnit = SelectedUsers.ToDictionary(u => u.Id, u => 1d), // Populate with actual data
                TotalAmount = TotalAmount,
                Description = Description,
                Category = Category
            };

            using (var client = new HttpClient())
            {
                var jsonSettings = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };
                var json = JsonSerializer.Serialize(transaction, jsonSettings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{Global.ApiEndpoint}/v1/groups/{_group.Id}/transactions", content);

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Transaction added successfully", "OK");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to add transaction", "OK");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}