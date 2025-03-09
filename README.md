# Chatbot de E-commerce com Integração Google Gemini 

## Descrição
Este projeto é um **chatbot para e-commerce** desenvolvido em **C#**, com integração à API do **Google Gemini** para fornecer respostas inteligentes e automatizadas aos clientes. O objetivo é melhorar a experiência do usuário, oferecendo suporte rápido e eficiente sobre produtos, pedidos e serviços da loja.

## Tecnologias Utilizadas
- **C# (.NET Core ou .NET 7+)** - Linguagem principal do projeto.
- **Google Gemini API** - Integração para processamento de linguagem natural.
- **ASP.NET Web API** - Para criar endpoints de comunicação.
- **Entity Framework Core** (opcional) - Para persistência de dados.
- **Banco de Dados (SQL Server, PostgreSQL ou MongoDB)** - Para armazenar histórico de interação.
- **Swagger** - Para documentação da API.

## Funcionalidades
- Responder perguntas frequentes sobre produtos, pedidos e suporte.
- Consultar status de pedidos.
- Recomendar produtos baseados no histórico do cliente.
- Integração com API do Google Gemini para respostas inteligentes.
- Suporte a múltiplos canais (Web, WhatsApp, Telegram, etc.).

## Como Executar o Projeto
1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/chatbot-ecommerce.git
   ```
2. Acesse a pasta do projeto:
   ```bash
   cd chatbot-ecommerce
   ```
3. Configure a chave da API do Google Gemini no `appsettings.json`:
   ```json
   {
     "GoogleGemini": {
       "ApiKey": "SUA_CHAVE_AQUI"
     }
   }
   ```
4. Instale as dependências do projeto:
   ```bash
   dotnet restore
   ```
5. Execute a aplicação:
   ```bash
   dotnet run
   ```
6. Acesse a documentação da API via Swagger:
   ```
   http://localhost:5000/swagger
   ```



