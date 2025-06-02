# HIV Treatment System

A comprehensive system for managing HIV treatment records, patients, doctors, and appointments.

## Environment Setup

### Required Environment Variables

Create a `.env` file in the root directory with the following variables:

```env
# Database Configuration
DB_SERVER=your_database_server
DB_NAME=your_database_name
DB_USER=your_database_user
DB_PASSWORD=your_database_password

# JWT Configuration
JWT_SECRET_KEY=your_jwt_secret_key

# Email Configuration
EMAIL_USER=your_email@gmail.com
EMAIL_PASSWORD=your_email_app_password
```

### Development Setup

1. Clone the repository
2. Set up the environment variables
3. Run the following commands:

```bash
cd HIVTreatmentSystem
dotnet restore
dotnet build
dotnet run --project HIVTreatmentSystem.API
```

### Production Setup

1. Set up the environment variables on your production server
2. Build the application:

```bash
dotnet publish -c Release
```

3. Deploy the published files to your production server

## API Documentation

The API documentation is available at:
- Development: `https://localhost:5001/swagger`
- Production: `https://your-domain.com/api-docs`

## Security

- All sensitive data is stored in environment variables
- JWT authentication is required for protected endpoints
- CORS is configured to allow only specific origins
- HTTPS is enforced in production

## Features

- Patient management
- Doctor management
- Treatment records
- Appointment scheduling
- Authentication and authorization
- Email notifications

## Contributing

1. Create a feature branch
2. Make your changes
3. Submit a pull request

## License

This project is licensed under the Academic License. 