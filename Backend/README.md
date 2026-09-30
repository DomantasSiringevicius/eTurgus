# Backend

ASP.NET Core Web API su švaria sluoksnine (Clean/Onion) architektūra ir MySQL duomenų baze.

## Struktūra

```
Backend.sln
src/
  Domain/               # nepriklauso nuo nieko
    Entities/
    IRepositories/
    IServices/
  Application/           # priklauso nuo Domain
    Dtos/
    Mappers/
    Services/
    UseCases/
  Infrastructure/        # priklauso nuo Domain + Application
    Migrations/
    Repositories/
    Seeders/
    DbContext.cs
  Presentation/           # priklauso nuo visų (composition root)
    Controllers/
    Program.cs
```

Priklausomybių kryptis: `Presentation -> Infrastructure -> Application -> Domain` (Domain nieko neimportuoja).

## Paleidimas

1. **Pakeisk MySQL prisijungimo duomenis** faile `src/Presentation/appsettings.json` (ir/arba `appsettings.Development.json`):

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Port=3306;Database=backend_db;User=root;Password=your_password;"
   }
   ```

2. **Įdiek EF Core CLI įrankį** (jei dar neturi):

   ```bash
   dotnet tool install --global dotnet-ef
   ```

3. **Atkurk paketus**:

   ```bash
   dotnet restore
   ```

4. **Sukurk pirmąją migraciją** (kadangi `Migrations/` kol kas tuščias):

   ```bash
   dotnet ef migrations add InitialCreate \
     --project src/Infrastructure \
     --startup-project src/Presentation
   ```

5. **Paleisk projektą** (migracijos ir seed'inimas įvyks automatiškai per `Program.cs`):

   ```bash
   dotnet run --project src/Presentation
   ```

6. Atsidaryk Swagger: **http://localhost:5080/swagger**

## Pastabos

- Naudojama `Pomelo.EntityFrameworkCore.MySql` paketas MySQL prieigai.
- `Program.cs` startuojant automatiškai kviečia `db.Database.Migrate()` ir `ProductSeeder.SeedAsync(db)` – patogu dev aplinkai; produkcinėje aplinkoje migracijas paprastai geriau paleisti atskiru CI/CD žingsniu.
- Pridėtas `Product` entity/DTO/UseCase/Controller rinkinys yra pavyzdys, parodantis, kaip sluoksniai susijungia — laisvai keisk ar trink pagal savo domeną.
