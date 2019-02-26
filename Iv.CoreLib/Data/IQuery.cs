using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Iv.Common;

namespace Iv.Data
{
    public interface IQuery<TEntity>: IDisposable
        where TEntity: class
    {

        /// <summary>
        /// Gets all Objects from database
        /// </summary>
        IQueryable<TEntity> GetAll(int maxRecords = ObjectDefSqlCommandBuilder.TopNRecordCount);

        /// <summary>
        /// Gets Objects from Database by Filter
        /// </summary>
        /// <param name="predicate">Specifies the filter</param>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate, int maxRecords = ObjectDefSqlCommandBuilder.TopNRecordCount);

        IQueryable<TEntity> Filter(Condition cond, string orderByExpression, int maxRecords = ObjectDefSqlCommandBuilder.TopNRecordCount);

        /// <summary>
        /// Gets Objects from database with filtering and paging
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <param name="filter">Specifies the filter</param>
        /// <param name="total">Returns the total records count</param>
        /// <param name="index">specifies the page index</param>
        /// <param name="size">specifies the page size</param>        
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, string orderByExpression, out int total, int index = 1, int size = ObjectDefSqlCommandBuilder.DefaultPageSize);

        IQueryable<TEntity> Filter(Condition cond, string orderByExpression, out int total, int index = 1, int size = ObjectDefSqlCommandBuilder.DefaultPageSize);

        TEntity FindByQuery(string queryName, IEnumerable<KV<string, object>> paramValues);

        IQueryable<TEntity> FilterByQuery(string queryName, IEnumerable<KV<string, object>> paramValues);

        IQueryable<TEntity> FilterByQuery(string queryName, IEnumerable<KV<string, object>> paramValues, out int total, int index = 1, int size = ObjectDefSqlCommandBuilder.DefaultPageSize);


        /// <summary>
        /// Find object using specified keys
        /// </summary>
        /// <param name="keys">specifies the search keys</param>
        TEntity Find(params object[] keys);

        /// <summary>
        /// Find objects by specified expression
        /// </summary>
        /// <param name="predicate">specifies the search expression</param>
        TEntity Find(Expression<Func<TEntity, bool>> predicate, params object[] includeList);

        bool Exists(params object[] keys);

    }
}