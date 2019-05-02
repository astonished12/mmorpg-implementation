using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Servers.Data.Client;
using Servers.Handlers.Login;
using Servers.Services;
using Servers.Services.AuthorizationServices;

namespace Servers.Modules
{
    public class LoginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<LoginAuthenticationHandler>().AsImplementedInterfaces();
            builder.RegisterType<LoginAccountCreationHandler>().AsImplementedInterfaces();
            builder.RegisterType<CharacterData>().AsImplementedInterfaces();
            //builder.RegisterType<UserPassAuthorizationService>().AsImplementedInterfaces();
            builder.RegisterType<UserSaltPassAuthorizationService>().AsImplementedInterfaces();
            builder.RegisterType<LoginCharacterListCharacterHandler>().AsImplementedInterfaces();
            builder.RegisterType<CharacterService>().AsImplementedInterfaces();
            builder.RegisterType<LoginCharacterCreationHandler>().AsImplementedInterfaces();
            builder.RegisterType<LoginCharacterSelectionHandler>().AsImplementedInterfaces();

        }
    }
}
