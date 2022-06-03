using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUOW
{
    private readonly IMapper _mapper;
    
    private IAmenityRepository? _amenity;
    private IApartAmenityRepository? _apartAmenity;
    private IApartmentRepository? _apartment;
    private IApartPictureRepository? _apartPicture;
    private IBillingRepository? _billing;
    private IFixedServiceRepository? _fixedService;
    private IHouseRepository? _house;
    private IPersonRepository? _person;
    private IPictureRepository? _picture;
    private IRentFixedServiceRepository? _rentFixedService;
    private IApartRentRepository? _apartRent;
    private IMonthlyServiceRepository? _monthlyService;
    private IRentMonthlyServiceRepository? _rentMonthlyService;
    private IApartInHouseRepository? _apartInHouse;

    public AppUOW(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    public IAmenityRepository Amenity => _amenity ??= new AmenityRepository(UOWDbContext, new AmenityMapper(_mapper));
    public IApartAmenityRepository ApartAmenity => _apartAmenity ??= new ApartAmenityRepository(UOWDbContext, new ApartAmenityMapper(_mapper));
    public IApartmentRepository Apartment => _apartment ??= new ApartmentRepository(UOWDbContext, new ApartmentMapper(_mapper));
    public IApartPictureRepository ApartPicture => _apartPicture ??= new ApartPictureRepository(UOWDbContext, new ApartPictureMapper(_mapper));
    public IApartRentRepository ApartRent => _apartRent ??= new ApartRentRepository(UOWDbContext, new ApartRentMapper(_mapper));
    public IBillingRepository Billing => _billing ??= new BillingRepository(UOWDbContext, new BillingMapper(_mapper));
    public IFixedServiceRepository FixedService => _fixedService ??= new FixedServiceRepository(UOWDbContext, new FixedServiceMapper(_mapper));
 
    public IHouseRepository House => _house ??= new HouseRepository(UOWDbContext, new HouseMapper(_mapper));
    public IPersonRepository Person => _person ??= new PersonRepository(UOWDbContext, new PersonMapper(_mapper));
    public IPictureRepository Picture => _picture ??= new PictureRepository(UOWDbContext, new PictureMapper(_mapper));

    public IRentFixedServiceRepository RentFixedService => _rentFixedService ??=
        new RentFixedServicesRepository(UOWDbContext, new RentFixedServiceMapper(_mapper));

    public IRentMonthlyServiceRepository RentMonthlyService => _rentMonthlyService ??=
        new RentMonthlyServiceRepository(UOWDbContext, new RentMonthlyServiceMapper(_mapper));

    public IMonthlyServiceRepository MonthlyService => _monthlyService ??=
        new MonthlyServiceRepository(UOWDbContext, new MonthlyServiceMapper(_mapper));

    public IApartInHouseRepository ApartInHouse =>
        _apartInHouse ??= new ApartInHouseRepository(UOWDbContext, new ApartInHouseMapper(_mapper));
}