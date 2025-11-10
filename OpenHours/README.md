# 🕒 OpeningHours

**OpeningHours** is a lightweight .NET library for evaluating whether a specific `DateTime` falls within defined business hours. It supports a rich, human-readable pattern syntax for specifying weekly schedules, date-based rules, time ranges, and exclusions.

---

## ✨ Features

- ✅ Define business hours using intuitive patterns
- 📅 Support for weekdays, specific dates, and nth weekday of the month (e.g., `Fr[2]`)
- ⏰ Time range support including overnight spans (e.g., `23:00-02:00`)
- 🚫 Exclusion rules override inclusions (e.g., `We off`)
- 🧪 Thoroughly tested with edge cases and complex combinations

---

## 📘 Pattern Syntax

Patterns are composed of **inclusions** and **exclusions**, separated by semicolons:

```text
Mo-Th 08:00-20:00;We off;Aug 20;Aug 21 off
```

### 🧩 Components

| Syntax | Description |
|--------|-------------|
| `Mo`, `Tu`, `We`, `Th`, `Fr`, `Sa`, `Su` | Weekdays |
| `08:00-20:00` | Time range (24h format) |
| `Fr[2]` | Nth weekday of the month (e.g., 2nd Friday) |
| `Aug 20` | Specific calendar date |
| `Aug 20 off` | Exclude a specific date |
| `Mo off` | Exclude a weekday |
| `00:00-24:00` | Full-day availability |
| `23:00-02:00` | Overnight span |
| `Jan 01-04 off` | Exclude a date range |

> ℹ️ Exclusions always take precedence over inclusions.

---

## 🔍 Usage

```csharp
bool isOpen = OpeningHours.IsNowWithinBusinessHours(
    "Mo-Fr 09:00-17:00;We off;Dec 25 off",
    DateTime.Parse("2025-12-25 10:00")
);
// Returns false (Christmas is excluded)
```

---

## ✅ Test Coverage

The library is validated with extensive NUnit test cases, including:

- ✔️ Basic weekday and time range checks
- ❌ Excluded weekdays and dates
- 📆 Specific dates and date ranges
- 🧠 Priority resolution (exclusions override inclusions)
- 🕛 Midnight and overnight spans
- 📅 Nth weekday logic (e.g., `Mo[1]`, `Fr[2]`)
- 🌍 Time zone-sensitive evaluations

Example test case:

```csharp
[TestCase("Mo-Th 08:00-20:00;We off", "2025-01-15 09:00", false)]
[TestCase("Fr[2] 08:00-10:00;Fr[3] off", "2025-10-10 08:30", true)]
[TestCase("Aug 20;Aug 20 off", "2025-08-20", false)]
```

---

## 🧪 Running Tests

Tests are written using [NUnit](https://nunit.org/). To run them:

```bash
dotnet test
```


## 🧠 Inspiration: opening_hours.js
This library was inspired by the excellent [opening_hours.js](https://github.com/opening-hours/opening_hours.js) project, which provides a robust parser and evaluator for the opening_hours tag used in OpenStreetMap (OSM). That tag encodes complex business hour patterns for facilities like shops, restaurants, and public services. 
Also you can evaluate your pattern [here](https://openingh.ypid.de/evaluation_tool/)

---

## 📄 License

MIT License. See [LICENSE](https://www.tldrlegal.com/license/gnu-lesser-general-public-license-v3-lgpl-3) for details.

---
