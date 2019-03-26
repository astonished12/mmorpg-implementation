using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Servers.Handlers.Regions;
using Servers.Models;
using Servers.Services.Interfaces;
using Servers.Services.RegionService;
using Servers.Services.WorldServices;

namespace Servers.Modules
{
    public class RegionModule: Module
    {
        protected bool WorldModuleWasWorldServiceWasLoaded { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            builder.RegisterType<RegionService>().AsImplementedInterfaces();
            builder.RegisterType<AssignAreaMapRegionHandler>().AsImplementedInterfaces();
            builder.RegisterType<ClientEnterRegion>().AsImplementedInterfaces();
            

        }
    }
}
