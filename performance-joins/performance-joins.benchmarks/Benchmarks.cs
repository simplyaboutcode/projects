using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;

namespace performance_joins.benchmarks
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        public List<Worker> Workers { get; set; }
        public List<Building> Buildings { get; set; }

        private readonly Algorithms _algorithms = new Algorithms();

        public Benchmarks()
        {
            //From 2 sets of data (workers and buildings) find workers which are working in building
            //Worker is working in buildng if building.Id == worker.BuildingId and worker.IsEmployed == true
            //if there is a match add worker to MatchedBuildings prop

            Workers = JsonConvert.DeserializeObject<List<Worker>>(File.ReadAllText(@"workers.json"));
            Buildings = JsonConvert.DeserializeObject<List<Building>>(File.ReadAllText(@"buildings.json"));
        }

        [Benchmark]
        public HashSet<Building> ProcessData()
        {
            return _algorithms.ProcessData(Workers, Buildings);
        }

        [Benchmark]
        public HashSet<Building> ProcessData2()
        {
            return _algorithms.ProcessData2(Workers, Buildings);
        }

        [Benchmark]
        public HashSet<Building> ProcessData3()
        {
            return _algorithms.ProcessData3(Workers, Buildings);
        }

        [Benchmark]
        public HashSet<Building> ProcessData4()
        {
            return _algorithms.ProcessData4(Workers, Buildings);
        }

        [Benchmark]
        public HashSet<Building> ProcessData5()
        {
            return _algorithms.ProcessData5(Workers, Buildings);
        }
    }
}