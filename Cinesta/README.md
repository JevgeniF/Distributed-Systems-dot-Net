﻿# Cinesta
 
 ### Docker image: https://hub.docker.com/repository/docker/jfenko/cinestawebapp  
 ### Azure web-preview with MWC CRUD and Swagger: https://cinesta.azurewebsites.net/

### Migrations

~~~
dotnet-ef migrations --project App.DAL.EF --startup-project WebApp add Initial
dotnet-ef migrations remove --project App.DAL.EF --startup-project WebApp --context AppDbContext
~~~

### Database

~~~
dotnet-ef database update --project App.DAL.EF --startup-project WebApp
dotnet-ef database drop --project App.DAL.EF --startup-project WebApp
~~~

### MVC Razor Controllers

~~~
cd WebApp
dotnet aspnet-codegenerator controller -name CastInMoviesController       -actions -m  App.Domain.Cast.CastInMovie    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name CastRolesController       -actions -m  App.Domain.Cast.CastRole    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name PersonsController       -actions -m  App.Domain.Common.Person    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name MovieDbScoresController       -actions -m  App.Domain.Movie.MovieDbScore    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name MovieDetailsController       -actions -m  App.Domain.Movie.MovieDetails    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name MovieGenresController       -actions -m  App.Domain.Movie.MovieGenre    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name VideosController       -actions -m  App.Domain.Movie.Video    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name AgeRatingsController       -actions -m  App.Domain.MovieStandardDetails.AgeRating    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name GenresController       -actions -m  App.Domain.MovieStandardDetails.Genre    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name MovieTypesController       -actions -m  App.Domain.MovieStandardDetails.MovieType    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ProfileMoviesController       -actions -m  App.Domain.Profile.ProfileMovie    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ProfileFavoriteMoviesController       -actions -m  App.Domain.Profile.ProfileFavoriteMovie    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name UserProfilesController       -actions -m  App.Domain.Profile.UserProfile    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name PaymentDetailsController       -actions -m  App.Domain.User.PaymentDetails    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name SubscriptionsController       -actions -m  App.Domain.User.Subscription    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name UserSubscriptionsController       -actions -m  App.Domain.User.UserSubscription    -dc AppDbContext -outDir Areas\Authorized\Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
~~~

### WebApi

~~~
cd WebApp
dotnet aspnet-codegenerator controller -name CastRolesController     -m App.Domain.Cast.CastRole     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name CastInMoviesController     -m App.Domain.Cast.CastInMovie     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name PersonsController     -m App.Domain.Common.Person     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name MovieDetailsController     -m App.Domain.Movie.MovieDetails     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name VideosController     -m App.Domain.Movie.Video     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name MovieGenresController     -m App.Domain.Movie.MovieGenre     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name MovieDBScoresController     -m App.Domain.Movie.MovieDbScore     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name GenresController     -m App.Domain.MovieStandardDetails.Genre     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name MovieTypesController     -m App.Domain.MovieStandardDetails.MovieType     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name AgeRatingsController     -m App.Domain.MovieStandardDetails.AgeRating     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name UserProfilesController     -m App.Domain.Profile.UserProfile     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name ProfileMoviesController     -m App.Domain.Profile.ProfileMovie     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name ProfileFavoriteMoviesController     -m App.Domain.Profile.ProfileFavoriteMovie     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name PaymentDetailsController     -m App.Domain.User.PaymentDetails     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name SubscriptionsController     -m App.Domain.User.Subscription     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
dotnet aspnet-codegenerator controller -name UserSubscriptionsController     -m App.Domain.User.UserSubscription     -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions  -f
~~~

### Identity pages

~~~
cd WebApp
dotnet aspnet-codegenerator identity -dc App.DAL.EF.AppDbContext -f
~~~

### Docker deployment
~~~
docker build -t [image] .
docker tag [image] [username]/[image]:[tag]
docker login -u [username] -p [password]
docker push [username]/[image]:[tag]
~~~
