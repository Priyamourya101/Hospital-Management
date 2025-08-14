# Hospital Management Backend

A comprehensive ASP.NET Core 8 backend for Hospital & Inventory Management using EF Core Code-First approach, JWT Authentication, Session Management, and Validation.

## Features

- **JWT Authentication**: Secure token-based authentication for all user types
- **Session Management**: User session tracking and management
- **Role-based Access Control**: Admin, Doctor, and Patient roles with different permissions
- **Appointment Management**: Complete appointment lifecycle management
- **Inventory Management**: Stock tracking, low stock alerts, and expiry monitoring
- **Order Management**: Patient order processing and tracking
- **Feedback System**: Patient feedback collection and management
- **Prescription Management**: Digital prescription issuance and tracking

## Technology Stack

- **Framework**: ASP.NET Core 8
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **Validation**: FluentValidation
- **API Documentation**: Swagger/OpenAPI

## Database Schema

### Core Entities
- **Admin**: System administrators
- **Doctor**: Medical professionals with specializations
- **Patient**: Hospital patients with medical history
- **Appointment**: Doctor-patient appointments
- **Inventory**: Medical supplies and equipment
- **Order**: Patient product orders
- **Feedback**: Patient feedback and ratings
- **Prescription**: Medical prescriptions

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/logout` - User logout

### Admin Operations
- `GET /api/admin/dashboard` - Admin dashboard statistics
- `GET /api/admin/details` - Admin profile details
- `GET /api/admin/appointments/all` - View all appointments

### Doctor Management
- `POST /api/doctor/register` - Register new doctor
- `GET /api/doctor/all` - Get all doctors
- `GET /api/doctor/{id}` - Get doctor by ID
- `PUT /api/doctor/{id}` - Update doctor details
- `DELETE /api/doctor/{id}` - Delete doctor
- `POST /api/doctor/issue-prescription` - Issue prescription
- `GET /api/doctor/prescription/{id}` - Get prescription details

### Patient Management
- `POST /api/patient/register` - Register new patient
- `GET /api/patient/all` - Get all patients
- `GET /api/patient/{id}` - Get patient by ID
- `GET /api/patient/details` - Get current patient details
- `PUT /api/patient/{id}` - Update patient details
- `DELETE /api/patient/{id}` - Delete patient

### Appointment Management
- `POST /api/appointment/patient` - Create appointment (patient)
- `GET /api/appointment/patient` - Get patient appointments
- `GET /api/appointment/doctor` - Get doctor appointments
- `PUT /api/appointment/status` - Update appointment status
- `GET /api/appointment/{id}` - Get appointment details

### Inventory Management
- `POST /api/inventory` - Add inventory item
- `GET /api/inventory` - Get all inventory
- `GET /api/inventory/{id}` - Get inventory by ID
- `PUT /api/inventory/{id}` - Update inventory
- `DELETE /api/inventory/{id}` - Delete inventory
- `GET /api/inventory/search` - Search inventory
- `GET /api/inventory/low-stock` - Get low stock items
- `GET /api/inventory/expiring` - Get expiring items
- `GET /api/inventory/dashboard` - Inventory dashboard

### Order Management
- `POST /api/order/place-order` - Place order (patient)
- `GET /api/order/patient` - Get patient orders
- `GET /api/order/{id}` - Get order details
- `PUT /api/order/{id}` - Update order
- `DELETE /api/order/{id}` - Delete order
- `GET /api/order/all` - Get all orders

### Feedback Management
- `POST /api/feedback/submit` - Submit feedback (patient)
- `GET /api/feedback/all` - Get all feedback
- `GET /api/feedback/{id}` - Get feedback by ID
- `GET /api/feedback/patient` - Get patient feedback

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Hospital
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection string**
   Edit `appsettings.json` and update the connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HospitalDB;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access Swagger UI**
   Navigate to `https://localhost:7001/swagger` to view the API documentation.

### Default Admin Credentials
- **Email**: admin@hospital.com
- **Password**: admin123

## Authentication

The API uses JWT tokens for authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

## Session Management

User sessions are managed server-side and include:
- User role (Admin/Doctor/Patient)
- User email
- User ID

## Error Handling

All endpoints return consistent error responses:
- `400 Bad Request` - Invalid input data
- `401 Unauthorized` - Authentication required
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

Error response format:
```json
{
  "message": "Error description",
  "error": "Detailed error information (in development)"
}
```

## Security Features

- **Password Hashing**: All passwords are hashed using SHA256
- **JWT Tokens**: Secure token-based authentication
- **Session Management**: Server-side session tracking
- **Input Validation**: Comprehensive request validation
- **CORS Configuration**: Cross-origin resource sharing setup

## Database Migrations

The application uses EF Core Code-First approach. The database will be automatically created when the application starts for the first time.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License. 