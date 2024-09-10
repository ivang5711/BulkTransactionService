# Bulk Transaction Service

<img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="c#" height="28px"> <img src="https://img.shields.io/badge/.NET8-5C2D91?style=for-the-badge&logo=&logoColor=white" alt=".NET8" height="28px"> <img src="https://img.shields.io/badge/postgresql-4169e1?style=for-the-badge&logo=postgresql&logoColor=white" alt="postgresql" height="28px"> <img src="https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=fff&style=for-the-badge" alt="Docker Badge">

A simple web service to perform bulk salary payment transaction.\
This is a practice app to demonstrate one of the possible approaches.

## Table of contents

- [Bulk Transaction Service](#bulk-transaction-service)
    - [Table of contents](#table-of-contents)
    - [Description](#description)
    - [How to run the app](#how-to-run-the-app)
        - [Run in Docker containers](#run-in-docker-containers)
        - [Run directly without using Docker containers](#run-directly-without-using-docker-containers)
    - [How to use](#how-to-use)
    - [Dependencies](#dependencies)
    - [Wrong Calls](#wrong-calls)
        - [IBAN not found](#iban-not-found)
        - [IBAN not valid](#iban-not-valid)
        - [Empty organization\_name](#empty-organization_name)
        - [organization\_bic misusage](#organization_bic-misusage)
        - ["abc" as amount](#abc-as-amount)

## Description

The app allows a user to perform a bulk payment transaction.
The app consists of WebApi with a single endpoint and a database.

<!-- TODO: install a Markdown linter since there are some warnings in the MD file. -->
<!-- TODO: The list is bad-indented. -->

The data flow:
1. The Salaries endpoint accepts POST requests. 
2. Then the body of a request goes through validation and mapping from\
request object to domain object.
1. Then the payment is done by invoking "PaySalaryAsync" method of the\
Payment Service.
1. The Payment service invokes "PerformPaymentAsync" method of the PaymentRepository class.
2. The "PerformPaymentAsync" method performs operations on database.\
5.1 First it retrieves from the database the account id of a user\
which performed the request.\
5.2 Then it updates the domain object with the corresponding account id.\
5.3 Then it calculates the total amount requested.\
5.4 Then it begins a SQL transaction and invokes a stored procedure to check\
if the bank account have enough funds to perform all the payments.\
5.5 In case the 5.4 stage was a success the account balance decreases by total\
amount of all requested transactions, and each of the requested transactions\
gets added to the transactions table in the database.

In case of success the methods return true to the calling counterparts and the controller returns 201 code to the caller eventually.

In case of failure of the transaction in the repository class the transaction\
rolls back, no changes apply to the database tables. The repository and\
service methods returen false and the controller returns 422 code to the caller.

Additional notes:

The app uses built-in DI container to register and inject dependencies.\
Also it uses Mapster mapping library to simplify code and map request to domain\
object in a cleaner way.
To handle Database initialization and migrations\
the app uses DbUp-PostgreSQL library.\
To perform database related operations the app uses Dapper.\
To perform validation the app uses FluentValidation.

## How to run the app

### Run in Docker containers

The app and the corresponding database are conteinerized with docker.\
You need docker installed in order use it. See <a href="https://docs.docker.com/get-started/get-docker/">Docker website</a> for more details.

Use docker compose to build and run the app:

Navigate to the source code folder and execute the command:

```
docker compose up --build
```


>**Note:** The script above build containers, performs database initialization\
and starts the containers.

To stop the containers use the following command:

```
docker stop bulk-transfer-api bulk-transfer-db
```

To start the containers altogether use the command:

```
docker start bulk-transfer-api bulk-transfer-db
```
>**Note:** You can also start/stop the containers separately

The database container uses attached volume to store data persistently.\
You can drop the data in the volume by using the command:

```
docker volume rm bulktransactionservice_postgres_data
```
>**Note:** The database container have to be stopped before dropping the attached volume
<!-- FIXME: wrong, they also need to be removed from stopped containers. -->
<!-- You have some options: manual remove containers with "docker rm -f <first 3 digits of containers>, -->
<!-- use "docker compose down --volumes" -->
<!-- use "docker volume rm bulktransactionservice_postgres_data --force" to see which containers are using your volume -->

### Run directly without using Docker containers

Alternatively, you can run the app and database without using docker contaier.\
First, you need to set up your own instance of a database.\
This app uses Postgresql. You can find more information on the <a href="https://www.postgresql.org/">Postgres website</a>.\
Then you need to update the connection string in the appsettings.json file.

After your database and connection string are ready,\
you can execute the following command from the solution folder to start the app:

```
dotnet run --project src/BulkTransactionServiceWebApi/BulkTransactionServiceWebApi.csproj
```

## How to use

You can send post requests to the {YourbaseUrl}/Salary endpoint.\
There are 2 sample requests stored alongside the source code for your disposal.\
The samples contain requests from the same sender.

[sample1.http](/requests/sample-request-1.http)\
[sample2.http](/requests/sample-request-2.http)

Corresponding account record already present in database. It initializes with all the necessuary values to test the functionality of the app with the sample requests.

Because the purpose of the app is to demonstrate bulk request handling, there\
are no other endpoints and no defined ways to setup new account but the direct database access.

## Dependencies

The app is build with and targets .NET 8 (LTS).\
You need to have .NET 8 Runtime installed in order to run the app.\
Visit <a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0">Microsoft website</a> for more information.

The app and the corresponding database are conteinerized with docker.\
You need docker installed in order use it. See <a href="https://docs.docker.com/get-started/get-docker/">Docker website</a> for more details.

This app uses Postgresql. If you don't want to use postgres in docker container\
you have to set up your own instance of the database.You can find more\
information on the <a href="https://www.postgresql.org/">Postgres website</a>.

## Wrong Calls

Below, you can find a list of calls that don't work as expected. You have two options:

1. Repro them via a REST-api client (Postman, cURL, insomnia) and fix them in the source code
2. You can write automated tests such as unit, integration or end2end tests

My favorite way is the second one but you can evaluate to mix them.

### IBAN not found

If you change the IBAN code to something not existing in the DB, you'll be presented with a stack trace as the response. `500` status code should not be returned here.

### IBAN not valid

If you change the IBAN code to something not valid, you'll be presented with a stack trace as the response. `500` status code should not be returned here.

### Empty organization_name

It must not return a `422` error.

### organization_bic misusage

If I use a wrong `organization_bic` value, it doesn't matter since you lookup accounts only by `organization_iban`.

### "abc" as amount

Manage this scenario, you cannot return a stack trace. As a rule of thumb, **a stack trace never has to be returned.**

