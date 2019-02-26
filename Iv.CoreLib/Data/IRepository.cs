using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Iv.Common;

namespace Iv.Data
{
    public interface IRepository<TEntity>: IQuery<TEntity>, ICommand<TEntity> 
        where TEntity: class
    {
    }
}