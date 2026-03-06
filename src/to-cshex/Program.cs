using System.CommandLine;

namespace to_cshex;

class Program
{
    private const string RawValueArgumentName = "rawValue";
    private const string ValueTypeArgumentName = "valueType";
    private const string VerboseOptionName = "--verbose";
    private const string CleanOptionName = "--clean";
    
    private const string CleanSeparator = " ";
    private const string CommaSeparator = ", ";

    static int Main(string[] args)
    {
        var convertCommand = BuildConvertCommand();
        convertCommand.SetAction((RunConvertCommand));
        
        ParseResult parseResult = convertCommand.Parse(args);
        return parseResult.Invoke();
    }

    private static RootCommand BuildConvertCommand()
    {
        var rawValueArgument = new Argument<string>(RawValueArgumentName)
        {
            Description = "The raw value to be converted. The format of the raw value should match the specified value type. " +
                          "For example, if the value type is 'int', the raw value should be a valid integer string (e.g., '123'). " +
                          "If the value type is 'string', the raw value can be any string (e.g., 'Hello, World!').",
        };

        var valueTypeArgument = new Argument<string>(ValueTypeArgumentName)
        {
            Description = "The type of the value to be converted. Supported types include: int, uint, short, ushort, byte, long, double, " +
                          "float, and string. The value type determines how the raw value will be interpreted and converted into a byte array."
        };

        var verboseOption = new Option<bool>(VerboseOptionName)
        {
            Description = "If set, the output will only include the converted byte array without any additional information.",
            Required = false
        };

        var cleanOption = new Option<bool>(CleanOptionName)
        {
            Description = "If set, the output byte array will be separated by spaces instead of comas.",
            Required = false
        };
        
        return new RootCommand("Convert a value to hex byte literals for use in C# code.")
        {
            rawValueArgument,
            valueTypeArgument,
            verboseOption,
            cleanOption
        };
    }
    
    private static void RunConvertCommand(ParseResult parseResult)
    {
        var rawValue = parseResult.GetRequiredValue<string>(RawValueArgumentName);
        var valueType = parseResult.GetRequiredValue<string>(ValueTypeArgumentName);
        var verbose = parseResult.GetValue<bool>(VerboseOptionName);
        var clean = parseResult.GetValue<bool>(CleanOptionName);
        
        var convertor = new CsHexConvertor();
        if (!convertor.IsSupportedType(valueType))
        {
            Console.WriteLine($"Error: Unsupported value type '{valueType}'. Supported types are: int, uint, short, ushort, byte, long, double, float, string.");
            return;
        }
        
        try
        {
            var convertedValue = convertor.Convert(valueType, rawValue);
            var separator = clean ? CleanSeparator : CommaSeparator;
            var output = string.Join(separator, convertedValue.Select(b => $"0x{b:x2}"));
            if (verbose)
            {
                Console.WriteLine($"Value: ({valueType.ToLowerInvariant()}) {rawValue}");
                Console.WriteLine($"Converted Value (byte array): [{output}]");
            }
            else
            {
                Console.WriteLine(output);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}