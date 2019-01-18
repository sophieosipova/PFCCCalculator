using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutorizationService
{
    public static class AutorizationCodeGenerator
    {
        static private readonly Random random = new Random((int)DateTime.Now.Ticks);

        public static string GetAutorizationCode ()
        {
            return random.Next().ToString();
        }
    }
}
