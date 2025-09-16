namespace ConversorMoedasMauiMvvm
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            BindingContext = new ViewModels.MainViewModel(); // Define o contexto de dados da página para uma nova instância de MainViewModel
        }

     
    }
}
