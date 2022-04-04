# :grinning: Overview :rocket:
Este projeto é a resolução de um desafio realizado para uma vaga de desenvolvedor. O objetivo deste desafio é avaliar os conhecimentos técnicos em programação.

# Tecnologias Utilizadas

* [C# .NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [Bootstrap](https://getbootstrap.com/)
* [JavaScript](https://www.javascript.com/)
* [HTML](https://www.w3schools.com/html/)
* [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
* [TSQL](https://docs.microsoft.com/pt-br/sql/t-sql/language-reference?view=sql-server-ver15)
* [Docker](https://docs.docker.com/engine/reference/builder/)
* [JWT](https://jwt.io/)
* [Swagger](https://swagger.io/)
* [Moq](https://github.com/moq/moq4)
* [Xunit](https://xunit.net/)

# Instruções
É necessário ter o [Docker](https://www.docker.com/get-started/) instalado na sua máquina juntamente com o [Docker Compose](https://docs.docker.com/compose/install/). Tenha certeza de ter ambos instalados, configurados e em execução na sua máquina. O projeto é composto por três componentes principais:

1. Banco de Dados (SQL Server)
2. API de Serviços
3. Front-End

Cada um destes componentes é representado por um container. Todos estes containers serão criados e configurados automaticamente, não existe a necessidade de nenhum comando manual a não ser o próprio `docker compose up -d`. Inclusive o database utilizado no projeto também será criado e configurado de forma automática.

## Clone o repositório
Primeiro, crie uma pasta em seu computador local e, em seguida, entre nela e execute o comando `git clone` do projeto conforme abaixo. Pode ser utilizado qualquer terminal a sua escolha para esta tarefa ([Power Shell](https://docs.microsoft.com/pt-br/powershell/scripting/overview?view=powershell-7.2), [Git Bash](https://git-scm.com/downloads), [Windows Terminal](https://www.microsoft.com/pt-br/p/windows-terminal/9n0dx20hk701?activetab=pivot:overviewtab), etc):
```
git clone https://github.com/LucasScheid/desafio-dev.git
```

## Entre na pasta do projeto
Ao finalizar com sucesso o comando `git clone` entre na pasta `desafio-dev` com o comando abaixo:
```
cd desafio-dev
```

## Estrutura de pastas do projeto
Após entrar na pasta `desafio-dev` é possível observar a estrutura de pastas e arquivos. o arquivo `docker-compose.yml` possui toda a configuração da infraestrutura da aplicação. A pasta `infra-db` possui internamente todos os arquivos de configuração do banco SQL Server e também o arquivo `create-database.sql` que é executado de forma automática durante a execução do Sql Server. Dentro da pasta `cnab-api` estão os projetos criados e a respectiva solution (`cnab-api.sln`) caso você queira abrir na sua IDE.

## Executando o projeto
Tenha certeza que o Docker está em execução na sua máquina local e execute o comando abaixo dentro da pasta `desafio-dev` e aguarde a finalização. Aproveite para tomar um :coffee: pois até baixar todas as imagens e executar o projeto pode demorar alguns minutos. Essa demora é normal na primeira execução pois todas as imagens serão baixadas na sua máquina local (caso você ainda não tiver elas baixadas). As próximas execuções serão mais rápidas pois as imagens já estarão baixadas na sua máquina local.
```
docker compose up -d
```

Caso necessário, use o comando abaixo para parar a execução do projeto.
```
docker compose down
```

## Verificando o status dos containers
Após o término da execução do comando `docker compose up -d` verifique o status dos containers com a execução do comando abaixo no seu terminal:
```
docker compose ps -a
```
![alt text](https://github.com/LucasScheid/desafio-dev/blob/main/imagens-doc/docker-ps-a.png)

A consulta pode ser feita através da interface gráfica do [Docker Desktop](https://www.docker.com/products/docker-desktop/):

![alt text](https://github.com/LucasScheid/desafio-dev/blob/main/imagens-doc/stack-up.png)

## Acessando o Front-End
Após a finalização do comando `docker compose up -d`, o acesso ao front pode ser feito no endereço abaixo:
```
http://localhost:8060/
```

O front disponibiliza cinco funcionalidades:

1. Upload: Permite escolher um arquivo para realizar a importação.
2. Lojas: Exibe as movimentações da loja selecionada.
3. Geral: Exibe todos os registros carregados no database via upload.
4. Tipos de Transação: Exibe as informações sobre os tipos de transação.
5. Status Database: Permite verificar se a infraestrutura de banco de dados está pronta para utilização.

![alt text](https://github.com/LucasScheid/desafio-dev/blob/main/imagens-doc/front-end.png)

Segue abaixo a funcionalidade de Upload.

![alt text](https://github.com/LucasScheid/desafio-dev/blob/main/imagens-doc/funcionalidade-upload.png)

Para testar a funcionalidade de upload utilize o arquivo [CNAB.txt](https://github.com/LucasScheid/desafio-dev/blob/main/CNAB.txt)

## :warning: Atenção, ponto Importante!!! :warning:
A infraestrutura de banco de dados utilizada neste projeto, leva em média 100 segundos para estar totalmente pronta para utilização com as devidas tabelas criadas. Na maioria das vezes o front-end da aplicação já encontra-se disponível mas o banco ainda não. Utilize a funcionalidade disponível no front end (endereço abaixo) para verificar se o banco já está 100% pronto para utilização. Caso estiver tudo certo, pode começar a utilização das demais funcionalidades do front-end.
```
http://localhost:8060/Consulta/StatusDatabaseIndex
```

Abaixo um exemplo quando está 100% pronto.

![alt text](https://github.com/LucasScheid/desafio-dev/blob/main/imagens-doc/consulta-status-banco-ok.png)

Abaixo um exemplo quando ainda está em execução, portanto é necessário aguardar.

![alt text](https://github.com/LucasScheid/desafio-dev/blob/main/imagens-doc/consulta-status-banco-nao-ok.png)

## Acessando a API (Back-End)
O acesso a API pode ser feito no endereço abaixo. Ela possui os métodos documentados na própria interface do [Swagger](https://swagger.io/).
```
http://localhost:8050/swagger/index.html
```
A API possui autenticação, o Token pode ser obtido através da realização de um POST no endereço abaixo na própria interface do Swagger:

```
http://localhost:8050/Login
```
Para o payload, utilize o JSON abaixo:
```
{
  "usuario": "api-cnab-user",
  "senha": "VN403HYdpzbDtfphmBeU"
}
```
![alt text](https://github.com/LucasScheid/desafio-dev/blob/main/imagens-doc/api-swagger.png)

## Acessando o Banco de Dados (SQL Server)
Caso necessário, o banco de dados SQL Server está disponível para ser acessado através do endereço abaixo. A porta utilizada é a padrão 1433.
```
localhost
```

Segue abaixo o usuário, senha e database:

```
user: sa
password: z9CzyUwTe3NAkjX
database: dbcnab
```

Segue abaixo as duas tabelas do projeto **arquivo** e **tipo_transacao** com a sua respectiva estrutura:
![alt text](https://github.com/LucasScheid/desafio-dev/blob/main/imagens-doc/tabelas-banco.png)

## Executando os testes de unidade
Para executar os testes, você vai precisar da CLI do .NET, caso não tiver [neste link](https://docs.microsoft.com/pt-br/dotnet/core/tools/) tem as instruções para a instalação. Uma outra opção seria abrir a solution via [Visual Studio](https://visualstudio.microsoft.com/pt-br/vs/community/) e executar o projeto de testes. A partir da pasta raiza `desafio-dev` execute o seguinte comando para entrar na pasta para execução dos testes de unidade: 
```
cd cnab-api\cnab-unit-tests
```

Após entrar nesta pasta, execute o comando abaixo:
```
dotnet test
```
Para obter mais detalhes sobre os testes, o mesmo comando pode ser usado com as seguintes opções:
```
dotnet test -l "console;verbosity=normal"
```

# Referência

Este desafio foi criado a partir das seguintes instruções: https://github.com/ByCodersTec/desafio-dev

---

Espero que gostem, obrigado pela oportunidade! :pray: :raised_hands:
