using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using CL.Services.Web.Models.User;
using CL.Services.Web.Repository;

namespace CL.Services.Web.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
                throw new ArgumentNullException("publicClientId");
            
            _publicClientId = publicClientId;
        }


        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<Business.User.UserManager>();

            Contracts.IUser foundUser = await userManager.FindAsync(context.UserName, context.Password);
            if (foundUser == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                context.Rejected();
                return;
            }

            Business.User.User user = new Business.User.User(foundUser);

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            
            AuthenticationProperties properties = CreateProperties(user);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            //context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }


        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
                context.Validated();
            

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

        public static AuthenticationProperties CreateProperties(Business.User.User user)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", user.UserName },
                { "userId", user.Id}
            };
            return new AuthenticationProperties(data);
        } 
    }
}