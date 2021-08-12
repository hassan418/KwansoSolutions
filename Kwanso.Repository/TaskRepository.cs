using Kwanso.Model.Poco;
using Kwanso.Repository.DataAccess;
using Kwanso.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwanso.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly KwansoContext _kwansoContext;
        public TaskRepository(KwansoContext kwansoContext)
        {
            _kwansoContext = kwansoContext;
        }
        public Tasks Create(Tasks model)
        {
            model.IsDeleted = false;
            model.Created_At = DateTime.Now;
            model.Updated_At = DateTime.Now;
            _kwansoContext.Tasks.Add(model);
            _kwansoContext.SaveChanges();
            return model;
        }

        public List<Tasks> Delete(List<int> Id)
        {
            var tasks = _kwansoContext.Tasks.Where(c => Id.Contains(c.Id)).ToList();
            foreach (var item in tasks)
            {
                item.IsDeleted = true;
                item.Updated_At = DateTime.Now;
            }
            _kwansoContext.UpdateRange(tasks);
            _kwansoContext.SaveChanges();
            return tasks;
        }

        public Tasks Get(int Id)
        {
            return _kwansoContext.Tasks.Where(c => c.Id == Id).FirstOrDefault();
        }

        public List<Tasks> GetAll()
        {
            return _kwansoContext.Tasks.Where(c=>c.IsDeleted == false).ToList();
        }

        public Tasks Update(Tasks model)
        {
            if (model == null)
            {
                throw new ArgumentException("model");
            }

            model.Updated_At = DateTime.Now;

            try
            {
                _kwansoContext.Entry(model).State = EntityState.Modified;
                _kwansoContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }

            return model;
        }
    }
}