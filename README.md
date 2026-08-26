
# MVP_TaskManager## API Overview

<img width="1465" height="827" alt="image" src="https://github.com/user-attachments/assets/b17b6f79-6e9a-43e2-985b-738329e7ecf0" />


# Task Manager API

A RESTful API for managing users and tasks.

## About

The project was built as a backend practice project using ASP.NET Core and PostgreSQL.

## Features

- User registration and authentication
- JWT authentication
- Role-based authorization (`User` / `Admin`)
- User management
- Task creation, updating and deletion
- Access control: users can manage their own data, while administrators have extended permissions
- Input validation
- Global exception handling
- Filtering, sorting and pagination
- Task status management

## Tech Stack

- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Npgsql
- JWT Bearer Authentication
- Swagger / OpenAPI

## Authorization

The API uses JWT Bearer authentication.

After successful authentication, the user receives a JWT token which should be included in subsequent requests:

```text
Authorization: Bearer <your_token>

