using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Servers.BackgroundThreads;
using Servers.Config;
using Servers.Data.Client;
using Servers.Handlers;
using Servers.Handlers.Proxy;
using Servers.Support;

namespace Servers.Modules
{
    public class ProxyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            //Normal builder.RegisterType class
            //builder.RegisterType<TestRequestEventHandler>().AsImplementedInterfaces();
            //builder.RegisterType<TestBackGroundThread>().AsImplementedInterfaces();
            builder.RegisterType<ClientCodeRemove>().AsImplementedInterfaces();
            builder.RegisterType<ServerType>().AsImplementedInterfaces();
            builder.RegisterType<CharacterData>().AsImplementedInterfaces();
            builder.RegisterType<ClientLoginFowardingRequestHandler>().AsImplementedInterfaces();
            builder.RegisterType<LoginAuthenticationResponseHandler>().AsImplementedInterfaces();

        }
    }
}
