using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using SharePay.Models;

namespace SharePay.UI
{
    public partial class TransactionsPage : ContentPage
    {

        public GroupModel Group { get; }

        public TransactionsPage(GroupModel group)
        {
            InitializeComponent();
            this.Group = group;
            BindingContext = new TransactionsViewModel(group);
        }

        private async void OnAddTransactionClicked(object sender, EventArgs e)
        {
            // Navigate to Add Transaction page
            await Navigation.PushAsync(new AddTransactionPage(Group));
        }

        private async void OnBalancesClicked(object sender, EventArgs e)
        {
            // Navigate to Balances page
            await Navigation.PushAsync(new BalancesPage(Group));
        }
    }

    public class TransactionsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TransactionViewModel> Transactions { get; set; } = new ObservableCollection<TransactionViewModel>();
        public string GroupName { get; set; }

        public TransactionsViewModel(GroupModel group)
        {
            GroupName = group.Name;
            LoadTransactions(group.Id);
        }

        private async void LoadTransactions(Guid groupId)
        {
            var transactions = await FetchTransactionsFromApi(groupId);
            foreach (var transaction in transactions)
            {
                Transactions.Add(transaction);
            }
        }

        private async Task<List<TransactionViewModel>> FetchTransactionsFromApi(Guid groupId)
        {
            using (var client = new HttpClient())
            {
                var jsonSettings = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };

                return await client.GetFromJsonAsync<List<TransactionViewModel>>($"{Global.ApiEndpoint}/v1/groups/{groupId}/transactions?page=1&size=10", jsonSettings);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}