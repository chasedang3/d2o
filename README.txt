## Prerequisites

- **Node.js** (v18) and **npm** installed globally.
- **Angular CLI** installed globally.
- **.NET SDK** (v8.0).
- **SQL Server** for database management.

## Getting Started

### Setting Up the Frontend

1. **Navigate to the Frontend Directory**:
   ```bash
   cd d2o-frontend

2. Install Dependencies:
	npm install

### Setting Up the Backend
1. Navigate to the Backend Directory:
	cd d2o-backend
2. Restore Dependencies:
	dotnet restore

### Start the Angular Development Server:
	ng serve
The application should be running on http://localhost:4200


Start the .NET Core API:
	dotnet run
The API should be running on https://localhost:7104

Setting up your database before running. 
- Change connection string in appsetting.json to your database.
- Run command in nugget management console
`` update-database



 
