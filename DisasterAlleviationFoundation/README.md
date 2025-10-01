# Disaster Alleviation Foundation - Web Application

This is an ASP.NET Core MVC web application converted from a WPF desktop application. The application provides a platform for the Disaster Alleviation Foundation to manage user authentication, volunteer registration, donations, and emergency response coordination.

## Features

### 🔐 Authentication System
- User registration and login
- Session-based authentication
- Role-based access (User, Volunteer, Admin, Donor)
- Secure password hashing using PBKDF2

### 📊 Dashboard
- Personalized welcome message
- Real-time statistics display
- Recent activity tracking
- Mission statement and organization information

### 🎯 Core Functionality
- **Volunteer Registration**: Users can sign up to become volunteers
- **Donation System**: Platform for accepting donations
- **Emergency Reporting**: System for reporting and tracking emergencies
- **User Management**: Complete user profile and account management

### 🎨 Modern UI/UX
- Responsive Bootstrap 5 design
- Font Awesome icons
- Modern gradient styling
- Mobile-friendly interface
- Accessible design patterns

## Technology Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Frontend**: Bootstrap 5, Font Awesome, jQuery
- **Session Management**: In-memory session storage
- **Authentication**: Custom session-based authentication
- **Styling**: Custom CSS with modern design patterns

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- A modern web browser

### Installation & Running

1. **Clone or navigate to the project directory**
   ```bash
   cd DisasterAlleviationFoundation
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the application**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the application**
   - Open your browser and navigate to: `http://localhost:5000`
   - The application will automatically redirect to the login page

### Demo Accounts

For testing purposes, the application includes pre-configured demo accounts:

- **Admin Account**
  - Email: `admin@daf.org`
  - Password: `admin123`

- **Volunteer Account**
  - Email: `volunteer@daf.org`
  - Password: `volunteer123`

## Project Structure

```
DisasterAlleviationFoundation/
├── Controllers/           # MVC Controllers
│   ├── AccountController.cs
│   ├── DashboardController.cs
│   └── HomeController.cs
├── Models/               # Data models and view models
│   ├── User.cs
│   ├── LoginViewModel.cs
│   ├── RegisterViewModel.cs
│   ├── DashboardViewModel.cs
│   └── AuthenticationResult.cs
├── Services/             # Business logic services
│   ├── IAuthenticationService.cs
│   └── AuthenticationService.cs
├── Views/                # Razor views
│   ├── Account/
│   ├── Dashboard/
│   └── Shared/
├── wwwroot/              # Static files (CSS, JS, images)
└── Program.cs            # Application startup configuration
```

## Key Conversion Changes from WPF

### Architecture Changes
- **MVVM to MVC**: Converted from WPF's MVVM pattern to ASP.NET Core MVC
- **Desktop to Web**: Transformed desktop UI to responsive web interface
- **Session Management**: Replaced in-memory user state with web sessions
- **View Models**: Adapted WPF view models to web-specific view models

### UI/UX Improvements
- **Responsive Design**: Mobile-first approach with Bootstrap 5
- **Modern Styling**: Contemporary web design with gradients and shadows
- **Accessibility**: Improved accessibility with proper ARIA labels and semantic HTML
- **User Feedback**: Enhanced user feedback with toast notifications and alerts

### Security Enhancements
- **CSRF Protection**: Anti-forgery tokens on all forms
- **Session Security**: Secure session configuration with HttpOnly cookies
- **Input Validation**: Server-side and client-side validation
- **Password Security**: Maintained strong password hashing from original

## Development Notes

### Session Configuration
- Session timeout: 30 minutes
- Cookie name: `DAF.Session`
- HttpOnly cookies for security
- In-memory session storage (suitable for development)

### Styling Approach
- Custom CSS with CSS Grid and Flexbox
- Bootstrap 5 for responsive components
- Font Awesome for consistent iconography
- CSS custom properties for theme colors

### Future Enhancements
- Database integration (Entity Framework Core)
- Email verification system
- Advanced role-based permissions
- Real-time notifications
- API endpoints for mobile app integration
- Docker containerization

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is part of the Disaster Alleviation Foundation and is intended for humanitarian purposes.

---

**Making a Difference Together** 🌍