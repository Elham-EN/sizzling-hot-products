# Sizzling Hot Products

A .NET 10 Web API that calculates the top sizzling hot product for a given day or date range.

## Stack

- **Backend:** ASP.NET Core 10, C#
- **Tests:** xUnit
- **Frontend:** React (Vite) — in progress

## Project Structure

```
ElhamSizzlingHotProdoucts/
├── inputs/               # Source JSON data files
├── API/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   └── Program.cs
├── API.Tests/
└── Client/               # React frontend (Vite + Tailwind CSS v4)
```

## How to Run

**Prerequisites:** .NET 10 SDK, Node.js 18+

```bash
# Run the API
cd API
dotnet run
```

Swagger UI: `https://localhost:5001/swagger`

```bash
# Run the React frontend
cd Client
npm install
npm run dev
```

Open `http://localhost:5173` in your browser.

```bash
# Run tests (from solution root)
dotnet test
```

## Endpoints

```
GET /api/sizzlinghotproducts/daily?date=2026-04-21
GET /api/sizzlinghotproducts/period?from=2026-04-21&to=2026-04-23
```

> Dates use `yyyy-MM-dd` format.

## Expected Outcomes

| Date / Period           | Top Product                                        |
| ----------------------- | -------------------------------------------------- |
| 21/04/2026              | Ezy Storage 37L Flexi Laundry Basket - White       |
| 22/04/2026              | Ezy Storage 37L Flexi Laundry Basket - White       |
| 23/04/2026              | Arlec 160W Crystalline Solar Foldable Charging Kit |
| 21/04/2026 – 23/04/2026 | Ezy Storage 37L Flexi Laundry Basket - White       |

## Assumptions

- Today's date is 23/04/2026 as specified in the challenge.
- A cancelled order removes the original completed order from the count entirely.
- Cancelled orders in the JSON sometimes omit `customerId` and `entries` — modelled as nullable.
- Query string dates are `yyyy-MM-dd` (ASP.NET Core default), while the JSON uses `dd/MM/yyyy` internally.
