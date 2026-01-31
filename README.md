# PetWorld - AI-Powered Pet Shop Assistant

**Clean Architecture + Blazor Server + Writer-Critic AI**

Professional pet shop chatbot demonstrating modern .NET architecture.

---

## Overview

AI-powered chat interface for pet product recommendations using Writer-Critic pattern:
- Writer Agent generates responses
- Critic Agent evaluates quality
- Max 3 iterations for optimal answers

**Live Demo**: http://localhost:5000 (after deployment)

---

## Technologies

| Component | Technology | Version |
|-----------|-----------|---------|
| Framework | .NET | 10.0 |
| UI | Blazor Server | - |
| Database | MySQL | 8.0 |
| ORM | Entity Framework Core | 9.0 |
| AI | OpenAI API | GPT-4o |
| Container | Docker | - |

---

## Quick Start

### Prerequisites
- Docker Desktop
- OpenAI API Key (https://platform.openai.com/)

### Steps

1. **Clone repository**
```bash
git clone https://github.com/YOUR_USERNAME/PetWorld.git
cd PetWorld
```

2. **Configure API key**
```bash
# Create .env file
OPENAI_API_KEY=sk-your-actual-key-here
```

3. **Run application**
```bash
docker compose up
```

4. **Access**
- Open: http://localhost:5000
- Chat page: Ask questions in Polish
- History page: View all conversations

---

## Project Structure

```
PetWorld/
├── src/
│   ├── PetWorld.Domain/           # Core entities
│   ├── PetWorld.Application/      # Business logic
│   ├── PetWorld.Infrastructure/   # Data + AI
│   └── PetWorld.Web/              # Blazor UI
├── Dockerfile
├── docker-compose.yml
└── README.md
```

**Architecture**: Clean/Onion (Dependencies point inward)

---

## Features

### Chat Interface
- Text input for customer questions
- AI responses with product recommendations
- Iteration count display (1-3)
- Polish language support

### History View
- Table with all conversations
- Columns: Date, Question, Answer, Iterations
- MySQL persistence

---

## Development

### Build
```bash
dotnet build
```

### Run Locally
```bash
cd src/PetWorld.Web
dotnet run
```

### Docker Commands
```bash
docker compose build        # Build images
docker compose up -d        # Start in background
docker compose logs -f web  # View logs
docker compose down -v      # Stop and remove
```

---

## Author

**Michał Bąkiewicz**

- **Technologies**: .NET 10, Blazor Server, Clean Architecture, OpenAI, Docker, MySQL
- **Pattern**: Writer-Critic AI (max 3 iterations)
- **Repository**: Recruitment Task Demonstration

---

## License

Recruitment task demonstration project.

---

**Ready to run**: `docker compose up`
