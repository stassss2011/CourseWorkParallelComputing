using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class Response
    {
        public string Query { get; init; }
        public int Count { get; init; }

        private readonly IEnumerable<WordInstance> _instances;

        public IEnumerable<WordInstance> Instances
        {
            get => _instances;
            init
            {
                _instances = value;
                Count = value.Count();
            }
        }
    }
}