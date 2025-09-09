# Contact Management API

A .NET 8 Web API project that allows users to register, authenticate, and manage their personal address book.  

This project was built as part of a .NET Technical Challenge and demonstrates user authentication with JWT tokens, secure password storage, and full CRUD operations for managing contacts.

---

## Features

- **User Authentication**
  - Register a new account
  - Sign in with existing credentials
  - JWT-based authentication for secure access  

- **Contact Management**
  - Add a contact (first name, last name, phone number, email, birthdate)
  - List all contacts
  - Retrieve a contact by ID
  - Update a contact
  - Delete a contact  

- **Additional Implemented Features**
  - Sorting by first name, last name, or birthdate  
  - Pagination for efficient listing  
  - Swagger documentation enabled  

- **Security**
  - Passwords hashed using `PasswordHasher`
  - JWT token expiration and validation  
  - Model validation for inputs (email, phone, password rules, birthdate check)

---

## Tech Stack

- .NET 8 Web API  
- Entity Framework Core 8 (SQL Server)  
- JWT Authentication  
- Swagger / OpenAPI  
- Postman (for testing)  

---

## Project Structure

```
ContactMangAPI/
 ├── Controllers/       # API endpoints
 ├── Data/              # DbContext and configuration
 ├── DTOs/              # Data transfer objects
 ├── Models/            # Entity models
 ├── Services/          # Business logic (Auth & Contact services)
 ├── Settings/          # JWT configuration
 ├── Program.cs         # Application entry point
 └── appsettings.json   # Configuration (DB connection, JWT settings)
```

---

## Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/Ritaj19Zamel/ContactMangAPI.git
cd ContactMangAPI
```

### 2. Configure the Database
Update `appsettings.json` with your SQL Server connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\ProjectModels;Database=AddressBookDb;Integrated Security=True;TrustServerCertificate=True;"
}
```

### 3. Run Migrations
```bash
dotnet ef database update
```

### 4. Run the Application
```bash
dotnet run
```

The API will be available at:
```
http://localhost:5052
https://localhost:7250
```

Swagger UI:
```
https://localhost:5001/swagger
```

---

## API Endpoints

### Authentication
- `POST /api/users/register` → Register a new account  
- `POST /api/users/login` → Authenticate and receive JWT  

### Contacts (requires JWT in `Authorization: Bearer <token>`)
- `POST /api/contacts` → Add a new contact  
- `GET /api/contacts` → List all contacts  
  - Supports sorting (`sortBy=FirstName|LastName|BirthDate`, `sortOrder=asc|desc`)  
  - Supports pagination (`pageNumber`, `pageSize`)  
- `GET /api/contacts/{id}` → Get contact by ID  
- `PUT /api/contacts/{id}` → Update a contact  
- `DELETE /api/contacts/{id}` → Delete a contact  

---

## JWT Authentication

- After logging in, copy the returned JWT token.  
- In Postman, add it to your request headers:
```
Authorization: Bearer <your-token-here>
```

---
## Postman Documentation

You can test all API endpoints directly using Postman.  
👉 [View Full Postman Documentation](https://documenter.getpostman.com/view/31177694/2sB3HnJegu)

---
## Demo Video
Link: [Demo Video](https://drive.google.com/file/d/109fPTNhtiZ7Dz0C_gvGoUaiaK5YyYttO/view?usp=sharing)  
A short video is included in the deliverables showing:  
- Registering a user  
- Logging in and getting a JWT  
- Adding, listing (with sorting and pagination), updating, and deleting contacts in Postman  

