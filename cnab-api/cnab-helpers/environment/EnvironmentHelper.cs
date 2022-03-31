namespace cnab_helpers.environment
{
    public class EnvironmentHelper : IEnvironmentHelper
    {
        public string? ReadContent(string variableName, string? defaultValue = null)
        {
            return string.IsNullOrEmpty(Environment.GetEnvironmentVariable(variableName)) ? defaultValue : Environment.GetEnvironmentVariable(variableName);
        }
    }
}
