# HIV Treatment and Medical Services System

This is a .NET 8.0 Web API project implementing Clean Architecture principles for a HIV Treatment and Medical Services System.

## Project Structure

### 1. Domain Layer (HIVTreatmentSystem.Domain)
- Core business logic and entities
- Contains:
  - Entities (Patient, Doctor, Treatment, Appointment)
  - Value Objects
  - Domain Events
  - Repository Interfaces
  - Domain Services

### 2. Application Layer (HIVTreatmentSystem.Application)
- Business use cases and application logic
- Contains:
  - DTOs
  - Interfaces
  - Application Services
  - Validation Logic
  - CQRS Commands and Queries

### 3. Infrastructure Layer (HIVTreatmentSystem.Infrastructure)
- External concerns and implementations
- Contains:
  - Database Context
  - Repository Implementations
  - External Service Integrations
  - Authentication/Authorization
  - Logging
  - Email Services

### 4. Presentation Layer (HIVTreatmentSystem.API)
- API endpoints and controllers
- Contains:
  - Controllers
  - API Models
  - Middleware
  - API Documentation

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server (or your preferred database)
- Visual Studio 2022 or VS Code

### Setup
1. Clone the repository
2. Restore NuGet packages
3. Update connection strings in appsettings.json
4. Run database migrations
5. Start the application

## Features
- Patient Management
- Doctor Management
- Treatment Planning
- Appointment Scheduling
- Medical Records
- Prescription Management
- Reporting and Analytics

## Architecture
This project follows Clean Architecture principles:
- Independent of frameworks
- Testable
- Independent of UI
- Independent of Database
- Independent of any external agency

## Dependencies
- Entity Framework Core
- MediatR
- AutoMapper
- FluentValidation
- JWT Authentication
- Swagger/OpenAPI
