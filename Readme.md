# Bulk Transaction Service

<img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="c#" height="28px"> <img src="https://img.shields.io/badge/.NET8-5C2D91?style=for-the-badge&logo=&logoColor=white" alt=".NET8" height="28px"> <img src="https://img.shields.io/badge/postgresql-4169e1?style=for-the-badge&logo=postgresql&logoColor=white" alt="postgresql" height="28px"> 

A simple web service to perform bulk salary payment transaction.\
This is a practice app to demonstrate one of the possible approaches.

## How to run the app

The app and the corresponding database are conteinerized with docker.\
You need docker installed in order use it. See <a href="https://docs.docker.com/get-started/get-docker/">docker website</a> for more details.

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

---

Alternatively, you can run the app and database without using docker contaier.\
First, you need to set up your own instance of a database.\
This app uses Postgresql. You can find more information on the <a href="https://www.postgresql.org/">Postgres website</a>.\
Then you need to update the connection string in the appsettings.json file.

After your database and connection string are ready,\
you can execute the following command from the solution folder to start the app:

```
dotnet run --project src/BulkTransactionServiceWebApi/BulkTransactionServiceWebApi.csproj
```

## Dependencies

The app is build with and targets .NET 8 (LTS).\
You need to have .NET 8 Runtime installed in order to run the app.\
Visit <a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0">Microsoft website</a> for more information.

The app and the corresponding database are conteinerized with docker.\
You need docker installed in order use it. See <a href="https://docs.docker.com/get-started/get-docker/">docker website</a> for more details.

This app uses Postgresql. If you don't want to use postgres in docker container\
you have to set up your own instance of the database.You can find more\
information on the <a href="https://www.postgresql.org/">Postgres website</a>.