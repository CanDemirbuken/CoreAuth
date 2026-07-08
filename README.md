# CoreAuth

![.NET](https://img.shields.io/badge/.NET-10-blueviolet)
![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-success)
![JWT](https://img.shields.io/badge/Auth-JWT-orange)
![Identity](https://img.shields.io/badge/Identity-ASP.NET%20Core-blue)
![License](https://img.shields.io/badge/License-MIT-green)

CoreAuth is a production-oriented authentication and authorization API built with **ASP.NET Core 10** and **Clean Architecture**. It is designed as a reusable foundation for future backend projects, providing a secure, maintainable, and extensible authentication infrastructure.

## 🚀 Features

- User Registration
- User Login
- JWT Access Token Authentication
- Refresh Token Authentication
- Refresh Token Rotation
- Refresh Token Revocation
- Role-Based Authorization
- ASP.NET Core Identity Integration
- Clean Architecture
- Repository Pattern
- Unit of Work Pattern
- Service Result Pattern
- Dependency Injection
- Swagger Integration

## 🏗️ Architecture

The solution follows the principles of **Clean Architecture** and is organized into the following layers:

```
CoreAuth.API
CoreAuth.Application
CoreAuth.Domain
CoreAuth.Persistence
CoreAuth.Infrastructure
```

### Responsibilities

- **API** → Controllers, Swagger configuration, Dependency Injection
- **Application** → Business logic, DTOs, Interfaces, Services
- **Domain** → Entities and core business models
- **Persistence** → Entity Framework Core, Repositories, Database access
- **Infrastructure** → Identity, JWT, External services

---

## 🔐 Authentication Flow

```
User Login
      │
      ▼
Generate Access Token
Generate Refresh Token
      │
      ▼
Store Refresh Token
      │
      ▼
Access Token Expires
      │
      ▼
POST /api/auth/refresh-token
      │
      ▼
Validate Refresh Token
Revoke Old Refresh Token
Generate New Token Pair
```

---

## 📦 Technologies

- ASP.NET Core 10
- Entity Framework Core
- ASP.NET Core Identity
- SQL Server
- JWT Authentication
- Clean Architecture
- Swagger / OpenAPI

---

## 📌 Current Status

✅ User Registration

✅ User Login

✅ JWT Authentication

✅ Refresh Token Lifecycle

✅ Refresh Token Rotation

✅ Refresh Token Revocation

✅ Role-Based Authorization

✅ Clean Architecture Implementation

---

## 🔮 Planned Improvements

- Refresh Token Hashing
- Refresh Token Reuse Detection
- Device & IP Tracking
- Transaction Handling
- Audit Logging
- Automatic Expired Token Cleanup

---

## 📖 Purpose

This project was created as a reusable authentication infrastructure for future ASP.NET Core applications. Instead of implementing authentication from scratch for every new project, CoreAuth serves as a production-ready starting point following modern software architecture principles.
