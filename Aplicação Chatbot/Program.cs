using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SimpleChatbot
{
    public class ChatGPTClient
    {
        private static readonly HttpClient httpClient = new HttpClient();

        // Histórico de mensagens
        private List<dynamic> mensagens = new List<dynamic>
        {
            new { role = "system", content = "Você é um assistente útil." }
        };

        public async Task<string> EnviarMensagem(string mensagem)
        {
            try
            {
                // Adiciona a mensagem do usuário ao histórico
                mensagens.Add(new { role = "user", content = mensagem });

                // Criar o payload da requisição
                var requestData = new
                {
                    model = "gpt-3.5-turbo", // Modelo utilizado
                    messages = mensagens,
                    max_tokens = 100, // Limite de palavras na resposta
                    temperature = 0.7 // Criatividade
                };

                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Adicionar o cabeçalho de autenticação
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {OpenAIConfig.ApiKey}");

                // Enviar a requisição
                var response = await httpClient.PostAsync(OpenAIConfig.Endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    dynamic responseObject = JsonConvert.DeserializeObject(responseJson);

                    // Captura a resposta da API
                    string resposta = responseObject.choices[0].message.content.ToString();

                    // Adiciona a resposta ao histórico
                    mensagens.Add(new { role = "assistant", content = resposta });

                    // Retorna a resposta
                    return resposta.Trim();
                }
                else
                {
                    return $"Erro: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                return $"Erro: {ex.Message}";
            }
        }
    }

    public class OpenAIConfig
    {
        public const string ApiKey = ""; // Substitua pela sua chave
        public const string Endpoint = "https://api.openai.com/v1/chat/completions";
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            // Mensagem inicial do chatbot
            Console.WriteLine("Bem-vindo ao Chatbot de Atendimento!");
            Console.WriteLine("Digite sua pergunta ou digite 'sair' para encerrar.\n");

            // Dicionário com perguntas e respostas pré-definidas
            Dictionary<string, string> faq = new Dictionary<string, string>
            {
                { "qual seu nome", "Sou o Chatbot de Atendimento." },
                { "como posso resetar minha senha", "Você pode resetar sua senha clicando em 'Esqueci minha senha' na tela de login." },
                { "quais são seus serviços", "Oferecemos suporte técnico, vendas e consultoria." },
                { "como entrar em contato", "Você pode nos enviar um e-mail para suporte@empresa.com ou ligar para (11) 1234-5678." }
            };

            var chatClient = new ChatGPTClient(); // Instancia do ChatGPTClient

            while (true)
            {
                // Entrada do usuário
                Console.Write("Você: ");
                string input = Console.ReadLine()?.ToLower(); // Converte a entrada para minúsculas

                // Verifica se o usuário quer encerrar a conversa
                if (input == "sair")
                {
                    Console.WriteLine("\nChatbot: Obrigado por usar nosso atendimento. Até mais!");
                    break;
                }

                // Procura uma resposta correspondente no dicionário
                bool respostaEncontrada = false;
                foreach (var pergunta in faq.Keys)
                {
                    if (input.Contains(pergunta))
                    {
                        Console.WriteLine($"Chatbot: {faq[pergunta]}");
                        respostaEncontrada = true;
                        break;
                    }
                }

                // Caso nenhuma resposta do dicionário seja encontrada, utilize o ChatGPT
                if (!respostaEncontrada)
                {
                    Console.WriteLine("Chatbot: Estou pensando..."); // Mensagem de espera
                    var resposta = await chatClient.EnviarMensagem(input); // Utilize await para chamadas assíncronas
                    Console.WriteLine($"Chatbot: {resposta}");
                }
            }
        }
    }
}
