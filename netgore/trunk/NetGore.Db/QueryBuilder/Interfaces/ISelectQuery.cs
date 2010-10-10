using System;

namespace NetGore.Db.QueryBuilder
{
    public interface ISelectQuery : IColumnCollectionBuilder<ISelectQuery>, IJoinedSelectQuery
    {
        ISelectQuery AllColumns();
        ISelectQuery Distinct();
    }
}