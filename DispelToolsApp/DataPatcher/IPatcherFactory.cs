namespace DispelTools.DataPatcher
{
    public interface IPatcherFactory
    {
        string PatcherName { get; }
        string PatchFileFilter { get; }
        string OutputFileFilter { get; }
        PatcherParams.OptionNames AcceptedOptions { get; }

        Patcher CreateInstance();
    }
}
