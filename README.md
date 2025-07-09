# life-roadmap
## 🏗️ Architecture Overview

![Architecture Diagram](docs/architecture.png)

```plaintext
life-roadmap/
│
├── api/
│   ├── LifeRoadmap.Application/             ← Core business logic, use cases, DTOs, contracts (depends on Domain)
│   │   ├── Configuration/
│   │   │   └── Mapping/                      ← AutoMapper profiles (Domain / DTO)
│   │   ├── DTOs/                             ← Data Transfer Objects
│   │   ├── Interfaces/
│   │   │   ├── Persistence/                  ← Repository related interfaces
│   │   │   │   └── Repositories/             
│   │   │   └── Services/                     ← Interfaces for services
│   │   └── UseCases/                         ← Use cases
│   │
│   ├── LifeRoadmap.Domain/                  ← Core domain layer (no dependencies)
│   │   ├── Enums/                            ← Domain enums
│   │   └── Models/                           ← Domain models
│   │
│   ├── LifeRoadmap.Infrastructure/          ← Infrastructure logic and data access (depends on Domain, Application and Database)
│   │   ├── Configuration/
│   │   │   └── Mapping/                      ← AutoMapper profile (Entity / Domain)
│   │   ├── Persistence/
│   │   │   └── Repositories/                 ← Repository implementations
│   │   └── Services/                         ← Service implementations for business logic
│   │
│   ├── LifeRoadmap.Database/                ← EF Core (Database-First) layer (no dependencies)
│   │   └── Entities/                         ← Scaffolded entities from the SQL Server database
│   │
│   └── LifeRoadmap.WebApi/                  ← HTTP API layer (entry point) (depends on Application and Infrastructure)
│       ├── Controllers/                      ← HTTP endpoints (API controllers)
│       ├── Exceptions/                       ← Exceptions
│       ├── Middleware/                       ← Cross-cutting middlewares
│       └── Program.cs                        ← App startup and configuration
│
├── db/
│   └── LifeRoadmap.DatabaseProject/         ← SQL Server Database Project (.sql schema scripts)
│
└── client/                                  ← Angular frontend
```

### Layers description

- **Domain** – The core of the application. Contains business models and enums. Pure logic, no dependencies.

- **Database** – EF Core scaffolded entities and DbContext generated from SQL Server (Database-First approach). No logic.

- **Application** – Depends on Domain. Contains use cases, DTOs, and interfaces for services, repositories, etc. Pure logic, no infrastructure.

- **Infrastructure** – Depends on Application, Domain, and Database. Provides implementations for interfaces (e.g. repositories, services), and maps EF entities to Domain models or DTOs.

- **WebApi** – Depends on Application and Infrastructure. Handles HTTP requests, DI setup, middleware, and endpoints.

- **SQL Server Database** – Managed via Database Project. Tables, constraints, and changes are maintained and versioned here.

- **Client (Angular)** – Frontend SPA that communicates with WebApi via HTTP.

---

## 🚀 Getting Started

After cloning the repository, run the following commands to build and start the backend and frontend:

### Backend (.NET API)

```bash
cd api/LifeRoadmap.WebApi
dotnet restore
dotnet build

# Optional:
# Run the scaffold script to update EF Core models from the database.
# Make sure you have installed dotnet-ef as a global tool:
#   dotnet tool install --global dotnet-ef
dotnet msbuild -t:ScaffoldDb

dotnet run
```

### Frontend (Angular)

```bash
cd client
npm install
npm start
```


The backend will be available at https://localhost:5001,
and Angular frontend at http://localhost:4200.