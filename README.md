
# RaceDay API - PROG6212 Part 2

## Project Description

RaceDay is a full-stack web-based event management system designed specifically for the South African road running, walking, and cycling community. This repository contains the backend RESTful API, built using ASP.NET Core Web API, C#, and Entity Framework Core using the Code-First approach.

The API allows Event Organisers to create and manage events, categories, and participant results. Participants can browse events, enter events, and track their personal performance history.

## System Roles

The API supports two distinct roles:

### 1. Organiser

- Create, update, and delete events.
- Define age and distance categories for events.
- View participant enrolments for their events.
- Record participants' finish times and finishing positions after an event concludes.

### 2. Participant

- Create an account and manage their profile.
- Browse available events and view event categories.
- Enter an event by selecting a specific category.
- View their own enrolments.
- Track their personal race results and history.

## Project Structure

The solution contains two main projects:

### RaceDay.API

The core ASP.NET Core Web API project.

- **Controllers/** — Contains the API endpoints for authentication, events, categories, enrolments, results, and users. Role-based access is enforced using `[Authorize]` attributes.
- **Models/** — Contains the Entity Framework Core models for Users, Organisers, Participants, Events, Categories, EventEnrolments, and Results.
- **DTOs/** — Contains Data Transfer Objects, such as `RegisterRequestDto` and `CreateEnrolmentDto`, used to transfer data securely between the client and API.
- **Data/** — Contains `RaceDayDbContext`, which configures the database schema, table names based on the Part 1 ERD, and entity relationships.

### RaceDay.Tests

The xUnit testing project. It contains tests for authentication, role enforcement, and CRUD operations using an in-memory database.

## Setup and Run Instructions

Follow these steps to run the API locally.

### Prerequisites

Ensure you have the following installed:

- Visual Studio 2026 or Visual Studio 2022 with the appropriate .NET SDK.
- SQL Server.
- SQL Server Management Studio (SSMS).

### 1. Clone the Repository

Open a terminal and run:

```bash
git clone https://github.com/Molobane28/Prog_RaceDay-.git
```

Alternatively, download or open your existing Part 2 repository.

### 2. Open the Solution

Open `RaceDay.slnx` in Visual Studio.

### 3. Configure the Database Connection

Open `appsettings.json` in the `RaceDay.API` project.

Update the `RaceDayConnection` connection string to match your local SQL Server instance.

Example:

```json
{
  "ConnectionStrings": {
    "RaceDayConnection": "Server=localhost\\SQLEXPRESS;Database=RaceDayDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Important:** Use the correct SQL Server instance name for your computer.

### 4. Apply Database Migrations

In Visual Studio:

1. Select **Tools**.
2. Select **NuGet Package Manager**.
3. Open **Package Manager Console**.
4. Set the Default project to `RaceDay.API`.
5. Run the following command:

```powershell
Update-Database
```

This applies the Entity Framework Core migrations to create or update the database.

### 5. Run the Application

1. Set `RaceDay.API` as the startup project.
2. Press **F5** or click the green Play button.
3. Open the Swagger UI in your browser if it does not open automatically.

Example:

```text
https://localhost:7081/swagger
```

The exact URL and port may differ on your computer.

Swagger allows you to test the API endpoints.

### 6. Run Unit Tests

In Visual Studio:

1. Open the **Test** menu.
2. Select **Run All Tests**.

Alternatively, use the Test Explorer to run the available tests and check their results.

## CI/CD Pipeline

This project uses GitHub Actions for Continuous Integration.

The workflow is configured in:

`.github/workflows/ci.yml`

When code is pushed to the configured branch, the pipeline can automatically:

- Set up the .NET environment.
- Restore NuGet dependencies.
- Build the solution.
- Run unit tests to check for errors.

### Successful CI/CD Build

Add a screenshot of the successful GitHub Actions workflow here.

To add your screenshot, upload the image to your GitHub repository and update this section with the image link.

Example:

```markdown
![Successful GitHub Actions Build](screenshots/github-actions-success.png)
```

Replace the example image path with the actual path to your screenshot.

## Video Presentation

The video presentation demonstrates the RaceDay API, including the application, Swagger endpoint testing, code explanation, and unit tests.

**YouTube Video:** Replace this text with your actual YouTube video link.

