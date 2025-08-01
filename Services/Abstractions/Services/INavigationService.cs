﻿using Models.Navigation;
using Models.Users;

namespace Services.Abstractions.Services;

public interface INavigationService : IService<Navigation>
{
    public Task<Navigation?> GetByParamsAsync(string? name = null, UserType? authorizedUserType = null);

    public Task<Navigation?> GetByRoleAsync(UserType role);
}
