using GameStore.api.Dtos;

namespace GameStore.api.Endpoints;

public static class GamesEndpoints
{
    const string BaseEndpoint = "/games";

    private static List<GameDto> games = [
        new(1, "Street Fighter", "Fighting", 19.99m, new DateOnly(1992, 7, 15)),
        new(2, "Mortal Kombat", "Fighting", 19.99m, new DateOnly(1992, 10, 15)),
        new(3, "Tekken", "Fighting", 19.99m, new DateOnly(1994, 12, 9))
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        // GET /games
        group.MapGet("", () => games);

        // GET /games/{id}
        group.MapGet("/{id}", (int id) => games.Find(game => game.Id == id) is GameDto game ? Results.Ok(game) : Results.NotFound());

        // POST /games
        group.MapPost("", (CreateGameDto newGame) =>
        {
            GameDto game = new(
            games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );
            games.Add(game);
            return Results.Created($"/games/{game.Id}", game);
        });

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var game = games.Find(g => g.Id == id);
            if (game is null) return Results.NotFound();

            games.Remove(game);
            games.Add(new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            ));
            return Results.Ok();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            var game = games.Find(g => g.Id == id);
            if (game is null) return Results.NotFound();

            games.Remove(game);
            return Results.Ok();
        });
    }
}
