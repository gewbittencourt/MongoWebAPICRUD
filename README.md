# MongoWebAPICRUD

Este é um projeto de WebAPI criado com o objetivo de estudar e aplicar conceitos de Clean Architecture e Domain-Driven Design (DDD). O projeto gerencia tarefas genéricas, utilizando MongoDB como banco de dados.

## Tecnologias Utilizadas

-   .NET Core
-   ASP.NET Core WebAPI
-   MongoDB
-   RestAPI
-   SOLID
-   Clean Architecture
-   Domain-Driven Design (DDD)
-   AutoMapper
-   XUnit
-   Testes Unitários

## Estrutura do Projeto

O projeto segue a estrutura de Clean Architecture, separando responsabilidades em diferentes camadas:

-   **Domain**: Contém as entidades e interfaces de repositório.
-   **Application**: Contém as interfaces de serviços, serviços de aplicação, BaseOutput, Input e Outputs.
-   **Infrastructure**: Implementa os repositórios e serviços para acessar o MongoDB.
-   **API**: Exposição dos endpoints REST para o gerenciamento das tarefas, BaseResponses para exposição de dados e respostas.
-   **Tests**: Implementação de testes unitários para a Controller, Service e Repository.

## Funcionalidades

-   **CRUD de Tarefas**: Permite a criação, leitura, atualização, finalização e exclusão de tarefas.
