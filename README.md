# Neighbours

## Quickstart

First, clone this repository. Then:

- Install the .NET Entity Framework CLI
  - `dotnet tool install --global dotnet-ef`
- Create the database/s in `psql`
  - `CREATE DATABASE neighbours;`
  - `CREATE DATABASE neighbours_development;`
  - `CREATE DATABASE neighbours_test;`
- Run the migration to create the tables
  - `cd` into `/NeighboursApp`
  - `dotnet ef database update`
  - `DATABASE_NAME=neighbours dotnet ef database update`
- Start the application, with the development database
  - `DATABASE_NAME=neighbours_development dotnet watch run`
- Go to `http://localhost:5123/`

## Running the Tests

- Install Chromedriver
  - `brew install chromedriver`
- Start the application, with the default (test) database
  - `dotnet watch run`
- Open a second terminal session and run the tests
  - `dotnet test`

### Troubleshooting

If you see a popup about not being able to open Chromedriver...

- Go to **System Preferences > Security and Privacy > General**
- There should be another message about Chromedriver there
- If so, Click on **Allow Anyway**

## Updating the Database

Changes are applied to the database programatically, using files called _migrations_, which live in the `/Migrations` directory. The process is as follows...

- Add model/s
  - For example, you might want to add a `Listings` table
  - We need to create a new table in the same way
- Generate the migration file
  - `cd` into `/NeighboursApp`
  - Decide what you want to call the migration file
  - `CreateListingsTable` would work for this one
  - `dotnet ef migrations add CreateListingsTable`
- Run the migration
  - `dotnet ef database update`

- Change the model/s
  * For example, you might want to add a title to the `Listing` model
  * In which case, you would add a new field there

- Generate the migration file
  - `cd` into `/NeighboursApp`
  - Decide what you wan to call the migration file
  - `AddTitleToLisitings` would work for this one
  - `dotnet ef migrations add AddTitleToListings`
- Run the migration
  - `dotnet ef database update`

### Troubleshooting

#### Seeing `role "postgres" does not exist`?

Your application tries to connect to the database as a user called `postgres`, which is normally created automatically when you install PostgresQL. If the `postgres` user doesn't exist, you'll see `role "postgres" does not exist`.

To fix it, you'll need to create the `postgres` user.

Try this in your terminal...

```
; createuser -s postgres
```

If you see `command not found: createuser`, start a new `psql` session and do this...

```sql
create user postgres;
```

#### Want to Change an Existing Migration?

Don't edit the migration files after they've been applied / run. If you do that, it'll probably lead to problems. If you decide that the migration you just applied wasn't quite right for some reason, you have two options

- Create and run another migration (using the process above)

OR...

- Rollback / undo the last migration
- Then edit the migration file before re-running it

How do you rollback a migration? Let's assume that you have two migrations, both of which have been applied.

1. CreateUsersTable
2. CreateListingsTable

To rollback the second, you again use `dotnet ef database update` but this time adding the name of the last 'good' migration. In this case, that would be `CreatedUsersTable`. So the command is...

- `dotnet ef database update CreateUsersTable`
- then `dotnet ef migrations remove` to remove the the migration file you want to revert/rollback

```

### Created by Team CSharpers

- Our Trello board https://trello.com/b/YRIgjUUX/final-project
```
