# eTurgus Backend

Elektroninės prekyvietės API. Parduotuvės skelbia prekes, o pirkėjai rašo prekių atsiliepimus.

Naudojama: ASP.NET Core 8 Web API, Entity Framework Core, SQLite, MediatR, JWT.

## Objektai

`Shop` → `Product` → `Review`

Parduotuvė turi daug prekių, prekė turi daug atsiliepimų.

`User` yra naudotojas su role. Parduotuvė turi savininką (`Shop.OwnerId`), atsiliepimas turi autorių (`Review.AuthorId`).

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
    Interfaces/         # IPasswordHasher, ITokenService
    UseCases/
  Infrastructure/       # duomenų bazė ir saugumas
    Repositories/
    Security/           # slaptažodžių maiša ir JWT kūrimas
    DbContext.cs
    DbSeeder.cs
  Presentation/         # kontroleriai ir programos paleidimas
    Controllers/
    Extensions/         # naudotojo id ir rolės skaitymas iš žetono
    Hypermedia/
    Program.cs
```

Priklausomybių kryptis: `Presentation -> Infrastructure -> Application -> Domain`.

## Paleidimas

Reikia .NET 8 SDK.

Prieš pirmą paleidimą reikia nustatyti JWT pasirašymo raktą. Jis nelaikomas `appsettings.json`, kad nepatektų į Git, o saugomas `dotnet user-secrets`. Raktas turi būti bent 32 simbolių ilgio, kitaip programa nepasileis:

```bash
dotnet user-secrets set "Jwt:Key" "cia-irasyk-bent-32-simboliu-ilgio-atsitiktini-rakta" --project src/Presentation
```

Paleidimas:

```bash
dotnet run --project src/Presentation
```

API pasileidžia adresu http://localhost:5080, Swagger: http://localhost:5080/swagger

Duomenų bazės paruošti nereikia. Pirmo paleidimo metu sukuriamas failas `src/Presentation/backend.db` ir užpildomas pradiniais duomenimis (5 parduotuvės su prekėmis ir atsiliepimais, naudotojai visoms rolėms). Norint pradėti nuo švarių duomenų, užtenka šį failą ištrinti ir paleisti iš naujo.

Prisijungimo eilutė yra `src/Presentation/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=backend.db"
}
```

Kiti JWT nustatymai yra `appsettings.json`:

```json
"Jwt": {
  "Issuer": "eTurgus",
  "Audience": "eTurgus",
  "AccessTokenExpirationMinutes": 15,
  "RefreshTokenExpirationDays": 7
}
```

CORS leidžiami adresai (frontendui) nurodomi `Cors:AllowedOrigins`, numatytasis `http://localhost:5173`.

## Autentifikacija ir autorizacija

Naudojamas JWT. Slaptažodžiai saugomi tik kaip maiša (PBKDF2, `PasswordHasher` iš ASP.NET Core Identity).

| Metodas | Kelias | Aprašymas |
|---|---|---|
| POST | `/api/auth/register` | Registracija: `Email`, `UserName`, `Password` (bent 6 simboliai), `Role` (`Buyer` arba `Seller`) |
| POST | `/api/auth/login` | Prisijungimas: `Email`, `Password`. Grąžina access žetoną, refresh žetoną įrašo į slapuką |
| POST | `/api/auth/refresh` | Pagal refresh slapuką išduoda naują access žetoną ir naują refresh žetoną |
| POST | `/api/auth/logout` | Atšaukia refresh žetoną ir ištrina slapuką |
| GET | `/api/auth/me` | Prisijungusio naudotojo duomenys (reikia žetono) |

### Žetonai

- **Access žetonas** yra JWT, galioja 15 min., pasirašytas HS256. Jame saugomi `sub` (naudotojo id), `email`, `unique_name`, `role` ir `jti`. Siunčiamas antraštėje `Authorization: Bearer <žetonas>`.
- **Refresh žetonas** yra atsitiktinė 64 baitų eilutė, galioja 7 d. ir saugoma duomenų bazėje (`RefreshTokens` lentelė). Jis siunčiamas tik `HttpOnly`, `Secure`, `SameSite=Strict` slapuke su keliu `/api/auth`, todėl JavaScript jo nemato. Kiekvieną kartą panaudotas jis atšaukiamas ir išduodamas naujas, tad seno žetono antrą kartą panaudoti nepavyks.

