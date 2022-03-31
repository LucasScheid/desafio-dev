namespace cnab_helpers.environment
{
    public interface IEnvironmentHelper
    {
        string? ReadContent(string variableName, string? defaultValue = null);
    }
}
