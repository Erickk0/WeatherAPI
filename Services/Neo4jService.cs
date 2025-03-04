using System;
using System.Security.Cryptography.X509Certificates;
using Neo4j.Driver;
using WeatherAPI.Models;

namespace Weather_API
{
    public class Neo4jService
    {
        private readonly IDriver _driver;

        public Neo4jService(string uri, string user, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }
    }
}