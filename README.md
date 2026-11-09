# PROG6212 POE - RaceDay Platform

Welcome to the repository for the RaceDay platform, submitted for the PROG6212 module. This project is divided into three distinct parts, each building upon the previous one to create a complete, cloud-integrated full-stack application.

## 📂 Project Navigation

Please click on the links below to view the specific documentation and setup instructions for each part of the project.

### [Part 1: Planning and Design](./PART1)
*Contains the initial project planning, including the SQL database script, the Entity Relationship Diagram (ERD), and the UI/UX design mockups.*

### [Part 2: RESTful Web API](./PART2)
*Contains the ASP.NET Core Web API backend. It handles database operations via Entity Framework Core, uses BCrypt for password hashing, and secures endpoints using session-based role authorization.*


### [Part 3: MVC Frontend, Azure & Docker](./POE)
*Contains the ASP.NET Core MVC frontend that consumes the Part 2 API. It features a custom retro-themed UI, integration with Azure Blob Storage for image uploads, and is fully containerized using Docker.*


---

## 🛠️ Technology Stack
- **Backend:** C#, ASP.NET Core 8 Web API, Entity Framework Core
- **Frontend:** ASP.NET Core MVC, Razor, Bootstrap, Custom CSS
- **Database:** SQL Server (LocalDB for local development, Docker container for deployment)
- **Cloud Storage:** Azure Blob Storage
- **Testing:** xUnit, EF Core InMemory
- **DevOps:** Docker, Docker Compose, GitHub Actions (CI/CD)

## 🔄 CI/CD Pipeline
This repository is configured with **GitHub Actions**. On every push to the `main` branch, the automated workflow restores dependencies, builds both the API and MVC projects, and runs the unit test suite to ensure application stability.


