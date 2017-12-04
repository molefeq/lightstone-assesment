using System;
using System.Data.Entity.Validation;

namespace Lightstone.DataAccess.Repository
{
    public class UnitOfWork : IDisposable
    {
        private LightstoneEntities _Context;
        private bool _Disposed;

        private LeaveRequestRepository _LeaveRequestRepository;
        private EmployeeRepository _EmployeeRepository;
        private RequestStatusRepository _RequestStatusRepository;

        public UnitOfWork()
        {
            _Context = new LightstoneEntities();
            _Disposed = false;
        }

        public LightstoneEntities Context
        {
            get
            {
                return _Context;
            }
        }

        public EmployeeRepository EmployeeRepository
        {
            get
            {
                if (this._EmployeeRepository == null)
                {
                    this._EmployeeRepository = new EmployeeRepository(_Context);
                }

                return _EmployeeRepository;
            }
        }
        
        public LeaveRequestRepository LeaveRequestRepository
        {
            get
            {
                if (this._LeaveRequestRepository == null)
                {
                    this._LeaveRequestRepository = new LeaveRequestRepository(_Context);
                }

                return _LeaveRequestRepository;
            }
        }

        public RequestStatusRepository RequestStatusRepository
        {
            get
            {
                if (this._RequestStatusRepository == null)
                {
                    this._RequestStatusRepository = new RequestStatusRepository(_Context);
                }

                return _RequestStatusRepository;
            }
        }

        public bool AutoDetectChanges
        {
            get
            {
                return _Context.Configuration.AutoDetectChangesEnabled;
            }
            set
            {
                _Context.Configuration.AutoDetectChangesEnabled = value;
            }
        }

        public bool ValidateOnSave
        {
            get
            {
                return _Context.Configuration.ValidateOnSaveEnabled;
            }
            set
            {
                _Context.Configuration.ValidateOnSaveEnabled = value;
            }
        }


        public void Save()
        {
            try
            {
                _Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                }
                throw ex;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._Disposed)
            {
                if (disposing)
                {
                    _Context.Dispose();
                }
            }

            this._Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}

