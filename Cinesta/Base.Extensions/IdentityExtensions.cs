using System.ComponentModel;
using System.Security.Claims;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Base.Extensions;

public static class IdentityExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        return GetUserId<Guid>(user);
    }
    
    public static string GetUserFullName(this ClaimsPrincipal user)
    {
        return (user.Claims.FirstOrDefault(c => c.Type == "aspnet.name")?.Value ?? "Unknown") + " " +
            (user.Claims.FirstOrDefault(c => c.Type == "aspnet.surname")?.Value ?? "Person");
    }
    
    public static string GetUserName(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == "aspnet.name")?.Value ?? "";
    }
    
    public static string GetUserSurname(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == "aspnet.surname")?.Value ?? "";
    }

    public static TKeyType GetUserId<TKeyType>(this ClaimsPrincipal user)
    {
        var idClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (idClaim == null) throw new NullReferenceException("NameIdentifier claim not found");

        var res = (TKeyType) TypeDescriptor.GetConverter(typeof(TKeyType)).ConvertFromInvariantString(idClaim.Value)!;
        return res;
    }
}