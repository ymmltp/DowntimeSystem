using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DowntimeSystem
{
    public class WindowsAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public WindowsAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var username = System.Environment.UserName;
            var domain = System.Environment.UserDomainName;
            var fullName = $"{domain}\\{username}";

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, fullName),
                new Claim(ClaimTypes.NameIdentifier, username)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}