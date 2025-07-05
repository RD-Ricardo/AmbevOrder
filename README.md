# 🛒 Order Service – Gestão de Pedidos com Integração Externa

Este projeto implementa um serviço de **gestão e cálculo de pedidos**

O serviço **Order** atua como um orquestrador de pedidos:

- 🔁 Recebe pedidos do **Produto Externo A** via API REST.
- 🧠 Realiza o **cálculo do valor total do pedido**, considerando o valor de cada produto multiplicado pela quantidade e somando o frete.
- 📤 Exponibiliza os **pedidos processados** para o **Produto Externo B** via uma API REST de consulta.

---

## 🧱 Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/en-us/)
- [Azure Service Bus](https://learn.microsoft.com/azure/service-bus-messaging/)
- [PostgreSQL](https://www.postgresql.org/)
- Repository Pattern
- API RESTful
- [AKS (Azure Kubernetes Service)](https://learn.microsoft.com/azure/aks/)
- [HELM](https://helm.sh/)
- [Docker](https://www.docker.com/)

### 📦 Exemplo de Requisição Criar Pedido

```bash
curl --location 'https://ambev-order-input.rdsolutions.digital/api/OrderInput/create' \
  --header 'Content-Type: application/json' \
  --data '{
    "customerName": "Ricardo Dias",
    "freightPrice": 25.50,
    "items": [
      {
        "productName": "Cerveja Skol Lata 350ml",
        "price": 3.50,
        "quantity": 12
      },
      {
        "productName": "Guaraná Antarctica 2L",
        "price": 6.90,
        "quantity": 3
      }
    ]
  }'
`````

Response: 
```bash
{
  "message": "Order created successfully",
  "orderId": "35fee97e-2125-46d0-8363-9846b60f7b05"
}
`````

### 📦 Exemplo de Requisição Visualisar Produtos Processados:
```bash
curl --location 'https://ambev-order-process.rdsolutions.digital/api/Order'
`````

Reponse:

```bash
[
    {
        "id": "7ee4a40d-df9c-4647-9170-9de9339deae8",
        "externId": "35fee97e-2125-46d0-8363-9846b60f7b05",
        "customerName": "Ricardo Dias",
        "freightPrice": 28.05,
        "processedAt": "2025-07-05T01:16:14.79509Z",
        "paidAt": "2025-07-05T01:16:14.79502Z",
        "createdAt": "2025-07-05T01:16:14.62636Z",
        "status": "Processed",
        "errorMessage": null,
        "totalPrice": 90.75,
        "items": [
            {
                "id": "b592e452-7d71-4301-a309-9ab3a25624dd",
                "productName": "Guaraná Antarctica 2L",
                "price": 6.90,
                "quantity": 3
            },
            {
                "id": "c795b48e-8b7c-4c08-a86a-8f57a2e1aaa6",
                "productName": "Cerveja Skol Lata 350ml",
                "price": 3.50,
                "quantity": 12
            }
        ]
    }
]
`````

### AKS

![image](https://github.com/user-attachments/assets/1f9f81d7-d429-43c6-90a6-d69f63fb260b)


