using Kwanso.Model.Poco;
using Kwanso.Repository.DataAccess;
using Kwanso.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kwanso.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly KwansoContext _kwansoContext;
        public UserRepository(KwansoContext kwansoContext)
        {
            _kwansoContext = kwansoContext;
        }
        public Users Create(Users model)
        {
             model.IsDeleted = false;
            _kwansoContext.Users.Add(model);
            _kwansoContext.SaveChanges();
            return model;
        }

        public List<Users> Delete(List<int> Id)
        {
            throw new NotImplementedException();
        }

        public Users Get(int Id)
        {
            return _kwansoContext.Users.Where(c => c.Id == Id).FirstOrDefault();
        }

        public List<Users> GetAll()
        {
            return _kwansoContext.Users.ToList();
        }

        public Users Update(Users model)
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
