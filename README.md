# Ticket-Management-System

Ticket Management System Setup

Requirements:

.NET SDK (6.0 or later)
MySQL Server & Workbench
Visual Studio Code
Database Setup:

Open MySQL Workbench and run: CREATE DATABASE TicketManagement;

Update appsettings.json with your database details: "ConnectionStrings": { "DefaultConnection": "Server=localhost;Database=TicketManagement;User=root;Password=yourpassword;" }

Run the following command to apply database migrations: dotnet ef database update

Run the Project:

Install dependencies: dotnet restore

Start the application: dotnet run

Open http://localhost:5000 in your browser.

Default Admin Login:

Username: admin
Password: admin123
