using Dapper;
using FileApi.DTO.Image;
using FileApi.Helpers;

namespace FileApi.Endpoints
{
    public static class ImagesEndPoint
    {

        public static void MapAdminsEndpoint(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("/Images");


            group.MapGet("/GetAllImages", async (DbConnFactoryHelper dbConnectionFactory) =>
            {
                using var conn = dbConnectionFactory.Create();


                const string qry = """
                                        SELECT 
                                                id, imageName, uploadedAt
                                            FROM tbl_images;
                                   """;

                var images = await conn.QueryAsync<ImageRes>(qry);

                return Results.Ok(images);
            });


            group.MapGet("/GetImageById/{id}", async (int id, DbConnFactoryHelper dbConnectionFactory) =>
            {
                using var conn = dbConnectionFactory.Create();


                const string qry = """
                                        SELECT 
                                                id, imageName, uploadedAt
                                            FROM tbl_images i
                                                WHERE iiId=@id;
                                   """;

                var image = await conn.QuerySingleOrDefaultAsync<ImageRes>(qry, new { id });

                return image is not null ? Results.Ok(image) : Results.NotFound($"Image with id of {id} was not found");
            });



            group.MapPost("/AddImage", async (IFormFile imageName, DbConnFactoryHelper dbConnectionFactory) =>
            {
                using var conn = dbConnectionFactory.Create();

                const string qry = """
                                        INSERT INTO tbl_images (imageName)
                                            VALUES (@imageName);
                                   """;

                var result = await conn.ExecuteAsync(qry, imageName);

                return Results.Ok(result);
            });


            group.MapGet("/GetImage", (string imageName) =>
            {

            });


        }
    }
}
