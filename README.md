<h1 align="center">Makeup API</h1>

<p align="center">
    <a>
        <img  alt="Linguagens" src="https://img.shields.io/github/languages/count/guilhermepalma/makeup_api?style=flat-square&label=Linguagens"/>
    </a>
    <a>
        <img  alt="Licensa" src="https://img.shields.io/github/license/guilhermepalma/makeup_api?style=flat-square&label=Licensa"/>
    </a>
    <a>
        <img  alt="Issues Abertas" src="https://img.shields.io/github/issues/guilhermepalma/makeup_api?style=flat-square&label=Issues"/>
    </a>
    <a>
        <img  alt="Pull Request Aprovados" src="https://img.shields.io/github/issues-pr-closed/guilhermepalma/makeup_api?color=green&style=flat-square&label=Pull%20Request"/>
    </a>
    <a>
        <img  alt="Ultimo Commit" src="https://img.shields.io/github/last-commit/guilhermepalma/makeup_api?style=flat-square&label=Ultimo%20Commit"/>
    </a>
    <a>
        <img  alt="Pull Request Aprovados" src="https://img.shields.io/github/repo-size/guilhermepalma/makeup_api?style=flat-square&label=Repo%20Size"/>
    </a>
    <a>
        <img  alt="Plataforma Suportada" src="https://img.shields.io/badge/Suported-Windows-blue?style=flat-square"/>
    </a>
</p>

<p align="center">🚧 Em <b>Desenvolvimento</b> 🚧</p>


