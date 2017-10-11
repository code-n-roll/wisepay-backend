# WisePay

Backend

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

#### Configuring database
Download and install [PostgreSQL](https://www.postgresql.org/download/).

Create Login Role with name and password specified in WisePay.Web/appsettings.Development.json ConnectionStrings section (you can change them but don't do that).

Create database with the 'wisepaydb' name (see ConnectionStrings again).

#### Updating database

Open WisePay.Web project

Restore required packages using `dotnet restore`

Run migrations `dotnet ef -p ../WisePay.DataAccess database update`

### Running

Open WisePay.Web project

Run command `dotnet run` or `dotnet watch run`