using Iv.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Data.GenericEF
{
    public class GenericDataCommand<T, TKey> : IDataCommand<T, TKey>
        where T : ObjectDefBase<TKey>
        where TKey : IComparable
    {

        protected IDataContext ctx;

        public string StoreName => throw new NotImplementedException();

        public GenericDataCommand(IDataContext ctx)
        {
            this.ctx = ctx;
        }

        public T Create(T entity)
        {
            entity.SetNew();
            T newEntity;
            SetEntityState<T, TKey>(entity, out newEntity);
            ctx.SaveChanges();
            return newEntity;
        }

        public T Create(T entity, IDataCommandSpecification<T> spec)
        {
            entity.SetNew();
            T newEntity;
            SetEntityState<T, TKey>(entity, out newEntity);
            spec.Prepare(entity);
            ctx.SaveChanges();
            return newEntity;
        }

        public void Delete(T entity)
        {
            entity.SetDeleted();
            T delEntity;
            SetEntityState<T, TKey>(entity, out delEntity);
            ctx.SaveChanges();
        }

        public T Update(T entity)
        {
            entity.SetDirty();
            T updEntity;
            SetEntityState<T, TKey>(entity, out updEntity);
            ctx.SaveChanges();
            return updEntity;
        }

        public T Update(T entity, IDataCommandSpecification<T> spec)
        {
            entity.SetDirty();
            T updEntity;
            SetEntityState<T, TKey>(entity, out updEntity);
            spec.Prepare(entity);
            ctx.SaveChanges();
            return updEntity;
        }

        public void SetEntityState<TEntity, TEntityKey>(TEntity e)
            where TEntity : ObjectDefBase<TEntityKey>
            where TEntityKey : IComparable
        {
            e.SetRefRelationship();
            if (e.IsDeleted)
                ctx.Set<TEntity>().Remove(e);
            else if (e.IsNew)
                ctx.Set<TEntity>().Add(e);
            else if (e.IsDirty)
                ctx.Entry<TEntity>(e).State = EntityState.Modified;
        }

        public void SetEntityState<TEntity, TEntityKey>(TEntity e, out TEntity output)
            where TEntity : ObjectDefBase<TEntityKey>
            where TEntityKey : IComparable
        {
            output = e;
            e.SetRefRelationship();
            if (e.IsDeleted)
                output = ctx.Set<TEntity>().Remove(e);
            else if (e.IsNew)
                output = ctx.Set<TEntity>().Add(e);
            else if (e.IsDirty)
                ctx.Entry<TEntity>(e).State = EntityState.Modified;
        }

        public void Initialize(string storeName)
        {
            throw new NotImplementedException();
        }
    }
}
