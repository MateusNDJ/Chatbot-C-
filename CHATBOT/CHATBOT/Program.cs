using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mscc.GenerativeAI;

class Program
{
    private static readonly string apiKey = "AIzaSyCF_6c8JA4N_9ZwaMi_pnsPhDm8BypH7TI";

    private static readonly Dictionary<string, string> RespostasFrequentes = new Dictionary<string, string>
    {
        { "status_pedido", "Seu pedido está a caminho ou aguardando envio. Caso precise de mais informações, entre em contato com o suporte. Att. AG A.I" },
        { "reembolso", "Questões de reembolso e cancelamento devem ser tratadas diretamente com o suporte da Shopee, pois não temos controle sobre essas áreas. Att. AG A.I" },
        { "pacote_extraviado", "Caso o pacote tenha sido extraviado ou entregue com defeito, entre em contato diretamente com o suporte da Shopee. Att. AG A.I" },
        { "produto_errado", "Caso tenha recebido um produto errado, entre em contato diretamente com o suporte da Shopee para resolver essa questão. Att. AG A.I" },
        { "produto_esgotado", "Houve um erro e o produto consta como esgotado. Em breve você será reembolsado. Att. AG A.I" },
        { "validadade_produto", "Devido à alta rotatividade, todos os produtos têm validade estendida. Fique tranquilo quanto à qualidade. Att. AG A.I" },
        { "produto_original", "Sim, todos os produtos são originais, comprados diretamente de fabricantes. Att. AG A.I" },
        { "troca_ou_endereco", "Após o pedido ser faturado, não conseguimos realizar trocas de item ou alterações de endereço. Att. AG A.I" },
        { "Kits_personalizados", "Infelizmente no momento não temos a opção de criar kit's especifico. Mas não se preocupe o senhor(a) pode estar adicionando os produtos desejado ao carinho e realizar a compra. Estamos a disposição.Att. AG A.I" }
    };

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


        // Verificar se é um elogio
        if (pergunta.ToLower().Contains("obrigado") || pergunta.ToLower().Contains("muito bom") || pergunta.ToLower().Contains("excelente"))
        {
            return "Olá! Agradeço muito pelo seu elogio! Estou aqui para ajudar. Qual é a sua dúvida? Att. AG A.I";
        }

        // Se a pergunta for sobre o status do pedido
        if (pergunta.ToLower().Contains("cade meu pedido") || pergunta.ToLower().Contains("onde está") || pergunta.ToLower().Contains("status"))
        {
            return "Olá! Seu pedido está a caminho ou aguardando envio. Pedimos que aguarde pacientemente e, caso precise de mais informações, entre em contato com o suporte. Att. AG A.I";
        }

        // Se a pergunta contém uma saudação ou está vaga, pedir mais detalhes
        if (pergunta.ToLower().Contains("duvida") ||
            pergunta.ToLower().Contains("não sei") ||
            pergunta.ToLower().Contains("olá") ||
            pergunta.ToLower().Contains("oi") ||
            pergunta.ToLower().Contains("bom dia") ||
            pergunta.ToLower().Contains("boa tarde") ||
            pergunta.ToLower().Contains("boa noite") ||
            pergunta.ToLower().Contains("ola"))
        {
            return "Olá! Qual é a sua dúvida? Estou aqui para ajudar. Att. AG A.I";
        }


        // Classificar o tipo de pergunta e o tipo de produto para selecionar uma resposta apropriada
        string tipoPergunta = ClassificarPergunta(pergunta);
        string tipoProduto = ClassificarProduto(pergunta);

        // Verificar se existe uma resposta padrão para o tipo de pergunta
        if (RespostasFrequentes.ContainsKey(tipoPergunta))
        {
            return FormatarResposta(RespostasFrequentes[tipoPergunta], tipoProduto);
        }

        // Se a pergunta for sobre um produto específico, responder com informações adequadas sobre o produto
        if (pergunta.ToLower().Contains("limpeza") || pergunta.ToLower().Contains("produto"))
        {
            return RespostaSobreProduto(pergunta);
        }

        // Se não for uma pergunta frequente, usar o modelo AI para gerar uma resposta com mais contexto (roleplay)
        string promptFinal = $"Você é um assistente virtual chatbot amigável de um e-commerce de produtos de limpeza, automotivos e eletrodomésticos como nosso chatbot voce não pode oferecer kits personalizados nunca falar mais do que necessario sobre a empresa normalmente os clientes perguntam da entrega ou prazo fale ola seu pedido logo chegara a voce aguarde espero sua compreensão. qualquer problema que envolva cancelemanto, extravio , reembolso o cliente tem que abrir um chamado diretamente com a shopee tambem se o cliente tiver duvida sobre algum produto tente responder de maneira obejtiva O cliente pergunta: {pergunta}. Responda com no máximo 200 caracteres, usando uma linguagem natural e simpática.";
        try
        {
            var response = await model.GenerateContent(promptFinal);
            return $" {response.Text.Trim()} Att. AG A.I";
        }
        catch (Exception ex)
        {
            return $"Desculpe, não consegui entender sua pergunta. Tente reformular e perguntarei novamente! Att. AG A.I";
        }
    }

    private static string FormatarResposta(string resposta, string tipoProduto)
    {
        string mensagemBase = $" {resposta}";

        switch (tipoProduto)
        {
            case "automotivo":
                return mensagemBase + " Observação: Para produtos automotivos, sempre verifique as recomendações do fabricante. Att. AG A.I";
            case "eletrodoméstico":
                return mensagemBase + " Observação: Para produtos eletrodomésticos, siga o manual para instruções específicas. Att. AG A.I";
            default:
                return mensagemBase + " Att. AG A.I"; // resposta padrão para produtos em geral
        }
    }

    private static string RespostaSobreProduto(string pergunta)
    {
        pergunta = pergunta.ToLower();

        // Respostas baseadas em categorias de produtos
        if (pergunta.Contains("limpeza automotiva"))
        {
            return "Olá! Para limpeza automotiva, recomendamos produtos com fórmulas mais fortes, ideais para remover sujeiras difíceis. Produtos como limpadores de estofados, plásticos e carpetes são ótimos para manter o seu carro sempre limpo e com um cheiro agradável. Certifique-se de usar o produto de acordo com as instruções. Att. AG A.I";
        }
        else if (pergunta.Contains("eletrodoméstico"))
        {
            return "Olá! Para a limpeza de eletrodomésticos, é importante usar produtos mais suaves, que não danifiquem as superfícies. Produtos como sprays de limpeza para telas e superfícies são ideais para manter seus aparelhos em perfeito estado. Verifique as recomendações do fabricante. Att. AG A.I";
        }
        else if (pergunta.Contains("limpeza"))
        {
            return "Olá! Para limpeza em geral, a escolha do produto depende do tipo de sujeira e da superfície a ser limpa. Produtos com fórmulas mais fortes são melhores para sujeiras pesadas, enquanto os mais suaves são indicados para manutenções regulares. Siga as instruções para melhores resultados. Att. AG A.I";
        }

        return "Olá! Não tenho informações específicas sobre esse produto, mas ficarei feliz em ajudar a escolher o melhor para sua necessidade. Se puder fornecer mais detalhes, poderei fornecer uma resposta mais precisa. Att. AG A.I";
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
        if (pergunta.Contains("eletrodoméstico") || pergunta.Contains("geladeira") || pergunta.Contains("micro-ondas"))
            return "eletrodoméstico";

        // Caso seja um produto genérico
        return "geral";
    }
}
