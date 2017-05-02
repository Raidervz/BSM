using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaMovilServer
{
    public class HomeModule : Nancy.NancyModule
    {
        public HomeModule()
        {
            Get["/"] = parameter => { return IndexPage(); };
        }

        private String IndexPage()
        {
            String html = @"<html><body><h1>Banca Movil Server is Up and Runnning...</h1></body></html>";

            return html;
        }
    }
}
