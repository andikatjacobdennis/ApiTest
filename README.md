# ApiTest

## Overview

`ApiTest` is a .NET Core project designed to manage products through a RESTful API. The API provides endpoints to create, retrieve, update, and list products. The project includes a web-based user interface and utilizes Swagger for API documentation and testing.

## Version Information

- **.NET Core Version**: 8.0.400
- **Entity Framework Core Version**: 8.0.8
- **Swagger Version**: 6.7.3
- **MOQ Version**: 4.20.72

## Operating System and Browser Support

### Supported Operating Systems (Tested)

- Windows 11 64-bit

### Supported Browsers (Tested)

- **Google Chrome Version**: 128.0.6613.121 (Official Build) (64-bit)
- **Opera Version**: 113.0.5230.62

## Libraries and Dependencies

The project uses the following libraries and dependencies:

- **Microsoft.EntityFrameworkCore.InMemory**: Provides an in-memory database for development and testing.
- **Microsoft.AspNetCore.Mvc**: For building the API endpoints and handling HTTP requests.
- **Swashbuckle.AspNetCore**: For generating Swagger documentation and UI.
- **Moq**: For unit testing and mocking dependencies.
- **MSTest.TestFramework**: For running unit tests.

## In-Memory Feature

For development and testing purposes, `ApiTest` uses an in-memory database. This feature allows you to run and test the application without needing a persistent database. All data is stored in memory and will be lost when the application restarts.

### Benefits

- **Quick Setup**: No need to configure a database server or connection strings.
- **Easy Testing**: Ideal for running unit tests and development without worrying about database state.

### How It Works

- **In-Memory Database Provider**: The project uses `Microsoft.EntityFrameworkCore.InMemory` to provide an in-memory implementation of the Entity Framework Core database provider.
- **Configuration**: In the `Program.cs` file, the application is configured to use the in-memory provider for development and testing environments.

To switch to an actual database for production, you would need to update the configuration to use a different database provider (e.g., SQL Server).

## Swagger UI

Swagger provides a convenient web interface for interacting with and testing your API endpoints. You can access Swagger UI by navigating to the following URL in your browser:

<a href="https://localhost:7134/Swagger/index.html" target="_blank">https://localhost:7134/Swagger/index.html</a>

### Using Swagger

1. **Open Swagger UI**: Navigate to the Swagger URL mentioned above.
2. **Explore Endpoints**: You will see a list of available API endpoints.
3. **Execute Requests**: Click on an endpoint to expand its details. You can fill in parameters and click the "Try it out" button to execute the request.
4. **View Responses**: Swagger will display the response status and data returned by the API.

## Product Page

You can view and interact with the product list through the web interface at:

<a href="https://localhost:7134/Products" target="_blank">https://localhost:7134/Products</a>

### Using the Product Page

1. **Access the Page**: Navigate to the provided URL in your browser.
2. **View Products**: The page displays a list of products retrieved from the API.
3. **Interact with Data**: Depending on the implementation, you may be able to add, update, or delete products directly from this page.

## Project Description

The `ApiTest` project includes the following main components:

1. **API Endpoints**: Managed through `ProductsController`, which includes actions for:
   - **CreateProduct**: Adds a new product to the database.
   - **GetProduct**: Retrieves a product by its ID.
   - **GetProducts**: Lists all products with optional filtering.
   - **UpdateProduct**: Updates an existing product’s details.

2. **Services**: The `IProductService` interface defines methods for product operations. Implementations of this interface handle data retrieval and persistence through the `ProductService`.

3. **Models**: The `Product` model represents the data structure of a product, including properties such as `Id`, `Name`, `Description`, `Price`, `Created`, and `LastUpdated`.

4. **In-Memory Database**: Uses `Microsoft.EntityFrameworkCore.InMemory` for development and testing, which allows quick setup and easy testing without a persistent database.

5. **Testing**: Unit tests for the controller are provided to ensure the correctness of API actions and service interactions.

## UML Diagrams

- **Use Case Diagram**:

 <a href="https://github.com/andikatjacobdennis/ApiTest/blob/main/Media/Diagrams/use-case-diagram.png" target="_blank">![Use Case Diagram](https://github.com/andikatjacobdennis/ApiTest/blob/main/Media/Diagrams/use-case-diagram.png)</a>
 
- **Class Diagram**:

<a href="https://github.com/andikatjacobdennis/ApiTest/blob/main/Media/Diagrams/class-diagram.png" target="_blank">![Class Diagram](https://github.com/andikatjacobdennis/ApiTest/blob/main/Media/Diagrams/class-diagram.png)</a>

- **Sequence Diagram**:

<a href="https://github.com/andikatjacobdennis/ApiTest/blob/main/Media/Diagrams/sequence-diagram.png" target="_blank">![Sequence Diagram](https://github.com/andikatjacobdennis/ApiTest/blob/main/Media/Diagrams/sequence-diagram.png)</a>

## Getting Started

To get started with the project:

1. **Clone the Repository**:

    ```bash
    git clone https://github.com/andikatjacobdennis/ApiTest.git
    cd your-repository-folder
    ```
    
    Alternatively you can use Visual Studio 2022 to clone

2. **Restore Dependencies**:

    ```bash
    dotnet restore
    ```

3. **Build the Solution**:

    ```bash
    dotnet build
    ```

4. **Run the Application**:

    ```bash
    cd ApiTest
    dotnet run
    ```

5. **Open Swagger UI**: Access Swagger UI at <a href="https://localhost:7134/Swagger/index.html" target="_blank">https://localhost:7134/Swagger/index.html</a>.

 <a href="https://github.com/andikatjacobdennis/ApiTest/blob/main/Media/Screenshots/swagger.png" target="_blank">![Swagger UI](https://github.com/andikatjacobdennis/ApiTest/blob/main/Media/Screenshots/swagger.png)</a>

6. **Access the Product Page**: Navigate to <a href="https://localhost:7134/Products" target="_blank">https://localhost:7134/Products</a> to interact with the product data.

  <a href="https://github.com/andikatjacobdennis/ApiTest/blob/main/Media/Screenshots/page.png" target="_blank">![Product Page](https://github.com/andikatjacobdennis/ApiTest/blob/main/Media/Screenshots/page.png)</a>

## Contributing

Feel free to contribute by submitting issues or pull requests. For more details on how to contribute, please refer to the CONTRIBUTING.md file.

## License

This project is licensed under the MIT License. See the LICENSE.md file for details.
