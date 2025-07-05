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

### 📦 Exemplo de Requisição visualisar produtos processado:
```bash
curl --location 'https://ambev-order-process.rdsolutions.digital/api/Order'´
`````

Reponse:

```bash
[
  {
    "orderId": "35fee97e-2125-46d0-8363-9846b60f7b05",
    "customerName": "Ricardo Dias",
    "freightPrice": 25.50,
    "totalPrice": 81.30,
    "status": "Processed",
    "items": [
      {
        "productName": "Cerveja Skol Lata 350ml",
        "price": 3.50,
        "quantity": 12,
        "total": 42.00
      },
      {
        "productName": "Guaraná Antarctica 2L",
        "price": 6.90,
        "quantity": 3,
        "total": 20.70
      }
    ]
  }
]
`````

