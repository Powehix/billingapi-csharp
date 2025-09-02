# BillingAPI

A sample **Billing API** built with ASP.NET Core, demonstrating clean architecture, design patterns and JWT authentication.

---

## 🚀 Run the API
1. Install [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).
2. Clone the repository.
3. Set BillingApi as Startup project.
4. Run IIS Express.
5. Open Swagger UI:
   - https://localhost:xxxx/swagger

---

## 🔑 Authentication
1. Login with default user:
   ```json
   POST /auth/login
   {
     "username": "test",
     "password": "1234"
   }
   ```
2. Copy the returned JWT token.
3. In Swagger, click **Authorize** and paste:
   ```
   Bearer <your_token>
   ```

---

## 📦 Orders API
- **POST /orders** → Create a new order
- **GET /orders/history** → View user’s orders and receipts

Gateways:
- `paypal` → always succeeds
- `stripe` → always fails

---

## 🧪 Run Tests
```bash
dotnet test
```

Tests cover:
- BillingService (success/failure/validation)
- PaymentGatewayFactory
- In-memory repositories
- User login validation
