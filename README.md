# Bank Microservices

Este proyecto implementa una arquitectura de **microservicios bancarios basada en eventos** utilizando el patrón **Saga** para garantizar la consistencia en transacciones distribuidas.  
El objetivo principal es aprender y practicar la comunicación entre microservicios, el uso de mensajería asincrónica, la compensación de operaciones y el despliegue de infraestructura en la nube de **Microsoft Azure** mediante **Terraform**.

---

## 🧩 Tecnologías principales

> `C#` `.NET 8` `ASP.NET` `EntityFrameworkCore` `MediatR` `Background Service` `Secrets` `Parallel Programming` `Async`  
> `Terraform` `Azure SQL Database` `Azure Service Bus` `Azure Function` `Azure App Service`  
> `Azure CosmosDB` `Azure Key Vault` `Azure Insights` `Azure Storage Account` `Azure Cost Management`

---

## 🏗️ Arquitectura de la solución

La solución está compuesta por múltiples microservicios independientes que se comunican entre sí mediante eventos publicados en **Azure Service Bus**.  
Cada microservicio gestiona su propio dominio y base de datos, aplicando los principios de **Domain-Driven Design (DDD)** y **CQRS (Command Query Responsibility Segregation)**.

La arquitectura implementa:
- Comunicación asincrónica basada en eventos.  
- Coordinación de transacciones distribuidas mediante el patrón **Saga**.  
- Integración de un **API Gateway** para el enrutamiento y autenticación de peticiones.  
- Persistencia independiente para cada microservicio con **Azure SQL Database** y **CosmosDB**.  
- Observabilidad centralizada utilizando **Application Insights**.

![Arquitectura de Microservicios](bank-microservices-diagram-infra.svg)

---

## 📦 Microservicios incluidos

1. **API Gateway** – Enrutamiento de solicitudes y autenticación.  
2. **Transaction Service** – Procesa las operaciones de transacción.  
3. **Balance Service** – Verifica y actualiza el saldo de las cuentas.  
4. **Transfer Service** – Coordina las transferencias entre cuentas.  
5. **Notification Service** – Envía notificaciones basadas en los eventos del sistema.  

Cada microservicio cuenta con su propia base de datos y configuración de conexión.

---

## ⚙️ Variables de entorno y secretos

Estas variables deben configurarse como **secretos del proyecto** (por ejemplo, en Azure App Service o Azure Key Vault):

```bash
SERVICE_BUS_CONNECTION_STRING=
SERVICE_BUS_TOPIC=

MS_TRANSACTION_SQL_CONNECTION_STRING=
MS_BALANCE_SQL_CONNECTION_STRING=
MS_TRANSFER_SQL_CONNECTION_STRING=

NOTIFICATION_COSMOSDB_CONENECTION_STRING=
NOTIFICATION_COSMOSDB_DATABASE_NAME=
NOTIFICATION_COSMOSDB_CONTAINER_NAME=
```

### Eventos y tópicos utilizados

**Tópico:** `bank-transfer`

**Eventos:**
- `transaction-initiated`, `transaction-completed`, `transaction-failed`
- `balance-initiated`, `balance-confirmed`, `balance-failed`
- `transfer-initiated`, `transfer-confirmed`, `transfer-failed`
- `transfer-confirmed-balance`, `transfer-failed-balance`

---

## ☁️ Infraestructura como código

La infraestructura necesaria para ejecutar los microservicios en Azure se define mediante **Terraform**, permitiendo automatizar la creación y configuración de recursos como bases de datos, Service Bus, CosmosDB, y App Services.

Repositorio de IaC (Terraform):  
👉 [bank-azure-infra-terraform](https://github.com/leo-marqz/bank-azure-infra-terraform.git)

---

## 🔗 Repositorios del proyecto

- **Proyecto principal (.NET):**  
  [bank-microservices-net](https://github.com/leo-marqz/bank-microservices-net.git)

- **Infraestructura (Terraform):**  
  [bank-azure-infra-terraform](https://github.com/leo-marqz/bank-azure-infra-terraform.git)

---

## 📘 Propósito educativo

Este proyecto fue desarrollado con fines de **aprendizaje y experimentación**, abordando conceptos clave como:
- Arquitectura de microservicios basada en eventos.  
- Patrón **Saga** para transacciones distribuidas.  
- Diseño desacoplado, resiliente y escalable en la nube.  
- Seguridad con **Microsoft Entra ID** y manejo de secretos.  
- Observabilidad con telemetría y logs centralizados.

---
