using System.IO;
using Microsoft.AspNetCore.Http;

namespace SearchAPI.Models
{
    public class IndexParams
    {
        public string JsonString { get; set; }
        public string IndexName { get; set; }
        public string ModelType { get; set; }
    }
}