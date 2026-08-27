# Task Manager API

A RESTful API for managing users and tasks.

## About

The project was built as a backend practice project using ASP.NET Core and PostgreSQL.

## Features
- User registration and authentication
- JWT authentication
- Role-based authorization (User / Admin)
- User management
- Task creation, updating and deletion
- Access control: users can manage their own data, while administrators have extended permissions
- Input validation
- Global exception handling
- Filtering, sorting and pagination
- Task status management
- Tech Stack
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Npgsql
- JWT Bearer Authentication
- Swagger / OpenAPI
- Docker

  
## Docker

The application is containerized using Docker.

Build the Docker image
'docker build -t my-project '
Run the container
'docker run --rm -p 8080:8080 my-project'

After starting the container, the API will be available at:

http://localhost:8080

## Swagger UI:

'http://localhost:8080/swagger'

Note: The application requires a PostgreSQL database. The connection string should be configured according to your environment.

## Authorization

The API uses JWT Bearer authentication.

After successful authentication, the user receives a JWT token which should be included in subsequent requests
