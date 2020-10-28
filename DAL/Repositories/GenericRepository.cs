using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ZaolisModel context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ZaolisModel context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }
        public void Create(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            if (entity != null)
                dbSet.Remove(entity);
        }

        public virtual IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
            return null;
        }

        public TEntity GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Update(TEntity entity)
        {
            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }


    }
}
