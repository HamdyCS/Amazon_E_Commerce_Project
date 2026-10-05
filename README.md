<div align="center">

# Amazon E-Commerce API

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-10.0-512BD4?style=flat-square&logo=dotnet)](https://learn.microsoft.com/aspnet/core/)
[![Entity Framework Core](https://img.shields.io/badge/EF_Core-10.0-512BD4?style=flat-square)](https://learn.microsoft.com/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![Redis](https://img.shields.io/badge/Redis-DC382D?style=flat-square&logo=redis&logoColor=white)](https://redis.io/)
[![Stripe](https://img.shields.io/badge/Stripe-635BFF?style=flat-square&logo=stripe&logoColor=white)](https://stripe.com/)
[![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black)](https://swagger.io/)

A production-oriented e-commerce backend built with ASP.NET Core 10 — multi-role marketplace with OTP-based onboarding, JWT + refresh-token sessions in HttpOnly cookies, Stripe checkout and webhooks, Redis caching and idempotency, Cloudinary media, and a three-layer architecture over SQL Server.

[Overview](#overview) · [Features](#core-features) · [Architecture](#architecture) · [Domain](#domain-model) · [Auth](#authentication--security) · [API](#api-reference) · [Getting Started](#getting-started)

</div>

---

## Overview

This repository is the **backend API only** for an Amazon-style marketplace. It supports four kinds of actors and everything they need to transact:

| Actor | What they do |
| --- | --- |
| **Customer** | Browses the catalog, maintains a shopping cart, checks out with Stripe or cash on delivery, tracks and cancels orders, saves addresses, and reviews products they have received. |
| **Seller** | Lists sellable offers (`SellerProduct`) against catalog products, sets prices and stock, and manages their own listings. |
| **Delivery Agent** | Registers the cities they serve and reads the parcels assigned to them (`need-to-delivery` / `deliveried`). |
| **Admin** | Manages the catalog, brands, banners, cities, shipping costs, users, reference data, and drives the order lifecycle (ship → deliver) while assigning delivery agents. |

The core domain vocabulary is deliberately close to the data model:

- An **Application** is a customer request — either an `Order` or a `Return`.
- An **ApplicationOrder** is a *status row* on an Application. Statuses are never mutated in place; every transition **appends a new row**, which gives you a built-in, queryable order history.
- A **Payment** is 1:1 with a ShoppingCart and carries its type (`CashOnDelivery` / `PrePaid`), status, refund status, and the Stripe `SessionId` / `InvoiceId`.
- A **ShoppingCart** has an `IsActive` flag; exactly one cart per customer is active, and checkout deactivates it.

> [!NOTE]
> There is no SignalR hub, no PDF reporting, and no automated test project in this repository. Those are listed under [Future Improvements](#future-improvements) rather than presented as existing features.

---

## Core Features

### Authentication & Identity

- OTP-based registration: 6-digit code, 10-minute expiry, emailed asynchronously through a background queue.
- Four separate registration endpoints (`register-customer`, `register-seller`, `register-deliveryagent`, and an Admin-only `register-admin`).
- Login via `UserManager`/`SignInManager` with ASP.NET Core Identity password hashing.
- **JWT access token + database-backed refresh token**, both issued as `HttpOnly` cookies (`access_token`, `refresh_token`).
- Token refresh and logout (logout revokes *all* refresh tokens for the user).
- Password reset via OTP, password change via old password, email change via OTP.
- External login with **GitHub** and **Google** (challenge → callback → find-or-create user → cookies → redirect).
- Self-service and admin-initiated **soft account deletion**, enforced after the fact by a global action filter.

### Authorization

- Four global roles: `Admin`, `Customer`, `Seller`, `DeliveryAgent` (`BusinessLayer/Roles/Role.cs`).
- Attribute-driven, endpoint-scoped: class-level `[Authorize]`/`[Authorize(Roles = ...)]` plus per-action overrides and `[AllowAnonymous]`.
- Resource scoping is done in handlers/queries (e.g. "cart belongs to this user", "review belongs to this user", "address belongs to this user").

### Catalog

- Hierarchical catalog: `ProductCategory → ProductSubCategory → Product`, with `Brand` and bilingual names/descriptions (`NameAr` / `NameEn`, `DescriptionAr` / `DescriptionEn`).
- Product CRUD with **multi-image upload to Cloudinary**, image replacement and remote deletion.
- `SellerProduct` offers per product: per-seller price and `NumberInStock`.
- Product **search with automatic Arabic/English detection**, plus category / sub-category / brand filtered paged search over seller offers.
- **Best-seller ranking** computed from delivered orders (raw SQL with `OFFSET/FETCH`).
- Per-user **recent searches** stored in Redis (last 10, de-duplicated).
- **Ratings & reviews**: one review per user per product (unique index), gated on a *delivered* purchase, with running `AvgRating` / `RatingCount` on the product.
- Promotional **banners** with date windows, display ordering, activation flags, and bulk operations.

### Cart, Checkout & Payments

- One active cart per customer (auto-created on first access), line-level quantity/price, stock clamping on add/update.
- **Pre-paid checkout**: creates a Stripe Checkout Session inside a DB transaction, stores the session id, and returns `SessionUrl` for redirect.
- **Cash on delivery**: single transaction that decrements stock, creates the payment, creates the order, and deactivates the cart.
- **Stripe webhook** with signature verification handling `payment_intent.succeeded`, `payment_intent.payment_failed`, `checkout.session.completed`, `checkout.session.expired`, and `refund.updated`.
- Compensating **automatic refund** when stock cannot be decremented after a successful payment.
- **Redis-backed idempotency** (`Idempotency-Key` header) on both checkout endpoints.
- Per-city shipping costs, included at payment time (not in the cart total).

### Orders & Delivery

- Order lifecycle `UnderProcessing → Shipped → Delivered`, or `→ Canceled` initiated by the customer.
- Admin-only shipment/delivery transitions with delivery-agent validation.
- Stock is restored on cancellation; COD payments flip to `Succeeded` on delivery.
- Return applications linked back to the original order.
- Delivery agent work queues (`need-to-delivery`, `deliveried`) and admin delivery views.
- Order summaries (latest, all, per-application) including estimated delivery window.

### Platform

- **Rate limiting**: fixed window partitioned by client IP, returning `429`.
- **Redis caching**: product-name index, city list, recent searches, idempotency records.
- **Background services**: product cache refresh, OTP email dispatch, order-status email dispatch.
- **Cloudinary** image upload/deletion for products, categories, brands, and banners.
- **Serilog** with console + SQL Server sinks (errors persisted to a `Logs` table).
- **Soft delete** through global query filters and a reflection-based repository helper.
- **Global exception handler** producing JSON error payloads with mapped status codes.
- **Swagger UI** in the Development environment.

---

## Architecture

The solution is a classic **three-layer architecture** with one ASP.NET Core host and two class libraries. Each layer owns its own contracts (`Contracks/`), and dependencies point strictly downward:

```text
┌─────────────────────────────────────────────────────────┐
│  ApiLayer              (presentation / composition)      │
│  Controllers · Action Filters · Middleware · DI wiring   │
│  Swagger · CORS · Rate limiter · Serilog · Program.cs    │
└───────────────────────────┬─────────────────────────────┘
                            │  references
                            ▼
┌─────────────────────────────────────────────────────────┐
│  BusinessLayer         (application + domain logic)      │
│  Services · DTOs · AutoMapper profiles · FluentValidation│
│  Background services · Queues · Options · Email templates │
└───────────────────────────┬─────────────────────────────┘
                            │  references
                            ▼
┌─────────────────────────────────────────────────────────┐
│  DataAccessLayer       (persistence + domain model)      │
│  Entities · Identity · EF Core DbContext & Migrations    │
│  Repositories · Unit of Work · Pagination · Validations  │
└─────────────────────────────────────────────────────────┘
```

**Layer responsibilities**

| Layer | Owns | Must not do |
| --- | --- | --- |
| `ApiLayer` | HTTP shape: routes, status codes, binding, filters, auth attributes, rate limiting, Swagger, CORS, service registration | Business rules, EF Core, SQL |
| `BusinessLayer` | Use-case orchestration, business rules, DTO↔entity mapping, validation, transactions (via Unit of Work), email/queueing, external SDKs (Stripe, Cloudinary, MailKit) | HTTP concepts (`HttpContext`, `IActionResult`) except for the token/cookie helper |
| `DataAccessLayer` | Entities, EF configuration, migrations, query construction, repository implementations, Identity storage | Business decisions |

**How a request flows**

```text
HTTP request
  → Rate limiter (fixed window, per IP)
  → Authentication (JWT read from `access_token` cookie)
  → Authorization ([Authorize] / role policies)
  → CheckIfUserIsNotDeletedFilter (global action filter)
  → Controller (thin: resolve caller id, call one service, map to status code)
  → Service (business rules, validation guards, transaction boundaries)
  → IUnitOfWork → IRepository<T> → AppDbContext → SQL Server
  → AutoMapper → DTO → JSON response
```

All wiring lives in `ApiLayer/Program.cs` plus `ApiLayer/Extensions/ServiceExtensions.cs`, which registers every service, all 28 repositories, the Unit of Work, JWT bearer, rate limiting, background queues, and hosted services.

---

## Design Patterns & Engineering Practices

| Pattern | Where it lives | Why it matters here |
| --- | --- | --- |
| **Layered architecture** | 3 projects with per-layer contracts | Keeps HTTP, business rules, and SQL independently testable and replaceable. |
| **Generic repository** | `GenericRepository<T>` implements `IGenericRepository<T>`; 27 entity-specific repositories live alongside it in `DataAccessLayer/Repositories` | Centralizes CRUD, paging, counting, and the reflection-based soft-delete so every entity behaves consistently. |
| **Unit of Work** | `DataAccessLayer/UnitOfWork/UnitOfWork.cs` exposes all repositories plus `BeginTransactionAsync` / `CommitTransactionAsync` / `RollbackTransactionAsync` / `CompleteAsync` | Gives one object for multi-aggregate transactions — checkout, webhook processing, OTP registration, return creation. |
| **Service layer** | 35 services behind 36 interfaces in `BusinessLayer` | Each use case is a single, named method that a controller can call in one line. |
| **DTO boundary** | 59 DTOs, 30 AutoMapper profiles | Entities never cross the API boundary; serialization, validation, and mapping stay decoupled from the schema. |
| **Options pattern** | `JwtOptions`, `MailOptions`, `StripeOptions`, `CloudinaryOptions`, `ApplicationOptions`, `RateLimitOptions` bound at startup, with fail-fast `Environment.Exit` when a required section is missing | Configuration is validated once at boot instead of failing on the first request. |
| **Action filter** | `CheckIfUserIsNotDeletedFilter` (global), `IdempotencyAttribute` (per-action) | Cross-cutting request concerns run before the action without polluting controllers. |
| **Exception handler** | `GlobalExceptionHandler : IExceptionHandler` + `AddProblemDetails()` | Unhandled exceptions become structured JSON with a mapped status code instead of a stack trace. |
| **Background queue (producer/consumer)** | `BackgroundQueue<T>` over `System.Threading.Channels`, typed as `IOtpEmailQueue` / `IUpdateOrderEmailQueue`, consumed by `BackgroundService`s | SMTP I/O never blocks a request thread; controllers enqueue and return immediately. |
| **Cache-aside + scheduled refresh** | `RedisCashService` (`IRedisCashService`) plus `ProductsCacheUpdateBackgroundService` | Expensive read paths (name search, city lookups) hit Redis; a background loop keeps the entry warm. |
| **Idempotency guard** | `IdempotencyAttribute` writing `idempotency:{key}` to Redis | Makes non-retryable checkout POSTs safe to retry from a flaky client. |
| **Soft delete** | `IsDeleted` / `DateOfDeletion` + `HasQueryFilter` + reflection in `GenericRepository.DeleteAsync` | Deleted rows stay for audit/relations and disappear from every query automatically. |
| **Fail-fast configuration** | Required `Options` sections abort startup when absent | Misconfiguration is caught at deploy time. |

> [!IMPORTANT]
> This is a layered architecture, not Clean Architecture: there is no separate Domain project, no dependency-inversion interfaces for the ORM, and no CQRS/MediatR pipeline. It is documented as what it is.

---

## Domain Model

```text
Person 1──1 User (IdentityUser, table "Users")
              │
              ├── RefreshToken *          (table RefreshTokens)
              ├── Otp *                   (per email/code, expiry + IsUsed)
              ├── UserAddress *           (one IsDefault per user, enforced by filtered unique index)
              ├── ProductReview *         (one per user per product, unique index)
              │
              ├── ShoppingCart * ──1 Payment (PaymentTypeId, PaymentStatusId, RefundStatusId,
              │        │                   SessionId, InvoiceId, shippingCostId, UserAddressId)
              │        └── SellerProductInShoppingCart *
              │                   └── SellerProduct * ── Product ── ProductSubCategory ── ProductCategory
              │                            │                   │                              │
              │                            │                   ├── ProductImage *            └── ProductCategoryImage *
              │                            │                   └── ProductReview *
              │                            └── Brand
              │
              ├── Application * (ApplicationType: Order | Return, ReturnApplicationId self-FK,
              │        │          EstimatedDeliveryFrom/To)
              │        └── ApplicationOrder * (ApplicationOrderType, ShoppingCartId, PaymentId,
              │                                 DeliveryId → User, CreatedBy → User)
              │
              ├── CityWhereDeliveryWork * ── City ── ShippingCost *
              ├── SellerProduct *          (listings owned by this seller)
              └── Brand / City / ProductCategory … (CreatedBy audit FKs)
```

**Lookup / reference tables**: `ApplicationTypes`, `ApplicationOrderTypes`, `PaymentsTypes`, `PaymentStatuses`, `RefundStatuses` (seeded with `HasData`), `Roles`, `UserRoles`, `Banners`.

### Key enums

| Enum | Values (numeric id) | Backing table |
| --- | --- | --- |
| `EnApplicationType` | `Order = 1`, `Return = 2` | `ApplicationTypes` |
| `EnApplicationOrderType` | `UnderProcessing = 1`, `Shipped = 2`, `Delivered = 3`, `Canceled = 4` | `ApplicationOrderTypes` |
| `EnPaymentType` | `CashOnDelivery = 1`, `PrePaid = 2` | `PaymentsTypes` |
| `EnPaymentStatus` | `Pending = 1`, `Succeeded = 2`, `Failed = 3` | `PaymentStatuses` |
| `EnRefundStatus` | `Pending = 1`, `Succeeded = 2`, `Failed = 3` | `RefundStatuses` (seeded) |
| `EnLang` | `English`, `Arabic` | — (search projection switch) |
| `EnOperation` | `Add = 1`, `Subtract = 2` | — (stock adjustment) |
| `EnProvider` | `GitHub = 1`, `Google` | — (external login) |

### Business rules encoded in the schema

- **Every foreign key is `DeleteBehavior.Restrict`** — `AppDbContext.ApplyDeleteRestrict` loops the model and forbids cascade deletes, so deletions are always explicit and soft where supported.
- **One default address per user**: filtered unique index `Ix_User_Default_Address` on `UserId` where `[IsDefault] = 1 AND [IsDeleted] = 0`; the service promotes the oldest remaining address when the default is removed.
- **One review per user per product**: unique index `IX_ProductId_UserId` on `ProductReviews`, plus a service-level guard.
- **Search indexes**: `Products.NameEn` and `Products.NameAr` are indexed specifically to accelerate name search.
- **Active cart invariant**: `ShoppingCart.IsActive` defaults to `true`; `FindActiveShoppingCartByUserIdAsync` auto-creates a cart and returns the existing active one.
- **Stock concurrency**: stock is decremented with a guarded raw `UPDATE … WHERE NumberInStock >= @qty`; zero affected rows means insufficient stock.

---

## Authentication & Security

### Session lifecycle

```text
POST /send-otp?email=        → 6-digit code persisted (10 min), enqueued to email worker
        ↓
POST /register-customer      → validate OTP → create Person + User (Identity) → consume OTP
        ↓                     → assign Customer role → issue tokens
        ↓
Cookies set: access_token (JWT, minutes) + refresh_token (DB row, days)
        ↓
Authenticated calls           → JWT read from cookie → claims (NameIdentifier, Email, Role*)
        ↓
POST /refresh-token          → validate DB refresh token → new JWT, cookies rewritten
        ↓
POST /logout                 → delete ALL refresh tokens for the user → delete cookies
```

### JWT

| Property | Value |
| --- | --- |
| Signing | HMAC-SHA256 with `Jwt:SigningKey` |
| Encryption | AES-256-KW + AES-128-CBC-HMAC-SHA256 with `Jwt:EncryptionKey` (first 32 chars) |
| Issuer / Audience / Lifetime | `Jwt:Issuar`, `Jwt:Audience`, `Jwt:LifeTimeMin` (dev: 10 min) |
| `ClockSkew` | `TimeSpan.Zero` |
| Claims | `ClaimTypes.NameIdentifier`, `ClaimTypes.Email`, one `ClaimTypes.Role` per role |
| Transport | **Not** in an `Authorization` header — `OnMessageReceived` reads `Request.Cookies["access_token"]` |

### Refresh tokens

Generated from 32 cryptographically random bytes (Base64), stored as a row with `CreatedAt` / `ExpiresAt` (`JwtRefreshToken:LifeTimeDays`, dev: 20 days) and `IsActive => ExpiresAt > UtcNow`. Validation requires row existence, ownership by the user, and an active window. Logout removes every row for the user.

### Cookies

Both tokens are written with `HttpOnly = true`, `Secure = true`, `SameSite = None`, `Path = "/"`, and the refresh-token lifetime — cross-site by design, because the SPA runs on a different origin (`http://localhost:5173`) than the API.

### OTP

Six-digit code generated with `Random.Next(100000, 1000000)`, stored with `ExpiresAt = UtcNow + Otp:LifeTimeMin` and `IsUsed`. Validity is `ExpiresAt >= UtcNow && !IsUsed`. Consumed (marked used) during registration and password reset inside a transaction.

### External login

```text
GET /login/customer/github?returnUrl=   ─┐
GET /login/customer/google?returnUrl=   ─┴→ Challenge (302) to provider
                                              ↓
GET /external-login-callback?returnUrl=&remoteError=
      → validate returnUrl against allowed origin (http://localhost:5173/)
      → GetExternalLoginInfoAsync
      → FindByLogin (linked account)  else  FindByEmail (existing account)
      → else create Person + User + AddToRole(Customer) + AddLogin (link provider)
      → issue JWT + refresh token → set cookies → Redirect(returnUrl)
```

### Other security measures

- Passwords hashed by ASP.NET Core Identity's default hasher (no custom `PasswordOptions` are configured in this repo).
- A **global action filter** rejects requests from soft-deleted users with `401` even when their JWT is still valid.
- Login returns `401` for unknown/bad credentials; a soft-deleted user is invisible to `FindByEmailAsync` because of the global query filter.
- Fixed-window **rate limiting** per remote IP on all controllers except the Stripe webhook and the banners controller (`429 Too Many Requests` on rejection).
- **Idempotency keys** on the two money-moving endpoints.
- Webhook **signature verification** using `Stripe::WebHookSecret` before any processing.
- Return-URL allow-listing on the OAuth callback (`Helper.IsValidReturnUrl`).
- CORS restricted to the configured front-end origin with credentials allowed.
- Required configuration sections abort startup when missing.

> [!WARNING]
> Secrets (`Jwt:SigningKey`, `Jwt:EncryptionKey`, `Stripe:*`, `Cloudinary:*`, `Authentication:*`, `Mail:AppPassword`) are supplied through **User Secrets** or environment variables. Nothing in this README is a real value.

---

## Authorization Model

### Global roles

| Role | Granted at | Typical powers |
| --- | --- | --- |
| `Admin` | `register-admin` (Admin-only) | Full catalog/brand/banner/city/shipping/reference CRUD, user lookup & deletion, ship/deliver transitions, return applications, all reports/lists |
| `Customer` | `register-customer`, external login | Cart, checkout, own orders, cancel, addresses, reviews, profile |
| `Seller` | `register-seller` | Own `SellerProduct` listings (create/update/delete), read own listings |
| `DeliveryAgent` | `register-deliveryagent` | Maintain served cities, read own delivery queues |

### How authorization is applied

1. **Class-level** `[Authorize]` or `[Authorize(Roles = ...)]` on the controller.
2. **Action-level** `[Authorize(Roles = ...)]` narrows or `[AllowAnonymous]` opens an endpoint inside an otherwise protected controller.
3. **No global fallback policy** is registered — controllers without `[Authorize]` are reachable anonymously.
4. **Resource scoping** happens in the service/repository query (caller's `UserId` from claims is combined with the route id), so a Customer can only see their own carts, addresses, reviews, and orders.

> [!TIP]
> In the endpoint tables below, **Public** means "no authorization attribute and the handler does not read claims". **Public ⚠** means there is no `[Authorize]` attribute but the handler resolves the caller from claims and returns `401` when absent.

---

## API Reference

Base URL in development: `http://localhost:5157` (HTTP profile) or `https://localhost:7027` (HTTPS profile) — see `ApiLayer/Properties/launchSettings.json`. Swagger UI is enabled in Development at `/swagger`.

25 controllers expose 170+ routes. All of them except `StripeController` (webhook) and `BannersController` carry
`[EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]` (dev: 20 requests / 10 s window, queue of 10, reject with `429`).

### Pagination envelope

Paged endpoints that return `PaginationResultDto<T>` use this exact shape:

```json
{
  "data": [ { "...": "..." } ],
  "totalCount": 137,
  "pageNumber": 1,
  "pageSize": 10,
  "nextPage": 2,
  "previousPage": null,
  "totalPages": 14,
  "hasNextPage": true,
  "hasPreviousPage": false
}
```

> Some product endpoints (`/api/products/all-paged`, `/api/products/all-paged-order-by-best-seler-desc`) return a **bare array** instead of this envelope.

### Authentication

<details>
<summary><b>Authentication endpoints</b> — prefix <code>/api/authentication</code> (class-level <code>[Authorize]</code>)</summary>

| Method | Route | Auth | Purpose |
| --- | --- | --- | --- |
| GET | `/api/authentication` | Auth | Current `UserDto` from the `NameIdentifier` claim |
| GET | `/api/authentication/is-email-exist?email=` | Public | Email uniqueness probe |
| POST | `/api/authentication/send-otp?email=` | Public | Generate + persist + enqueue a 6-digit OTP |
| GET | `/api/authentication/is-otp-valid?otp=&email=` | Public | Check OTP is active and unused |
| POST | `/api/authentication/register-customer` | Public | Register + `Customer` role + **issues cookies** |
| POST | `/api/authentication/register-seller` | Public | Register + `Seller` role (no tokens issued) |
| POST | `/api/authentication/register-deliveryagent` | Public | Register + `DeliveryAgent` role (no tokens issued) |
| POST | `/api/authentication/register-admin` | Admin | Register + `Admin` role |
| POST | `/api/authentication/login` | Public | Verify credentials → JWT + refresh cookies |
| POST | `/api/authentication/refresh-token` | Public | New JWT from the `refresh_token` cookie |
| POST | `/api/authentication/logout` | Auth | Revoke all refresh tokens + delete cookies |
| PUT | `/api/authentication/reset-password` | Public | OTP + new password |
| PUT | `/api/authentication/update-password` | Auth | Change password with old password |
| PUT | `/api/authentication/update-email` | Auth | Change email/username after OTP validation |
| PUT | `/api/authentication` | Auth | Update profile (name, DOB, phone) |
| DELETE | `/api/authentication` | Auth | Soft-delete own account |
| DELETE | `/api/authentication/{Id}` | Admin | Soft-delete any account |
| GET | `/api/authentication/all?pageNumber=&pageSize=` | Admin | Paged users |
| GET | `/api/authentication/count` | Admin | Total user count |
| GET | `/api/authentication/login/customer/github?returnUrl=` | Public | 302 challenge to GitHub |
| GET | `/api/authentication/login/customer/google?returnUrl=` | Public | 302 challenge to Google |
| GET | `/api/authentication/external-login-callback?returnUrl=&remoteError=` | Public | Provider callback → cookies → redirect |

</details>

<details>
<summary><b>Users, addresses, cities</b></summary>

| Method | Route | Auth | Purpose |
| --- | --- | --- | --- |
| GET | `/api/admin/users/get-by-email` | Admin | Look up a user by email (body: raw string) |
| GET | `/api/user-addresses/all` | Auth | Own addresses |
| GET | `/api/user-addresses/count` | Auth | Own address count |
| GET | `/api/user-addresses/{Id}` | Auth | Single own address |
| POST | `/api/user-addresses` | Auth | Add address (applies default-address rule) → `201` |
| PUT | `/api/user-addresses/{Id}` | Auth | Update address + default handling |
| DELETE | `/api/user-addresses/{Id}` | Auth | Soft delete; oldest address promoted to default |
| GET | `/api/cities/all-paged?page=&pageSize=` | Public | Paged cities (Redis cached) |
| GET | `/api/cities/all` | Public | All cities (Redis `cities:all`) |
| GET | `/api/cities/{Id}` | Public | City by id |
| POST | `/api/cities` | Admin | Create city + refresh cache |
| PUT | `/api/cities/{Id}` | Admin | Update city + refresh cache |
| DELETE | `/api/cities/{Id}` | Admin | Soft delete city + refresh cache |

</details>

### Catalog

<details>
<summary><b>Products</b> — prefix <code>/api/products</code> (class-level <code>[Authorize]</code>)</summary>

| Method | Route | Auth | Notes |
| --- | --- | --- | --- |
| GET | `/api/products/{Id}` | Auth | Product with images |
| GET | `/api/products/name-ar/{NameAr}` | Auth | By Arabic name |
| GET | `/api/products/name-en/{NameEn}` | Auth | By English name |
| GET | `/api/products/all` | Auth | All products |
| GET | `/api/products/count` | Auth | Count |
| GET | `/api/products/all-paged?pageNumber=&pageSize=` | Auth | Paged, bare array |
| GET | `/api/products/all-order-by-best-seler-desc` | Auth | Sorted by delivered-order count (raw SQL) |
| GET | `/api/products/all-paged-order-by-best-seler-desc?pageNumber=&pageSize=` | Auth | Paged best-seller (raw SQL + `OFFSET/FETCH`) |
| POST | `/api/products` | Admin | **multipart/form-data** (`CreateProductDto`, `Images: List<IFormFile>`) — uploads to Cloudinary inside the transaction |
| POST | `/api/products/range` | Admin | Bulk create (form) |
| PUT | `/api/products/{Id}` | Admin | Update; deletes old Cloudinary images then uploads new |
| DELETE | `/api/products/{Id}` | Admin | Soft delete + Cloudinary/image-row cleanup |
| GET | `/api/products/search?query=&pageSize=` | Public | Name autocomplete, **auto Arabic/English detection** → `string[]` |
| GET | `/api/products/recent-search` | Auth | Last ≤ 10 search strings from Redis |
| POST | `/api/products/recent-search` | Auth | Save a search (`{ "searchQuery": "laptop" }`), de-duplicated |

</details>

<details>
<summary><b>Seller products</b> — prefix <code>/api/seller-products</code></summary>

| Method | Route | Auth | Notes |
| --- | --- | --- | --- |
| GET | `/api/seller-products?pageNumber=&pageSize=` | Public | Paged offers → pagination envelope (defaults `1`/`10`) |
| GET | `/api/seller-products/{Id}` | Public | Offer by id |
| GET | `/api/seller-products/products/{ProductId}` | Public | All offers for a product |
| GET | `/api/seller-products/categories/{productCategoryId}?pageNumber=&pageSize=` | Public | Filter by category |
| GET | `/api/seller-products/sub-categories/{productSubCategoryId}?pageNumber=&pageSize=` | Public | Filter by sub-category |
| GET | `/api/seller-products/brands/{brnadId}?pageNumber=&pageSize=` | Public | Filter by brand |
| GET | `/api/seller-products/search?query=&pageNumber=&pageSize=` | Public | Paged bilingual name search |
| GET | `/api/seller-products/seller` | Seller | Own listings |
| GET | `/api/seller-products/admin/seller/{sellerId}` | Admin | A seller's listings |
| POST | `/api/seller-products` | Seller | Create offer |
| POST | `/api/seller-products/range` | Seller | Bulk create (transaction) |
| PUT | `/api/seller-products/{Id}` | Seller | Update own offer |
| DELETE | `/api/seller-products/{Id}` | Seller | Delete own offer |
| DELETE | `/api/seller-products/admin/{Id}` | Admin | Delete any offer |

</details>

<details>
<summary><b>Categories, sub-categories, brands, banners, reviews</b></summary>

**Product categories** — `/api/product-categories`

| Method | Route | Auth |
| --- | --- | --- |
| GET | `/api/product-categories/{Id}` · `/name-ar/{NameAr}` · `/name-en/{NameEn}` · `/all` · `/count` · `/all-paged?pageNumber=&pageSize=` | Public |
| POST | `/api/product-categories` · `/range` (form, images) | Public ⚠ |
| PUT | `/api/product-categories/{Id}` (form) | Public ⚠ |
| DELETE | `/api/product-categories/{Id}` | Admin |

**Product sub-categories** — `/api/product-sub-categories` (class-level `Admin`)

`GET {Id}` · `GET name-ar/{NameAr}` · `GET name-en/{NameEn}` · `GET all` · `GET count` · `GET all-paged` · `GET all/{Id}` · `POST ""` · `POST range` · `PUT {Id}` · `DELETE {Id}` — **all Admin**.

**Brands** — `/api/brands`

| Method | Route | Auth |
| --- | --- | --- |
| GET | `/api/brands/{Id}` · `/name-ar/{NameAr}` · `/name-en/{NameEn}` · `/all` · `/count` · `/all-paged?pageNumber=&pageSize=` | Public |
| POST | `/api/brands` · `/range` (form, single image) | Admin |
| PUT | `/api/brands/{Id}` (form) | Admin |
| DELETE | `/api/brands/{Id}` | Admin |

**Banners** — `/api/banners` (class-level `Admin`, no rate limiting)

| Method | Route | Auth |
| --- | --- | --- |
| GET | `/api/banners/all?pageNumber=&pageSize=` | Admin |
| GET | `/api/banners/all/active` | **Public** (`AllowAnonymous`) |
| GET | `/api/banners/{id}` | Admin |
| POST | `/api/banners` (multipart) · `/bulk` | Admin |
| PUT | `/api/banners` · `/range` | Admin |
| DELETE | `/api/banners/{id}` · `/active` | Admin |

**Reviews** — `/api/products/{productId}/reviews`

| Method | Route | Auth | Notes |
| --- | --- | --- | --- |
| GET | `…/avg` · `…/{Id}` · `…/all` · `…/all-paged?pageNumber=&pageSize=` | Public | Read-only |
| POST | `…` | Auth | Requires a **delivered** purchase; one review per user per product |
| PUT | `…/{Id}` | Auth | Owner only |
| DELETE | `…/{Id}` | Auth | Owner only |
| DELETE | `…/{Id}/admin` | Admin | Moderation delete |

</details>

### Cart, payments & orders

<details>
<summary><b>Shopping cart</b> — prefix <code>/api/shopping-carts</code></summary>

| Method | Route | Auth | Notes |
| --- | --- | --- | --- |
| GET | `/api/shopping-carts/active` | Customer | Auto-creates the cart if missing |
| GET | `/api/shopping-carts/{ShoppingCartId}` | Admin | Any cart |
| GET | `/api/shopping-carts/all` | Customer | Own carts |
| GET | `/api/shopping-carts/total-price` | Customer | Cart subtotal (shipping excluded) |
| POST | `/api/shopping-carts` | Customer | Create / return existing active cart. **Known issue:** the action targets route name `GetShoppingCartById`, but the registered name is `GetShoppingCart`, so a successful call throws at `CreatedAtRoute`. |

**Cart lines** — `/api/shopping-carts/{ShoppingCartId}/seller-products` (class-level `Customer`)

| Method | Route | Notes |
| --- | --- | --- |
| GET | `/{SellerProductInShoppingCartId}` | Line detail |
| POST | `""` | Add or update a line; quantity clamped to `NumberInStock` |
| POST | `/bulk` | Add many lines |
| PUT | `/{SellerProductInShoppingCartId}` | Update quantity, recompute `TotalPrice` |
| DELETE | `/{id}` | Remove line, returns the updated cart |

</details>

<details>
<summary><b>Payments</b> — prefix <code>/api/shopping-carts/payments</code> (class-level <code>Customer</code>)</summary>

| Method | Route | Auth | Notes |
| --- | --- | --- | --- |
| GET | `/total-price` | Customer | Cart total **+** shipping cost for the address's city. Note: the action takes a `PaymentDto` (`userAddressId`, `shoppingCartId`) — with `[ApiController]` this complex type is bound from a **JSON body**, not query string. |
| GET | `/session/{sessionId}` | Customer | Payment by Stripe session id (scoped to caller) |
| GET | `/application-orders/{applicationOrderId}` | Customer | Payment for one of the caller's order rows |
| POST | `/pre-paid` | Customer | **Requires `Idempotency-Key`** → creates Stripe session |
| POST | `/cash-on-delivery` | Customer | **Requires `Idempotency-Key`** → full checkout in one transaction |

`POST /api/stripe` (anonymous, no rate limiting) is the Stripe webhook.

</details>

<details>
<summary><b>Orders (Applications)</b></summary>

**Customer** — `/api/applications/{ApplicationId}` (class-level `Customer`)

| Method | Route | Notes |
| --- | --- | --- |
| GET | `/active-application-orders` | Current status row |
| GET | `/track-application-orders` | Full status history |
| POST | `/cancel` | Cancel if not delivered/canceled → restores stock + emails |

**Customer summaries** — `/api/applications`

| Method | Route | Auth |
| --- | --- | --- |
| GET | `/api/applications/all` | Customer |
| GET | `/api/applications/{ApplcationId}/order-application-summary` | Customer |
| GET | `/api/applications/order-application-summaries` | Customer |
| GET | `/api/applications/latest-application-order-summary` | Customer |

**Admin** — `/api/admin/applications` (class-level `Admin`)

| Method | Route | Notes |
| --- | --- | --- |
| GET | `/application-orders/{ApplicationOrderId}` | Status row by id |
| GET | `/{ApplicationId}/application-orders` | Full history |
| GET | `/application-orders/active-under-processing` | Payable + unshipped queue |
| GET | `/application-orders/active-shipping` | Shipped, not yet delivered |
| GET | `/application-orders/active-delivered` | Delivered rows |
| POST | `/{ApplicationId}/shipping-application-orders` | Body: `"<deliveryUserId>"` → `201`, assigns `DeliveryId`, emails |
| POST | `/{ApplicationId}/delivered-application-orders` | `201`, marks COD payment `Succeeded`, emails |

**Returns & admin reads** — `/api/applications`

| Method | Route | Auth |
| --- | --- | --- |
| GET | `/api/applications/all-return` | Admin |
| GET | `/api/applications/all-user-return` | Admin (body: user id) |
| GET | `/api/applications/{ApplcationId}/shopping-cart` | Admin |
| POST | `/api/applications/{ApplcationId}/return` | Admin |

**Delivery agent** — `/api/applications/application-order/delivery-orders` (class-level `DeliveryAgent`)

| Method | Route |
| --- | --- |
| GET | `/need-to-delivery` |
| GET | `/deliveried` |

**Admin delivery views** — `/api/admin/applications/application-order/delivery-orders` (class-level `Admin`)
`GET /need-to-delivery` · `GET /deliveried`

</details>

<details>
<summary><b>Reference data & shipping</b></summary>

| Controller | Prefix | Endpoints |
| --- | --- | --- |
| `ApplicationTypes` | `/api/application-types` | `GET {id}` (Public) · `GET all` (Admin) · `PUT {id}` (Admin) |
| `ApplicationOrderTypes` | `/api/application-order-types` | `GET {id}` (Public) · `GET all` (Admin) · `PUT {id}` (Admin) |
| `PaymentTypes` | `/api/payment-types` | `GET {id}` (Public) · `GET all` (Admin) · `PUT {id}` (Admin) |
| `ShippingCosts` | `/api/shipping-costs` (class `Auth`) | `GET {id}` (Admin) · `GET cities/{citiyId}` (Auth) · `GET all` · `GET paged` (Admin) · `POST ""` · `POST range` · `PUT {id}` · `DELETE {id}` (Admin) |
| `Delivery cities` | `/api/deliveries` | `GET cities/cities-where-Delivery-workId/{id}` (Public) · `GET cities/{CityId}` (DeliveryAgent) · `GET cities/admin/{id}` (Admin) · `GET cities` (DeliveryAgent) · `GET {DeliveryId}/cities` (Admin) · `POST cities` · `POST cities/range` · `PUT cities/{id}` · `DELETE cities/{id}` (DeliveryAgent) |

</details>

---

## Endpoint Inventory

| Method | Endpoint | Auth | Purpose |
| --- | --- | --- | --- |
| GET | `/api/authentication` | Auth | Current user profile |
| GET | `/api/authentication/is-email-exist` | Public | Email exists? |
| POST | `/api/authentication/send-otp` | Public | Send OTP |
| GET | `/api/authentication/is-otp-valid` | Public | Validate OTP |
| POST | `/api/authentication/register-customer` | Public | Register customer (+cookies) |
| POST | `/api/authentication/register-seller` | Public | Register seller |
| POST | `/api/authentication/register-deliveryagent` | Public | Register delivery agent |
| POST | `/api/authentication/register-admin` | Admin | Register admin |
| POST | `/api/authentication/login` | Public | Login |
| POST | `/api/authentication/refresh-token` | Public | Refresh JWT |
| POST | `/api/authentication/logout` | Auth | Revoke sessions |
| PUT | `/api/authentication/reset-password` | Public | OTP password reset |
| PUT | `/api/authentication/update-password` | Auth | Change password |
| PUT | `/api/authentication/update-email` | Auth | Change email |
| PUT | `/api/authentication` | Auth | Update profile |
| DELETE | `/api/authentication` | Auth | Delete own account |
| DELETE | `/api/authentication/{Id}` | Admin | Delete account |
| GET | `/api/authentication/all` | Admin | Paged users |
| GET | `/api/authentication/count` | Admin | User count |
| GET | `/api/authentication/login/customer/github` | Public | GitHub challenge |
| GET | `/api/authentication/login/customer/google` | Public | Google challenge |
| GET | `/api/authentication/external-login-callback` | Public | OAuth callback |
| GET | `/api/admin/users/get-by-email` | Admin | User lookup |
| GET | `/api/user-addresses/all` · `/count` · `/{Id}` | Auth | Read own addresses |
| POST | `/api/user-addresses` | Auth | Add address |
| PUT | `/api/user-addresses/{Id}` | Auth | Update address |
| DELETE | `/api/user-addresses/{Id}` | Auth | Delete address |
| GET | `/api/cities/all-paged` · `/all` · `/{Id}` | Public | Read cities |
| POST/PUT/DELETE | `/api/cities` · `/{Id}` | Admin | Manage cities |
| GET | `/api/products/{Id}` · `/name-ar/{NameAr}` · `/name-en/{NameEn}` · `/all` · `/count` · `/all-paged` · `/all-order-by-best-seler-desc` · `/all-paged-order-by-best-seler-desc` | Auth | Read catalog |
| POST | `/api/products` · `/range` | Admin | Create products |
| PUT | `/api/products/{Id}` | Admin | Update product |
| DELETE | `/api/products/{Id}` | Admin | Delete product |
| GET | `/api/products/search` | Public | Name autocomplete |
| GET/POST | `/api/products/recent-search` | Auth | Recent searches |
| GET | `/api/seller-products` · `/{Id}` · `/products/{ProductId}` · `/categories/{id}` · `/sub-categories/{id}` · `/brands/{id}` · `/search` | Public | Browse offers |
| GET | `/api/seller-products/seller` | Seller | Own listings |
| GET | `/api/seller-products/admin/seller/{sellerId}` | Admin | Listings by seller |
| POST | `/api/seller-products` · `/range` | Seller | Create offers |
| PUT | `/api/seller-products/{Id}` | Seller | Update offer |
| DELETE | `/api/seller-products/{Id}` | Seller | Delete own offer |
| DELETE | `/api/seller-products/admin/{Id}` | Admin | Delete any offer |
| GET | `/api/product-categories/{Id}` · `/name-ar/{NameAr}` · `/name-en/{NameEn}` · `/all` · `/count` · `/all-paged` | Public | Read categories |
| POST | `/api/product-categories` · `/range` | Public ⚠ | Create categories |
| PUT | `/api/product-categories/{Id}` | Public ⚠ | Update category |
| DELETE | `/api/product-categories/{Id}` | Admin | Delete category |
| *all* | `/api/product-sub-categories/**` | Admin | Sub-category CRUD & reads |
| GET | `/api/brands/{Id}` · `/name-ar/{NameAr}` · `/name-en/{NameEn}` · `/all` · `/count` · `/all-paged` | Public | Read brands |
| POST/PUT/DELETE | `/api/brands` · `/range` · `/{Id}` | Admin | Manage brands |
| GET | `/api/banners/all` · `/{id}` | Admin | Read banners |
| GET | `/api/banners/all/active` | Public | Active banners |
| POST/PUT/DELETE | `/api/banners` · `/bulk` · `/range` · `/{id}` · `/active` | Admin | Manage banners |
| GET | `/api/products/{productId}/reviews/avg` · `/{Id}` · `/all` · `/all-paged` | Public | Read reviews |
| POST | `/api/products/{productId}/reviews` | Auth | Create review (purchase required) |
| PUT | `/api/products/{productId}/reviews/{Id}` | Auth | Update own review |
| DELETE | `/api/products/{productId}/reviews/{Id}` | Auth | Delete own review |
| DELETE | `/api/products/{productId}/reviews/{Id}/admin` | Admin | Delete any review |
| GET | `/api/shopping-carts/active` · `/all` · `/total-price` | Customer | Read cart |
| GET | `/api/shopping-carts/{ShoppingCartId}` | Admin | Read any cart |
| POST | `/api/shopping-carts` | Customer | Create cart |
| GET/POST/PUT/DELETE | `/api/shopping-carts/{ShoppingCartId}/seller-products/**` | Customer | Cart line CRUD |
| GET | `/api/shopping-carts/payments/total-price` · `/session/{sessionId}` · `/application-orders/{id}` | Customer | Payment reads |
| POST | `/api/shopping-carts/payments/pre-paid` | Customer | Stripe checkout session |
| POST | `/api/shopping-carts/payments/cash-on-delivery` | Customer | COD checkout |
| POST | `/api/stripe` | Public | Stripe webhook |
| GET | `/api/applications/{ApplicationId}/active-application-orders` · `/track-application-orders` | Customer | Track order |
| POST | `/api/applications/{ApplicationId}/cancel` | Customer | Cancel order |
| GET | `/api/applications/all` · `/{id}/order-application-summary` · `/order-application-summaries` · `/latest-application-order-summary` | Customer | Order summaries |
| GET | `/api/applications/all-return` · `/all-user-return` · `/{id}/shopping-cart` | Admin | Admin reads |
| POST | `/api/applications/{ApplcationId}/return` | Admin | Create return |
| GET | `/api/admin/applications/application-orders/{id}` · `/{id}/application-orders` · `/active-under-processing` · `/active-shipping` · `/active-delivered` | Admin | Order queues |
| POST | `/api/admin/applications/{id}/shipping-application-orders` · `/delivered-application-orders` | Admin | Advance status |
| GET | `/api/applications/application-order/delivery-orders/need-to-delivery` · `/deliveried` | DeliveryAgent | Own delivery queue |
| GET | `/api/admin/applications/application-order/delivery-orders/need-to-delivery` · `/deliveried` | Admin | Delivery views |
| GET | `/api/application-types/{id}` | Public | Reference read |
| GET/PUT | `/api/application-types/all` · `/{id}` | Admin | Reference manage |
| GET | `/api/application-order-types/{id}` | Public | Reference read |
| GET/PUT | `/api/application-order-types/all` · `/{id}` | Admin | Reference manage |
| GET | `/api/payment-types/{id}` | Public | Reference read |
| GET/PUT | `/api/payment-types/all` · `/{id}` | Admin | Reference manage |
| GET | `/api/shipping-costs/cities/{citiyId}` | Auth | Shipping cost by city |
| GET/POST/PUT/DELETE | `/api/shipping-costs/**` | Admin | Manage shipping costs |
| GET | `/api/deliveries/cities/cities-where-Delivery-workId/{id}` | Public | Read delivery city |
| GET/POST/PUT/DELETE | `/api/deliveries/cities/**` | DeliveryAgent | Manage served cities |
| GET | `/api/deliveries/cities/{CityId}` · `/cities/admin/{id}` · `/{DeliveryId}/cities` | Admin | Delivery admin reads |

---

## Request / Response Examples

> [!IMPORTANT]
> Authentication uses **cookies**, not `Authorization` headers. Send requests with credentials enabled (e.g. `fetch(url, { credentials: 'include' })`).

### 1. Send an OTP

```http
POST /api/authentication/send-otp?email=customer@example.com
```

```json
"Send otp to email successfuly"
```

### 2. Register a customer

```http
POST /api/authentication/register-customer
Content-Type: application/json
```

```json
{
  "otp": "483920",
  "userDto": {
    "firstName": "Sarah",
    "lastName": "Ahmed",
    "dateOfBirth": "1998-04-12",
    "phoneNumber": "+201000000000",
    "email": "customer@example.com",
    "password": "Passw1!"
  }
}
```

```json
{
  "id": "8f4c1a2e-3b7d-4f0a-9c6b-1d2e3f4a5b6c",
  "firstName": "Sarah",
  "lastName": "Ahmed",
  "dateOfBirth": "1998-04-12T00:00:00",
  "phoneNumber": "+201000000000",
  "email": "customer@example.com",
  "password": "…",
  "roles": ["Customer"]
}
```

Sets `Set-Cookie: access_token=…; HttpOnly; Secure; SameSite=None` and `refresh_token=…`.

### 3. Login

```http
POST /api/authentication/login
Content-Type: application/json
```

```json
{ "email": "customer@example.com", "password": "Passw1!" }
```

→ `200` with `UserDto` + auth cookies. `401 "Invaild email or password"` on failure.

### 4. Refresh / logout

```http
POST /api/authentication/refresh-token      → 200 { "message": "successfuly" }
POST /api/authentication/logout             → 200 { "message": "Logout successfuly" }
```

### 5. Add an item to the cart

```http
POST /api/shopping-carts/1/seller-products
Content-Type: application/json
```

```json
{ "quantity": 2, "sellerProductId": 14, "shoppingCartId": 1 }
```

→ `200` with the updated `ShoppingCartDto` (lines carry `quantity`, `totalPrice`, `productNameEn`, `productNameAr`, `productImageUrl`).

### 6. Cash-on-delivery checkout

```http
POST /api/shopping-carts/payments/cash-on-delivery
Idempotency-Key: 3f0f4a1c-7d55-4f0a-9f77-2ab6c5f1e9d0
Content-Type: application/json
```

```json
{ "userAddressId": 3, "shoppingCartId": 1 }
```

```json
{ "applicationId": 42 }
```

> [!WARNING]
> Omitting `Idempotency-Key` returns `400 "Idempotency-Key header is required"`; a concurrent duplicate returns `409 "Request is already in progress"`.

### 7. Pre-paid (Stripe) checkout

```http
POST /api/shopping-carts/payments/pre-paid
Idempotency-Key: 8b6f0d2a-2f4b-4b4f-8b3d-77f1c9a2e6c1
Content-Type: application/json
```

```json
{
  "userAddressId": 3,
  "shoppingCartId": 1,
  "successUrl": "http://localhost:5173/checkout/success",
  "cancelUrl": "http://localhost:5173/checkout/cancel"
}
```

```json
{ "sessionUrl": "https://checkout.stripe.com/c/pay/cs_test_…", "sessionId": "cs_test_…" }
```

After the redirect, the SPA reads `?sessionId=` and polls:

```http
GET /api/shopping-carts/payments/session/cs_test_…
→ { "id": 7, "userAddressId": 3, "shoppingCartId": 1, "paymentStatusId": 1 }
```

The order itself is materialized by the `payment_intent.succeeded` webhook.

### 8. Cancel an order

```http
POST /api/applications/42/cancel
```

Requires the `Customer` role. → `200` with the new `Canceled` `ApplicationOrderDto`; stock is restored and a cancellation email is enqueued.
`400/500` if the order was already delivered or canceled.

### 9. Admin ships an order

```http
POST /api/admin/applications/42/shipping-application-orders
Content-Type: application/json
```

```json
"delivery-user-id-abc"
```

→ `201` + `Location: /api/admin/applications/application-orders/{id}`; a `Shipped` row is appended with `DeliveryId`, and a shipped email is enqueued.

### 10. Paged bilingual offer search

```http
GET /api/seller-products/search?query=لابتوب&pageNumber=1&pageSize=10
```

```json
{
  "data": [ { "id": 14, "price": 899.0, "numberInStock": 12, "productNameEn": "…", "productNameAr": "…" } ],
  "totalCount": 3,
  "pageNumber": 1,
  "pageSize": 10,
  "nextPage": null,
  "previousPage": null,
  "totalPages": 1,
  "hasNextPage": false,
  "hasPreviousPage": false
}
```

### 11. Leave a review

```http
POST /api/products/9/reviews
Content-Type: application/json
```

```json
{ "numberOfStars": 5, "message": "Exactly as described", "name": "Sarah", "productId": 9 }
```

`400 "User has not bought this product."` without a delivered order,
`400 "User has already reviewed this product."` on a duplicate.

---

## Error Handling

Three cooperating mechanisms:

1. **Controller-level `try/catch`** — most actions catch and return `StatusCode(500, ex.Message)` (or `400`) with a plain message. This is the dominant path.
2. **`GlobalExceptionHandler : IExceptionHandler`** — registered via `AddExceptionHandler<GlobalExceptionHandler>()` + `AddProblemDetails()` and `app.UseExceptionHandler()`. It logs the exception and maps exception *types* to status codes:

   | Exception | Status |
   | --- | --- |
   | `ArgumentException`, `ArgumentNullException`, `ArgumentOutOfRangeException`, `InvalidOperationException` | `400` |
   | `UnauthorizedAccessException` | `401` |
   | `KeyNotFoundException` | `404` |
   | anything else | `500` |

   Response body:

   ```json
   {
     "error": {
       "message": "Active shopping cart not found.",
       "statusCode": 404,
       "time": "2026-10-05T12:00:00Z"
     }
   }
   ```

3. **`ParamaterException`** — a small guard class in both `BusinessLayer` and `DataAccessLayer` that throws `ArgumentException`/`ArgumentNullException` for null/empty/invalid arguments before any I/O happens.

> [!NOTE]
> `AddProblemDetails()` is registered, but responses are produced by the custom handler above rather than by `ProblemDetailsFactory`. Error payloads are plain JSON, not RFC 7807 `application/problem+json`.

---

## Validation

Validation happens at **three levels**:

**1. Data annotations on DTOs** (the primary mechanism, enforced automatically by `[ApiController]` model binding → `400 ValidationProblemDetails`):

```csharp
// UserDto
[Required, MinLength(6), MaxLength(10)] public string Password { get; set; }
[Required, EmailAddress]                public string Email { get; set; }
[Required, CustomValidation(typeof(PersonValidtion), "DateOfBirthValidtion")]
public DateTime DateOfBirth { get; set; }   // must be 18+ years old

// AddSellerProductToShoppingCartDto
[Required, Range(1, double.MaxValue)] public int Quantity { get; set; }

// ProductReviewDto
[Required, Range(1, 5)] public int NumberOfStars { get; set; }
```

**2. FluentValidation** — registered with
`AddValidatorsFromAssemblyContaining<UpdateBannerDtoValidition>()` plus `SharpGrip.FluentValidation.AutoValidation.Mvc` with body and form binding sources enabled. Validators currently exist for banners:

```csharp
// CreateBannerDtoValidator
RuleFor(x => x.Title).NotEmpty();
RuleFor(x => x.Image).NotEmpty();
RuleFor(x => x.StartDate).NotEmpty().Must(d => d >= DateTime.UtcNow.Date);
RuleFor(x => x.EndDate).NotEmpty().Must((x, d) => d.Date >= x.StartDate.Date);
```

`CreateBannerDtoListValidator` applies the same rules per item via `RuleForEach` for the bulk endpoint.

**3. Domain guards** inside services (`KeyNotFoundException`, `InvalidOperationException("User has already reviewed this product.")`, stock checks) surface as `404`/`400` through the exception handler.

---

## Database & Persistence

| Concern | Implementation |
| --- | --- |
| Provider | SQL Server via `Microsoft.EntityFrameworkCore.SqlServer` 10 |
| Context | `AppDbContext : IdentityDbContext<User>` (27 entities + Identity tables) |
| Identity tables | Mapped to `Users`, `Roles`, `UserRoles` (`PhoneNumberConfirmed` ignored) |
| Migrations | 61 migrations in `DataAccessLayer/Migrations`, plus a full `Created Database script.sql` at the repo root |
| Relationships | Explicit `HasOne/WithMany` with named FK constraints (`FK_Products_BrandId`, etc.) |
| Delete behavior | **`Restrict` on every FK** (`ApplyDeleteRestrict`) |
| Global filters | `HasQueryFilter(e => !e.IsDeleted)` on `User`, `Product`, `ProductCategory`, `ProductSubCategory`, `Brand`, `City`, `ShippingCost`, `SellerProduct`, `ProductReview`, `UserAddress`, `Banner` |
| Default tracking | `ChangeTracker.QueryTrackingBehavior = NoTracking` in the context constructor |
| Transactions | `IUnitOfWork.BeginTransactionAsync/CommitTransactionAsync/RollbackTransactionAsync` |
| Pagination | `PaginationResult<T>` computed in SQL with `Skip/Take` and `CountAsync` |

**Indexes of note**

| Table | Index | Purpose |
| --- | --- | --- |
| `Products` | `NameEn`, `NameAr` | Accelerate bilingual name search |
| `ProductReviews` | `IX_ProductId_UserId` **unique** | One review per user per product |
| `UserAddresses` | `Ix_User_Default_Address` **unique filtered** (`IsDefault = 1 AND IsDeleted = 0`) | At most one default address per user |
| `ProductCategories` | `Name_En`, `Name_Ar` unique | Category name uniqueness |
| `Banners` | `DisplayOrder` | Stable banner ordering |

**Read optimizations** used across repositories: `AsNoTracking()` for read paths, `AsSplitQuery()` where an entity is loaded with multiple collection includes, `CountAsync` short-circuit before paging, DB-side `Skip/Take`, and projections for search/cache DTOs.

---

## Redis & Caching

Redis is used for **four distinct concerns** (via `IDistributedCache` / `AddStackExchangeRedisCache`):

| Concern | Key pattern | TTL | Read | Write / invalidate |
| --- | --- | --- | --- | --- |
| Product name index | `products:all` | `Redis:ProductsDurationInHours` (dev 12 h) / 24 h fallback | `ProductService.SearchByNameEnAsync` / `SearchByNameArAsync` | `UpdateProductsInRedisCacheAsync()` — **on cold miss and on the background schedule only** |
| City list | `cities:all` | default 24 h | `CityService` list/get | Rewritten on city create/update/delete |
| Recent searches | `recentSearches:{UserId}` | default 24 h | `GetRecentSearchesAsync` (last 10, deduped) | `AddRecentSearchAsync` |
| Idempotency | `idempotency:{Idempotency-Key}` | `InProgress` marker 5 s; cached response 5 min | `IdempotencyAttribute` | Written around the action; removed on non-2xx |

`RedisCashService` serializes values as JSON and logs + rethrows on failure. A missing key returns `default`.

**Scheduled refresh** — `ProductsCacheUpdateBackgroundService` runs immediately at startup and then every `Redis:ProductsDurationInHours` hours (dev: `12`), creating a DI scope and rewriting `products:all` (`List<ProductCashDto>` = `Id`, `NameEn`, `NameAr`).

> [!WARNING]
> **Cache invalidation is time-based, not event-based.** Product mutations do not touch `products:all`; changes become visible to the Redis-backed search path after the background refresh or TTL expiry. Note also that the two Redis-backed search methods currently have no live endpoint — the `lang`-header variant in `ProductsController` is commented out — while the active `GET /api/products/search` goes straight to SQL.

---

## Background Processing & Async Email

```text
Request thread                     Background worker
──────────────                     ────────────────
OtpService.AddNewOtpAsync
  └─ IOtpEmailQueue.EnQueueAsync ──► EmailOtpBackgroundService
                                       └─ IMailService.SendOtpEmailAsync (MailKit SMTP)

ApplicationOrderService
  └─ IUpdateOrderEmailQueue
        .EnQueueAsync ─────────────► UpdateOrderEmailBackgroundService
                                       └─ IMailService.SendUpdateOrderEmailAsync

ProductsCacheUpdateBackgroundService ──► ProductService.UpdateProductsInRedisCacheAsync (loop)
```

Queues are `System.Threading.Channels`-backed singletons (`BackgroundQueue<T>` with `Channel.CreateUnbounded<T>()`); the workers block on `ReadAsync`, create a scope per item, and log-and-continue on failure. Every order status transition (`UnderProcessing`, `Shipped`, `Delivered`, `Canceled`) enqueues a templated email.

---

## Logging & Observability

Serilog is wired through `BuilderExtensions.UseSerilog()` (`ReadFrom.Configuration` + `builder.Host.UseSerilog()`):

```json
"WriteTo": [
  { "Name": "Console" },
  { "Name": "MSSqlServer", "Args": { "tableName": "Logs", "autoCreateSqlTable": true,
      "restrictedToMinimumLevel": "Error" } }
],
"Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId", "WithProcessId" ]
```

- **Console**: `Information` (with `Microsoft` → `Warning`, `System` → `Error`).
- **SQL Server**: only `Error`-level events are persisted to the `Logs` table (auto-created).
- Enrichers add machine, thread, and process identifiers to every event.
- Structured logging with message templates is used in repositories, `UnitOfWork`, `RedisCashService`, `MailService`, background services, and the exception handler.
- There is **no** correlation-id middleware and no distributed tracing.

---

## Email System

| Piece | Detail |
| --- | --- |
| Library | **MailKit** `SmtpClient` — `ConnectAsync(host, port, StartTls)` → `AuthenticateAsync` → `SendAsync` |
| From | Display name `"Amazon E-Commerce"`, address from `Mail:Email` |
| Settings | `Mail:Email`, `Mail:AppPassword`, `Mail:Host`, `Mail:Port` (dev targets Ethereal SMTP) |
| Templates | `Templates/OtpEmailTemplate.html` (six `{{OTP_n}}` slots), `Templates/OrderUpdateEmailTemplate.html` (`{{Image}}`, `{{message}}`, `{{TrackOrderUrl}}`) |
| OTP email | Subject `Otp is: {code}`, plain text body + HTML template |
| Order status email | Subject/body built by `Helper.GeUpdateOrderEmailContect`, image from `ApiLayer/wwwroot/images/Order{Status}.png`, tracking link `{FrontEndBaseUrl}/{TrachOrderPath}/{ApplicationId}` |
| Delivery | Queued (OTP and order status) — never sent inline in a request; the return-application notice is sent synchronously |
| Lifetime | `IMailService` registered **Transient**; queues and workers are singletons |

Failures are logged and swallowed by the workers (the item is already dequeued).

---

## Configuration

`ApiLayer/appsettings.json` is empty (`{}`); all development configuration lives in `ApiLayer/appsettings.Development.json`. Secrets are supplied through **User Secrets** (the project has a `UserSecretsId`) or environment variables.

### Non-secret configuration

| Key | Purpose | Dev value |
| --- | --- | --- |
| `ConnectionStrings:sqlServerConnectionString` | SQL Server | `Server=.;Database=Amazon_E_Commerce_DB;…` |
| `Serilog:*` | Console + `Logs` table sinks | see above |
| `Redis:ConnectionString` / `Redis:InstanceName` | Redis | `localhost:6379` / `AmazonEcommerce_` |
| `Redis:ProductsDurationInHours` | Product cache refresh interval | `12` |
| `Jwt:Issuar` / `Jwt:Audience` / `Jwt:LifeTimeMin` | JWT validation & lifetime | `http://localhost:5157` / `http://localhost:5133` / `10` |
| `JwtRefreshToken:LifeTimeDays` | Refresh token + cookie lifetime | `20` |
| `Otp:LifeTimeMin` | OTP expiry | `10` |
| `RateLimitOption:PermitLimit` / `Window` / `QueueLimit` | Fixed-window limiter | `20` / `10` / `10` |
| `ApplicationSettings:BaseUrl` | Public API base for email images | ngrok URL in dev |
| `ApplicationSettings:FrontEndBaseUrl` | SPA origin for links | `http://localhost:5173` |
| `ApplicationSettings:TrachOrderPath` | Track-order path segment | `my-account/orders` |
| `Mail:Email` / `Mail:AppPassword` / `Mail:Host` / `Mail:Port` | SMTP | Ethereal test account |

### Secrets (User Secrets / environment variables — placeholders only)

```bash
dotnet user-secrets set "Jwt:SigningKey"        "<32+ byte random string>"   --project ApiLayer
dotnet user-secrets set "Jwt:EncryptionKey"     "<32 byte random string>"    --project ApiLayer
dotnet user-secrets set "Stripe:SecretKey"      "sk_test_…"                  --project ApiLayer
dotnet user-secrets set "Stripe:WebHookSecret"  "whsec_…"                    --project ApiLayer
dotnet user-secrets set "Cloudinary:CloudName"  "…"                          --project ApiLayer
dotnet user-secrets set "Cloudinary:ApiKey"     "…"                          --project ApiLayer
dotnet user-secrets set "Cloudinary:ApiSecret"  "…"                          --project ApiLayer
dotnet user-secrets set "Authentication:Github:ClientId"     "…"             --project ApiLayer
dotnet user-secrets set "Authentication:Github:ClientSecret" "…"             --project ApiLayer
dotnet user-secrets set "Authentication:Google:ClientId"     "…"             --project ApiLayer
dotnet user-secrets set "Authentication:Google:ClientSecret" "…"             --project ApiLayer
```

The application **exits at startup** if `ApplicationSettings`, `Jwt`, `Mail`, `Stripe`, `Cloudinary`, or `RateLimitOption` cannot be bound.

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server (LocalDB, Express, or a full instance) — or use the provided `Created Database script.sql`
- Redis server on `localhost:6379`
- Accounts/keys for [Stripe](https://stripe.com/), [Cloudinary](https://cloudinary.com/), and an SMTP provider (Ethereal works for development)
- Optional: GitHub and Google OAuth apps for external login

### Clone

```bash
git clone https://github.com/HamdyCS/Amazon_E_Commerce_Project.git
cd Amazon_E_Commerce_Project
```

### Configure

Set the connection string in `ApiLayer/appsettings.Development.json`, then store the secrets listed above with `dotnet user-secrets`.

### Database

```bash
dotnet ef database update --project DataAccessLayer --startup-project ApiLayer
```

### Run

```bash
dotnet run --project ApiLayer
```

Then open `http://localhost:5157/swagger` (HTTP profile) or `https://localhost:7027/swagger` (HTTPS profile).

> [!TIP]
> Development mail goes to Ethereal. Watch the console output for the Ethereal message URL to preview OTP and order-status emails. For GitHub/Google callbacks and Stripe webhooks during local development, expose the API through a tunnel (the configured `ApplicationSettings:BaseUrl` is an ngrok URL).

---

## Project Structure

```text
Amazon_E_Commerce_Project/
├── Amazon_E_Commerce_Project.sln
├── Created Database script.sql          # Full DB script (alternative to migrations)
├── sql scripts/                         # Ad-hoc SQL utilities
│
├── ApiLayer/                            # Presentation + composition root
│   ├── Controllers/                     # 25 controllers
│   ├── Extensions/                      # ServiceExtensions (DI), BuilderExtensions (Serilog), AppExtensions
│   ├── Filters/                         # CheckIfUserIsNotDeletedFilter, IdempotencyAttribute
│   ├── Help/                            # Claim/URL/metadata helpers
│   ├── MiddleWares/                     # Alternate (unregistered) deleted-user middleware
│   ├── wwwroot/images/                  # Order-status images used in emails
│   ├── Program.cs                       # Bootstrap, options binding, middleware pipeline
│   ├── appsettings.Development.json
│   └── Properties/launchSettings.json
│
├── BusinessLayer/                       # Application + domain logic
│   ├── BackgroundServices/              # ProductsCacheUpdate, EmailOtp, UpdateOrderEmail workers
│   ├── Contracks/                       # 36 service interfaces
│   ├── Services/                        # 35 service implementations
│   ├── Dtos/                            # 59 DTOs
│   ├── Mapper/                          # GenericMapper + 30 AutoMapper profiles
│   ├── Validations/                     # FluentValidation validators (banners)
│   ├── Options/                         # Jwt / Mail / Stripe / Cloudinary / RateLimit / Application options
│   ├── Roles/                           # Role constants
│   ├── Templates/                       # OTP + order-update HTML emails
│   ├── Exceptions/                      # GlobalExceptionHandler, ParamaterException
│   ├── Help/                            # Email content, OTP, image URL helpers
│   └── Enums/                           # EnProvider
│
└── DataAccessLayer/                     # Persistence + domain model
    ├── Contracks/                       # 28 repository interfaces
    ├── Repositories/                    # 28 repositories (incl. GenericRepository<T>)
    ├── Data/                            # AppDbContext
    ├── Entities/                        # 27 entities
    ├── Identity/Entities/               # User : IdentityUser
    ├── Enums/                           # Application/payment/refund/language enums
    ├── Migrations/                      # 61 EF Core migrations
    ├── Pagination/                      # PaginationResult<T>
    ├── UnitOfWork/                      # UnitOfWork + IUnitOfWork
    ├── Validations/                     # PersonValidtion (18+ age rule)
    ├── Exceptions/                      # ParamaterException
    └── Help/                            # Language detection helper
```

---

## Important Business Flows

### Registration with OTP

```text
Client                    API                          Database              SMTP
  │  POST /send-otp        │                             │                      │
  │───────────────────────►│ generate 6 digits          │                      │
  │                        │ persist (10 min expiry) ──► │                      │
  │                        │ enqueue ─────────────────────────────────────────► │
  │◄────── 200 ────────────│                             │                      │
  │                        │            (worker dequeues, MailKit sends)        │
  │  POST /register-*      │                             │                      │
  │  { otp, userDto } ────►│ BEGIN TX                    │                      │
  │                        │ validate OTP ──────────────►│                      │
  │                        │ reject if email exists      │                      │
  │                        │ INSERT Person ─────────────►│                      │
  │                        │ UserManager.CreateAsync ───►│ (Identity hash)      │
  │                        │ mark OTP used ─────────────►│                      │
  │                        │ COMMIT                      │                      │
  │◄────── 200 + cookies ──│  (customer only)            │                      │
```

### Pre-paid checkout

```text
Customer ──POST /payments/pre-paid (+Idempotency-Key)──► PaymentService
                                                          │ BEGIN TX
                                                          │ validate active cart / address / shipping cost
                                                          │ INSERT Payment (PrePaid, Pending)
                                                          │ StripeService.CreateSessionAsync
                                                          │ UPDATE Payment.SessionId
                                                          │ COMMIT TX
        ◄────────── { sessionUrl, sessionId } ────────────┘
   │
   └─► redirect to Stripe ──► success/cancel URL with ?sessionId=
                                  │
Stripe ──POST /api/stripe (signature verified)──► payment_intent.succeeded
                                                    │ BEGIN TX
                                                    │ UPDATE Payment (Succeeded, InvoiceId = PaymentIntentId)
                                                    │ ├─ stock decrement (guarded raw UPDATE)
                                                    │ │    └─ on failure → RefundStatus=Pending + Stripe refund
                                                    │ ├─ INSERT Application (Order, ETA +2/+5 days)
                                                    │ ├─ INSERT ApplicationOrder (UnderProcessing) + email
                                                    │ └─ UPDATE ShoppingCart.IsActive = false
                                                    │ COMMIT TX
```

### Cash-on-delivery checkout

```text
Customer ──POST /payments/cash-on-delivery (+Idempotency-Key)──►
   BEGIN TX
     validate cart/address/shipping cost
     decrement stock (guarded UPDATE)
     INSERT Payment (CashOnDelivery, Pending)
     INSERT Application (Order) + ApplicationOrder (UnderProcessing)  + email
     deactivate cart
   COMMIT TX  ──► { applicationId }
        │
Admin  ──POST .../shipping-application-orders──► ApplicationOrder (Shipped) + DeliveryId + email
Admin  ──POST .../delivered-application-orders─► ApplicationOrder (Delivered)
                                                 └─ if COD → PaymentStatus = Succeeded
Customer ──POST /applications/{id}/cancel───────► ApplicationOrder (Canceled) + stock restored + email
```

### Review gating

```text
POST /products/{id}/reviews
   → user authenticated?
   → has a Delivered order containing this product?   (else 400 "User has not bought this product.")
   → already reviewed?                                 (else 400 "User has already reviewed this product.")
   → BEGIN TX: INSERT ProductReview → recompute Product.AvgRating / RatingCount → COMMIT
```

---

## Technical Highlights

| Highlight | Engineering value |
| --- | --- |
| **Append-only order status log** | Status history, tracking, and "active row" resolution are queries, not mutable state — no lost updates on status. |
| **Guarded raw SQL stock updates** | `UPDATE … SET NumberInStock = NumberInStock - @q WHERE NumberInStock >= @q` gives a concurrency-safe decrement without pessimistic locks; the compensating Stripe refund handles the failure case. |
| **Dual-mode checkout** | One cart/payment model supports both immediate (COD) and asynchronous (Stripe webhook) order materialization with the same domain entities. |
| **Idempotency filter over Redis** | Converts non-idempotent money endpoints into retryable ones with an in-progress guard and a replayed response. |
| **Signed + encrypted JWT in HttpOnly cookies** | Tokens are never readable by JS, and the payload is both HMAC-signed and AES-encrypted. |
| **DB-backed refresh tokens** | Logout revokes every session server-side; a stolen refresh token is useless once the user logs out. |
| **Channel-based mail queue** | SMTP latency and failures are removed from the request path while keeping ordering per queue. |
| **Reflection-based soft delete** | One `GenericRepository.DeleteAsync` implementation serves every entity that has `IsDeleted`/`DateOfDeletion`, and falls back to a hard delete otherwise. |
| **Filtered unique index for default address** | The "one default address" rule is enforced by SQL, not only by application code. |
| **Dual-language catalog with auto-detection** | Search switches projection (`NameEn`/`NameAr`) from the query itself by scanning the Arabic Unicode block. |
| **Bilingual domain data** | `NameAr`/`NameEn` and `DescriptionAr`/`DescriptionEn` on products, categories, brands, cities, and reference rows. |
| **Background cache warming** | The product name index is refreshed on a schedule instead of on every mutation, trading bounded staleness for a simple, predictable invalidation story. |
| **Fail-fast options binding** | Missing required sections terminate startup rather than surfacing as runtime null refs. |

---

## Performance Considerations

- **`NoTracking` by default** — the `AppDbContext` constructor sets `QueryTrackingBehavior.NoTracking`, so read-only queries do not populate the change tracker unless explicitly requested.
- **`AsNoTracking()`** used explicitly across generic and specialized read paths.
- **`AsSplitQuery()`** wherever a seller offer is loaded with both `ProductImages` and `ProductReviews` collections, avoiding the cartesian explosion of joins.
- **Server-side pagination** — `Skip((pageNumber-1)*pageSize).Take(pageSize)` with a separate `CountAsync`, and `OFFSET/FETCH` in the raw best-seller SQL.
- **Index-backed search** — `Products.NameEn` / `NameAr` indexes exist specifically for name lookups; `ProductReviews(ProductId, UserId)` unique index backs review de-duplication.
- **Redis** removes SQL round-trips for the product-name index, city list, recent searches, and idempotency replay.
- **Set-based stock updates** — one `ExecuteSqlAsync` per line instead of read-modify-write round-trips.
- **Queued SMTP** — no network I/O on the request thread for OTP or status emails.
- **Count short-circuit** — paged/filter queries return an empty page when `CountAsync` is `0`, skipping the data query.
- **Rate limiting** protects the API from bursts at the edge of the pipeline.

Known hotspots (documented honestly, not optimizations): several catalog list endpoints load product images with a per-row query; the Redis product index has no event-driven invalidation; and the cancellation path issues a rollback without a matching begin.

---

## Security Considerations

**Implemented**

- Identity password hashing; passwords never returned or logged.
- JWT signed (HMAC-SHA256) **and** encrypted (AES), `ClockSkew = Zero`, issuer/audience/lifetime validated.
- `HttpOnly` + `Secure` + `SameSite=None` cookies for both tokens — no token in JS-reachable storage.
- Server-side refresh-token revocation on logout.
- OTP with expiry and single-use consumption for registration and password reset.
- Email-change requires a valid OTP and checks for address collisions.
- Role-based endpoint authorization plus per-resource ownership checks in queries.
- Soft-deleted accounts rejected by a global filter even with a valid JWT.
- OAuth return-URL allow-list; provider errors surfaced as `400`.
- Rate limiting per IP (`429`), idempotency keys on payment endpoints.
- Stripe webhook signature verification before any side effect.
- CORS restricted to the configured front-end origin with credentials.
- Required secrets kept out of source control (User Secrets); startup fails when they are missing.
- All FKs `Restrict` — no accidental cascading deletes.

**Not implemented / to strengthen**

- No CSRF token layer (mitigated by `SameSite=None` cookies only insofar as the origin is trusted).
- No refresh-token rotation on use (the same token remains valid until expiry or logout).
- No password complexity options beyond Identity defaults (`6–10` chars enforced on DTOs).
- No request body size limits, no HTTPS-only enforcement in Development.
- Webhook handling is not idempotent — a replayed `payment_intent.succeeded` would re-run its side effects.

---

## Testing

There is **no test project** in this solution (the `.sln` contains only `ApiLayer`, `BusinessLayer`, and `DataAccessLayer`). Verification is currently manual via Swagger UI and the running front end. See [Future Improvements](#future-improvements).

---

## API Documentation / Swagger

- Provided by **Swashbuckle** (`AddSwaggerGen()` + `AddEndpointsApiExplorer()`).
- Enabled only when `ASPNETCORE_ENVIRONMENT=Development`; `app.UseSwagger()` / `app.UseSwaggerUI()` are guarded by `app.Environment.IsDevelopment()`.
- Default URL: `/swagger`.
- Authentication is cookie-based, so to call protected operations from Swagger you must obtain cookies first (`POST /api/authentication/login` with *Enable credentials* / a browser session) — there is **no** `ApiKey`/`Bearer` security definition configured in Swagger.

---

## Frontend Integration

Verified from configuration and code:

| Concern | Value |
| --- | --- |
| Front-end origin | `http://localhost:5173` (CORS policy `AllowWebsite`, `AllowAnyMethod/AllowAnyHeader`, **`AllowCredentials`**) |
| Auth transport | Cookies `access_token` + `refresh_token` — the SPA must send `credentials: 'include'` |
| Refresh strategy | Call `POST /api/authentication/refresh-token` when the access token expires (10 min in dev) |
| External login | SPA hits `/api/authentication/login/customer/{github,google}?returnUrl=<allowed origin>/…` and is redirected back with cookies set |
| Order emails | Links built from `ApplicationSettings:FrontEndBaseUrl` + `TrachOrderPath` + application id |
| Checkout return | SPA supplies `successUrl` / `cancelUrl` and reads `?sessionId=` from the query string |
| Real time | None — there is no SignalR hub; the SPA must poll for new state |

---

## Trade-offs & Design Decisions

| Decision | Trade-off |
| --- | --- |
| **Layered architecture instead of Clean Architecture / CQRS** | Simpler to navigate and fewer projects to maintain, but business logic sits in classes that reference EF entities directly, so the domain is not persistence-ignorant. |
| **Append-only `ApplicationOrder` rows** | Full, cheap history and simple "active status" queries, at the cost of a slightly more awkward write path and no in-place status edits. |
| **JWT in HttpOnly cookies instead of `Authorization` headers** | XSS cannot exfiltrate tokens and the browser manages expiry, but the API becomes origin-dependent (CORS with credentials, `SameSite=None`) and Swagger needs a cookie session. |
| **DB-backed refresh tokens** | Enables real server-side revocation on logout, at the price of a DB read on every refresh. |
| **Stock decrement only at payment time** | No phantom reservations while browsing, so overselling is prevented exactly at the transaction that matters — but a user can sit in a cart with items that sell out. |
| **Time-scheduled cache refresh instead of write-through invalidation** | Very simple and load-friendly; accepts up to one refresh interval of staleness on the Redis product index. |
| **Redis idempotency keys instead of Stripe idempotency keys** | Works uniformly across both payment methods and replays the exact response; requires Redis to be available for checkout. |
| **`Channel`-based in-process mail queue** | Zero infrastructure cost and ordered processing, but messages are lost on process crash and the queue does not survive a restart. |
| **Reflection-based soft delete in the generic repository** | One implementation for every entity, with a compile-time-unsafe string property lookup and a silent hard-delete fallback. |
| **`DeleteBehavior.Restrict` globally** | Prevents accidental data loss, but every cleanup must be handled explicitly in code. |
| **Server-side email templating with MailKit** | Full control over content and no third-party email API dependency, but you own deliverability and template maintenance. |
| **Fixed-window rate limiting by IP** | Cheap and effective against bursts; less fair than a sliding window and inaccurate behind a proxy that doesn't forward the real client IP. |

---

## Future Improvements

Clearly labelled as *not yet implemented*:

- Unit and integration test projects (repository, service, and `WebApplicationFactory` endpoint tests).
- Dockerfile + `docker-compose` for SQL Server, Redis, and the API.
- CI/CD pipeline and automated migrations on deploy.
- Event-driven cache invalidation for `products:all`.
- Refresh-token rotation and reuse detection.
- Webhook event-id deduplication for idempotent Stripe processing.
- Distributed tracing / correlation IDs across requests and background workers.
- Swagger cookie-authentication definition for one-click authenticated testing.
- Sliding-window or token-bucket rate limiting, plus per-user limits behind the proxy.
- Centralized `ProblemDetails` responses across all controllers instead of mixed `try/catch` messages.
- SignalR for live order-status updates.
- Cleanup of the currently unused/alternate code paths (unregistered middleware, commented Redis search action, unused DTOs).
