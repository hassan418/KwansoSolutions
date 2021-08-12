using System;
using System.Collections.Generic;
using System.Text;

namespace Kwanso.Repository.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        List<T> GetAll();
        T Get(int Id);
        T Create(T model);
        T Update(T model);
        List<T> Delete(List<int> Id);
    }
}