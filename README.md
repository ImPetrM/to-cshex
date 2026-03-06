# to-cshex

CLI tool that converts primitive C# values into hexadecimal byte literals suitable for C# byte arrays.

## Example

Convert an integer:

```bash
to-cshex 42 int
```

Output:

```text
0x2A, 0x00, 0x00, 0x00
```

Convert a string:

```bash
to-cshex "Hello" string
```

Output:

```text
0x48, 0x65, 0x6C, 0x6C, 0x6F
```

## Usage

```
to-cshex <rawValue> <valueType> [options]
```

### Arguments

`rawValue`
The raw value to convert. The format must match the selected value type.

Examples:

```
123
"Hello"
3.14
true
```

`valueType`
Specifies how the raw value is interpreted before conversion.

Supported types:

```
int
uint
short
ushort
byte
long
double
float
string
```

## Options

```
--verbose       Print additional information along with the converted byte array
--clean         Output bytes separated by spaces instead of commas
-h, --help      Show help information
--version       Show version information
```

## Example Output for C# Code

The output can be directly used in C#:

```csharp
byte[] data =
{
    0x48, 0x65, 0x6C, 0x6C, 0x6F
};
```

## Typical Use Cases

* debugging binary protocols
* preparing payloads for networking
* writing unit tests with byte arrays

## Build

```
dotnet build
```

## Run

```
dotnet run -- 42 int
```
