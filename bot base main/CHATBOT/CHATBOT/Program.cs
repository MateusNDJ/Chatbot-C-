using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Mscc.GenerativeAI;
using static System.Formats.Asn1.AsnWriter;

class Program
{
    private static readonly string apiKey = "AIzaSyCF_6c8JA4N_9ZwaMi_pnsPhDm8BypH7TI";

   

    static async Task Main(string[] args)
    {
        var googleAI = new GoogleAI(apiKey);
        var model = googleAI.GenerativeModel(Model.GeminiPro);

        while (true)
        {
            Console.Write("Digite sua pergunta (ou 'sair' para encerrar): ");
            string input = Console.ReadLine();
            if (input.ToLower() == "sair")
                break;

            string resposta = await ObterRespostaAsync(model, input);
            Console.WriteLine("Resposta:");
            Console.WriteLine(resposta);
        }
    }

    private static async Task<string> ObterRespostaAsync(GenerativeModel model, string pergunta)
    {

        // Classificar o tipo de pergunta e o tipo de produto para selecionar uma resposta apropriada
        string tipoPergunta = ClassificarPergunta(pergunta);
        string tipoProduto = ClassificarProduto(pergunta);


        // Se não for uma pergunta frequente, usar o modelo AI para gerar uma resposta com mais contexto (roleplay)
        string promptFinal = $"Você é um chatbot de um marketplace especializado em responder dúvidas sobre pedidos, produtos e suporte. Use as mensagens fornecidas abaixo como **base de referência** para criar respostas personalizadas. As respostas devem ser claras, amigáveis, objetivas e adaptadas ao contexto da pergunta do cliente:\r\n\r\n### **Mensagens Base:**\r\n\r\n\r\n### **Diretrizes:**\r\n- Sempre inicie com uma saudação amigável, adequada ao horário de Brasília-SP (exemplo: 'Olá, bom dia!').\r\n- Analise o contexto da pergunta para responder de forma personalizada, baseando-se nas mensagens fornecidas, mas adaptando ao caso específico do cliente.\r\n- Para dúvidas sobre o status do pedido (envio, entrega, rastreamento):\r\n  - Responda apenas se for claramente solicitado pelo cliente.\r\n  - Utilize uma abordagem neutra, amigável e empática.\r\n  - Reforce que informações detalhadas sobre o envio podem ser obtidas diretamente no painel da Shopee.\r\n- Para perguntas sobre produtos (exemplo: limpeza, automotivos, eletrodomésticos):\r\n  - Responda com base nas instruções do fabricante.\r\n  - Mantenha a resposta objetiva e clara.\r\n- Nunca forneça informações sobre políticas internas, kits personalizados ou brindes.\r\n- Caso o cliente elogie, agradeça educadamente e incentive mais perguntas.\r\n- Se a pergunta for vaga ou confusa, peça educadamente para reformular ou fornecer mais detalhes.\r\n- Se não souber responder, oriente o cliente a buscar ajuda diretamente com o suporte da Shopee ou forneça uma sugestão prática.\r\n- Sempre mostre empatia e compreensão em situações de cancelamentos, atrasos ou extravios.\r\n- Adapte sua resposta para ter no máximo 150 caracteres, mantendo uma linguagem amigável e natural.\r\n\r\nO cliente pergunta: {pergunta}. Responda com base nas mensagens e diretrizes acima, criando uma resposta personalizada e apropriada ao contexto.";
        try
        {
            var response = await model.GenerateContent(promptFinal);
            return $" {response.Text.Trim()} Att. AG Assistent";
        }

        catch (Exception ex)
        {
            return $"Desculpe, não consegui entender sua pergunta. Tente reformular e perguntarei novamente! Att. AG Assistent";
        }
    }
    private static string FormatarResposta(string resposta, string tipoProduto)
    {
        string mensagemBase = $" {resposta}";
        switch (tipoProduto)
        {
            case "automotivo":
                return mensagemBase + " Observação: Para produtos automotivos, sempre verifique as recomendações do fabricante. Att. AG Assistent";
            case "eletrodoméstico":
                return mensagemBase + " Observação: Para produtos eletrodomésticos, siga o manual para instruções específicas. Att. AG Assistent";
            default:
                return mensagemBase + " Att. AG Assistent"; // resposta padrão para produtos em geral
        }
    }
    private static string ClassificarPergunta(string pergunta)
    {
        pergunta = pergunta.ToLower();
        if (pergunta.Contains("status") || pergunta.Contains("a caminho") || pergunta.Contains("enviado"))
            return "status_pedido";
        if (pergunta.Contains("reembolso") || pergunta.Contains("cancelamento"))
            return "reembolso";
        if (pergunta.Contains("extraviado") || pergunta.Contains("defeito"))
            return "pacote_extraviado";
        if (pergunta.Contains("produto errado") || pergunta.Contains("pedido errado") || pergunta.Contains("veio errado"))
            return "produto_errado";
        if (pergunta.Contains("esgotado"))
            return "produto_esgotado";
        if (pergunta.Contains("validade"))
            return "validadade_produto";
        if (pergunta.Contains("original"))
            return "produto_original";
        if (pergunta.Contains("trocar") || pergunta.Contains("substituir") || pergunta.Contains("endereço"))
            return "troca_ou_endereco";

        // Caso não se enquadre em nenhum tipo específico
        return "outro";
    }
    private static string ClassificarProduto(string pergunta)
    {
        pergunta = pergunta.ToLower();

        if (pergunta.Contains("automotivo") || pergunta.Contains("carro") || pergunta.Contains("pneu"))
            return "automotivo";
        if (pergunta.Contains("eletrodoméstico") || pergunta.Contains("ventilador") || pergunta.Contains("liquidificador"))
            return "eletrodoméstico";

        // Caso seja um produto genérico
        return "geral";
    }
}
