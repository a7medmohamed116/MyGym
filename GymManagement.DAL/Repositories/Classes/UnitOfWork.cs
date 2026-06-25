using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using MyGym.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<string, object> _repositories = [];

        //================DATABASE CONNECTION================
        public UnitOfWork(GymDbContext dbContext ,ISessionRepository sessionrepo)//
        {
            _dbContext = dbContext;
            SessionRepository = sessionrepo;// then register in program.cs <ISessionRepo ,SessionRepo> 
        }

        public ISessionRepository SessionRepository { get; }// get only

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            //Check if repository already exists in the dictionary or not  ?? IDctionary will create it above
            // Like IGenericRepository<Member>   => need Name
            var TypeName = typeof(TEntity).Name;
            // if exist in dictionary => use it 

            if (_repositories.TryGetValue(TypeName, out object? value))
            {
                return (IGenericRepository<TEntity>)value;
            }

            // if not exist => create repo => Add dictionary => retuen repo
            else
            {
                var repo = new GenericRepository<TEntity>(_dbContext);
                _repositories[TypeName] = repo;
                return repo;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _dbContext.SaveChangesAsync(ct);

        //خلص كل العمليه واعمل سيف اتشينج ف الاخر تحت ترانزاكشن واحده 
        //تحفه يحافظ ع العلاقات  
        // افتكر مشكله ميمبر احمد وان الاوتو مابر بتاع الهيلث ريكورد بتاعو كان بيرجع  نال
        //بسبب متضاف مانول قبل حوار ترانزكشن  واحده وسيف اتشينج واحده ف دمرلي علاقه 1 1 ماست ماست
    }

}
