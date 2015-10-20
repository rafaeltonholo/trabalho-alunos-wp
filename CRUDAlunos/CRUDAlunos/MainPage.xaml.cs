using CRUDAlunos.Aplicacao.ViewObjects;
using CRUDAlunos.Domain.Ioc;
using CRUDAlunos.ViewModels.Base;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace CRUDAlunos {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        private IViewModel<AlunoView> ViewModel { get; set; }

        public MainPage() {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            ViewModel = DependencyCore.Instance.GetInstance<IViewModel<AlunoView>>();
            ViewModel.Page = this;
            DataContext = ViewModel;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            ViewModel.ReloadData();
        }
    }
}
