using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using DataModel;

namespace DataService
{
    public abstract class EntityService<T> : IEntityService<T> where T : BaseEntity
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IGenericRepository<T> _genericRepository;

        public EntityService(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = repository;
        }


        public virtual void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _genericRepository.Add(entity);
            _unitOfWork.SaveChanges();
        }


        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _genericRepository.Edit(entity);
            _unitOfWork.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _genericRepository.Delete(entity);
            _unitOfWork.SaveChanges();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _genericRepository.GetAll();
        }
    }
}
