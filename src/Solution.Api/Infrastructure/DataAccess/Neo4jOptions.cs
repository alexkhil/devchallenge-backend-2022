﻿public class Neo4jOptions
{
    public const string SectionPath = "Neo4j";

    public Uri Uri { get; set; }

    public string User { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
