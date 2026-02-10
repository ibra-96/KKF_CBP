using System;

namespace AlphaPeople.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}