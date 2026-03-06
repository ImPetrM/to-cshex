namespace to_cshex;

public class CsHexConvertor
{
    private readonly Dictionary<string, Func<string, byte[]>> _converters;

    public CsHexConvertor()
    {
        _converters = new Dictionary<string, Func<string, byte[]>>(StringComparer.OrdinalIgnoreCase)
        {
            ["int"] = (raw => BitConverter.GetBytes(int.Parse(raw))),
            ["uint"] = (raw => BitConverter.GetBytes(uint.Parse(raw))),
            ["short"] = (raw => BitConverter.GetBytes(short.Parse(raw))),
            ["ushort"] = (raw => BitConverter.GetBytes(ushort.Parse(raw))),
            ["byte"] = (raw => [byte.Parse(raw)]),
            ["long"] = (raw => BitConverter.GetBytes(long.Parse(raw))),
            ["double"] = (raw => BitConverter.GetBytes(double.Parse(raw))),
            ["float"] = (raw => BitConverter.GetBytes(float.Parse(raw))),
            ["string"] = (raw => System.Text.Encoding.UTF8.GetBytes(raw)),
        };
    }
    
    public bool IsSupportedType(string type) => _converters.ContainsKey(type);
    
    public bool TryConvert(string type, string rawValue, out byte[] convertedValue)
    {
        if (_converters.TryGetValue(type, out var converter))
        {
            convertedValue = converter(rawValue);
            return true;
        }

        convertedValue = [];
        return false;
    }
    
    public byte[] Convert(string type, string rawValue)
    {
        if (TryConvert(type, rawValue, out var value))
        {
            return value;
        }

        throw new NotSupportedException($"Type '{type}' is not supported for conversion.");
    }
}