# ?? PetWorld - AI-Powered Pet Shop Assistant

> **Recruitment Task**: Clean Architecture + Blazor Server + Writer-Critic AI Pattern

Professional pet shop chatbot application built with .NET 10, Blazor Server, and OpenAI integration, demonstrating modern software architecture principles.

---

## ?? Table of Contents
- [Overview](#overview)
- [Technologies](#technologies)
- [Architecture](#architecture)
- [Features](#features)
- [Quick Start](#quick-start)
- [Configuration](#configuration)
- [Project Structure](#project-structure)

---

## ?? Overview

PetWorld is a production-ready web application that enables customers to ask questions about pet products through an AI-powered chat interface. The system uses a **Writer-Critic pattern** where:
- **Writer Agent** generates customer-facing responses
- **Critic Agent** evaluates response quality
- **Orchestrator** iterates up to 3 times for optimal answers

### Live Demo
Once deployed, the application runs at: `http://localhost:5000`

---

## ?? Technologies

| Category | Technology | Version |
|----------|-----------|---------|
| **Framework** | .NET | 10.0 |
| **UI** | Blazor Server | .NET 10 |
| **Database** | MySQL | 8.0 |
| **ORM** | Entity Framework Core | 9.0 |
| **AI** | OpenAI API | GPT-4o |
| **Containerization** | Docker | Latest |
| **Architecture** | Clean/Onion Architecture | - |

---

## ?? Architecture

### Clean Architecture (Onion)
```
???????????????????????????????????????????
?         PetWorld.Web (UI)               ?  ? Blazor Server, Pages, Components
???????????????????????????????????????????
?   PetWorld.Infrastructure               ?  ? EF Core, Repositories, AI Service
???????????????????????????????????????????
?   PetWorld.Application                  ?  ? Business Logic, DTOs, Services
???????????????????????????????????????????
?   PetWorld.Domain (Core)                ?  ? Entities, Interfaces (no dependencies)
???????????????????????????????????????????
```

**Dependency Flow**: Domain ? Application ? Infrastructure ? Web

### Writer-Critic AI Pattern
```
User Question
    ?
[Writer Agent] ? Generates answer based on product catalog
    ?
[Critic Agent] ? Evaluates: {approved: true/false, feedback: "..."}
    ?
Approved? ? Return answer + iteration count
Not Approved? ? Writer improves (max 3 iterations)
```

---

## ? Features

### 1. Chat Interface (`/chat`)
- ? Text input for customer questions
- ? Real-time AI responses with product recommendations
- ? Displays iteration count (1-3)
- ? Loading spinner during processing
- ? Error handling

### 2. History View (`/history`)
- ? DataGrid with all conversations
- ? Columns: Date, Question, Answer, Iteration Count
- ? Newest first ordering
- ? Persistent storage in MySQL

### 3. Product Catalog
10 pre-seeded products across categories:
- Dog food & accessories
- Cat food & accessories
- Aquarium supplies
- Small animals (rodents)

---

## ?? Quick Start

### Prerequisites
- **Docker Desktop** installed and running
- **OpenAI API Key** from https://platform.openai.com/

### Step 1: Clone Repository
```bash
git clone https://github.com/YOUR_USERNAME/PetWorld.git
cd PetWorld
```

### Step 2: Configure OpenAI API Key
Create a `.env` file in the project root:
```bash
OPENAI_API_KEY=sk-your-actual-api-key-here
```

### Step 3: Run Application
```bash
docker compose up
```

Wait for:
- MySQL to become healthy (~10 seconds)
- Application to start (~30 seconds)
- Console message: `Now listening on: http://[::]:8080`

### Step 4: Access Application
Open browser: **http://localhost:5000**

### Step 5: Test Chat
1. Navigate to **Chat** page
2. Enter question: *"Jaka karma jest najlepsza dla mojego kota?"*
3. Click **Wyœlij** (Send)
4. Wait 10-30 seconds for AI response
5. View answer + iteration count

### Step 6: Check History
1. Navigate to **Historia** (History)
2. Verify conversation is saved

---

## ?? Configuration

### Environment Variables
| Variable | Description | Default | Required |
|----------|-------------|---------|----------|
| `OPENAI_API_KEY` | OpenAI API key | - | ? Yes |
| `OPENAI_MODEL_NAME` | GPT model | `gpt-4o` | ? No |

### Connection Strings
**Docker (default)**:
```
Server=mysql;Port=3306;Database=petworld;User=root;Password=petworld123;Charset=utf8mb4;
```

**Local Development**:
```
Server=localhost;Port=3306;Database=petworld;User=root;Password=yourpassword;Charset=utf8mb4;
```

### Ports
| Service | Internal | External |
|---------|----------|----------|
| Web App | 8080 | 5000 |
| MySQL | 3306 | 3306 |

---

## ?? Project Structure

```
PetWorld/
??? src/
?   ??? PetWorld.Domain/
?   ?   ??? Entities/           # Product, ChatHistory
?   ?   ??? Interfaces/         # Repository & Service contracts
?   ?
?   ??? PetWorld.Application/
?   ?   ??? DTOs/               # Data Transfer Objects
?   ?   ??? Services/           # ChatService (orchestration)
?   ?
?   ??? PetWorld.Infrastructure/
?   ?   ??? Data/               # DbContext, DbInitializer
?   ?   ??? Repositories/       # EF Core implementations
?   ?   ??? Services/           # AiChatService (Writer-Critic)
?   ?
?   ??? PetWorld.Web/
?       ??? Components/
?       ?   ??? Pages/          # Chat.razor, History.razor
?       ?   ??? Layout/         # NavMenu, MainLayout
?       ??? Program.cs          # DI configuration
?       ??? appsettings.json    # Configuration
?
??? Dockerfile                  # Multi-stage .NET build
??? docker-compose.yml          # MySQL + Web services
??? .env                        # API key (git-ignored)
??? .gitignore                  # Excludes secrets
??? README.md                   # This file
```

---

## ?? Development

### Build Locally
```bash
# Build solution
dotnet build

# Run web project (requires local MySQL)
cd src/PetWorld.Web
dotnet run
```

### Docker Commands
```bash
# Build images
docker compose build

# Start in background
docker compose up -d

# View logs
docker compose logs -f web

# Stop and remove volumes
docker compose down -v
```

### Database
```bash
# Connect to MySQL
docker exec -it petworld-mysql mysql -uroot -ppetworld123 petworld

# Check products
SELECT * FROM Products;

# Check chat history
SELECT * FROM ChatHistories ORDER BY CreatedAt DESC LIMIT 10;
```

---

## ?? Key Implementation Details

### 1. Writer-Critic Implementation
- **Max 3 iterations** as per requirements
- **HTTP-based** OpenAI integration (no Azure SDK)
- **Fail-safe**: If Critic fails to parse, answer is approved
- **Stateless**: Each request is independent

### 2. Database Strategy
- **EnsureCreated()** for automatic table creation
- **Seed data** on startup (10 products)
- **UTF-8 charset** for Polish characters
- **No migrations** (simple deployment)

### 3. Clean Architecture Benefits
- **Testable**: Core logic independent of infrastructure
- **Maintainable**: Clear separation of concerns
- **Scalable**: Easy to swap implementations
- **SOLID principles**: Dependency inversion throughout

---

## ?? Technical Decisions

### Why No Migrations?
? Faster deployment  
? Simpler for demo/recruitment  
? `EnsureCreated()` sufficient for containerized env  
? Not recommended for production with existing data

### Why HTTP Instead of Azure SDK?
? Simpler implementation  
? Works with OpenAI.com keys directly  
? No version conflicts  
? Minimal dependencies (3 packages)

### Why Blazor Server?
? Real-time UI updates  
? No client-side JS framework needed  
? Full .NET debugging experience  
? Requirement specification

---

## ?? Troubleshooting

### Issue: "OpenAI quota exceeded"
**Solution**: Add credits to OpenAI account  
? https://platform.openai.com/account/billing

### Issue: "Connection refused" (MySQL)
**Solution**: Wait 10-15 seconds for MySQL health check  
```bash
docker compose restart web
```

### Issue: Port 5000 already in use
**Solution**: Change port in `docker-compose.yml`
```yaml
ports:
  - "8080:8080"  # Changed from 5000
```

### Issue: Polish characters display incorrectly
**Solution**: Already configured with UTF-8 charset  
If issue persists:
```bash
docker compose down -v
docker compose up --build
```

---

## ?? Notes for Recruiters

### Architecture Review Points
1. **Clean Architecture**: Proper dependency flow (inward)
2. **SOLID Principles**: DI throughout, single responsibility
3. **Modern C#**: Primary constructors, records, file-scoped namespaces
4. **Docker**: Single-command deployment (`docker compose up`)
5. **No over-engineering**: Simple, pragmatic solutions

### Code Highlights
- **Writer-Critic Pattern**: Custom AI orchestration (max 3 iterations)
- **EF Core**: Code-first with proper entity configuration
- **Blazor**: Interactive components with proper state management
- **Error Handling**: Graceful degradation, user-friendly messages
- **UTF-8 Support**: Full Polish language support

### Interview Topics
? Explain Onion Architecture layers  
? Justify Writer-Critic iteration limit  
? Discuss SOLID principles in code  
? Database strategy (EnsureCreated vs Migrations)  
? Docker containerization benefits

---

## ?? License

This project is a recruitment task demonstration.  
Not for commercial use.

---

## ????? Author

Recruitment Task Submission  
**Technologies**: .NET 10, Blazor Server, OpenAI, Docker, MySQL  
**Architecture**: Clean Architecture (Onion)  
**Pattern**: Writer-Critic AI

---

**Ready to run with one command: `docker compose up`** ??
