public class Neo4jOptions
{
    public const string SectionPath = "Neo4j";

    public string Uri { get; set; } = string.Empty;

    public string User { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
