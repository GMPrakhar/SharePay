using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
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
    }

    public class BalancesViewModel : INotifyPropertyChanged
    {
        private GroupModel _group;
        public ObservableCollection<BalancedTransactionModel> Balances { get; set; } = new ObservableCollection<BalancedTransactionModel>();

        public BalancesViewModel(GroupModel group)
        {
            _group = group;
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
            using (var client = new HttpClient())
            {
             
                var jsonSettings = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };   
                return await client.GetFromJsonAsync<List<BalancedTransactionModel>>($"{Global.ApiEndpoint}/v1/groups/{_group.Id}/transactions/consolidated", jsonSettings);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}