# GigApp.Backend (Karegiver)

Backend, API, and Vendor Management System (VMS) repository for the Gig App, also known as "Karegiver".

## Overview

GigApp.Backend is the server-side component of the Karegiver platform, which connects caregivers with healthcare facilities for gig work opportunities. This repository contains:

- REST API endpoints (GigApp.Api)
- Database access layer and business logic
- Vendor Management System (VMS) web application
- Admin web applications

## Features

The application supports a comprehensive set of features for both caregivers and healthcare facilities:

- User management (administrators, vendor managers, job managers, caregivers)
- Facility and client location registration
- Job posting and management
- Payment processing
- Caregiver registration and onboarding
- Job search, application, and acceptance workflows
- Time tracking and job completion
- Ratings and feedback

For a detailed breakdown of all features and user stories, please refer to the [features documentation](/docs/features.md), which includes links to feature demonstration videos.

## Technical Documentation

### Database

The database schema is defined in SQL scripts located in the [/database](/database) directory.

| Database Engine | Script | Deployed To |
| --- | --- | --- |
| PostgreSQL | [Create_Database_PostgreSQL.sql](/database/PostgreSQL/Create_Database_PostgreSQL.sql) | Server=db13352.public.databaseasp.net;Database=db13352; |
| MSSQL | [Create_Database_MSSQL.sql](/database/MSSQL/Create_Database_MSSQL.sql) | Not yet deployed; Will be deployed to [Amazon Aurora](https://aws.amazon.com/rds/aurora/) |

This script contains all the table definitions, constraints, and initial data required to set up a new instance of the application database.

### Project Structure

The application follows the Clean Architecture pattern, which promotes separation of concerns and dependency inversion. This architecture organizes code into concentric layers with dependencies pointing inward, resulting in a loosely coupled, highly testable system.

- [`GigApp.Domain`](/GigApp.Domain) - Core domain entities and business logic, with no external dependencies. Contains:

  - Business models and entities (`Entities/`)
  - Domain exceptions and value objects
  - Core business rules and domain-specific interfaces

- [`GigApp.Application`](/GigApp.Application) - Application business logic and use cases. Implements:

  - CQRS pattern (Command Query Responsibility Segregation)
  - Feature-driven organization of code (`Features/`)
  - Interface definitions for external dependencies (`Interfaces/`)
  - Validation, behaviors, and business rules
  - Mediator pattern for decoupling handlers

- [`GigApp.Infrastructure`](/GigApp.Infrastructure) - External concerns and implementation details:

  - Database access via Entity Framework Core (`Database/`, `Repositories/`)
  - Third-party service integrations (`Services/`)
  - Caching, messaging, and other infrastructure concerns
  - Implementations of interfaces defined in the Application layer

- [`GigApp.Api`](/GigApp.Api) - RESTful API controllers and endpoints:

  - Presentation layer and API contracts
  - Request/response models
  - Authentication and authorization
  - API documentation (Swagger)
  - Dependency injection configuration

- [`VMS.Client`](/VMS.Client) - Vendor Management System web application:

  - Blazor WebAssembly UI for vendor portal
  - UI components and pages
  - Client-side validation and state management

- [`database`](/database) - Database scripts and migrations:

  - SQL scripts for schema creation
  - Seed data and migrations
  - Database diagrams and documentation

- [`docs`](/docs) - Project documentation:
  - Feature specifications and user stories
  - Architecture diagrams
  - API documentation

### CQRS Implementation

The Karegiver application implements the Command Query Responsibility Segregation (CQRS) pattern to separate read and write operations. This pattern provides several benefits:

1. **Separation of concerns** - Write operations (commands) are separated from read operations (queries)
2. **Scalability** - Allows for independent scaling of read and write workloads
3. **Simplicity** - Each operation is focused on a single responsibility
4. **Performance optimization** - Queries can be optimized for reading data without affecting commands

#### CQRS Example: Job Management

The Job Management feature demonstrates CQRS implementation in the application:

**Command Example - Creating a Job:**

The [`CreateJob`](/GigApp.Application/CQRS/Implementations/Jobs/Commands/CreateJob.cs) command handles creating a new job posting:

```csharp
// Command - Represents the request
public class Command : ICommand
{
    public string Auth0Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Rate { get; set; }
    public long AddressId { get; set; }
    // Other job properties...
}

// Handler - Processes the command
public class Handler : ICommandHandler<Command>
{
    private readonly IJobRepository _jobRepository;
    private readonly IValidator<Command> _validator;

    public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
    {
        // Validate command
        // Create job in repository
        // Return result
    }
}
```

**Query Example - Retrieving Job Applications:**

The [`GetJobApplication`](/GigApp.Application/CQRS/Implementations/Jobs/Queries/GetJobApplication.cs) query retrieves job application data:

```csharp
// Query - Represents the request for data
public class Query : IQuery
{
    public long JobId { get; set; }
}

// Handler - Processes the query
public class Handler : IQueryHandler<Query>
{
    private readonly IJobApplicationRepository _jobApplicationRepository;

    public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
    {
        // Validate query
        // Retrieve job application data
        // Return result
    }
}
```

This separation allows for specialized optimization of read and write operations, making the system more maintainable and scalable.

## Development Environment

### Setup

1. Clone the repository
2. Create the database using the [PostgreSQL script](/database/PostgreSQL/Create_Database_PostgreSQL.sql)
3. Configure connection strings in the appropriate appsettings.json files
4. Run the API and client applications

### Manual Testing Procedure

1. Load from Postman: https://gig.runasp.net/swagger/v1/swagger.json
2. Set parameter: https://gig.runasp.net/
3. Login: aksh27@gmail.com pwd: aksh27@gmail.com
4. Use token to make API calls

## Environments

- **Staging VMS**: https://vms.runasp.net
- **Production**: TBD

## License

Proprietary - All rights reserved

```

```
