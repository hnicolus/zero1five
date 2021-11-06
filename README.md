# Zero1Five
<hr/>

## Technologies

* .NET Core 5.0
* ASP .NET Core 5.0
* Abp Framework 4.2
* Entity Framework Core 
* AutoMapper
* XUnit,Shouldy , NSubstitute

### Database Configuration

If you using Postgresql Verify that the **DefaultConnection** connection string within **appsettings.json** in the web project and the DbMigrator project points to a valid Postgresql Server instance. 

### Database Migrations

To use `dotnet-ef` for your migrations please add the following flags to your commands (I am assuming  you are executing from the solution directory)

* `-p src/Zero1Five.EntityFrameworkCore.DbMigrations` 
* `-s src/Zero1Five.Web`

For example, to add a new migration from the root folder:

 `dotnet ef migrations add "MIGRATION_NAME" -p src/Zero1Five.EntityFrameworkCore.DbMigrations -s src/Zero1Five.Web `

to run migrations you could easily do :
`dotnet ef database update -p src/Zero1Five.EntityFrameworkCore.DbMigrations -s src/Zero1Five.Web`

or go to the DbMigrator project and run type `dotnet run`


## Getting Started

1. Navigate to solution folder 
2. Open Terminal 
3. run `dotnet restore` 
4. Navigate to `src/Zero1Five.Web` and run `dotnet run` 

## Overview

<img src="https://github.com/abpframework/abp/blob/dev/docs/en/images/ddd-microservice-simple.png"/>

### Domain

This will contain all Aggregates and entities ,Business rules ,Business services , Business exceptions, interfaces,Shared types.

### Application

This layer contains all application logic, acts as a facade the presentation ,it basically   glues all the layers together. It  depends on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application .

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services,Storage

### Presentation

The Client that will be presentedto the User ,this can be a website or a mobile application or Desktop application

