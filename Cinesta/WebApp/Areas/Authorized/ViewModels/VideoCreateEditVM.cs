﻿#pragma warning disable CS1591
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class VideoCreateEditVM
{
    public Video Video { get; set; } = default!;

    public SelectList? MovieDetailsSelectList { get; set; }
}