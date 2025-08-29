namespace StringsAreEvil.API.Utility.Common;

public struct Constants
{
    public const short FacadeFips = 01089;
}

public class Flags
{
    [Flags]
    public enum TeachSchoolSites
    {
        None = 0b_0000_0000,
        SystemsOnly = 0b_0000_0001,
        Colleges = 0b_0000_0010,
        NonPublic = 0b_0000_0100,
        Public = 0b_0000_1000,
        Other = 0b_0001_0000,
        Deleted = 0b_0010_0000,
        All = 0b_0011_1111,
    }
}
