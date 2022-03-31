namespace cnab_helpers.database
{
    public interface ISqlCommandHelper
    {
        string GetCompleteTableName(string tableName, bool addNoLockInstruction = false, string tableAlias = null);
    }
}
