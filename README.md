# Wonga Developer Assessment

A full-stack web application with user authentication built using React, C#, PostgreSQL, and Docker.

## Tech Stack

- **Frontend:** React
- **Backend:** C# ASP.NET Core Web API
- **Database:** PostgreSQL
- **Containerization:** Docker & Docker Compose
- **Authentication:** JWT Tokens
- **Password Hashing:** BCrypt

## Project Structure

```
wonga-assessment/
├── backend/
│   ├── WongaApi/          # C# API
│   └── WongaApi.Tests/    # Unit Tests
├── frontend/
│   └── wonga-app/         # React App
└── docker-compose.yml
```

## Features

- User Registration (First Name, Last Name, Email, Password)
- User Login with JWT authentication
- Protected User Details page (only accessible when logged in)
- Password hashing with BCrypt
- Unit tests with xUnit

## Prerequisites

- Docker Desktop
- Node.js & npm
- .NET SDK 9.0

## How to Run

### 1. Clone the repository

```bash
git clone <your-repo-url>
cd wonga-assessment
```

### 2. Start the Backend & Database

```bash
docker-compose up --build
```

This will:
- Start a PostgreSQL database on port 5432
- Build and start the C# API on port 8080
- Automatically run database migrations

### 3. Start the Frontend

Open a new terminal:

```bash
cd frontend/wonga-app
npm install
npm start
```

The React app will open at **http://localhost:3000** (or 3001 if 3000 is busy)

## API Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | /api/Auth/register | Register a new user | No |
| POST | /api/Auth/login | Login and get JWT token | No |
| GET | /api/Auth/me | Get current user details | Yes |

### Swagger UI

API documentation available at: **http://localhost:8080/swagger**

## Running Unit Tests

```bash
cd backend/WongaApi.Tests
dotnet test
```

5 unit tests covering:
- Successful registration
- Duplicate email registration
- Successful login
- Login with wrong password
- Login with non-existent user

## How It Works

1. User registers via the React frontend
2. Password is hashed using BCrypt before storing in PostgreSQL
3. User logs in and receives a JWT token
4. JWT token is stored in localStorage
5. Token is sent with every request to protected endpoints
6. The User Details page is only accessible with a valid token