# PetWorld 🐾

> AI-powered pet shop assistant using Writer-Critic pattern with OpenAI GPT-4o

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4)](https://blazor.net/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1)](https://www.mysql.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED)](https://www.docker.com/)

## What This Does

A Blazor Server chatbot that recommends pet products using AI quality assurance:

- 🤖 **Writer Agent** generates product recommendations
- 🔍 **Critic Agent** evaluates quality (iterates up to 3x)
- 💬 Chat interface in Polish
- 📊 Conversation history stored in MySQL

## Tech Stack

```
.NET 9 + C# 13
Blazor Server (Interactive)
OpenAI GPT-4o (Microsoft Agents AI)
Entity Framework Core 9 + MySQL 8
Docker + Docker Compose
Clean Architecture
```

## Quick Start

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [OpenAI API Key](https://platform.openai.com/api-keys)

### Run with Docker

```bash
# 1. Clone
git clone https://github.com/thekcr85/PetWorld.git
cd PetWorld

# 2. Create .env file with your API key
echo OPENAI_API_KEY=sk-your-key-here > .env

# 3. Start
docker compose up

# 4. Open browser
# → http://localhost:5000
```

That's it! The app will:
- Start MySQL database
- Run migrations automatically
- Seed sample products
- Launch Blazor app on port 5000

## Local Development

Without Docker:

```bash
# 1. Start MySQL
docker run -d -p 3306:3306 \
  -e MYSQL_ROOT_PASSWORD=petworld123 \
  -e MYSQL_DATABASE=petworld \
  mysql:8.0

# 2. Update src/PetWorld.Web/appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=petworld;User=root;Password=petworld123;"
  },
  "OpenAI": {
    "ApiKey": "sk-your-key-here",
    "ModelName": "gpt-4o"
  }
}

# 3. Run
cd src/PetWorld.Web
dotnet run
```

## Project Structure

```
src/
├── PetWorld.Domain/              # 🎯 Core (Entities, Interfaces)
├── PetWorld.Application/         # 💼 Business Logic
├── PetWorld.Infrastructure/      # 🔧 Data + AI Services
└── PetWorld.Web/                 # 🎨 Blazor UI
    └── Components/Pages/
        ├── Home.razor           # Landing page
        ├── Chat.razor           # AI chat interface
        └── History.razor        # Conversation log
```

**Clean Architecture** - dependencies flow inward (Web → Infra → App → Domain)

## How Writer-Critic Works

```
User asks: "Jaka karma dla psa?"

┌─────────────────────────────────────┐
│ Iteration 1                         │
├─────────────────────────────────────┤
│ Writer: "Polecam karmę X i Y"      │
│ Critic: ❌ "Brak cen"               │
└─────────────────────────────────────┘
         ↓
┌─────────────────────────────────────┐
│ Iteration 2                         │
├─────────────────────────────────────┤
│ Writer: "Karma X (50zł), Y (80zł)"  │
│ Critic: ✅ "Approved"                │
└─────────────────────────────────────┘
         ↓
    Final Answer (2 iterations)
```

## Pages

| Route | Description |
|-------|-------------|
| `/` | Redirects to Chat |
| `/chat` | AI chat interface |
| `/history` | Conversation history table |

## Configuration

### Environment Variables (.env)

```bash
OPENAI_API_KEY=sk-your-actual-api-key-here
```

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=mysql;Database=petworld;User=root;Password=petworld123;"
  },
  "OpenAI": {
    "ApiKey": "YOUR_OPENAI_API_KEY",
    "ModelName": "gpt-4o"
  }
}
```

## NuGet Packages

### PetWorld.Infrastructure

```xml
<PackageReference Include="Microsoft.Agents.AI.OpenAI" Version="1.0.0-preview.260128.1" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
<PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="9.0.0" />
<PackageReference Include="OpenAI" Version="2.8.0" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="9.0.0" />
```

**All packages are production-ready and compatible with .NET 9!**

## Docker Commands

```bash
# Start services
docker compose up

# Run in background
docker compose up -d

# View logs
docker compose logs -f web

# Stop & remove
docker compose down -v

# Rebuild
docker compose build --no-cache
```

## Sample Questions

Try these in the chat (Polish):

```
"Jaka karma dla psa jest najlepsza?"
"Potrzebuję akcesoriów dla kota"
"Co polecasz dla chomika?"
```

## Development Commands

```bash
# Build solution
dotnet build

# Restore packages
dotnet restore

# Run locally
cd src/PetWorld.Web
dotnet run

# Clean build artifacts
dotnet clean
```

## Architecture Highlights

✅ **Clean Architecture** - testable, maintainable  
✅ **Writer-Critic Pattern** - AI quality assurance  
✅ **Blazor Server** - real-time interactive UI  
✅ **EF Core 9** - modern ORM with MySQL  
✅ **Microsoft Agents AI** - structured AI workflows  
✅ **Docker** - one-command deployment  

## Author

**Michał Bąkiewicz** • [GitHub](https://github.com/thekcr85)

Recruitment task demonstrating:
- **Clean Architecture** with proper layer separation
- **AI Integration** using Writer-Critic pattern with Microsoft Agents AI
- **Blazor Server** with interactive components (.NET 9)
- **Docker containerization** for one-command deployment
- **Entity Framework Core 9** with MySQL (Pomelo 9.0.0 provider)

**Project Repository**: [github.com/thekcr85/PetWorld](https://github.com/thekcr85/PetWorld)

---

## License

MIT License - Recruitment demonstration project

---

**Get Started:** `docker compose up` 🚀

