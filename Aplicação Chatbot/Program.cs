using System;
using System.Collections.Generic;

namespace SimpleChatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Mensagem inicial do chatbot
            Console.WriteLine("Bem-vindo ao Chatbot de Atendimento!");
            Console.WriteLine("Digite sua pergunta ou digite 'sair' para encerrar.\n");

            // Dicionário com perguntas e respostas
            Dictionary<string, string> faq = new Dictionary<string, string>
            {
                { "qual seu nome", "Sou o Chatbot de Atendimento." },
                { "como posso resetar minha senha", "Você pode resetar sua senha clicando em 'Esqueci minha senha' na tela de login." },
                { "quais são seus serviços", "Oferecemos suporte técnico, vendas e consultoria." },
                { "como entrar em contato", "Você pode nos enviar um e-mail para suporte@empresa.com ou ligar para (11) 1234-5678." }
            };

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

                // Resposta padrão caso nenhuma correspondência seja encontrada
                if (!respostaEncontrada)
                {
                    Console.WriteLine("Chatbot: Desculpe, não entendi sua pergunta. Tente novamente.");
                }
            }
        }
    }
}
