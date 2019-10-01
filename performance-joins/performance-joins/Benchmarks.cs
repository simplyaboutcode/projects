using System.Collections.Generic;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;

namespace performance_joins
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        public List<Worker> Workers { get; set; }
        public List<Building> Buildings { get; set; }

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
            var employedWorkers = Workers.Where(n => n.IsEmployed == true); // O(n)
            HashSet<Building> matchedBuildings = new HashSet<Building>();
            foreach (var building in Buildings) // O(n)
            {
                foreach (var worker in employedWorkers) //O(n)
                {
                    if (building.Id == worker.BuildingId)
                    {
                        building.Workers.Add(worker.Id);
                        matchedBuildings.Add(building);
                    }
                }
            }
            return matchedBuildings; // TOTAL: O(n^2)
        }

        [Benchmark]
        public HashSet<Building> ProcessData2()
        {
            HashSet<Building> matchedBuildings = new HashSet<Building>();
            Workers
                .Where(n => n.IsEmployed == true) // O(n)
                .Join(Buildings, worker =>
                    worker.BuildingId, building => building.Id, (worker, building) => // O(n + m) => O(n)
                {
                    building.Workers.Add(worker.Id);
                    return matchedBuildings.Add(building);
                }).ToList();
            return matchedBuildings; // TOTAL: O(n)
        }

        [Benchmark]
        public HashSet<Building> ProcessData3()
        {
            var groupedWorkers = Workers
                .Where(n => n.IsEmployed == true) // O(n)
                .GroupBy(n => n.BuildingId); // O(n)
            HashSet<Building> matchedBuildings = new HashSet<Building>();
            var dictWorkers = groupedWorkers.ToDictionary(t => t.Key, t => t);

            foreach (var building in Buildings) //O(n)
            {
                if (dictWorkers.ContainsKey(building.Id))
                {
                    building.Workers.AddRange(dictWorkers[building.Id].Select(n => n.Id));
                    matchedBuildings.Add(building);
                }
            }
            return matchedBuildings; // TOTAL: O(n)
        }
    }
}