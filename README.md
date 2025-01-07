# ProductsAPI

Welcome to ProductsAPI! This API provides detailed access to product information within our company. It supports viewing operations for both registered and unregistered users, and create, update, delete operations for users with staff roles via OAuth security.

## Live project
https://thamco-productsapi-drf5guh7dfa8ceb7.uksouth-01.azurewebsites.net/products

## Features
- **Secure Access**: Enhanced security with OAuth for staff operations.
- **DevOps Practices**: Automated build, test, deployment, and database migrations through Azure.
- **Resilience**: Implementation of Retry and Circuit Breaker patterns.
- **Anti-XSRF**: Tools in place to prevent cross-site request forgery attacks.

## Requirements
- .NET Core 8.0 or later
- Azure subscription for deployment
- Postman for API testing

## Getting Started
Follow these steps to get your local development environment running:

1. **Clone the repository**:
   ```bash
   git clone [repository-url]
2. **Restore dependancies**:
   ```bash
   dotnet restore
3. **Build the project**:
   ```bash
   dotnet build
4. **Run the application**:
   ```bash
   dotnet run

## Usage
Use Postman to interact with the API. Here are some common requests:

#### View Products:
GET /api/products

#### Add a New Product (Staff only):
POST /api/products

## Contributing
Interested in contributing? Here's how you can help:

1. Fork or branch the repository.
2. Make changes and ensure tests pass in ProductsAPI.Test.
3. Submit a pull request detailing your changes.
