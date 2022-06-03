using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUOW : IUnitOfWork
{
    IAmenityRepository Amenity { get; }
    IApartAmenityRepository ApartAmenity { get; }
    IApartmentRepository Apartment { get; }
    IApartPictureRepository ApartPicture { get; }
    IApartRentRepository ApartRent { get; }
    IBillingRepository Billing { get; }
    IFixedServiceRepository FixedService { get; }
    IHouseRepository House { get; }
    IPersonRepository Person { get; }
    IPictureRepository Picture { get; }
    IRentFixedServiceRepository RentFixedService { get; }
    IRentMonthlyServiceRepository RentMonthlyService { get; }
    IMonthlyServiceRepository MonthlyService { get; }
    IApartInHouseRepository ApartInHouse { get; }
}