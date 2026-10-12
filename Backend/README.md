# eTurgus Backend

Elektroninės prekyvietės API. Parduotuvės skelbia prekes, o pirkėjai rašo prekių atsiliepimus.

Naudojama: ASP.NET Core 8 Web API, Entity Framework Core, SQLite, MediatR.

## Objektai

`Shop` → `Product` → `Review`

Parduotuvė turi daug prekių, prekė turi daug atsiliepimų.

## Struktūra

```
Backend.sln
src/
  Domain/               # esybės ir repozitorijų sąsajos
    Entities/
    IRepositories/
  Application/          # DTO, išimtys ir panaudojimo atvejai (MediatR)
    Dtos/
    Exceptions/
    UseCases/
  Infrastructure/       # duomenų bazė
    Repositories/
    DbContext.cs
    DbSeeder.cs
  Presentation/         # kontroleriai ir programos paleidimas
    Controllers/
    Hypermedia/
    Program.cs
```

Priklausomybių kryptis: `Presentation -> Infrastructure -> Application -> Domain`.

## Paleidimas

Reikia .NET 8 SDK.

```bash
dotnet run --project src/Presentation
```

API pasileidžia adresu http://localhost:5080, Swagger: http://localhost:5080/swagger

Duomenų bazės paruošti nereikia. Pirmo paleidimo metu sukuriamas failas `src/Presentation/backend.db` ir užpildomas pradiniais duomenimis (5 parduotuvės su prekėmis ir atsiliepimais). Norint pradėti nuo švarių duomenų, užtenka šį failą ištrinti ir paleisti iš naujo.

Prisijungimo eilutė yra `src/Presentation/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=backend.db"
}
```

## Prieigos taškai

| Metodas | Kelias | Aprašymas |
|---|---|---|
| GET | `/api/shops` | Parduotuvių sąrašas |
| POST | `/api/shops` | Sukurti parduotuvę |
| GET | `/api/shops/{shopId}` | Viena parduotuvė |
| PUT | `/api/shops/{shopId}` | Atnaujinti parduotuvę |
| DELETE | `/api/shops/{shopId}` | Ištrinti parduotuvę |
| GET | `/api/shops/{shopId}/summary` | Parduotuvės suvestinė (prekių ir atsiliepimų skaičius, įvertinimų vidurkis) |
| GET | `/api/shops/{shopId}/products` | Parduotuvės prekių sąrašas |
| POST | `/api/shops/{shopId}/products` | Sukurti prekę |
| GET | `/api/shops/{shopId}/products/{productId}` | Viena prekė |
| PUT | `/api/shops/{shopId}/products/{productId}` | Atnaujinti prekę |
| DELETE | `/api/shops/{shopId}/products/{productId}` | Ištrinti prekę |
| GET | `/api/shops/{shopId}/products/{productId}/reviews` | Prekės atsiliepimų sąrašas |
| POST | `/api/shops/{shopId}/products/{productId}/reviews` | Sukurti atsiliepimą |
| GET | `/api/shops/{shopId}/products/{productId}/reviews/{reviewId}` | Vienas atsiliepimas |
| PUT | `/api/shops/{shopId}/products/{productId}/reviews/{reviewId}` | Atnaujinti atsiliepimą |
| DELETE | `/api/shops/{shopId}/products/{productId}/reviews/{reviewId}` | Ištrinti atsiliepimą |

Duomenys POST ir PUT užklausose siunčiami kaip `form-data`.

### Sąrašų parametrai

Visi sąrašai puslapiuojami: `page` (numatyta 1) ir `pageSize` (numatyta 10, daugiausia 50).

| Sąrašas | Filtrai |
|---|---|
| Parduotuvės | `search` |
| Prekės | `search`, `minPrice`, `maxPrice` |
| Atsiliepimai | `minRating` |

Pavyzdys: `GET /api/shops/{shopId}/products?search=medus&maxPrice=10&page=1&pageSize=5`

### Statuso kodai

| Kodas | Kada |
|---|---|
| 200 | GET ir PUT pavyko |
| 201 | POST pavyko |
| 204 | DELETE pavyko |
| 400 | Trūksta lauko, netinkamas tipas arba blogi sąrašo parametrai |
| 404 | Resursas arba jo tėvinis objektas nerastas |
| 409 | Toks pavadinimas jau yra |
| 422 | Netinkama reikšmė (pvz. kaina ne didesnė už 0, įvertinimas ne 1–5) |

## Testavimas

Postman rinkinys: `../postman/eTurgus.postman_collection.json`

Importavus jį į Postman ir paleidus API, aplankus reikia vykdyti iš eilės. Kūrimo užklausos pačios išsaugo `shopId`, `productId` ir `reviewId`, o paskutinis aplankas sukurtus duomenis ištrina.
