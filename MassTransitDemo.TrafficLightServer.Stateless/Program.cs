using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Topshelf;

namespace MassTransitDemo.TrafficLightServer.Stateless
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(config => config.Service(x => new Service()));
        }
    }
}
