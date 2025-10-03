# User Management API (ASP.NET Core 8)

## Overview
Simple backend to manage user accounts:
- Register & login (JWT)
- Protected CRUD on /users
- Webhook dispatch on login
- Logging with Serilog

## Run
1. dotnet restore
2. dotnet ef database update
3. dotnet run

## Config
Set secrets in appsettings.json or environment:
- Jwt:Secret
- Webhook:Url
- ConnectionStrings:DefaultConnection

## API
Endpoints:
- POST /auth/register
- POST /auth/login
- GET /users (JWT required)
- GET /users/{id} (JWT required)
- PUT /users/{id} (JWT required)
- DELETE /users/{id} (JWT required)

## Webhook
On login: sends POST to `Webhook:Url` with payload:
{
 "event":"user_logged_in", "timestamp":"...", "activeUsers":[ ... ]
}

Failures logged.

## Logging
Serilog writes to console by default. Configure file sink in appsettings.json.
