using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : 
        IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false) {

            if(typeof(TEntity) == typeof(Product)){


               return  trackChanges? await _context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync() as IEnumerable<TEntity> 
                    : await _context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).AsNoTracking().ToListAsync() as IEnumerable<TEntity>;
            }




            return trackChanges ? await _context.Set<TEntity>().ToListAsync() 
                 : await _context.Set<TEntity>().AsNoTracking().ToListAsync() ;


        }
           
        public async Task<TEntity?> GetAsync(TKey id)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return await _context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).FirstOrDefaultAsync(P => P.Id == id as int?) as TEntity;
            }
            return await _context.Set<TEntity>().FindAsync(id) ;
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public  void Update(TEntity entity)
        {
            _context.Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> specifications, bool trackChanges = false)
        {
          return await ApplySpecifications(specifications).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(ISpecifications<TEntity, TKey> specifications)
        {
            return await ApplySpecifications(specifications).FirstOrDefaultAsync();
        }
        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity,TKey> specifications)
        {
           return SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), specifications);
        }
    }
}
