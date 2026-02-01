# Integration.BrasilApi.Fipe.Net 🚗💨

[![.NET 10](https://img.shields.io/badge/.NET-10-blue.svg)](https://dotnet.microsoft.com/)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-green.svg)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Sistema de consulta de preços da Tabela FIPE integrado à [BrasilAPI](https://brasilapi.com.br/). O projeto consome dados em tempo real, exibe em uma interface responsiva e gera automaticamente um relatório executivo em PDF na área de trabalho do usuário.



## 🚀 Tecnologias Utilizadas

* **C# 10 / .NET 10**
* **ASP.NET Core MVC**: Interface do usuário e roteamento.
* **Flurl.Http**: Consumo fluente e resiliente da API REST.
* **QuestPDF**: Motor de alta performance para geração de relatórios PDF.
* **xUnit**: Testes de integração e unitários.
* **Bootstrap 5**: Layout responsivo e moderno.

## 🏗️ Arquitetura

O projeto foi construído seguindo os princípios da **Clean Architecture** (Arquitetura Limpa), separado em camadas para garantir testabilidade e baixo acoplamento:

1.  **Domain**: Contém as interfaces (`IFipeService`) e modelos de dados (`VeiculoFipeResponse`).
2.  **Infrastructure**: Implementa o consumo externo com Flurl e a lógica de geração de documentos com QuestPDF.
3.  **Web/API**: Camada de apresentação MVC, controladores e configurações de Injeção de Dependência.
4.  **Tests**: Testes automatizados para validar a integração e a saída de arquivos.

## 📋 Pré-requisitos

* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* Visual Studio 2022 ou VS Code

## 🔧 Configuração e Execução

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/seu-usuario/Integration.BrasilApi.Fipe.Net.git](https://github.com/seu-usuario/Integration.BrasilApi.Fipe.Net.git)
    cd Integration.BrasilApi.Fipe.Net
    ```

2.  **Restaure as dependências:**
    ```bash
    dotnet restore
    ```

3.  **Execute o projeto Web:**
    ```bash
    dotnet run --project src/Integration.BrasilApi.Fipe.Api
    ```

4.  **Acesse no navegador:**
    `https://localhost:7102` (ou a porta indicada no console).

## 🧪 Testes

Para garantir que a integração e a geração de PDF estão funcionando corretamente:

```bash
dotnet test
```

Nota: Os testes configuram automaticamente a licença Community do QuestPDF e validam se o arquivo é criado corretamente no diretório de usuário.

## 📂 Relatório PDF Profissional

O sistema utiliza a biblioteca **QuestPDF** para gerar um documento executivo destinado ao cliente final. O fluxo ocorre da seguinte forma:

1. **Geração Automática**: Assim que o serviço recebe o retorno da BrasilAPI, o PDF é construído em memória e persistido no disco.
2. **Localização**: O arquivo é salvo na Área de Trabalho (Desktop) do usuário logado.
3. **Nomenclatura**: O arquivo segue o padrão `Fipe_CODIGO_FIPE.pdf`.

### Exemplo de execução via CLI para gerar o relatório:
```bash
dotnet run --project Integration.BrasilApi.Fipe.Api
```