﻿using System.ComponentModel.DataAnnotations;
using App.DTO.Movie;
using Base.Domain;

namespace App.DTO.Profile;

public class ProfileFavoriteMovie: DomainEntityId<Guid>
{
    public Guid UserProfileId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Profile.ProfileFavoriteMovie), Name = nameof(UserProfile))]
    public UserProfile? UserProfile { get; set; }

    public Guid MovieDetailsId { get; set; }

    [Display(ResourceType = typeof(Resources.App.Domain.Profile.ProfileFavoriteMovie), Name = nameof(MovieDetails))]
    public MovieDetails? MovieDetails { get; set; }
}