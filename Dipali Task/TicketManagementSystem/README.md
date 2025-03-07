Ticket Management System Setup

Requirements:
- .NET SDK (6.0 or later)
- MySQL Server & Workbench
- Visual Studio Code

Database Setup:
1. Open MySQL Workbench and run:
   CREATE DATABASE TicketManagement;

2. Update appsettings.json with your database details:
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=dipali_task;User=root;Password=root;Trusted_Connection=True;MultipleActiveResultSets=true"
  }

3. Run the following command to apply database migrations:
   dotnet ef database update

Run the Project:
1. Install dependencies:
   dotnet restore

2. Start the application:
   dotnet run

3. Open http://localhost:5000 in your browser.

Default Admin Login:
- Username: admin
- Password: admin123