# Conteudos
- [Conteudos](#conteudos)
  - [Sobre o Projeto](#sobre-o-projeto)
  - [Funcionalidades](#funcionalidades)
    - [Versão 0.1 (Atual)](#versão-01-flushed-atual)
    - [Versão 1.0](#versão-10-nerd_face)
  - [Documentação API (Versão 0.1)](#documentação-api-versão-01)
    - [Metodos HTTP](#metodos-http)
    - [Formação da URL](#formação-da-url)
    - [Variaveis do Usuario](#variaveis-do-usuario)
    - [Endpoints do Usuario](#endpoints-do-usuario)
  - [Como Executar ?](#como-executar-)
  - [Tecnologias](#tecnologias)
  - [Como Contribuir ?](#como-contribuir-)
  - [Contribuidores](#contribuidores)
  - [Referencias](#referencias)
---

## Sobre o Projeto
#### API desenvolvida na Linguagem ASP.NET FRAMEWORK - Será utilizada na Obtenção e Armazenamento de Dados de Usuario do [Projeto Makep](https://github.com/guilhermepalma/makeup).

Essa API somente pode ser executada no **[Visual Studio](https://visualstudio.microsoft.com/pt-br/)** e no Sistema Operacional **Windows**. Por conta disso, sua hospedagem online é mais dificil, uma vez que a maioria dos servidores gratuitos não possuem ambientes configurados para o **Docker Windows** e nem para a **Linguagem .NET Framework**.

Este projeto foi desenvolvido a partir dos conhecimentos na linguagem .NET FRAMEWORK e dos **Videos 1 e 2** citados na referencia.


## Funcionalidades

<!-- todo: Alterar ao mudar para a Versão 1.0 -->
### Versão 0.1 :flushed: (Atual)
Nessa Versão já é possivel consumir e utilizar a API (Consulte: [Como Executar ?](#como-executar-)).

As Funcionalidades desenvolvidas são:
- Usuario
  - [X] CRUD (Create, Read, Update e Delete)
  - [X] Geração do Token do Usuario
  - [X] Validação do Login (Email e Senha + Token)
- Maquiagens
  - [X] CRUD (Create, Read, Update e Delete)
  - [X] Classes e Tabelas normalização da Marca e Tipo da Maquiagem
  - [X] Favoritar e Desfavoritar uma Maquiagem
  - [X] Exclusão de Registros (Marca e Tipo) não Utilizados após uma Exclusão/Atualização de uma Maquiagem


### Versão 1.0 :nerd_face:
O objetivo da Versão 1.0 é lançar a API conforme o planejado e estipulado na Analise de Requisitos da API, inserindo novos endpoints e metodos, fechando Issues abertas e trazendo novas melhorias ao software já desenvolvido

As seguintes Funcionalidades que serão desenvolvidas:
- Interface/Layout
  - [ ] Criação de Telas da Documentação na API
  - [ ] Criação de Exemplos da API
- Localização do Usuario
  - [ ] CRUD (Create, Read, Update e Delete)
  - [ ] Analise das Localizações Corretas X Erradas
- Encerramento das [Issues Abertas](https://github.com/GuilhermePalma/Makeup_API/issues?q=created%3A<2021-10-01+type%3Aissue+)
- Disponibilização em algum serviço de Hospedagem

## Documentação API (Versão 0.1)

<!-- todo: alterar ao mudar a versão -->
#### A documentação abaixo contem os Metodos HTTP, Endpoints, Variaveis e Respostas disponiveis na **Versão 0.1** (Versão Atual da API Desenvolvida no branch [main](https://github.com/GuilhermePalma/Makeup_API/tree/main))

Obs: Os demais Branchs (ex: [develop](https://github.com/GuilhermePalma/Makeup_API/tree/develop)) podem estar em **Versões Diferentes e Instáveis**

### Metodos HTTP
|Metodo|Descrição|
|------|---------|
| ```GET``` | Obtem as Informação com um **Parametro** inserido em uma Query na **URL** |
| ```POST``` | Insere e/ou Obtem Dados a partir de **parametros** passados pelo **body** (corpo) da Solicitação |
| ```PUT``` | **Atualiza** Dados (como Usuarios, Maquiagens) no Banco de Dados |
| ```DELETE``` | **Exclui** Dados (como Usuarios, Maquiagens) no Banco de Dados |

### Formação da URL
|Parametros|Descrição|
|----------|---------|
|```localhost:porta/api/controller/action_name```| URL Padrão: A partir da Substituição da substituição do ```controller``` e ```action_name``` é possivel **utilizar** a API |
|```localhost:porta```| (Parte Visual não desenvolvida) em que o ISS Express Executa a Aplicação |
|```/api```| Permite que **utilize da API**, caso contrario, mostraria as Views (Parte Visual não Desenvolvida)|
|```/controller```| Será substituido pelo **nome da Classe** que deseja os Dados (disponiveis: "**User**" e "**Favorites**") |
|```/action_name```| Será Substituido pelo **nome do metodo** que será executado |
| | |
|```localhost:porta/api/user/action_name```|URL para acessar o Controller dos **Usuario**|
|```localhost:porta/api/favorites/action_name```|URL para acessar o Controller das **Favoritadas**|

### Variaveis do Usuario

  |Variavel|Descrição|Valores|Tamanho Minimo|Tamanho Maximo|Espaço em Branco|
  |--------|---------|-------|--------------|--------------|----------------|
  | ```string Name``` | **Nome e Sobrenome** do Usuario | texto | 3 Caracteres | 120 Caracteres | :heavy_check_mark: |
  | ```string Email``` | **Email** do Usuario | texto | 8 Caracteres | 150 Caracteres | :x: |
  | ```string Password``` | **Senha** do Usuario | texto | 3 Caracteres | 40 Caracteres | :x: |
  | ```int Id``` | Numero de **Identificação** do Usuario | numero | 1 | 2147483647 | :x: |
  | ```string Nickname``` | **Nome de Usuario** (ex: @nome_usuario) | texto | 3 Caracteres | 40 Caracteres | :x: |
  | ```string Idioms``` | **Idioma** que o Aplicativo está disponivel | "Portugues", "Ingles", "Espanhol" e "Frances" |--------------|--------------| :x: |
  | ```boolean Theme_is_night``` | Define se o Tema do Usuario está salvo como **Dark ou Light Theme** | true, false |--------------|--------------| :x: |

### Endpoints do Usuario

<details>
  <summary><samp>&#9776; POST: Geração do JWT (JSON Web Token)</samp></summary>

  - Gera um Token (JWT) para o Usuario usar na Autenticação nas Etapas Seguintes. O Token contém:
    - ```Name``` e ```Nickname``` do Usuario
    - Prazo de expiração (5 Dias)
    - Local do Repositorio do Emissor ([Makeup API](https://github.com/guilhermepalma/makeup_api)) e Receptor ([Makeup - Android](https://github.com/guilhermepalma/makeup))
    - Criptografado cima de uma chave HMAC autogerada e armazenada no Servidor (ISS Express)

  - Request (**POST**)
    - URL ```localhost:porta/api/user/GenerationTokenUser```
    - Header
      ```
          "Content-Type": "application/json"
      ```
    - Body
      ```
          {
              "Email": "emailtest@gmeil.com",
              "Password": "accountforteste"
          }
      ```

  <!-- todo: adicionar um token avalido -->
  - Response 200 (application/json)
    - Body → ```string token``` (Token Abaixo não é Valido, sendo somente um Exemplo)
      ```
          {
              "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"
          }
      ```
  - Retornos de Erro:
    - Response 404
      - Body → Mensagem de Erro da API (Validação, Dados Inexistentes, Erro na Conexão, etc)
      - Ex: Senha com 2 Caracteres
        ```
            {
                "Senha deve ter no Minimo 3 Caracteres"
            }
        ```

</details>

<details>
  <summary><samp>&#9776; POST: Login do Usuario</samp></summary>

  - A partir do Email, Senha e JWT (JSON Web Token) verifica se o Usuario está cadastrado

  - Request (**POST**)
    - URL ```localhost:porta/api/user/Login```
    - Header (Token Abaixo não é Valido, sendo somente um Exemplo)
      - Substituir o campo ```this.token.user``` pelo Token (JWT) do Usuario
      ```
          "Content-Type": "application/json"
          "Authorization": "Bearer this.token.user"
      ```
    - Body
      ```
          {
              "Email": "emailtest@gmeil.com",
              "Password": "accountforteste"
          }
      ```

  - Response 200 (application/json)
    - Body → true (Usuario Validado)
      ```
          {
              true
          }
      ```
  - Possiveis Retorno de Erro:
    - Response 201
      - Body → Erro em alguma etapa da Autenticação do Token (JWT)
      - Ex: Não Incluir o "Bearer" no "Authorization"
       ```
        {
            "Esquema de Autenticação Invalido"
        }
       ```
    - Response 404
      - Body → Mensagem de Erro da API (Validação, Dados Inexistentes, Erro na Conexão, etc)
      - Ex: Email não preenchido
       ```
        {
            "Campo Email Obrigatorio."
        }
       ```

</details>

<details>
  <summary><samp>&#9776; GET: Informações do Usuario</samp></summary>

  - Obtem o Usuario (sem a Senha) a partir do seu ID e Token (JWT)

  - Request (**GET**)
    - URL ```localhost:porta/api/user/Information```
    - Header (Token Abaixo não é Valido, sendo somente um Exemplo)
      - Substituir o campo ```this.token.user``` pelo Token (JWT) do Usuario
      ```
          "Content-Type": "application/json"
          "Authorization": "Bearer this.token.user"
      ```
    - Parametros GET
      ```
          {
              "Id": "4",
          }
      ```

  - Response 200 (application/json)
    - Body → Usuario
      ```
        {
              "Id": "4",
              "Name": "Name Test",
              "Email": "emailtest@gmeil.com",
              "Password": null,
              "Nickname": "user_test",
              "Idioms": "Portugues",
              "Theme_is_night": false
        }
      ```
  - Possiveis Retorno de Erro:
    - Response 201
      - Body → Erro em alguma etapa da Autenticação do Token (JWT)
      - Ex: Token (JWT) não Informado
       ```
        {
            "Token não Encontrado"
        }
       ```

    - Response 404
      - Body → Mensagem de Erro da API (Validação, Dados Inexistentes, Erro na Conexão, etc)
      - Ex: Usuario não Cadastrado
       ```
        {
            "Usuario não Encontrado no Banco de Dados"
        }
       ```

</details>

<details>
  <summary><samp>&#9776; POST: Registrando um Usuario</samp></summary>

  - Insere um Usuario no Banco de Dados e Gera um Token

  - Request (**POST**)
    - URL ```localhost:porta/api/user/Register```
    - Header
      ```
          "Content-Type": "application/json"
      ```
    - Body → Instancia de um Usuario
      ```
          {
              "Name": "Name Test",
              "Email": "emailtest@gmeil.com",
              "Password": "accountforteste",
              "Nickname": "user_test",
              "Idioms": "Portugues",
              "Theme_is_night": false
          }
      ```

  - Response 200 (application/json)
    - Body → Token Gerado (JWT)
      ```
        {
              "token.for.user"
        }
      ```
  - Possiveis Retorno de Erro:
    - Response 404
      - Body → Mensagem de Erro da API (Validação, Dados Inexistentes, Erro na Conexão, etc)
      - Ex: Usuario com Idioma não Disponivel
       ```
        {
            "Idioma não Cadastrado"
        }
       ```

</details>


#### Documentação dos 7 Metodos Restantes em Desenvolvimento :zzz:
<!-- Adicionar Endpoints abaixo

User:
  // PUT+ JWT: api/user/Update -- Atualiza Senha,Idioma e Tema
  Parametros: Id, Nome, Nickname, Senha, Idioma, Tema
  Retorno:
      - Erro = 404 ou 201 ? (Not Found ou Unathorized)
      - true

  // PUT + JWT: api/user/UpdateNickname -- Atualiza somente o Nickname do Usuario
  Parametros: Email, Senha, ID e Nickname
  Retorno:
      - Erro = 404 ou 201 ? (Not Found ou Unathorized)
      - JWT (string)

  // POST + JWT: api/user/UpdateEmail -- Atualiza somente o Email do Usuario
  Parametros: Email, Senha e ID
  Retorno:
      - Erro = 404 ou 201 ? (Not Found ou Unathorized)
      - true

  // DELETE + JWT:  api/User/Delete --
  Parametros: Email, Senha e ID
  Retorno:
      - Erro = 404 ou 201 ? (Not Found ou Unathorized)
      - true


Favoritas:
  // POST + JWT: api/favorites/newfavorite -- Adicionar Favorito
  Parametros: Nome, Marca e Tipo (Makeup) + Email, Senha
  Retorno:
      - Erro = 404 ou 201 ? (Not Found ou Unathorized)
      - Instancia de 1 Makeup
          (string name, string type, string brand, int id_makeup, int id_type, int id_brand, string Error_validation)

  // POST + JWT: api/favorites/RemoveFavorite -- Remove um Favorito do Usuario
  Parametros: Nome, Marca e Tipo (Makeup) + Email, Senha
  Retorno:
      - Erro = 404 ou 201 ? (Not Found ou Unathorized)
      - True

  // POST + JWT: api/favorites/ListFavorites -- Lista de Favoritos de um Usuario
  Parametros: Email e Senha
  Retorno:
      - Erro = 404 ou 201 ? (Not Found ou Unathorized)
      - List<Makeup>
          (string name, string type, string brand, int id_makeup, int id_type, int id_brand, string Error_validation)
 -->

## Como Executar ?

#### A API tem que ser configurada (ISS Express) para que seja possivel acessa-la na rede Local e sem ocorrer problemas de permissões.
#### Este topico ainda está sendo aprimorado e desenvolvido. Por favor, aguarde :disappointed_relieved:


## Tecnologias

#### As Tecnologias e Ferramentas Presentes no desenvolvimento do Projeto são:
- [Git - Versionamento do Codigo](https://git-scm.com/downloads)
- [GitHub - Hospedagem do Codigo-Fonte](https://github.com/guilhermepalma/makeup_api)
- [Visual Studio - Ambiente de Desenvolvimento (IDE)](https://visualstudio.microsoft.com/pt-br/)
- [Visual Studio Code (IDE)](https://code.visualstudio.com/download)- [Markdown All in One](https://github.com/yzhang-gh/vscode-markdown)
- [Swagger Inspector - Testes na API](https://swagger.io/tools/swagger-inspector/)
- [WakaTime - Marcador de Tempo](https://wakatime.com/projects/makeup_api)


## Como Contribuir ?
#### Sua Contribuição nesse Projeto é Bem-Vinda :blush: !!! Caso tenha novas implementações ou sugestões, sinta-se a vontade de abrir **Issues**, **Pull Requests** ou até mesmo **contate-me**

Esse repositorio está disponibilizado sob a [Licensa MIT](LICENSE)-[Restrições](http://escolhaumalicenca.com.br/licencas/mit/), ou seja, um Projeto Open-Source !

Caso queira contribuir com o Projeto, siga os Passos Abaixo:
- 1- Crie um **Fork** desse Repositorio
- 2- **Clone** seu repositorio Fork
- 3- Crie um **novo branch** para Alterações (ex: ```git checkout -b feature/change_something```)
- 4- Faça suas alterações e envie-as para seu repositorio (**push**)
  - Busque deixar **comentarios** em Ingles ou Portugues para que os demais entendam o que foi alterado
- 5- Ao concluir suas alterações, inicie um **Pull Request**
  - a. Faça uma **descrição** detalhada das suas alterações e mudanças
  - b. Crie um **Checklist** (Utilizando "\[X]") das alterações Desenvolvidas
  - c. Insira Labels na sua Issue
- 6- Aguarde a sua **Pull Request** ser Aprovada :wink:

## Contribuidores

<div align="center" style="text-align:center; align-items: center;">

|       |
|-------|
|<a href="https://github.com/guilhermepalma"><img style="border-radius: 50%;" src="https://avatars.githubusercontent.com/u/54846154?v=4" width="200" alt="Guilherme Palma"/></a> |
|<a href="https://github.com/guilhermepalma" ><b>Guilherme Palma</b></a><br/> <p>:desktop_computer: Desenvolverdor</p>|

</div>


## Referencias
- [1 - JSON Web Token - Chave Assimetrica](https://www.brunobrito.net.br/jwt-com-chave-assimetrica/)
- [2 - JSON Web Token - Assinatura HMAC](https://www.brunobrito.net.br/jwt-assinaura-digital-rsa-ecdsa-hmac/)
- [3 - Permissão de Escrita e Leitura no IIS Express - JWT](https://stackoverflow.com/a/11856540)
- [4 - Tutorial da Criação do Json Web Token](https://www.youtube.com/watch?v=AUbGk5Ab40A)
- [5 - Utilização do JObject URL](http://www.macoratti.net/19/06/mvc5_webapimp1.htm)