namespace DeskMetrics.OperatingSystem.Hardware
{
    public interface IHardware
    {
        string ProcessorName { get; }
        int ProcessorArchicteture { get; }
        int ProcessorCores { get; }
        double MemoryTotal { get; }
        double MemoryFree { get; }
        long DiskTotal { get; }
        long DiskFree { get; }
        string ScreenResolution { get; }
        string ProcessorBrand { get; }
        double ProcessorFrequency { get; }
    }
}
