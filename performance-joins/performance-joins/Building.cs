using System.Collections.Generic;

namespace performance_joins
{
    public class Building
    {
        public string Id { get; set; }
        public List<string> Workers { get; set; } = new List<string>();
    }
}
