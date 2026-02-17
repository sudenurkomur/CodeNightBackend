# 🎵 CodeNight — Hackathon Backend API

**Müzik dinleme aktivitelerinden metrik hesaplayan, challenge/badge/leaderboard sistemi sunan production-kalitesinde REST API.**

---

## 🏗️ Mimari

```
src/
├── Domain/          → Entity, Enum, Constant (sıfır bağımlılık)
├── Application/     → CQRS (Command/Query + Handler), DTO, Validator, Interface
├── Infrastructure/  → EF Core DbContext, Fluent API Configuration, Migration
└── WebApi/          → Controller, Middleware, Health Check, Swagger
```

**Clean Architecture** — Bağımlılık yönü dıştan içe. Application katmanında EF Core bağımlılığı yok.

---

## 🛠️ Teknolojiler

| Teknoloji | Amaç |
|-----------|-------|
| .NET 8 | Runtime + Web API |
| PostgreSQL 16 | Veritabanı |
| Entity Framework Core | ORM (Npgsql) |
| MediatR | CQRS pattern |
| FluentValidation | Request validation pipeline |
| Docker & Docker Compose | Containerization |
| SonarQube | Kod kalitesi (A/A/A rating) |
| Swagger / OpenAPI | API dokümantasyonu |

---

## 🚀 Hızlı Başlangıç

```bash
# 1. Repo'yu klonla
git clone https://github.com/sudenurkomur/CodeNightBackend.git
cd CodeNightBackend

# 2. .env dosyası oluştur
cat > .env << EOF
POSTGRES_USER=codenight
POSTGRES_PASSWORD=codenight123
POSTGRES_DB=codenightdb
EOF

# 3. Docker ile ayağa kaldır
docker compose up -d --build

# 4. Swagger'ı aç
open http://localhost:8088/swagger

# 5. Health check kontrol
curl http://localhost:8088/health
```

> Migration otomatik çalışır, ayrı komut gerekmez.

---

## 📡 API Endpoint'ler

Base URL: `/api/v1`

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | `/dashboard?as_of_date=` | Özet dashboard |
| GET | `/users?as_of_date=&q=&limit=&sort=` | Kullanıcı listesi |
| GET | `/users/{id}?as_of_date=` | Kullanıcı detay + state |
| GET | `/users/{id}/challenge-awards` | Challenge sonuçları |
| GET | `/users/{id}/points-ledger` | Puan geçmişi |
| GET | `/users/{id}/badges` | Rozetler |
| GET | `/users/{id}/notifications` | Bildirimler |
| GET | `/leaderboard?as_of_date=&limit=` | Sıralama |
| GET | `/challenges` | Challenge listesi |
| POST | `/challenges` | Challenge oluştur |
| PATCH | `/challenges/{id}` | Challenge güncelle |
| POST | `/processing/run?asOfDate=` | İşlem pipeline (idempotent) |
| POST | `/what-if/users/{id}` | What-if simülasyonu |
| GET | `/stats/top-genres?as_of_date=` | Genre dağılımı |

---

## 🔄 İşlem Pipeline (`POST /processing/run`)

```
User State Hesapla → Challenge Evaluate → Award Seç (priority) 
→ Points Ledger → Badge Kontrol → Notification Oluştur
```

- **İdempotent:** Aynı gün tekrar çalıştırılırsa duplicate üretmez
- **Tek ödül kuralı:** Aynı gün birden fazla challenge tetiklenirse, priority en düşük olan seçilir

---

## 📐 Prensipler

- **SOLID** — Single Responsibility handler'lar, Interface Segregation, Dependency Inversion
- **DRY** — Constant sınıfları, ortak ApiResponse wrapper, tek ValidationBehavior pipeline
- **KISS** — Gereksiz abstraction yok, en basit doğru çözüm
- **CQRS** — Command (yazma) ve Query (okuma) ayrımı MediatR ile

---

## 🏥 Health Check

- `GET /health` → API + PostgreSQL durumu
- Docker Compose health check (30s interval)
- Background logger: 10 dakikada bir durum loglar

---

## 📊 Kod Kalitesi (SonarQube)

```
✅ Quality Gate: PASSED
✅ Reliability: A (0 Bug)
✅ Security: A (0 Vulnerability)
✅ Maintainability: A
```

---

## 🗄️ Veritabanı Şeması (13 Tablo)

```
users ──┬── events
        ├── user_states (1-1)
        ├── challenge_awards ── triggered_challenges ── challenges
        │         └── challenge_decisions
        ├── points_ledger
        ├── badge_awards ── badges
        └── notifications

artists ── tracks
```

---

## 🐳 Docker Servisleri

| Servis | Port | Açıklama |
|--------|------|----------|
| postgres | 5434 | PostgreSQL 16 (volume kalıcı) |
| webapi | 8088 | .NET 8 API |
| sonarqube | 9000 | Kod kalitesi |

---

## 👥 Ekip

Hackathon projesi — Backend: .NET 8 Clean Architecture
