<div align="center">

# Amazon E-Commerce Project

[![.NET](https://img.shields.io/badge/.NET-10.0-purple?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-10.0-512bd4?style=flat-square&logo=dotnet)](https://docs.microsoft.com/aspnet/core/)
[![Entity Framework Core](https://img.shields.io/badge/EF_Core-10.0-512bd4?style=flat-square)](https://docs.microsoft.com/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![License](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](LICENSE)

A full-featured e-commerce backend API built with ASP.NET Core, featuring product management, shopping carts, Stripe payment integration, role-based authentication, and more.

[Overview](#overview) - [Architecture](#architecture) - [Features](#features) - [Tech Stack](#tech-stack) - [Getting Started](#getting-started) - [Project Structure](#project-structure) - [API Overview](#api-overview) - [Configuration](#configuration)

</div>

## Overview

This is a production-ready e-commerce REST API inspired by Amazon's core functionality. It provides a complete backend for managing products, categories, brands, shopping carts, orders, payments, and user management with role-based access control for **Admin**, **Customer**, **Seller**, and **Delivery Agent** roles.

The API follows a clean 3-layer architecture with proper separation of concerns, making it maintainable and scalable.

> [!NOTE]
> This project is the backend API only. It assumes a separate frontend application (e.g., React/Vue with Vite) running on `http://localhost:5173`.

## Architecture

```
┌─────────────────────────────────────────────────────────┐
│                      ApiLayer                           │
│            Controllers, Filters, Middleware              │
│              Swagger, CORS, DI Wiring                   │
├─────────────────────────────────────────────────────────┤
│                    BusinessLayer                        │
│        Services, DTOs, Mapping, Validation              │
│           Background Services, Email Templates          │
├─────────────────────────────────────────────────────────┤
│                   DataAccessLayer                       │
│          EF Core DbContext, Repositories                │
│              Unit of Work, Entities                     │
└─────────────────────────────────────────────────────────┘
```

**Key Design Patterns:**

- **Generic Repository Pattern** -- Reusable data access with `IGenericRepository<T>`
- **Unit of Work Pattern** -- Transaction management across 22+ repositories
- **Service Layer Pattern** -- Domain-specific business logic per entity
- **DTO Pattern** -- 59 data transfer objects for clean layer boundaries

## Features

### Authentication & Authorization

- JWT access + refresh tokens stored in HttpOnly cookies
- Token signing (HMAC-SHA256) and encryption (AES-256-KW)
- OTP-based email verification for registration
- External login via GitHub and Google OAuth
- 4 user roles: Admin, Customer, Seller, Delivery Agent
- Refresh token rotation with database-backed validation

### Product Management

- Full CRUD with multi-language support (Arabic + English)
- Hierarchical categories: Categories > SubCategories > Products
- Brand management with Cloudinary-hosted images
- Product search with Redis caching and auto language detection
- Product ratings and reviews with purchase verification
- Best-seller ranking

### Shopping & Payments

- Active shopping cart per customer with item management
- Stripe Checkout Sessions for pre-paid orders
- Cash on Delivery payment option
- Stripe Webhook handler for payment events (success, failure, refund)
- Idempotency protection on payment endpoints via Redis
- Automatic refund on stock update failure

### Order Management

- Full order lifecycle: Under Processing > Shipped > Delivered
- Admin order management with status filtering
- Delivery agent assignment
- Email notifications on order status changes
- Per-city shipping cost configuration

### Additional Features

- **Rate Limiting** -- Fixed window limiter by IP address
- **Redis Caching** -- Distributed cache with background refresh for products
- **Cloudinary Integration** -- Image upload and management
- **Background Services** -- Async email processing, cache updates
- **Serilog Logging** -- Console + SQL Server sinks
- **Soft Delete** -- Global query filters with `IsDeleted` flag
- **Global Exception Handling** -- ProblemDetails-based error responses

## Tech Stack

| Component | Technology |
|---|---|
| Framework | ASP.NET Core 10.0 |
| ORM | Entity Framework Core 10.0 |
| Database | SQL Server 2022 |
| Authentication | ASP.NET Core Identity + JWT Bearer |
| External Auth | GitHub OAuth, Google OAuth |
| Payment | Stripe.net |
| Image Storage | Cloudinary |
| Caching | Redis (StackExchange.Redis) |
| Email | MailKit (SMTP) |
| Logging | Serilog |
| Validation | FluentValidation + SharpGrip AutoValidation |
| API Docs | Swagger / Swashbuckle |

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (LocalDB or full instance)
- [Redis](https://redis.io/) (for caching)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) v17.11+ (recommended) or VS Code

### External Services

You will need accounts/keys for the following services:

- [Stripe](https://stripe.com/) -- Payment processing
- [Cloudinary](https://cloudinary.com/) -- Image storage
- [Ethereal](https://ethereal.email/) or your SMTP provider -- Email delivery

### Installation

1. Clone the repository:

```bash
git clone https://github.com/your-username/Amazon_E_Commerce_Project.git
cd Amazon_E_Commerce_Project
```

2. Update the connection string and service keys in `ApiLayer/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "sqlServerConnectionString": "Server=.;Database=Amazon_E_Commerce_DB;Integrated Security=True;..."
  },
  "Redis": {
    "ConnectionString": "localhost:6379",
    "InstanceName": "AmazonEcommerce_",
    "ProductsDurationInHours": 12
  },
  "Stripe": {
    "SecretKey": "sk_test_..."
  },
  "Cloudinary": {
    "CloudName": "...",
    "ApiKey": "...",
    "ApiSecret": "..."
  },
  "Authentication": {
    "GitHub": {
      "ClientId": "...",
      "ClientSecret": "..."
    },
    "Google": {
      "ClientId": "...",
      "ClientSecret": "..."
    }
  }
}
```

3. Apply database migrations:

```bash
dotnet ef database update --project DataAccessLayer --startup-project ApiLayer
```

4. Run the application:

```bash
dotnet run --project ApiLayer
```

5. Open Swagger UI at `https://localhost:<port>/swagger` to explore the API.

> [!TIP]
> The application uses Ethereal email in development. Check the console logs for the Ethereal web interface URL to view sent emails.

## Project Structure

```
Amazon_E_Commerce_Project/
├── ApiLayer/                          # Presentation layer (ASP.NET Core Web API)
│   ├── Controllers/                   # 25 API controllers
│   ├── Extensions/                    # DI, Serilog, and middleware registration
│   ├── Filters/                       # Action filters (deleted user check, idempotency)
│   ├── MiddleWares/                   # Custom middleware components
│   ├── Program.cs                     # Application entry point and configuration
│   └── appsettings.Development.json   # Development configuration
│
├── BusinessLayer/                     # Business logic layer
│   ├── BackgroundServices/            # Hosted services (email, cache refresh)
│   ├── Contracks/                     # Service interfaces (36 contracts)
│   ├── Dtos/                          # Data transfer objects (59 DTOs)
│   ├── Mapper/                        # AutoMapper profiles (30 profiles)
│   ├── Services/                      # Business service implementations (35 services)
│   ├── Templates/                     # HTML email templates
│   ├── Options/                       # Configuration option classes
│   ├── Roles/                         # Role constants
│   └── Exceptions/                    # Global exception handling
│
├── DataAccessLayer/                   # Data access layer
│   ├── Contracks/                     # Repository interfaces (28 contracts)
│   ├── Data/                          # EF Core DbContext
│   ├── Entities/                      # Domain entities (27 entities)
│   ├── Identity/                      # Custom Identity entities
│   ├── Migrations/                    # EF Core migrations (60+)
│   ├── Pagination/                    # Generic pagination support
│   ├── Repositories/                  # Repository implementations (28 repositories)
│   └── UnitOfWork/                    # Unit of Work pattern
│
└── sql scripts/                       # Database scripts
```

## API Overview

The API exposes **25 controllers** with the following major route groups:

| Route Group | Description | Auth Required |
|---|---|---|
| `/api/authentication` | Registration, login, logout, OAuth, OTP, password reset | Mixed |
| `/api/products` | Product CRUD, search, best sellers, recent searches | Yes |
| `/api/shopping-carts` | Cart management, item operations, total price | Customer |
| `/api/shopping-carts/payments` | Stripe checkout, cash on delivery, webhooks | Customer/Anonymous |
| `/api/admin/applications` | Admin order management, delivery agent assignment | Admin |
| `/api/applications/.../delivery-orders` | Delivery agent order views | DeliveryAgent |
| `/api/product-categories` | Category and subcategory management | Mixed |
| `/api/brands` | Brand management | Mixed |
| `/api/banners` | Promotional banner management | Mixed |
| `/api/users-addresses` | User address management | Customer |

> [!NOTE]
> Swagger UI is enabled in Development mode and provides full interactive API documentation at `/swagger`.

## Configuration

All configuration lives in `ApiLayer/appsettings.Development.json`. Key sections:

| Section | Purpose |
|---|---|
| `ConnectionStrings` | SQL Server database connection |
| `Redis` | Redis cache connection and product cache TTL |
| `Jwt` | JWT issuer, audience, and token lifetime |
| `JwtRefreshToken` | Refresh token expiry (default: 20 days) |
| `Mail` | SMTP email provider settings |
| `Stripe` | Stripe secret key for payment processing |
| `Cloudinary` | Cloudinary credentials for image storage |
| `Authentication` | GitHub and Google OAuth client credentials |
| `ApplicationSettings` | Base URL, frontend URL, order tracking path |
| `RateLimitOption` | Rate limiting: 20 requests per 10s window, 10 queued |
| `Serilog` | Logging configuration (console + SQL Server) |
