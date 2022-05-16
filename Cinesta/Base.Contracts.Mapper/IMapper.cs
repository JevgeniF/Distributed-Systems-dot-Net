namespace Base.Contracts.Mapper;

public interface IMapper<TOut, TIn>
{
    TOut? Map(TIn? entity);
    TIn? Map(TOut? entity);
}