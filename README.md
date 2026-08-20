# Smart City Complaint Management System

A web based complaint tracking system that allows citizens to report civic issues and track their status. The system uses Gemini AI to analyze complaint descriptions and optional images to identify the issue, category, and priority. Administrators can review complaints, assign departments, and update their status.

## Members and their contribution
- ### <ins>**Gopal Ji Mishra (IN26009581)**</ins>

* Developed the **Gemini AI integration** using `GeminiService.cs`.
* Integrated the Gemini API.
* Implemented AI analysis of the **complaint description** and optional **uploaded image**.
* Configured the AI to identify:
  * **Detected Issue**
  * **Category of Department**
  * **Priority**
* Designed the prompt to generate practical complaint categories for proper department routing.
* Integrated the AI generated result so that it is stored along with the complaint in **SQL Server**.

- ### <ins>**Abhijit Kumar Sharma (IN26011441)**</ins>

- Worked on `AuthController.cs`.
- Added user registration and login functionality.
- Worked on the `Models` folder.
- Created the models needed for users and complaints.
- Connected the authentication and model parts with the project.

- ### <ins>**[Alby John Benny] (IN26011618)**</ins>

- Worked on `Program.cs` and the **application startup configuration**.
- Configured the ASP.NET Core Web API application and its services.
- Registered application services required by the project.
- Configured **Entity Framework Core** and the SQL Server database connection.
- Configured the `ApplicationDbContext` for database access.
- Registered the `GeminiService` for Gemini AI integration.
- Configured API controllers and the application's middleware pipeline.
- Worked on the overall **backend application setup and service dependency configuration**.

### <ins>**[Vijay Krishna P M] (IN26011854)**</ins>

- Worked on `ApplicationDbContext.cs` and the **database integration**.
- Implemented **Entity Framework Core** for database operations.
- Connected the **User and Complaint models** with the SQL Server database.
- Configured `DbSet<Complaint>` and `DbSet<User>` for database entities.
- Worked on **storing and retrieving complaint and user data** through the database context.
- Worked with **Entity Framework Core migrations** for database schema management.
- Contributed to the overall **SQL Server database and backend data management**.

## Features

- Citizen registration and login
- Admin login and dashboard
- Complaint submission with location
- Optional single-image upload
- Gemini AI complaint analysis
- AI-generated issue detection
- AI-generated complaint category
- AI-generated priority
- Complaint storage in SQL Server
- Citizen complaint tracking
- Admin complaint management
- Department assignment
- Complaint status updates:
  - Pending
  - In Progress
  - Resolved
- AI analysis stored with the complaint
- Modern responsive frontend

## Technology Stack

### Frontend
- HTML
- CSS
- JavaScript

### Backend
- ASP.NET Core Web API
- C#

### Database
- Microsoft SQL Server / SQL Server Express
- Entity Framework Core
- Entity Framework Core Migrations

### AI
- Google Gemini API
- Gemini 2.5 Flash

## Database

The project uses a SQL Server database named:

SmartCityComplaintDB

### Users

Stores information about registered users:

- Id
- Name
- Email
- Password
- Role

### Complaints

Stores:

- Id
- Description
- Location
- Category
- Priority
- Status
- Department
- UserId
- AIAnalysis

UserId identifies the citizen who submitted the complaint.

## Prerequisites

- Visual Studio — with ASP.NET and web development workload. Microsoft lists this workload as the prerequisite for ASP.NET Core Web API development.
- .NET SDK — project is using .NET 10.
- SQL Server Express — for storing users and complaints.
- SQL Server Management Studio (SSMS) — to view and manage the database.
