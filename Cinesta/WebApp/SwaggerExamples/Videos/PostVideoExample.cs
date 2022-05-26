using Swashbuckle.AspNetCore.Filters;

namespace WebApp.SwaggerExamples;

public class PostVideoExample: IExamplesProvider<object>
{
    public object GetExamples()
    {
        return new
        {
            Id = "c285c9c8-e93d-4885-9284-8d85a9555eb7",
            Season = 1,
            Title = new
            {
                en = "Episode 1: Pilot",
                ee = "Episood 1: Piloot",
                ru = "Эпизод 1: Пилотный"
            },
            FileUri = "www.videoHosting.com/series/episode1.avi",
            Duration = "0001-01-01T00:45",
            Description = new
            {
                en = "Pilot episode of new series",
                ru = "Пилотный эпизод нового сериала",
                ее = "Uue sarja piloot episood"
            },
            MovieDetailsId = "58aad3e0-0f1a-4d7f-ae90-6f40442e0960"
        };
    }
}