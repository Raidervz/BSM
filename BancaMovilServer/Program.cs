using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaMovilServer
{
    class Program
    {
        const string DOMAIN = "http://localhost:8088";
        static void Main(string[] args)
        {
            // create a new self-host server
            var nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri(DOMAIN));
            // start
            nancyHost.Start();
            Console.WriteLine("REST service listening on " + DOMAIN);
            // stop with an <Enter> key press
            Console.ReadLine();
            nancyHost.Stop();
        }

        public class Bootstrapper : Nancy.DefaultNancyBootstrapper
        {
            protected virtual Nancy.Bootstrapper.NancyInternalConfiguration InternalConfiguration
            {
                get
                {
                    return Nancy.Bootstrapper.NancyInternalConfiguration.Default;
                }
            }
        }
    }
}
