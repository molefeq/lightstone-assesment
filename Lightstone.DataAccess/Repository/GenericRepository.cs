using Lightstone.Common.Models;
using LCE =Lightstone.Common.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lightstone.DataAccess.Repository

{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal LightstoneEntities _Context;
        internal DbSet<TEntity> _DbSet;

        public GenericRepository(LightstoneEntities context)
        {
            _Context = context;
            _DbSet = context.Set<TEntity>();
        }

        public LightstoneEntities Context
        {
            get
            {
                return _Context;
            }
        }

        public virtual IEnumerable<TEntity> GetPagedEntities(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> orderExpression = null, PagingObject pagingObject = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = Include(query, includeProperties);
            }

            if (orderExpression != null)
            {
                return GetPagedEntityData(query, orderExpression, pagingObject);
            }

            return query.ToList();
        }

        public virtual IEnumerable<TEntity> GetPagedEntityData(IQueryable<TEntity> query, Expression<Func<TEntity, object>> orderExpression, PagingObject pagingObject)
        {
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = OrderEntities(pagingObject.SortOrder, orderExpression);

            if (!pagingObject.IncludeAllDataInd)
            {
                int elementsToSkip = (pagingObject.PageIndex - 1) * pagingObject.PageSize;

                return orderby(query).Skip(elementsToSkip).Take(pagingObject.PageSize).ToList<TEntity>();
            }
            else
            {
                return orderby(query).ToList<TEntity>();
            }
        }

        public virtual Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderEntities(LCE.SortOrder sortOrder, Expression<Func<TEntity, object>> orderExpression)
        {
            var unaryExpression = orderExpression.Body as UnaryExpression;

            if (unaryExpression != null)
            {
                var propertyExpression = (MemberExpression)unaryExpression.Operand;
                var parameters = orderExpression.Parameters;

                if (propertyExpression.Type == typeof(decimal))
                {
                    var newExpression = Expression.Lambda<Func<TEntity, decimal>>(propertyExpression, parameters);

                    if (sortOrder == LCE.SortOrder.DESC)
                    {
                        return p => p.OrderByDescending(newExpression);
                    }
                    else
                    {
                        return p => p.OrderBy(newExpression);
                    }
                }

                if (propertyExpression.Type == typeof(DateTime))
                {
                    var newExpression = Expression.Lambda<Func<TEntity, DateTime>>(propertyExpression, parameters);

                    if (sortOrder == LCE.SortOrder.DESC)
                    {
                        return p => p.OrderByDescending(newExpression);
                    }
                    else
                    {
                        return p => p.OrderBy(newExpression);
                    }
                }

                if (propertyExpression.Type == typeof(int))
                {
                    var newExpression = Expression.Lambda<Func<TEntity, int>>(propertyExpression, parameters);
                    if (sortOrder == LCE.SortOrder.DESC)
                    {
                        return p => p.OrderByDescending(newExpression);
                    }
                    else
                    {
                        return p => p.OrderBy(newExpression);
                    }
                }
            }

            if (sortOrder == LCE.SortOrder.DESC)
            {
                return p => p.OrderByDescending(orderExpression);
            }
            else
            {
                return p => p.OrderBy(orderExpression);
            }
        }

        public virtual IEnumerable<TEntity> GetEntities(Expression<Func<TEntity, bool>> filter = null,
                                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                        string includeProperties = "")
        {
            IQueryable<TEntity> query = _DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = Include(query, includeProperties);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual int CountEntities(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public virtual TEntity GetById(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            IQueryable<TEntity> query = _DbSet;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = Include(query, includeProperties);
            }

            return query.Where(filter).FirstOrDefault<TEntity>();
        }

        public virtual void Insert(TEntity entity)
        {
            _DbSet.Add(entity);
        }

        public virtual void Delete(int id)
        {
            TEntity entityToDelete = _DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _DbSet.Attach(entityToDelete);
            }
            _DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            if (_Context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                _DbSet.Attach(entityToUpdate);
            }

            _Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual IQueryable<TEntity> Include(IQueryable<TEntity> query, string includeList)
        {
            foreach (var includeProperty in includeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public virtual void Dispose()
        {
            _Context.Dispose();
        }
    }
}
