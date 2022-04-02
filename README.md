# Overview
Este projeto é a resolução de um desafio realizado para uma vaga de desenvolvedor. O intuito deste desafio é avaliar os conhecimentos técnicos em programação.

# Instruções para execução do projeto via Docker Compose
É necessário ter o [Docker](https://www.docker.com/get-started/) instalado na sua máquina juntamente com o [Docker Compose](https://docs.docker.com/compose/install/). Tenha certeza que tenha ambos instalados, configurados e em execução na sua máquina. O projeto é composto por três componentes principais:

1. Banco de Dados (SQL Server)
2. API de Serviços
3. Front-End

Cada um destes componentes é representado por um container na stack. Todos eles serão criados e configurados automaticamente, não tem a necessidade de nenhum comando manual. Inclusive o database utilizado no projeto também será criado e configurado de forma automática.

## Clone o repositório
Primeiro, crie uma pasta em seu computador local e, em seguida, entre nela e execute o comando git clone do projeto conforme abaixo:
```
git clone https://github.com/LucasScheid/desafio-dev.git
```

## Entre na pasta do projeto
Ao finalizar com sucesso o comando git clone entre na pasta `desafio-dev`:
```
cd desafio-dev
```

## Executando a projeto via Docker Compose
Tenha certeza que o Docker está em execução na sua máquina local e execute o comando abaixo dentro da pasta `desafio-dev`. Pode ser utilizado qualquer terminal para esta tarefa (Power Shell, Git Bash, Windows Terminal, etc):
```
docker compose up -d
```


## Verificando o status dos containers
Após o término da execução do `docker compose up -d` verifique o status dos containers com o comando abaixo:
```
docker compose ps -a
```

## Acessando o Front-End
Após a finalização do comando para execução completa de todos os componentes do projeto, o acesso ao front pode ser feito no endereço abaixo.
```
http://localhost:8060/
```

O front disponibiliza cinco funcionalidades:

1. Upload: Permite escolher um arquivo para realizar a importação.
2. Lojas: Exibe as movimentações da loja selecionada.
3. Geral: Exibe todos os registros carregados no database via upload.
4. Tipos de Transação: Exibe as informações sobre os tipos de transação.
5. Status Database: Permite verificar se a infraestrutura de banco de dados está pronta para utilização.

## Atenção, ponto Importante!!!
O banco de dados leva em média 100 segundos para estar totalmente pronto para utilização com as devidas tabelas criadas. Na maioria das vezes o front-end da aplicação já encontra-se disponível mas o banco ainda não. Utilize a funcionalidade disponível no endereço abaixo para verificar se o banco já está 100% pronto para utilização. Caso estiver tudo certo, pode começar a utilização das demais funcionalidades do front-end.
```
http://localhost:8060/Consulta/StatusDatabaseIndex
```

## Acessando a API (Back-End)
O acesso a API pode ser feito no endereço abaixo. Ela possui os métodos documentados na própria interface do [Swagger](https://swagger.io/).
```
http://localhost:8050/swagger/index.html
```
A API possui autenticação, o Token pode ser obtido através da realização de um POST no endereço abaixo na própria interface do Swagger:

```
http://localhost:8050/Login
```
Para o payload, utilize o JSON abaixo
```
{
  "usuario": "api-cnab-user",
  "senha": "VN403HYdpzbDtfphmBeU"
}
```

## Acessando o Banco de Dados (SQL Server)
Caso necessário alguma verificação, o banco de dados SQL Server está disponível para ser acessado através do endereço abaixo. A porta utilizada é a padrão 1433.
```
localhost
```

Segue abaixo o usuário, senha e database:

```
user: sa
password: z9CzyUwTe3NAkjX
database: dbcnab
```

## Teste imagem
![alt text](https://super.abril.com.br/wp-content/uploads/2019/06/site_temponatureza.png)

# Referência

Este desafio foi criado a partir das seguintes instruções: https://github.com/ByCodersTec/desafio-dev

---

Obrigado pela oportunidade!
