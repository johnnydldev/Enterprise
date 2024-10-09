﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOControllers.ManagerControllers
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> getAll(); 
        Task<T> getById(int id);
        Task<int> create(T model);
        Task<bool> edit(T model);
        Task<bool> delete(int id);
        Task<int> getMaxId();
        Task<List<T>> allMatchedBy(int idModel);
        Task<List<T>> allMatchedWith(string name);
        Task<List<T>> allMatches(int idModel, string name);
    }
}
