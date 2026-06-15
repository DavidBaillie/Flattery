# Flattery

A lightweight, attribute-based flat file generator for .NET that transforms complex in-memory structures into fixed-width text files.

## Overview

Flattery is designed to generate flat files from complex structures in memory. It's an attribute-based file generator that uses index-based formatting to convert data types to strings. Flattery generates/exports strings that can be saved via I/O to disk in the form of text files.

### Key Features

- **Attribute-Based Configuration**: Define flat file layout directly on your model properties
- **Index-Based Formatting**: Precise control over field positioning using start and end indices

## Quick Start

### Basic Example

Define your model using Flattery attributes:

```csharp
using Flattery.Attributes;

public class CustomRecord
{
    [AlphabeticField(start: 0, end: 10)]
    public required string Name { get; set; }

    [UnsignedIntegerField<uint>(start: 11, end: 13)]
    public required uint Age { get; set; }

    [FloatingPointField<double>(start: 14, end: 20, DecimalDigits = 2)]
    public required double BankBalance { get; set; }

    [BooleanField(start: 21, end: 21)]
    public required bool IsActive { get; set; }
}
```

Generate flat file output:

```csharp
using Flattery;

var customRecord = new CustomRecord
{
    Name = "John",
    Age = 25,
    BankBalance = 1500.75,
    IsActive = true
};

var builder = new FlatFileBuilder();
builder.AppendRecord(customRecord);

string flatFileContent = builder.Build();
// Output: "John       0251500.75Y"

// Save to file
File.WriteAllText("customRecord.txt", flatFileContent);
```

## Available Field Attributes

Flattery provides specialized attributes for different data types:

| Attribute | Type | Purpose |
|-----------|------|---------|
| `AlphabeticField` | `string` | Alphabetic characters only |
| `AlphaNumericField` | `string` | Alphanumeric characters |
| `SignedIntegerField<T>` | `IBinaryInteger<T>` | Signed integer types (int, long, etc.) |
| `UnsignedIntegerField<T>` | `IUnsignedNumber<T>` | Unsigned integer types (uint, ulong, etc.) |
| `FloatingPointField<T>` | `IFloatingPoint<T>` | Floating-point numbers (double, float, decimal) |
| `BooleanField` | `bool` | Boolean values |
| `DateOnlyField` | `DateOnly` | Date values |

## Detailed Example

```csharp
using Flattery;
using Flattery.Attributes;

public class InventoryItem
{
    [AlphabeticField(start: 0, end: 14)]
    public required string ProductName { get; set; }

    [UnsignedIntegerField<uint>(start: 15, end: 19)]
    public required uint SKU { get; set; }

    [FloatingPointField<decimal>(start: 20, end: 28, DecimalDigits = 2)]
    public required decimal Price { get; set; }

    [SignedIntegerField<int>(start: 29, end: 33)]
    public required int Quantity { get; set; }
}

// Usage
var items = new[]
{
    new InventoryItem { ProductName = "Widget", SKU = 10001, Price = 29.99m, Quantity = 100 },
    new InventoryItem { ProductName = "Gadget", SKU = 10002, Price = 49.99m, Quantity = 50 }
};

var builder = new FlatFileBuilder();

foreach (var item in items)
{
    builder.AppendRecord(item);
}

string output = builder.Build();
File.WriteAllText("inventory.txt", output);
```

## Field Positioning

Each field attribute requires `start` and `end` parameters that define the character positions in the output record:

- **start**: Zero-indexed starting position (inclusive)
- **end**: Zero-indexed ending position (inclusive)

Fields must not overlap, and the positions determine the fixed-width layout of each line in the output file.

```
Position:  0         1         2
           0123456789012345678901
           John       0251500.75Y
           └─┬──────┘└─┬───────┘|
             |          |       └─ BooleanField (21-21)
             |          └─ FloatingPointField (14-20)
             └─ AlphabeticField (0-10)
```

## Advanced Usage

### Multiple Records

Chain `AppendRecord()` calls to add multiple records:

```csharp
var builder = new FlatFileBuilder();

foreach (var customer in customers)
{
    builder.AppendRecord(customer);
}

string flatFile = builder.Build();
// Each record on its own line
```

### Fixed Row Length

Control the total width of each record:

```csharp
var builder = new FlatFileBuilder();
builder.AppendRecord(customer, fixedRowLength: 50);
```

### Clearing Records

Start fresh without creating a new builder:

```csharp
builder.Clear();
builder.AppendRecord(newCustomer);
```

### Getting Record Length

Determine the required line width:

```csharp
int recordLength = builder.GetRecordLength(customer);
```

## Exception Handling

Flattery validates field definitions and record data:

- `ArgumentOutOfRangeException`: Field indices are invalid or record is too short
- `InvalidOperationException`: Fields overlap or record has no Flattery attributes
- `ArgumentNullException`: Null record passed to builder

```csharp
try
{
    builder.AppendRecord(customer);
}
catch (InvalidOperationException ex)
{
    // Handle overlapping or missing attributes
    Console.WriteLine($"Configuration error: {ex.Message}");
}
```

## Performance Considerations

- **Stack Allocation**: Uses `stackalloc` for efficient memory management
- **Span-Based Formatting**: Minimizes allocations during string conversion
- **Fixed-Width Format**: Optimized for high-volume record processing

## Supports

- .NET 6
- .NET 7
- .NET 8
- .NET 9
- .NET 10

## License

See [LICENSE.txt](LICENSE.txt) for details.
