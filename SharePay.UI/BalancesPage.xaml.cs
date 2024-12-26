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
    public partial class BalancesPage : ContentPage
    {
        public GroupModel Group { get; private set; }

        public BalancesPage(GroupModel group)
        {
            InitializeComponent();
            this.Group = group;
            BindingContext = new BalancesViewModel(group);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }

    public class BalancesViewModel : INotifyPropertyChanged
    {
        private GroupModel _group;
        public ObservableCollection<BalancedTransactionModel> Balances { get; set; } = new ObservableCollection<BalancedTransactionModel>();
        public ICommand SettleUpCommand { get; }

        public BalancesViewModel(GroupModel group)
        {
            _group = group;
            SettleUpCommand = new Command<BalancedTransactionModel>(async (balance) => await SettleUp(balance));
            LoadBalances();
        }

        private async void LoadBalances()
        {
            var balances = await FetchBalancesFromApi();
            foreach (var balance in balances)
            {
                Balances.Add(balance);
            }
        }

        private async Task<List<BalancedTransactionModel>> FetchBalancesFromApi()
        {
            // Add JSON seralizer options
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };
            
            using var client = new HttpClient();
            return await client.GetFromJsonAsync<List<BalancedTransactionModel>>($"{Global.ApiEndpoint}/v1/groups/{_group.Id}/transactions/consolidated", options);
        }

        private async Task SettleUp(BalancedTransactionModel balance)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm", $"Settle up {balance.Amount:C} from {balance.FromName} to {balance.ToName}?", "Yes", "No");
            if (!confirm) return;

            var transaction = new TransactionModel
            {
                FromUser = balance.From, // Replace with actual user ID
                ToUsers = new HashSet<Guid> { balance.To }, // Replace with actual user ID
                DivisionStrategyPerUserUnit = new Dictionary<Guid, double> { { balance.To, 1 } }, // Populate with actual data
                TotalAmount = balance.Amount,
                Description = "Settle up",
                Category = TransactionCategory.General
            };

            // Add JSON seralizer options
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };
            
            using (var client = new HttpClient())
            {
                var json = JsonSerializer.Serialize(transaction, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{Global.ApiEndpoint}/v1/groups/{_group.Id}/transactions", content);

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Transaction added successfully", "OK");
                    Balances.Remove(balance);
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