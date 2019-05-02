using Servers.Models.Interfaces;
using Servers.Services.Interfaces;

namespace Servers.Services
{
    public class InterestManagement: IInterestManagement
    {
        private IRegion Region { get; set; }

        public InterestManagement(IRegion region)
        {
            Region = region;
        }

        public void ComputeAreaOfInterest()
        {

        }
    }
}
