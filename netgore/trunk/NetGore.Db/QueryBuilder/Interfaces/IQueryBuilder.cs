namespace NetGore.Db.QueryBuilder
{
    public interface IQueryBuilder
    {
        IInsertQuery Insert(string tableName);

        IUpdateQuery Update(string tableName);

        ISelectQuery Select(string tableName, string alias = null);

        IDeleteQuery Delete(string tableName);
    }
}