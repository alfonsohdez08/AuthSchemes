using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AuthPolicies
{
    public abstract class DummyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        protected DummyAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            await Task.CompletedTask;

            if (!Context.Request.Headers.TryGetValue("authorization", out var authHeaderValue) ||
                !authHeaderValue.ToString().Split(',').Any(s => s == Name))
            {
                return AuthenticateResult.NoResult();
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, Name)
            };            
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        public abstract string Name { get; }
    }
}