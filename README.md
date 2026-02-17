# 🎵 CodeNight Backend

CodeNight is a **production-grade REST API** that analyzes music listening activity and provides a complete **challenge, badge, and leaderboard** system.

The project focuses on **clean architecture, scalability, and high code quality**, making it suitable for real-world backend applications.

---

## 🏛️ Architecture

- **Clean Architecture**
  - Domain  
  - Application  
  - Infrastructure  
  - WebApi
- **CQRS** with **MediatR**
- **SOLID**, **DRY**, **KISS** principles

---

## 🚀 Features

### Infrastructure
- Docker Compose setup (**PostgreSQL 16**, **.NET 8 Web API**, **SonarQube**)
- Entity Framework Core with Fluent API  
  - 13 tables  
  - Composite keys  
  - Indexes  
  - `snake_case` naming
- Automatic database migrations on startup
- CSV-based normalized seed data
- Global exception handling middleware
- FluentValidation pipeline behavior
- Swagger / OpenAPI documentation
- Health check endpoint with background logging

### API Capabilities (14 Endpoints)
- Dashboard overview
- User listing with search, pagination, and sorting
- User details with computed state
- Challenge results and awards
- Points ledger (time-series data for charts)
- Badge system
- Notification system
- Top-N leaderboard
- Challenge CRUD (Create / Update / List)
- Idempotent processing pipeline
- *What-if* user simulation
- Aggregated statistics (e.g. top genres)

---

## 🔄 Processing Pipeline

1. User state calculation (daily, 7-day, streak)
2. Challenge evaluation (rule/condition parsing)
3. Award selection (single-award rule with priority)
4. Points ledger recording
5. Badge evaluation and awarding
6. Notification creation

---

## 📊 Code Quality

**SonarQube Quality Gate: PASSED**

- Reliability: **A** (0 Bugs)
- Security: **A** (0 Vulnerabilities)
- Maintainability: **A**

---

## 🧪 Running Locally

```bash
docker compose up -d --build
