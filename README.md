# \# 📅 OpeningHours Library

# 

# The \*\*OpeningHours\*\* library provides a flexible and expressive way to define and evaluate business hours and availability patterns. It supports a rich syntax for specifying time ranges, weekday rules, calendar dates, and exclusions.

# 

# \## ✅ Purpose

# 

# This library helps determine whether a specific `DateTime` falls within defined business hours, using a pattern-based string format. It is ideal for scheduling systems, booking platforms, and availability checks.

# 

# ---

# 

# \## 🧪 Test Coverage Summary

# 

# The library is thoroughly tested across a wide range of scenarios, including:

# 

# \### 🕒 Time Ranges

# \- `08:00-20:00` — Basic time intervals

# \- `00:00-24:00` — Full-day availability

# \- `23:00-02:00` — Overnight spans

# \- `10:00-08:00` — Invalid or reversed ranges are handled gracefully

# 

# \### 📆 Weekday Rules

# \- `Mo-Th` — Inclusive weekday ranges

# \- `Fr\[2]` — Specific weekday occurrences (e.g., 2nd Friday of the month)

# \- `Mo\[1],Mo\[3]` — Multiple specific weekday occurrences

# \- `Mo-Th off` — Weekday exclusions

# \- `We off` — Single weekday exclusion

# 

# \### 📅 Calendar Dates

# \- `Aug 20` — Specific date inclusion

# \- `Aug 20 off` — Specific date exclusion

# \- `Jan 01-04 off` — Date range exclusion

# \- `Nov 06 22:00-00:00 off` — Partial-day exclusion

# 

# \### 🧠 Priority Rules

# \- Exclusions always override inclusions

# \- Multiple overlapping rules are resolved with exclusion precedence

# 

# \### 🧭 Edge Cases

# \- Midnight boundaries (`00:00`, `24:00`)

# \- Time zone-sensitive evaluations

# \- Invalid or ambiguous patterns (e.g., `Su-Mo`) are handled

# 

# ---

# 

# \## 📌 Pattern Syntax Overview

# 

# Patterns are composed of \*\*inclusions\*\* and \*\*exclusions\*\*, separated by semicolons:

# 

# ```text

# Mo-Th 08:00-20:00;We off;Aug 20;Aug 21 off

# ```

# 

# \- \*\*Weekdays\*\*: `Mo`, `Tu`, `We`, `Th`, `Fr`, `Sa`, `Su`

# \- \*\*Time Ranges\*\*: `HH:mm-HH:mm`

# \- \*\*Specific Dates\*\*: `MMM dd` or `MMM dd-HH:mm`

# \- \*\*Exclusions\*\*: Append `off` to any rule

# 

# \### Examples

# 

# | Pattern | Meaning |

# |--------|---------|

# | `Mo-Fr 09:00-17:00` | Open weekdays 9–5 |

# | `Fr\[2] 08:00-10:00` | Open on 2nd Friday of the month |

# | `Aug 20;Aug 20 off` | Aug 20 is excluded despite being included |

# | `00:00-24:00;We off` | Open all day except Wednesdays |

# 

# ---

# 

# \## 🔍 API Usage

# 

# ```csharp

# bool isOpen = OpeningHours.IsNowWithinBusinessHours("Mo-Th 08:00-20:00;We off", DateTime.Parse("2025-01-15 09:00"));

# ```

# 

# Returns `false` because Wednesday is excluded.

# 

# ---

# 

# \## 🧪 Unit Testing

# 

# The library is validated with NUnit test cases like:

# 

# ```csharp

# \[TestCase("Mo-Th 08:00-20:00;We off", "2025-01-15 09:00", false)]

# \[TestCase("Aug 20;Aug 20 off", "2025-08-20", false)]

# \[TestCase("Fr\[2] 08:00-10:00;Fr\[3] off", "2025-10-10 08:30", true)]

# ```

# 

# Each test ensures correct evaluation of complex patterns and edge cases.

# 

# ---



