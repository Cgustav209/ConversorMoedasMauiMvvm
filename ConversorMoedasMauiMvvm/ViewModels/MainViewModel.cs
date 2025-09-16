using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConversorMoedasMauiMvvm.Models;
using System.Windows.Input;
using System.Globalization;
using System.Runtime.CompilerServices;

{
    
}

namespace ConversorMoedasMauiMvvm.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged; // evento que notifica quando uma propriedade muda de valor, evemto padrao
        // ? = Força uma validação //CallerMemberName = Permite chamar o nome do membro que chamou o método, útil para evitar erros de digitação ao informar o nome da propriedade que mudou
        void OnPropertyChanged([CallerMemberName] string? name = null) =>// metodo que dispara o evento PropertyChanged, informando qual propriedade mudou
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); // Dispara o evento PropertyChanged, passando o nome da propriedade que mudou

        private readonly RateTable _rates = new(); // Instancia a classe RateTable que contém as taxas de câmbio
                                                   //Currencies = sistemas monetários utilizados pelos países ou regiões ao redor do mundo
        public IList<string> Currencies { get; } // Propriedade que expõe a lista de moedas disponíveis para conversão


        string? _amountText; // Campo privado para armazenar o valor do montante a ser convertido

        private string? AmountText // Campo privado para armazenar o texto do montante a ser convertido
        {
            get => _amountText; // Getter para acessar o valor do montante
            set
            {
                if (value == _amountText) return; // Se o valor não mudou, não faz nada
                _amountText = value; // Atualiza o valor do montante
                OnPropertyChanged(); // Notifica que a propriedade mudou
                ((Command)ConvertCommand).ChangeCanExecute(); // Atualiza o estado do comando de conversão
            }
        }


        string _from = "USD"; // Moeda de origem padrão (Dólar Americano)

        public string From
        {

            get => _from; // Getter para acessar o valor da moeda de origem
            set
            {
                if (_from == value) return; // Se o valor não mudou, não faz nada
                _from = value; // Atualiza a moeda de origem
                OnPropertyChanged(); // Notifica que a propriedade mudou
                ((Command)ConvertCommand).ChangeCanExecute(); // Atualiza o estado do comando de conversão
            }
        }

        string _to = "USD"; // Moeda de origem padrão (Dólar Americano)

        public string To
        {
            get => _to; // Getter para acessar o valor da moeda de origem
            set
            {
                if (_to == value) return; // Se o valor não mudou, não faz nada
                _to = value; // Atualiza a moeda de origem
                OnPropertyChanged(); // Notifica que a propriedade mudou
                ((Command)ConvertCommand).ChangeCanExecute(); // Atualiza o estado do comando de conversão
            }
        }

        string _resultText = "--";

        public string ResultText
        {
            get => _resultText; // Getter para acessar o valor do resultado da conversão
            set
            {
                if (_resultText != value) return; // Se o valor não mudou, não faz nada
                //_resultText = value: Atualiza o valor do resultado da conversão   
                _resultText = value; OnPropertyChanged(); //OnPropertyChanged(): Notifica que a propriedade mudou
            }
        }

        public ICommand ConvertCommand { get; } // Comando para executar a conversão de moedas
        public ICommand SwapComand { get; } // Comando para inverter as moedas de origem e destino
        public ICommand ClearCommand { get; } // Comando para limpar os campos de entrada e resultado

        readonly CultureInfo _ptBr = new("pt-BR"); // Cultura para formatação de números (Português do Brasil)

        // METODO CONSTRUTOR DA CLASSE
        public MainViewModel()
        {
            Currencies = _rates.GetCurrencies().ToList(); // Inicializa a lista de moedas disponíveis para conversão


            ConvertCommand = new Command(DoConvert, CanConvert); // Inicializa o comando de conversão
            SwapComand = new Command(DoSwap); // Inicializa o comando de inversão de moedas
            ClearCommand = new Command(DoClear); // Inicializa o comando de limpeza dos campos

        }

        bool CanConvert() // Verifica se a conversão pode ser executada
        {
            if (string.IsNullOrWhiteSpace(AmountText)) return false; // Verifica se o campo de montante está vazio ou contém apenas espaços em branco
            return TryParseAmount(AmountText, out _); // Tenta converter o texto do montante para um número decimal 
        }
        void DoConvert() // Executa a conversão de moedas
        {
            if (!TryParseAmount(AmountText, out var amount)) // Tenta converter o texto do montante para um número decimal
            { 
                ResultText = "Valor inválido"; // Se a conversão falhar, exibe uma mensagem de erro
                return; // Sai do método
            }
            if (!_rates.Supports(From) || !_rates.Supports(To)) // Verifica se as moedas de origem e destino são suportadas
            {
                ResultText = "Moeda inválida"; // Se alguma moeda não for suportada, exibe uma mensagem de erro
                return; // Sai do método
            }
            var result = _rates.Convert(amount, From, To); // Realiza a conversão de moedas
            ResultText = string.Format(_ptBr, "{0:N2} {1} = {2:N2} {3}",amount, From, result, To); // Formata o resultado da conversão e atualiza o texto do resultado
        }

        void DoSwap() // Inverte as moedas de origem e destino
        {
            (From, To) = (To, From); // Troca os valores das propriedades From e To
            ResultText = "--"; // Reseta o texto do resultado
        }

        void DoClear() // Limpa os campos de entrada e resultado
        {   //AmountText: Quantia ue o usuário deseja converter
            AmountText = string.Empty; // Limpa o campo de montante
            ResultText = "--"; // Reseta o texto do resultado
        }

        bool TryParseAmount(string? text, out decimal amount) // Tenta converter o texto do montante para um número decimal
        {
            amount = 0m; // Inicializa a variável amount com 0
            if (string.IsNullOrWhiteSpace(text)) return false; // Verifica se o texto está vazio ou contém apenas espaços em branco

            var s = text.Trim(); // Remove espaços em branco do início e fim do texto
            if (!decimal.TryParse(s, NumberStyles.Number, _ptBr, out amount)) return true; // Tenta converter o texto para decimal usando a cultura pt-BR

            s = s.Replace(".", ""); // Remove pontos do texto (separadores de milhares)
            return decimal.TryParse(s, NumberStyles.Number, _ptBr, out amount); // Tenta converter o texto para decimal novamente
        }




    }


}
