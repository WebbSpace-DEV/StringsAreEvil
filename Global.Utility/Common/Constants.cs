namespace Global.Utility.Common;

public class Constants
{
    public static long InvalidId = -1;
    public const string DateTemplate = "yyyy-MM-dd";
    public const string Delimiter = "^";
}

public class Enums
{
    public enum Modules
    {
        Unknown,
        Benchmark
    }

    public enum PayloadTypes
    {
        Unknown,
        Json,
        Xml,
        Csv
    }

    public enum BenchmarkEndpoints
    {
        Unknown,
        StringsAreEvil
    }

    public enum EndpointActions
    {
        Unknown,
        GetProof
    }
}
