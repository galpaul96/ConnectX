namespace ConnectX.Api.MongoDb;

public class MongoDbOptions
{
    public string MyDb_Host { get; set; } = string.Empty;
    public int MyDb_Port { get; set; }
    public string? MyDb_Username { get; set; }
    public string? MyDb_Password { get; set; }
    public string? MyDb_AuthenticationDatabase { get; set; }
    public string? MyDb_AuthenticationMechanism { get; set; }
    public string? MyDb_Uri { get; set; }
    public string DatabaseName { get; set; } = "Students";

    public string GetConnectionString()
    {
        return !string.IsNullOrWhiteSpace(MyDb_Uri)
            ? MyDb_Uri
            : BuildConnectionString();
    }

    private string BuildConnectionString()
    {
        var credentials = string.Empty;

        if (!string.IsNullOrWhiteSpace(MyDb_Username))
        {
            credentials = UriEscape(MyDb_Username);

            if (!string.IsNullOrWhiteSpace(MyDb_Password))
            {
                credentials += $":{UriEscape(MyDb_Password)}";
            }

            credentials += "@";
        }

        var queryParameters = new List<string>();

        if (!string.IsNullOrWhiteSpace(MyDb_AuthenticationDatabase))
        {
            queryParameters.Add($"authSource={UriEscape(MyDb_AuthenticationDatabase)}");
        }

        if (!string.IsNullOrWhiteSpace(MyDb_AuthenticationMechanism))
        {
            queryParameters.Add($"authMechanism={UriEscape(MyDb_AuthenticationMechanism)}");
        }

        var queryString = queryParameters.Count > 0
            ? $"?{string.Join("&", queryParameters)}"
            : string.Empty;

        return $"mongodb://{credentials}{MyDb_Host}:{MyDb_Port}/{queryString}";
    }

    private static string UriEscape(string value)
    {
        return System.Uri.EscapeDataString(value);
    }
}
