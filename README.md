# EclipseWorks Challenge - API tarefas
### Developed by **Vinicius Lacerda Gonsalez**

Este projeto consiste em uma API construída com .NET 9.0

##build
Tenha a **.NET 9 SDK** instalada.
Para fazer o build:
```bash
cd EclipseWorksChallange
dotnet build
```

##Run
Tenha a **.NET 9 SDK** instalada.
Para rodar o projeto:
```bash
cd EclipseWorksChallange 
dotnet run 
```
## Swagger
http://localhost/swagger

## Test
To validate the system using unit tests, run:
```bash
cd EclipseWorks.Test
dotnet test
```

## Docker Build
```bash
docker build -t eclipseworks-api .
```

## Docker Run
```bash
docker run -p 8080:8080 -p 8081:8081 --name eclipseworks_container eclipseworks-api
```

## Database
Banco de dados utilizado foi SQL Server e o script está no repositorio
CriacaoDB.sql

##Informações
- **Language:** C# (.NET 9)
- **Testing Framework:** xUnit
---

##Perguntas ao PO
Qual o tipo de autenticação e como vai ser validada no backend (API)?
Qual a ferramenta de observabilidade utilizada ? Para onde serão enviados os logs?
Ha um secrets manager configurado para as connection strings ? 
Porque permitir apagar um projeto e não so desativa-lo ? 
As tarefas após concluidas ou canceladas podem retornar ao status pendente ? e a data de conclusão ?
Precisamos de um endpoint especifio para consultar o histórico?
Existe limite de caracteres por tarefa ou comentario?
Terá alguma integração externa como email ou calendário ?

##Melhorias
Implementar health checks e endpoints de readiness/liveness
Melhorar tratamento de erros
Implementar log - se opensearch ou cloudwatch -> console.writeline | loggin estruturado
Melhorias nos testes unitarios
Adicionar migração para o banco de dados (EF core)
Isolar endpoints de relatorio

**Thanks**
