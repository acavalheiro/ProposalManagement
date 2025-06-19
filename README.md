# Proposal Management API

Simple Proof of Concept of using Clena Architecture.

## 🛠️ Technologies Used

- **.NET Core/8+** - Backend framework
- **Entity Framework Core** - ORM for database operations
- **SQL Server** - Primary database
- **ASP.NET Core Web API** - RESTful API framework
- **JetBrains Rider** - Primary IDE
- **SQL Server Management Studio (SSMS) 21** - Database management


## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/acavalheiro/ProposalManagement.git
cd proposal-management-api
```

### 2. Database Setup

#### Important: Database Schema and Seeding Order

⚠️ **Critical Setup Steps:**

1. **Database Schema**: Run script `001-Model.sql` first to create the database schema
2. **Seed Data**: Run script `002-Seed.sql` after the schema to populate initial data

#### Using SQL Server Management Studio (SSMS):

1. Open SSMS and connect to your SQL Server instance
2. Execute the database scripts in the following order:
   - **First**: Run `001_CreateSchema.sql` (or similar schema file)
   - **Second**: Run `002_SeedData.sql` (or similar seed file)


### 3. Configuration

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "ProposalManagement": "Server=(localdb)\\mssqllocaldb;Database=ProposalManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true;"
  }
}
```

### 4. Install Dependencies

```bash
dotnet restore
```

### 5. Run the Application

```bash
dotnet run
```

The API will be available at `https://localhost:5001` or `http://localhost:5000`.

## 📚 API Documentation

Once the application is running, you can access:

- **Swagger UI**: `https://localhost:5001/swagger`
- **API Endpoints**: `https://localhost:5001/api/`

### Main Endpoints

- `GET /api/Proposal/GetByItem` - Get all proposals by Item
- `POST /api/Proposal/Create` - Create new proposal
- `PUT /api/Proposal/{id}/CounterProposal` - Create a counter proposal based on existing one
- `PUT /api/Proposal/{id}/Approve` - Approve proposal
- `PUT /api/Proposal/{id}/Reject` - Reject proposal

  ### 5. Consideration
  -Authenticated Users, to "emulate" an authenticated user, every request has the property "authenticatedUserId" Guid.


