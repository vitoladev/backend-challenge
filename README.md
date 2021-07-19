# BackendChallenge

### Tecnologias utilizadas
Utilizei F# com o framework giraffe por ele facilitar o desenvolvimento em um estilo funcional e ser simples de estruturar a aplicação.

O MongoDB foi utilizado como banco de dados pela sua excelente integração com o ecossistema do .NET, estruturando os dados em forma de documentos dentro de coleções.

### Rodando a aplicação
Basta copiar o .env, setando a connection string do mongodb e logo depois rodar o docker-compose
```bash
cp .env.example .env
docker-compose up
```

## Rotas da aplicação

## Serviço de CMS
POST
```
http://localhost:8080/api/cms
```
O serviço de CMS irá receber as informações de uma receita e em seguida irá salva-lá no banco de dados. É possível utilizar nos ingredientes tanto o ID de um ingrediente que já existente, quanto as informações de um novo ingrediente.
Caso alguma propriedade não seja adicionada no payload, será retornado um erro 422 mostrando que falta aquela propriedade para salvar a receita no banco, fazendo com que os dados fiquem consistentes.

#### Exemplo de request
```curl
curl --location --request POST 'http://localhost:8080/api/cms' \
--data-raw '{
    "name": "A simple recipe",
    "description": "A simple description",
    "ingredients": [
        {
            "quantity": "one quarter",
            "unit": "kg",
            "food": "beef"
        },
        {
            "id": "-h_yXRSQ6kyzVzy4PgdSuQ"
        }
    ],
    "equipments": [
        {
            "tool": "knife",
            "quantity": 1
        }
    ]
}'
```
#### Resposta: 
```
{
  "id": "OHmGCfFeq0inlqVH4BXeQg",
  "name": "A simple recipe",
  "description": "A simple description",
  "ingredients": [
    {
      "id": "-h_yXRSQ6kyzVzy4PgdSuQ",
      "food": "some food",
      "quantity": "one quarter",
      "unit": "g"
    },
    {
      "id": "n-eqR0KZCUixMM5wAIzFuA",
      "food": "beef",
      "quantity": "one quarter",
      "unit": "kg"
    }
  ],
  "equipments": [
    {
      "tool": "knife",
      "quantity": 1
    }
  ]
}
```

## Serviço de Receita

### Buscar receitas salvas no banco de dados, limitando-se a um batch de 8 receitas por request (paginação)
GET
```
http://localhost:8080/api/recipes?page=1
```
#### Exemplo de request:
```
curl --location --request GET 'http://localhost:8080/api/recipes?page=1'
```
#### Exemplo de Resposta:
```json
{
  "data": [...],
  "totalPages": 4
}
```
### Buscar receitas que contenham o ID especificado do ingrediente
#### Exemplo de request:
```
curl --location --request GET 'http://localhost:8080/api/recipes/ingredient/-h_yXRSQ6kyzVzy4PgdSuQ'
```
#### Exemplo de resposta:
```json
[
  {
    "id": "40zdPx29BUG4F_SMJCxq7A",
    "name": "teste",
    "description": "uma descrição de exemplo",
    "ingredients": [
      {
        "id": "-h_yXRSQ6kyzVzy4PgdSuQ",
        "food": "some food",
        "quantity": "one quarter",
        "unit": "g"
      },
      {
        "id": "I3JPr_MkRUKtdV_hhZd2-g",
        "food": "chicken",
        "quantity": "10",
        "unit": "kg"
      }
    ],
    "equipments": [
      {
        "tool": "example",
        "quantity": 2
      }
    ]
  },
  {
    "id": "OHmGCfFeq0inlqVH4BXeQg",
    "name": "A simple recipe",
    "description": "A simple description",
    "ingredients": [
      {
        "id": "-h_yXRSQ6kyzVzy4PgdSuQ",
        "food": "some food",
        "quantity": "one quarter",
        "unit": "g"
      },
      {
        "id": "n-eqR0KZCUixMM5wAIzFuA",
        "food": "beef",
        "quantity": "one quarter",
        "unit": "kg"
      }
    ],
    "equipments": [
      {
        "tool": "knife",
        "quantity": 1
      }
    ]
  }
]
```
