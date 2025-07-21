RetailShop - WPF Desktop Application
------------------------------------------------------------------------------
⚠️ Disclaimer
This is a personal project built for learning and demonstration purposes. It is not based on real-world professional experience and is not intended for production use. The primary goal of this project was to practice and apply concepts related to .NET, WPF, and software architecture patterns.
------------------------------------------------------------------------------
📖 About The Project
RetailShop is a desktop application for managing basic retail operations. It is built using Windows Presentation Foundation (WPF) and .NET 8, with a focus on creating a clean, layered architecture that separates business logic from the user interface.
The application explores common software design patterns like the Repository Pattern, Dependency Injection, and MVVM to create a solution that is maintainable and scalable.
------------------------------------------------------------------------------
🛠️ Technology Stack
This project is built using the following technologies and libraries:
Frameworks:
.NET 8
Windows Presentation Foundation (WPF)
Data Access:
Entity Framework Core 8: Main ORM for database interactions.
Dapper: A micro-ORM used for scenarios where raw SQL performance is desired.
SQL Server: The backend relational database.
Architecture & Patterns:
Layered Architecture: (Domain and Presentation layers)
MVVM (Model-View-ViewModel): To separate UI from logic in the WPF project.
Repository Pattern: To abstract the data access layer.
Dependency Injection: Using Microsoft.Extensions.DependencyInjection.
Utility Libraries:
AutoMapper: To simplify object-to-object mapping.
ZXing.Net & QRCoder: For generating and handling barcodes and QR codes.
Microsoft.Extensions.Configuration: For managing configuration from appsettings.json.
------------------------------------------------------------------------------
🏗️ Architectural Overview
The solution is divided into two main projects, promoting a clear separation of concerns.
1. Domain Project
This is a .NET class library that acts as the core of the application. It contains all the business logic, data models, and services, and has no dependency on the UI.
Entities: Contains the POCO (Plain Old CLR Object) classes that represent the database tables.
Repositories: Defines interfaces for data access operations, implementing the Repository Pattern. This decouples the business logic from the data access implementation.
Services: Contains the business logic of the application.
Mapper: Includes AutoMapper profiles for mapping between entities and ViewModels/DTOs.
2. RetailUI Project
This is the WPF application that the user interacts with. It is responsible for the presentation logic and is built using the MVVM (Model-View-ViewModel) pattern.
It has a project reference to the Domain project to access business logic and data.
Views: Contains the XAML files (.xaml) that define the user interface.
ViewModels: Contains the classes that the Views bind to. They hold the presentation logic and manage the state of the UI.
DbContext: Contains the Entity Framework DbContext class.
Services: Contains UI-specific services, like navigation or dialog services.
appsettings.json: Used to store configuration data, such as the database connection string.
------------------------------------------------------------------------------
✨ Features
The application is designed to support the following features:
Category Management: Creating, viewing, and managing product categories.
Inventory Management: Handling incoming goods and updating stock levels.
Barcode/QR Code Support: Utilizes libraries for potential integration with barcode scanners or QR code generation for products.
------------------------------------------------------------------------------
🚀 Getting Started
To get a local copy up and running, follow these simple steps.
Prerequisites
Visual Studio 2022 or later
.NET 8 SDK
SQL Server (e.g., Express, Developer, or LocalDB)
Installation
Clone the repository:
Generated sh
git clone https://github.com/your-username/RetailShop.git
Use code with caution.
Sh
Configure the database connection:
Open the appsettings.json file in the RetailUI project.
Modify the ConnectionString to point to your local SQL Server instance.
You will need to create the database and run the necessary migrations (if using EF Core migrations) or create the tables manually based on the Entities.
Build the solution:
Open the RetailShop.sln file in Visual Studio.
Build the solution to restore the NuGet packages.
Run the application:
Set the RetailUI project as the startup project.
Press F5 or the "Start" button to run the application.

--------------------------------------------------------------------------------


