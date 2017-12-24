using ProctorApi.Models;
using ProctorApi.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using ProctorApi.Repositories;

namespace ProctorApi.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private const string SelectedCustomerSettingName = "selectedCustomerId";

        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                User user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                if (!user.IsActive)
                {
                    context.SetError("invalid_grant", "Error logging in user.");
                    return;
                }

                UserRepository _userRepository = new UserRepository();

                var roles = userManager.GetRoles(user.Id);
                var userInfo = _userRepository.GetUserById(user.Id);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                    OAuthDefaults.AuthenticationType);
                // TODO: add claims here to oAuthIdentity
                ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = CreateProperties(user.UserName, user.Id, roles, userInfo);

                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch (Exception ex)
            {
                context.SetError("Critical Error", "Critical Error logging in");
            }

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, string userId, IEnumerable<string> roles, User user)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "userId", userId },
                { "roles", string.Join(",", roles.Select(x=>x.ToLower())) },
                { "user", Newtonsoft.Json.JsonConvert.SerializeObject(user) }
            };
            return new AuthenticationProperties(data);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var userId = context.Ticket.Properties.Dictionary["userId"];
            if (string.IsNullOrEmpty(userId))
            {
                context.SetError("invalid_grant", "User Id not set.");
                return Task.FromResult<object>(null);
            }

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            User user = userManager.Users.Single(i => i.Id == userId);

            if (user == null)
            {
                context.SetError("invalid_grant", "User not found.");
                return Task.FromResult<object>(null);
            }

            if (!user.IsActive)
            {
                context.SetError("invalid_grant", "Error logging in user.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }

}