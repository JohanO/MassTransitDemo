using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Topshelf;
using Topshelf.HostConfigurators;

namespace MassTransitDemo.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(config => config.Service(x => new DemoService()));
        }
    }
}
