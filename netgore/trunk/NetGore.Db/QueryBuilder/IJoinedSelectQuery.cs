namespace NetGore.Db.QueryBuilder
{
    public interface IJoinedSelectQuery : IQueryResultFilter
    {
        IJoinedSelectQuery InnerJoin(string table, string alias, string joinCondition);
        IJoinedSelectQuery InnerJoinOnColumn(string table, string alias, string thisJoinColumn, string otherTable, string otherTableJoinColumn);
    }
}