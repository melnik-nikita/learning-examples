using System.Text.Json.Serialization;

namespace DynamoDb.Transactions;

public class MovieByYear
{
    [JsonPropertyName("pk")]
    public string Pk => ReleaseYear.ToString();

    [JsonPropertyName("sk")]
    public string Sk => Title;

    public Guid Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int AgeRestriction { get; set; }
    public int ReleaseYear { get; set; }
    public int RottenTomatoesPercentage { get; set; }
}

public class MovieByTitle
{
    [JsonPropertyName("pk")]
    public string Pk => Title;

    [JsonPropertyName("sk")]
    public string Sk => RottenTomatoesPercentage.ToString();

    public Guid Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int AgeRestriction { get; set; }
    public int ReleaseYear { get; set; }
    public int RottenTomatoesPercentage { get; set; }
}
