// Na Model, estará a estrutura de cambio ficticia regras de conversão das moedas
namespace ConversorMoedasMauiMvvm.Models
{
    public class RateTable
    {
        private readonly Dictionary<string, decimal> _toBRL = new()
        {
            ["BRL"] = 1.00m, // Real Brasileiro          
            ["USD"] = 5.60m,  // Dólar Americano
            ["EUR"] = 6.10m // Euro
        };
        public IReadOnlyDictionary<string, decimal> toBRL => _toBRL;  // essa linha é como uma ponte de vizualização entre o dicionario privado e o publico


        // Método para converter um valor de uma moeda para outra
        public IEnumerable<string> GetCurrencies() => _toBRL.Keys.OrderBy(k => k);
        //public IEnumerable: Permite retornar uma coleção de valores sem expor a estrutura interna

        //STRING: indica que a coleção contem elementos do tipo string

        //GetCurrencies(): Nome do método que retorna a coleção de strings

        //_toBRL.Keys: Acessa as chaves do dicionário _toBRL, que são os códigos das moedas (ex: "BRL", "USD", "EUR")

        //OrderBy(k => k): Ordena as chaves em ordem alfabética

        public bool Supports(string code) => _toBRL.ContainsKey(code);  //Verifica se a moeda é suportada, retornando true ou false ************************ DUVIDA AQUI *************************

        //Método PRINCIPAL para converter um valor de uma moeda para outra
        public decimal Convert(decimal amount, string from, string to)
        {
            if (!Supports(from) || !Supports(to)) return 0m; // Retorna 0 se a moeda não for suportada
            if ( from == to) return amount; // Retorna o valor original se as moedas forem iguais

            var brl = amount * _toBRL[from]; // Converte o valor para BRL (Real Brasileiro) 
            return brl / _toBRL[to]; // Converte o valor de BRL para a moeda de destino
        }


    }
}
