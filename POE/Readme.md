# RaceDay - Part 3: MVC Frontend, Azure Blob Storage & Docker

This is the Part 3 submission for the PROG6212 POE. It consists of an ASP.NET Core MVC web application that consumes the Part 2 API, integrated with Azure Blob Storage and fully containerized using Docker.

## 🚀 Features Implemented
- **Role-Based Interfaces:** Distinct dashboards and navigation for Organisers and Participants.
- **Session Management:** Secure session cookies passed between the MVC app and the API via a custom `ApiService`.
- **Azure Blob Storage:** Organisers can upload event banner images. The MVC app sends the file to the API, which uploads it to Azure.
- **Docker:** The entire stack (API, MVC, and SQL Server) runs in isolated containers.

## 🐳 How to Run with Docker

You do not need to install .NET or SQL Server locally to run this application.

### Prerequisites
- **Docker Desktop** must be installed and running on your machine.

### Steps to Run
1. Open a terminal at the **root** of this repository (where `docker-compose.yml` is located).
2. Run the following command to build and start the containers:
   ```bash
   docker compose up --build
3. Once the containers are running, open your browser and navigate to:
http://localhost:7201
4. To stop the application, press Ctrl + C in the terminal.

CI/CD Pipeline
The GitHub Actions workflow automatically restores, builds, and tests both the API and the MVC projects on every push to the main branch.
<img width="1917" height="1040" alt="Screenshot 2026-11-08 233346" src="https://github.com/user-attachments/assets/d4a04bfd-d243-43f5-bd6d-50ee1bd1a464" />

Video Demonstration
Watch the full walkthrough of the MVC application, Azure Blob integration, and Docker setup here:

Click here to watch the YouTube Video: 

Author
Oregolele Mangwegape

Module: PROG6212
