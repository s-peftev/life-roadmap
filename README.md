# life-roadmap
## 🏗️ Architecture Overview

```plaintext
Solution 'LifeRoadmap'

src
├── Infrastructure
│   ├── LR.Infrastructure
│   │   ├── DependencyInjection     // DI resolvers and registration logic
│   │   │   └── Resolvers           
│   │   ├── Extensions              // Extension methods for infrastructure setup
│   │   └── Utils                   // Infrastructure-related utility classes
│   ├── LR.Mapping
│   │   ├── DtoToEntity             // Mapping logic from DTOs to domain entities
│   │   └── EntityToDto             // Mapping logic from domain entities to DTOs
│   └── LR.Persistance
│       └── Repositories            // Concrete implementations of domain repositories

├── LR.API
│   ├── Properties                  // Assembly metadata (e.g., launchSettings.json)
│   ├── Controllers                 // API endpoints
│   ├── Exceptions                  // Custom API exception types
│   ├── Filters                     // Global filters like error handling or validation
│   ├── Middleware                  // Custom HTTP middleware
│   ├── Models                      // API request/response models
│   │   ├── RequestModels
│   │   └── ResponseModels
│   ├── appsettings.json            // App configuration
│   └── Program.cs                  // Application startup logic

├── LR.Application
│   ├── DTOs                        // Data Transfer Objects used between layers
│   ├── Interfaces
│   │   ├── Services                // Application service contracts
│   │   └── Utils                   // Utility service interfaces (e.g., token generators)
│   ├── Services                    // Application service implementations
│   └── Validators                  // Input validation logic (e.g., FluentValidation)

├── LR.Domain
│   ├── Constants                   // Domain-level constant values
│   ├── Entities                    // Core domain models
│   ├── Enums                       // Domain-related enumerations
│   ├── Interfaces
│   │   └── Repositories            // Repository interfaces (contracts)
│   └── ValueObjects                // Immutable types with value-based equality

├── LR.DatabaseProject              // SQL Server Database Project (.sql schema scripts)

└── LR.Client                       // Angular frontend
```

The backend will be available at https://localhost:5001,
and Angular frontend at http://localhost:4200.