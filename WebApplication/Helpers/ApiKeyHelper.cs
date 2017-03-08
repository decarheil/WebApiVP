using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;

namespace WebApplication.Helpers
{
    public static class ApiKeyHelper
    {
        public static string GenerateAccessKey()
        {
            var rngProvider = RNGCryptoServiceProvider.Create();
            var bytes = new byte[15];
            rngProvider.GetBytes(bytes);;

            return Convert.ToBase64String(bytes).ToUpper().Replace("+", "0").Replace("/", "9");
        }
    }
}