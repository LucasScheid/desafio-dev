# :grinning: Overview :rocket:
Este projeto é a resolução de um desafio realizado para uma vaga de desenvolvedor. O objetivo deste desafio é avaliar os conhecimentos técnicos em programação.

# Instruções
É necessário ter o [Docker](https://www.docker.com/get-started/) instalado na sua máquina juntamente com o [Docker Compose](https://docs.docker.com/compose/install/). Tenha certeza de ter ambos instalados, configurados e em execução na sua máquina. O projeto é composto por três componentes principais:

1. Banco de Dados (SQL Server)
2. API de Serviços
3. Front-End

Cada um destes componentes é representado por um container na stack. Todos eles serão criados e configurados automaticamente, não tem a necessidade de nenhum comando manual a não ser o próprio `docker compose up -d`. Inclusive o database utilizado no projeto também será criado e configurado de forma automática.

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

## Executando o projeto
Tenha certeza que o Docker está em execução na sua máquina local e execute o comando abaixo dentro da pasta `desafio-dev` e aguarde a finalização. Aproveite para tomar um :coffee: pois até baixar todas as imagens e executar o projeto pode demorar alguns minutos.
```
docker compose up -d
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

# Referência

Este desafio foi criado a partir das seguintes instruções: https://github.com/ByCodersTec/desafio-dev

---

Espero que gostem, obrigado pela oportunidade! :pray: :raised_hands:
