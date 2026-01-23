# DeveloperStore - Sales API

This project is an API prototype developed as part of a technical evaluation.

The solution implements a **complete CRUD for sales records**, applying **Clean Architecture**, **Domain-Driven Design (DDD)**, and the **Result Pattern** to ensure clear separation of concerns, business rule consistency, and predictable error handling.

---

## Architecture Overview

The project follows **Clean Architecture** principles:

```
src/
 |-- Api               -> Minimal APIs, endpoints, middleware
 |-- Application       -> Use cases (commands, queries), orchestration
 |-- Domain            -> Entities, aggregates, domain rules, domain events
 |-- Infrastructure    -> EF Core, PostgreSQL, repositories, persistence
 |-- SharedKernel      -> Result, Error, base abstractions
```

### Architectural Decisions

- **DDD (Domain-Driven Design)**  
  Business rules are enforced exclusively inside the Domain layer.

- **Result Pattern**  
  Expected business outcomes (success/failure) are modeled using `Result` and `Error`, avoiding business exceptions.

- **Minimal APIs**  
  Endpoints are thin and delegate all business logic to the Application layer.

- **Domain Events**  
  Domain events are raised by aggregates and dispatched after persistence.

- **Database**  
  PostgreSQL with Entity Framework Core.

- **Containerization**  
  Fully containerized using Docker and Docker Compose.

---

## Domain Model

### Sale

A sale contains:
- Sale number
- Sale date
- Customer (External Identity)
- Branch (External Identity)
- Items
- Total amount
- Cancelled / Not Cancelled status

### SaleItem

Each sale item includes:
- Product (External Identity)
- Quantity
- Unit price
- Discount
- Total amount
- Cancelled / Not Cancelled status

### Business Rules

- Purchases **above 4 identical items** -> **10% discount**
- Purchases **between 10 and 20 identical items** -> **20% discount**
- **Maximum 20 identical items** per product
- **No discount** for quantities below 4 items

All business rules are enforced in the **Domain layer** using the Result Pattern.

---

## Technology Stack

- **.NET 8**
- **C#**
- **ASP.NET Core Minimal APIs**
- **Entity Framework Core**
- **PostgreSQL**
- **Docker & Docker Compose**
- **Self Implementation of MeditR**
- **FluentValidation**
- **Serilog**

---

## Running the Application

### Prerequisites

- Docker
- Docker Compose

---

### Running with Docker Compose

From the repository root:

```bash
docker compose up --build
```

The API will be available at:

```
http://localhost:8080
```

---

## Database

- **Database:** PostgreSQL
- **Migrations:** Executed automatically on startup
- **Persistence:** Entity Framework Core

---

## API Endpoints (Examples)

### Create Sale

```http
POST /sales
```

**Response:**
- `201 Created` - Sale successfully created
- `400 Bad Request` - Business rule violation
- `409 Conflict` - Sale already exists

---

### Get Sale by Id

```http
GET /get/{id}
```

---

### Cancel Sale

```http
PUT /cancel
```

---

### Cancel Sale

```http
DELETE /sales/{id}
```

---

## Error Handling

The API uses:
- **Result Pattern** for business errors
- **Problem Details (RFC 7807)** for HTTP error responses

Unexpected errors are handled by a global exception middleware and mapped to `500 Internal Server Error`.

---

## Domain Events

The following domain events are raised and dispatched:

- `SaleCreatedDomainEvent`
- `SaleCancelledDomainEvent`
- `SaleItemCancelledDomainEvent`

Domain events are dispatched **after `SaveChanges`**, ensuring transactional consistency.  
No external message broker is used; events are logged internally.

---

## Testing

Tests can be executed with:

```bash
dotnet test
```

---

## Notes

- External entities (Customer, Branch, Product) are referenced using the **External Identity pattern**, avoiding direct coupling between bounded contexts.
- The project intentionally avoids overengineering to keep the solution focused and readable.

---

## Conclusion

This project demonstrates:
- Clean Architecture
- Proper application of DDD
- Consistent use of the Result Pattern
- Clear separation of concerns
- Predictable and maintainable API design

---
