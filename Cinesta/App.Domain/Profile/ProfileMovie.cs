﻿using App.Domain.Movie;
using Base.Domain;

namespace App.Domain.Profile;

public class ProfileMovie : DomainEntityMetaId
{
    public Guid UserProfileId { get; set;}
    public UserProfile? UserProfile { get; set; }
    
    public Guid MovieDetailsId { get; set; }
    public MovieDetails? MovieDetails { get; set; }
}