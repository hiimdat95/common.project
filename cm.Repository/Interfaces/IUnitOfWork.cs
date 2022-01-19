using System;

namespace cm.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}