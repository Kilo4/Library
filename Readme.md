
# How to navigate in the project
The project is divided into two directories
```bash
|-- Library
|  |-- local (for docker and scripts) 
|  |-- src (for code)
```

## How to run database and rabbitMq
From root of project run command
``docker-compose -f local/docker-compose.yaml up``

## Add and Run migration
Add migration
``dotnet ef migrations add <MigrationName> --context AppDbContext``

Migrations are always triggered when the application is launched

