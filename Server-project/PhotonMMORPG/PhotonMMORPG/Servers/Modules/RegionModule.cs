using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Servers.Handlers.Regions;
using Servers.Models;

namespace Servers.Modules
{
    public class RegionModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ClientEnterRegion>().AsImplementedInterfaces();

            builder.RegisterType<World>().AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

        }
    }
}
