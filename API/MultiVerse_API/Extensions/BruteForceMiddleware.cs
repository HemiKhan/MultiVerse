public class BruteForceMiddleware
{
    private readonly RequestDelegate _next;
    private static Dictionary<string, int> _failedAttempts = new Dictionary<string, int>();
    private static Dictionary<string, DateTime> _blockedIPs = new Dictionary<string, DateTime>();
    private const int MaxFailedAttempts = 5; // Adjust as needed
    private static readonly TimeSpan BlockDuration = TimeSpan.FromMinutes(10); // Adjust as needed

    public BruteForceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string ipAddress = context.Connection.RemoteIpAddress!.ToString();

        // Check if the IP address is blocked
        if (_blockedIPs.ContainsKey(ipAddress))
        {
            DateTime unblockTime = _blockedIPs[ipAddress];
            if (unblockTime > DateTime.Now)
            {
                // Still blocked, return 403 Forbidden
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync($"IP address {ipAddress} is blocked. Please try again later.");
                return;
            }
            else
            {
                // Unblock the IP address
                _blockedIPs.Remove(ipAddress);
            }
        }

        // Check failed attempts
        if (_failedAttempts.TryGetValue(ipAddress, out int failedAttempts) && failedAttempts >= MaxFailedAttempts)
        {
            // Block the IP address for a certain duration
            DateTime unblockTime = DateTime.Now.Add(BlockDuration);
            _blockedIPs[ipAddress] = unblockTime;

            // Reset failed attempts
            _failedAttempts.Remove(ipAddress);

            // Return 403 Forbidden
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync($"IP address {ipAddress} is blocked. Please try again later.");
            return;
        }

        await _next(context);

        // If request failed (e.g., failed login attempt), track it
        if (context.Response.StatusCode == 401) // Unauthorized
        {
            if (_failedAttempts.ContainsKey(ipAddress))
            {
                _failedAttempts[ipAddress]++;
            }
            else
            {
                _failedAttempts.Add(ipAddress, 1);
            }
        }
    }
}
