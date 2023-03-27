namespace MicroBlog.UI.Core.Helpers;

public static class JwtParser
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];

        var jsonBytes = ParseBase64WithoutPadding(payload);

        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object?>>(jsonBytes);

        if (keyValuePairs is not null)
        {
            ExtractRolesFromJwt(claims, keyValuePairs);
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty)));

            return claims;
        }

        return new List<Claim>();
    }

    private static void ExtractRolesFromJwt(ICollection<Claim> claims, IDictionary<string, object?> keyValuePairs)
    {
        keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);
        if (roles != null)
        {
            var parsedRoles = roles.ToString()?.Trim().TrimStart('[').TrimEnd(']').Split(',');
            if (parsedRoles is not null)
            {
                if (parsedRoles.Length > 1)
                {
                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0]));
                }
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }
}
