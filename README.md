# Product Manager

A sample web application with .Net Core 8.0 with Clean Architecture and CQRS patterns.

## Description

This project reflects a product manager service and it includes:
* Clean Architecture pattern
* CQRS pattern
* MediatR
* Unit tests with xUnit
* Entity Framework Core

## Getting Started

### Dependencies

* .Net Framework Core 8.0
* Microsoft SQL server 2022
* (Optional) Docker

### Installing
Install Nuget dependencies with
```shell
dotnet restore
```

### Executing program

Start compose.yml to install and run Microsoft SQL Server and .Net core 8.0 images, and then run the web application with
```shell
docker compose up -d
```
