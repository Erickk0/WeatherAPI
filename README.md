# WeatherAPI

WeatherAPI is a simple CQRS-based .NET Core application that interacts with a **Neo4j** database to store and retrieve weather data. It follows a **Clean Architecture** approach and uses **FluentAssertions, NSubstitute, and xUnit** for testing.

## Features
- Store weather data (temperature, humidity, wind speed).
- Query weather records efficiently using **Neo4j**.
- Asynchronous execution with **C# Task-based programming**.

## 🛠️ Technologies Used
- **.NET 8.0**
- **Neo4j.Driver** (for interacting with Neo4j database)
- **MediatR** (CQRS pattern implementation)

## 🚀 Getting Started

### **1 Prerequisites**
- Install **.NET SDK 8.0+**
- Install **Neo4j Community/Enterprise Edition**
- Install **Neo4j Desktop** (optional)
- Configure **Neo4j Bolt** connection in `appsettings.json`:
  ```json
  {
    "Neo4j": {
      "Uri": "bolt://localhost:7687",
      "Username": "neo4j",
      "Password": "yourpassword"
    }
  }


### **2 Clone Repository**
- git clone https://github.com/yourusername/WeatherAPI.git
- cd WeatherAPI


### **3 Install Dependencies**
- dotnet restore


### **4 Build & Run**
- dotnet build
- dotnet run


### **Project Structure**
WeatherAPI/
│── WeatherAPI.csproj       # .NET Core Project File
│── appsettings.json        # Configuration File (Database, Logging)
│── /CQRS                   # Command & Query Handlers
│── /Models                 # Weather Model
│── /Services               # Business Logic & Data Access
│── Program.cs              # Main Entry Point



---

### **📝 TODO.md**
```markdown
# ✅ TODO List - WeatherAPI

## 🔨 Development
- [x] Set up **.NET 8** and project structure
- [x] Install required NuGet packages (Neo4j.Driver, MediatR, FluentAssertions, etc.)
- [x] Implement **CQRS pattern** for weather data storage
- [ ] Implement **CRUD operations** for weather data
  - [x] Create weather record
  - [x] Read weather records
  - [ ] Update weather record
  - [x] Delete weather record
- [ ] Implement **error handling** and logging

## 🛠️ Testing
- [ ] Set up **xUnit** for unit testing
- [ ] Mock **Neo4j.Driver** with **NSubstitute**
- [ ] Add test coverage for:
  - [ ] CreateWeatherHandler
  - [ ] ReadWeatherHandler
  - [ ] UpdateWeatherHandler
  - [ ] DeleteWeatherHandler

## 🚀 Deployment & Optimization
- [ ] Dockerize the application
- [ ] Configure **CI/CD pipeline** for automated builds
- [ ] Optimize database queries for **better performance**

```
### **License**
- This project is licensed under the MIT License - see the LICENSE file for details.





