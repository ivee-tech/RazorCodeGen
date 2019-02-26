using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Iv.Common;

namespace Iv.Data
{
    public interface ICommand<TEntity>: IDisposable
        where TEntity: class
    {
        #region Create
            /// <summary>
            /// Create new object in database
            /// </summary>
            /// <param name="t">specifies the new object to create</param>
            TEntity Create(TEntity t);
        #endregion

        #region Update
            /// <summary>
            ///Update object changes and save to database 
            /// </summary>
            /// <param name="t">specifies the object to update</param>
            int Update(TEntity t);
        #endregion 

        #region Delete
            /// <summary>
            /// Delete the object from database
            /// </summary>
            /// <param name="t">specifies the object to delete</param>
            int Delete(TEntity t);

            /// <summary>
            /// Delete objects from database by specified expression
            /// </summary>
            /// <param name="predicate">specifies the conditional expression</param>
            int Delete(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region Execute

            int Execute(string commandName, IEnumerable<KV<string, object>> paramValues);

            object ExecuteScalar(string commandName, IEnumerable<KV<string, object>> paramValues);

        #endregion

        #region Unit of work

        IUnitOfWork UnitOfWork { get; }

        #endregion

    }
}