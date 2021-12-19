using System;
using System.Diagnostics;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using Nest;

namespace SearchAPI.Services
{
    public static class AwsElasticSearchConfig
    {
        public static IElasticClient CreateClient(string accessKey, string secretKey)
        {
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var options = new AWSOptions
            {
                Credentials = credentials,
                Region = RegionEndpoint.USWest1
            };
            var httpConnection = new AwsHttpConnection(options);

            var pool = new SingleNodeConnectionPool(new Uri("https://search-sad-properties-ivkl2y4cj3zv4hee4oi6qs34hi.us-west-1.es.amazonaws.com"));
            var config = new ConnectionSettings(pool, httpConnection);
            return new ElasticClient(config);
        }
    }
}