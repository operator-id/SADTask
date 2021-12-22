using System;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using Microsoft.Extensions.Configuration;
using Nest;

namespace SearchAPI.Services
{
    public static class AwsElasticSearchConfig
    {
        public static IElasticClient CreateClient(IConfiguration configuration)
        {
            var accessKey = configuration["AWSOpenSearch:AccessKey"];
            var secretKey = configuration["AWSOpenSearch:SecretKey"];
            var url = configuration["AWSOpenSearch:DomainUrl"];
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var options = new AWSOptions
            {
                Credentials = credentials,
                Region = RegionEndpoint.USWest1
            };
            var httpConnection = new AwsHttpConnection(options);
            var pool = new SingleNodeConnectionPool(new Uri(url));
            var settings = new ConnectionSettings(pool, httpConnection);
            return new ElasticClient(settings);
        }
    }
}