Swagger'yje spausk **Authorize** ir įklijuok `accessToken` iš `login` atsakymo.

### Rolės

| Rolė | Kaip gaunama |
|---|---|
| `Buyer` | Registruojantis |
| `Seller` | Registruojantis |
| `Admin` | Tik seederyje. Registruojantis `Admin` pasirinkti negalima (422) |

### Teisės

Visi `GET` prieinami be prisijungimo.

| Veiksmas | Buyer | Seller | Admin |
|---|---|---|---|
| Sukurti parduotuvę | – | Taip (tampa savininku) | Taip |
| Keisti ar trinti parduotuvę | – | Tik savo | Visas |
| Kurti, keisti ar trinti prekes | – | Tik savo parduotuvėje | Visas |
| Rašyti atsiliepimą | Taip | Taip | Taip |
| Keisti ar trinti atsiliepimą | Tik savo | Tik savo | Visus |

Be žetono rašymo veiksmai grąžina 401. Su žetonu, bet be reikiamos rolės arba ne savininkui, grąžinama 403.

### Pradiniai naudotojai

Visų seederio naudotojų slaptažodis yra `Slaptazodis123`.

| El. paštas | Rolė |
|---|---|
| `admin@eturgus.lt` | Admin |
| `info@kaimoskoniai.lt`, `labas@rankudarbo.lt`, `pagalba@techkampas.lt`, `labas@knygulentyna.lt`, `info@sodogerybes.lt` | Seller (kiekvienas savo parduotuvės savininkas) |
| `jonas@eturgus.lt`, `aiste@eturgus.lt` ir kiti atsiliepimų autoriai | Buyer |

Pirkėjo el. paštas sudaromas iš atsiliepimo autoriaus vardo be lietuviškų raidžių, pvz. „Rūta“ → `ruta@eturgus.lt`.

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
| 401 | Nėra žetono, jis netinkamas ar pasibaigęs, neteisingi prisijungimo duomenys arba refresh žetonas |
| 403 | Neturi reikiamos rolės arba resursas ne tavo |
| 404 | Resursas arba jo tėvinis objektas nerastas |
| 409 | Toks pavadinimas arba el. paštas jau yra |
| 422 | Netinkama reikšmė (pvz. kaina ne didesnė už 0, įvertinimas ne 1–5) |

## Testavimas

Postman rinkinys: `../postman/eTurgus.postman_collection.json`

Importavus jį į Postman ir paleidus API, aplankus reikia vykdyti iš eilės:

- `0. Auth` užregistruoja naują pardavėją ir pirkėją, prisijungia jais bei seederio `admin@eturgus.lt` ir `info@kaimoskoniai.lt`, o žetonus išsaugo kintamuosiuose. Taip pat tikrina registracijos ir prisijungimo klaidas, `refresh` ir `/me`.
- `1–4` aplankai tikrina CRUD, puslapiavimą, filtrus ir klaidas. Parduotuves ir prekes kuria pardavėjas, atsiliepimus rašo pirkėjas.
- `5. Authorization errors` tikrina 401 ir 403 atvejus (be žetono, pirkėjas kuria parduotuvę, svetimas pardavėjas, ne autorius) ir kad administratorius gali keisti svetimą parduotuvę.
- `6. Delete` ištrina sukurtus duomenis, `7. Logout` atsijungia ir tikrina, kad refresh žetonas nebegalioja.

Kūrimo užklausos pačios išsaugo `shopId`, `productId` ir `reviewId`. Rinkiniui reikia seederio naudotojų, todėl API turi veikti su pradiniais duomenimis.

Paleisti iš komandinės eilutės su Postman CLI:

```bash
postman collection run ../postman/eTurgus.postman_collection.json
```
