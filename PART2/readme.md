# RaceDay — PROG6212 Part 2 API

A RESTful ASP.NET Core Web API that powers the RaceDay event management system. It handles user authentication, event management, participant enrolment, and race results with role-based access control.

## 📖 Project Overview

This is Part 2 of the RaceDay POE. It is the backend API that will be consumed by the MVC front end in Part 3. The API exposes all endpoints planned in Part 1 and follows the database schema defined in the original SQL script.

## 🛠️ Technologies Used

| Category | Technology |
|---|---|
| Framework | ASP.NET Core 8.0 Web API |
| ORM | Entity Framework Core 8 (Code-First) |
| Database | SQL Server (LocalDB) |
| Authentication | BCrypt password hashing + session-based auth |
| Testing | xUnit + EF Core InMemory + Microsoft.AspNetCore.Mvc.Testing |
| API Docs | Swagger (Swashbuckle) |
| CI/CD | GitHub Actions |

## 👥 User Roles

The system supports two roles, each with different permissions.

### 🏃 Organiser
- Create, update, and delete race events
- Create and remove event categories (age brackets and distances)
- Capture race results (finish times and positions)
- View the full participant roster for their events
- View and update their own profile

### 🏅 Participant
- Browse all public events
- Enrol into events under a specific category
- View and cancel their own enrolments
- View their personal race results
- View and update their own profile

## 🚀 Setup Instructions

### Prerequisites
- Visual Studio 2022 with the **ASP.NET and web development** workload installed
- .NET 8.0 SDK
- SQL Server LocalDB (included with Visual Studio)

### Getting Started
1. Clone the repository to your local machine.
2. Open the solution file `RaceDay.sln` in Visual Studio.
3. Restore NuGet packages (Visual Studio does this automatically when the solution loads).
4. Open `appsettings.json` and confirm the connection string. The default uses LocalDB, so no changes are needed if you are running on a standard Visual Studio installation.
5. Apply the database migration using the Package Manager Console. The migration will create the RaceDayDB database and all its tables.
6. Press F5 in Visual Studio to run the API.

### Accessing the API
Once the API is running, Swagger UI will open automatically in your browser. All endpoints can be tested directly from Swagger — no additional tooling is needed.

## 🔐 Authentication

The API uses **session-based authentication**.

1. A new user registers as either an Organiser or a Participant.
2. On successful login, the server stores the UserID and UserRole in a server-side session and returns a session cookie.
3. All protected endpoints require this cookie. The custom AuthorizeRole filter checks the session role and returns:
   - **401 Unauthorized** if no session exists
   - **403 Forbidden** if the user's role does not match the endpoint requirement

Passwords are hashed with **BCrypt** before storage. Plain-text passwords are never persisted.

## 📡 API Endpoints

### 🔑 Authentication
| Method | Route | Role | Description |
|---|---|---|---|
| POST | /api/auth/register | Public | Register a new user |
| POST | /api/auth/login | Public | Log in and start a session |

### 👤 Users
| Method | Route | Role | Description |
|---|---|---|---|
| GET | /api/users/profile | Authenticated | View own profile |
| PUT | /api/users/profile | Authenticated | Update own profile |

### 🏁 Events
| Method | Route | Role | Description |
|---|---|---|---|
| GET | /api/events | Public | List all events |
| GET | /api/events/{id} | Public | Get event by ID |
| POST | /api/events | Organiser | Create event |
| PUT | /api/events/{id} | Organiser | Update event |
| DELETE | /api/events/{id} | Organiser | Delete event |

### 🏷️ Categories
| Method | Route | Role | Description |
|---|---|---|---|
| GET | /api/events/{eventID}/categories | Public | List categories for an event |
| POST | /api/events/{eventID}/categories | Organiser | Create category |
| DELETE | /api/categories/{id} | Organiser | Delete category |

### 📝 Enrolments
| Method | Route | Role | Description |
|---|---|---|---|
| POST | /api/enrolments | Participant | Enrol in an event |
| GET | /api/enrolments/my-enrolments | Participant | View own enrolments |
| DELETE | /api/enrolments/{id} | Participant | Cancel enrolment |
| GET | /api/events/{eventId}/enrolments | Organiser | View participant roster |

### 🥇 Results
| Method | Route | Role | Description |
|---|---|---|---|
| POST | /api/events/{eventId}/results | Organiser | Capture a result |
| GET | /api/events/{eventId}/results | Public | View event leaderboard |
| GET | /api/results/my-results | Participant | View personal results |

## ✅ Unit Testing

Tests are located in the RaceDay.Tests project and cover both success and failure scenarios.

| Test File | Coverage |
|---|---|
| AuthTests.cs | Registration success, duplicate email failure, login success, wrong password failure |
| RoleAuthorizationTests.cs | Participant blocked from creating events (403), unauthenticated blocked (401), Organiser blocked from enrolling (403) |
| EventsTests.cs | Organiser can create events (201), public can view events (200) |
| EnrollmentTests.cs | Participant can view own enrolments, invalid references rejected, roster access restricted |

**Result:** All 12 tests passing.

To run the tests, open Test Explorer in Visual Studio and click **Run All Tests**.

## 🔄 CI/CD Pipeline

This project uses **GitHub Actions** to automatically build and test the API on every push to the main branch.

The workflow performs the following steps:
1. Restore NuGet dependencies
2. Build the API project
3. Run all unit tests

### Successful CI/CD Green Build:
<img width="1477" height="568" alt="image" src="https://github.com/user-attachments/assets/7aedfd86-1dbe-4f2a-b985-6edba89d1fbd" />


## 🎥 Video Demonstration

📺 **Watch the API demonstration:** https://youtu.be/pLxrnFUttkY

The video covers:
- Project structure (Controllers, Models, DTOs, DbContext)
- Swagger UI walkthrough of all endpoints
- Session-based authentication flow
- Role-based authorization (Participant blocked from creating an event — 403 Forbidden)
- Registration, login, and enrolment demonstration
- Unit tests running in Test Explorer
- GitHub Actions green build

## 📂 Project Structure
PART2/
├── RaceDay/ # Main Web API
│ ├── Controllers/ # API endpoints (6 controllers)
│ ├── Models/ # EF Core entities
│ ├── DTOs/ # Data Transfer Objects
│ ├── Data/ # RaceDayDbContext
│ ├── Filters/ # Custom AuthorizeRole filter
│ ├── Migrations/ # EF Core migrations
│ ├── appsettings.json # Config and connection string
│ └── Program.cs # Startup configuration
│
└── RaceDay.Tests/ # xUnit test project
├── RaceDayWebApplicationFactory.cs
├── TestHelpers.cs
├── AuthTests.cs
├── RoleAuthorizationTests.cs
├── EventsTests.cs
└── EnrollmentTests.cs
