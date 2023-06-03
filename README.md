# ToTask 

ToTask is a simple todo app implemented as an ASP.NET Core API project using Dapper as the data access framework. This readme file provides essential information for setting up and running the project.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) (version 3.1 or later)
- MySQL Server

## Setup

1. Clone the repository:
   ```shell
   git clone https://github.com/ciomeczek/ToTask
   cd ToTask
   ```
   
2. Restore project dependencies:
    ```shell
    dotnet restore
    ```
   
3. Create a copy of the `.env.dist` file and rename it to `.env.` This file will store configuration values for the application.
    ```shell
    cp .env.dist .env
    ```
    Modify the values in the `.env` file to match your environment. The `.env` file takes precedence over the appsettings.json file for configuration values.

3. Configuration Options:
   * Option 1: Using .env file (recommended)
     * Create a copy of the `.env.dist` file and rename it to `.env.` This file will store configuration values for the application.
       ```shell
       cp .env.dist .env
       ```
     * Modify the values in the `.env` file to match your environment. The `.env` file takes precedence over the `appsettings.json` file for configuration values.
   * Option 2: Using appsettings.json file
     * Update the values in the `appsettings.json` file to match your environment.

## MySQL Database Setup
ToTask uses a MySQL database for storing todo items. Follow these steps to set up the MySQL database:

1. Create a new MySQL database:
    ```sql
    CREATE DATABASE DB;
    ```
   
2. Select the newly created database:
    ```sql
    USE DB;
    ```

3. Create a table to store user information:
    ```sql
    CREATE TABLE `Users` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
    `Password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `Users_UN` (`Username`)
    ) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
    ```

4. Create a table to store todo items:
    ```sql
    CREATE TABLE `Todos` (
      `Id` int NOT NULL AUTO_INCREMENT,
      `Title` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
      `Content` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
      `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
      `UserId` int NOT NULL,
      PRIMARY KEY (`Id`),
      KEY `Todos_FK` (`UserId`),
      CONSTRAINT `Todos_FK` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`)
    ) ENGINE=InnoDB AUTO_INCREMENT=80 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
    ```

## Running the Application
To run the ToTask API, execute the following command:

```shell
dotnet run
```

The application will start, and you can access it at http://localhost:5115 (or https://localhost:5115 with HTTPS).

## License

This project is licensed under the GNU General Public License (GPL). Feel free to modify and use it according to the terms and conditions of the GPL.
