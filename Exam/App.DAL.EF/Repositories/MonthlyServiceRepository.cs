using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;


    public class MonthlyServiceRepository : BaseEntityRepository<MonthlyService, Domain.MonthlyService, AppDbContext>,
        IMonthlyServiceRepository
    {
        public MonthlyServiceRepository(AppDbContext dbContext, IMapper<MonthlyService, Domain.MonthlyService> mapper) : base(dbContext, mapper)
        {
        }
    }