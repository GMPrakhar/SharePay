using SharePay.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharePay.UI
{
    public partial class GroupsPage : ContentPage
    {
        public GroupsPage()
        {
            InitializeComponent();
            BindingContext = new GroupsViewModel();
        }

        private async void OnGroupSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is GroupModel selectedGroup)
            {
                await Navigation.PushAsync(new TransactionsPage(selectedGroup));
            }
        }
    }

    public class Group
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class GroupsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<GroupModel> Groups { get; set; } = new ObservableCollection<GroupModel>();

        public GroupsViewModel()
        {
            LoadGroups();
        }

        private async void LoadGroups()
        {
            var groups = await FetchGroupsFromApi();
            foreach (var group in groups)
            {
                Groups.Add(group);
            }
        }

        private async Task<List<GroupModel>> FetchGroupsFromApi()
        {
            using var client = new HttpClient();
            var userId = new Guid(Global.User);
            var jsonSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

            return await client.GetFromJsonAsync<List<GroupModel>>($"{Global.ApiEndpoint}/v1/user/{userId}/groups", jsonSettings);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}