using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Servers.BackgroundThreads;
using Servers.Handlers;

namespace Servers.Modules
{
    public class TestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            //Normal builder.RegisterType class
            builder.RegisterType<TestRequestResponseHandler>().AsImplementedInterfaces();
            builder.RegisterType<TestRequestEventHandler>().AsImplementedInterfaces();
            builder.RegisterType<TestBackGroundThread>().AsImplementedInterfaces();
            
        }
    }
}
