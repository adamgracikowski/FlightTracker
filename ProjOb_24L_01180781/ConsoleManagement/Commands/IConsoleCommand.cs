namespace ProjOb_24L_01180781.ConsoleManagement.Commands
{
    /// <summary>
    /// Represents common characteristics of all valid console commands.
    /// </summary>
    public interface IConsoleCommand
    {
        bool Execute();
        ulong ExecutionCounter { get; }
        bool Executed { get; }
    }
}
