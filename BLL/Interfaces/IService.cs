using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IService<T> where T : class
    {
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Remove(T entity);
        void Edit(T entity);
        void Dispose();
    }
}
