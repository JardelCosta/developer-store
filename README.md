# Developer Store - Sales API

Projeto para avaliação técnica. Implementa uma API para gerenciamento de registros de vendas, aplicando princípios de Domain-Driven Design (DDD), Clean Architecture com CQRS e suporte a eventos de domínio.

O API gerencia:
- Número da venda
- Data da venda
- Cliente (com External Identity denormalizado)
- Valor total da venda
- Filial (com External Identity denormalizado)
- Produtos, quantidades, preços unitários, descontos e totais por item
- Status de cancelamento (venda ou item individual)

Regras de negócio implementadas:
- Desconto de 10% para 4+ itens idênticos
- Desconto de 20% para 10-20 itens idênticos
- Limite máximo de 20 itens idênticos por produto
- Sem desconto para <4 itens

Eventos "publicados" via Domain Events (disparados no SaveChanges e logados no console).

## Requisitos
- .NET 8
- Git
- Postman ou Swagger para testar os endpoints (Swagger já está configurado na API)

## Configuração
1. Clone o repositório:
git clone https://github.com/JardelCosta/developer-store.git
- cd developer-store
2. Restaure pacotes NuGet:
- dotnet restore
3. DataBase é InMemory, não é necessário migrations ou setup de DB externo. O DB é criado em memória ao rodar a API.

## Execução
1. Rode a API: