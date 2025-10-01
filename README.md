# TestShop Backend

A minimal **Shop API** built with **.NET 8**, showcasing:
- JWT authentication
- Product listing with pagination (infinite scroll ready)
- Shopping cart (add/update/remove, total calculation, persisted in DB)
- Post-login notification workflow (mock, persisted in DB)
- Cross-cutting concerns: centralized error handling, `Result<T>`, `ErrorResponse`, FluentValidation

---

## How to Run Locally

### 1. Database setup
The project uses **SQL Server** by default.

Update your `appsettings.json` with your connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TestShopDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 2. Apply migrations & seed data
```bash
cd TestShop.Api
dotnet ef migrations add InitialCreate --project ../TestShop.Infrastructure --startup-project .
dotnet ef database update --project ../TestShop.Infrastructure --startup-project .
```

This will create tables and seed:
- **10 demo products** (Laptop, Phone, Headphones, etc.)
- **Demo user**:  
  Username: `demo`  
  Password: `Pass123!`

### 3. Run the API
```bash
dotnet run --project TestShop.Api
```

Swagger UI will be available at:
```
https://localhost:5001/swagger
```

---

## Endpoints

- `POST /api/auth/login` – authenticate, returns JWT + user profile.  
- `GET /api/products?page=1&pageSize=12` – product listing (supports infinite scroll).  
- `GET /api/products/{id}` – product details.  
- `GET /api/cart` – get user cart (requires `Authorization: Bearer ...`).  
- `POST /api/cart` – add/update cart item (`{ productId, quantity }`).  
- `DELETE /api/cart/{productId}` – remove item.  
- `GET /api/notifications/latest` – fetch latest login notification.  

---

## Architecture Decisions & Trade-offs

- **Clean Architecture**:
  - `Domain` – entities (`User`, `Product`, `CartItem`, `Notification`).
  - `Application` – DTOs, interfaces, validators.
  - `Infrastructure` – EF Core, repositories, Auth & Notification services.
  - `Api` – Controllers, Middleware.
- **Error handling**: Centralized in `ExceptionHandlingMiddleware` → always structured JSON:
  ```json
  { "code": "UNAUTHORIZED", "message": "Invalid credentials" }
  ```
- **Result<T> & ErrorResponse**: unified success/error format across all endpoints.
- **Pagination**: `PagedResult<T>` ensures scalable infinite scroll support.
- **Cart persistence**: chosen **DB persistence** instead of `localStorage` → allows multi-device consistency and server-side truth.
- **Notifications**: implemented as DB records (Queued → Processed). Trade-off: mock only, but can easily evolve into SignalR, SendGrid, or message queues.

---

---

## Why this architecture?

**Goal:** make the code easy to understand, extend, and test if the system grows into many services.

- **Clean layering (Domain → Application → Infrastructure → Api)**  
  - *Domain* holds pure business entities with no framework dependencies → stable core.  
  - *Application* defines contracts (DTOs, interfaces, validators) → the API and Infra depend on it, not vice‑versa.  
  - *Infrastructure* implements details (EF Core, repositories, JWT, notification store) → swappable (e.g., MySQL, Dapper, queues) with minimal impact.  
  - *Api* is a thin HTTP shell (controllers, middleware) → easy to replace by gRPC or background workers in future.

- **Separation of concerns & SOLID**  
  Each layer has a single responsibility; dependencies point inward (dependency rule). This reduces coupling and clarifies ownership.

- **Cross‑cutting as reusable shared pieces**  
  `Result<T>`, `ErrorResponse`, `PagedResult<T>` and `ExceptionHandlingMiddleware` live outside feature code and are reused across endpoints. If we split into multiple services, these can be extracted into a shared package.

- **Testability by design**  
  Repositories and services are behind interfaces → unit tests can mock them; Infrastructure can be tested with EF InMemory; API can be tested with `WebApplicationFactory` without a real DB.

- **Scalability & evolvability**  
  - Replace EF Core with a different persistence layer without touching controllers.  
  - Swap DB‑persisted notifications with SignalR, SendGrid or a queue (RabbitMQ/ServiceBus) by changing only Infrastructure/Services.  
  - Add more bounded contexts (Orders, Payments) as new folders with their own DTOs/validators without changing shared contracts.

- **Consistency & DX**  
  Centralized error handling + unified response envelope (`Result<T>`) create predictable API behavior for clients and reviewers.


## Next Steps (with more time)

- Implement background `HostedService` to process notifications asynchronously.  
- Add refresh tokens & role-based authorization.  
- Extend unit & integration tests (especially for controllers with `WebApplicationFactory`).  
- Add OpenAPI annotations & Postman collection.  
- Implement a frontend with React (infinite scroll, cart persistence with localStorage + API sync).  

---

## Summary

This backend demonstrates:
- Proper layered architecture
- Centralized error handling
- Validation with FluentValidation
- Seeded demo data
- Test coverage (unit, integration, API)
