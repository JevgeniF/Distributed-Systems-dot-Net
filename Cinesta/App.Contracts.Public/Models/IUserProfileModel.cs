﻿using App.Contracts.BLL.Services;
using App.Public.DTO.v1;
using Base.Contracts.Public;

namespace App.Contracts.Public.Models;

public interface IUserProfileModel : IEntityModel<UserProfile>, IUserProfileServiceCustom<UserProfile>
{
}