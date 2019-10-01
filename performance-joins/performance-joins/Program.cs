using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace performance_joins
{
    class Program
    {
        public static List<Worker> Workers { get; set; }
        public static List<Building> Buildings { get; set; }
        public static HashSet<Building> MatchedBuildings { get; set; } = new HashSet<Building>();

        static void Main(string[] args)
        {
            //From 2 sets of data (workers and buildings) find workers which are working in building
            //Worker is working in buildng if building.Id == worker.BuildingId and worker.IsEmployed == true
            //if there is a match add worker to MatchedBuildings prop

            Workers = JsonConvert.DeserializeObject<List<Worker>>(File.ReadAllText(@"..\..\..\workers.json"));
            Buildings = JsonConvert.DeserializeObject<List<Building>>(File.ReadAllText(@"..\..\..\buildings.json"));

            MeasureTime((Workers, Buildings) => ProcessData(Workers, Buildings));
            MeasureTime((Workers, Buildings) => ProcessData2(Workers, Buildings));
            MeasureTime((Workers, Buildings) => ProcessData3(Workers, Buildings));
            Console.Read();
        }

        static void ProcessData(List<Worker> workers, List<Building> buildings)
        {
            var employedWorkers = workers.Where(n => n.IsEmployed == true); // O(n)

            foreach (var building in buildings) // O(n)
            {
                foreach (var worker in employedWorkers) //O(n)
                {
                    if (building.Id == worker.BuildingId)
                    {
                        building.Workers.Add(worker.Id);
                        MatchedBuildings.Add(building);
                    }
                }
            } // TOTAL: O(n^2)
        }

        static HashSet<Building> ProcessData2(List<Worker> workers, List<Building> buildings)
        {
            HashSet<Building> matchedBuildings = new HashSet<Building>();
            workers
                .Where(n => n.IsEmployed == true) // O(n)
                .Join(buildings, worker =>
                    worker.BuildingId, building => building.Id, (worker, building) => // O(n + m) => O(n)
                    {
                        building.Workers.Add(worker.Id);
                        return matchedBuildings.Add(building);
                    }).ToList();
            return matchedBuildings; // TOTAL: O(n)
        }

        static HashSet<Building> ProcessData3(List<Worker> workers, List<Building> buildings)
        {
            var groupedWorkers = workers
                .Where(n => n.IsEmployed == true) // O(n)
                .GroupBy(n => n.BuildingId); // O(n)
            HashSet<Building> matchedBuildings = new HashSet<Building>();
            var dictWorkers = groupedWorkers.ToDictionary(t => t.Key, t => t);

            foreach (var building in buildings) //O(n)
            {
                if (dictWorkers.ContainsKey(building.Id))
                {
                    building.Workers.AddRange(dictWorkers[building.Id].Select(n => n.Id));
                    matchedBuildings.Add(building);
                }
            }
            return matchedBuildings; // TOTAL: O(n)
        }

        static void MeasureTime(Action<List<Worker>, List<Building>> processingMethod)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            processingMethod(Workers, Buildings);
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }
    }
}
