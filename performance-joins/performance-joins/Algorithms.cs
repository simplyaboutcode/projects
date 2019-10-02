using System.Collections.Generic;
using System.Linq;

namespace performance_joins
{
    public class Algorithms
    {
        public HashSet<Building> ProcessData(List<Worker> workers, List<Building> buildings)
        {
            var employedWorkers = workers.Where(n => n.IsEmployed == true); // O(n)
            HashSet<Building> matchedBuildings = new HashSet<Building>();
            foreach (var building in buildings) // O(n)
            {
                foreach (var worker in employedWorkers) //O(n)
                {
                    if (building.Id == worker.BuildingId)
                    {
                        //building.Workers.Add(worker.Id); // <-- this line is problematic for unit tests and benchmark test. It changes source data.
                        matchedBuildings.Add(building);
                    }
                }
            }
            return matchedBuildings; // TOTAL: O(n^2)
        }

        public HashSet<Building> ProcessData2(List<Worker> workers, List<Building> buildings)
        {
            HashSet<Building> matchedBuildings = new HashSet<Building>();
            workers
                .Where(n => n.IsEmployed) // O(n)
                .Join(buildings, worker =>
                    worker.BuildingId, building => building.Id, (worker, building) => // O(n + m) => O(n)
                    {
                        //building.Workers.Add(worker.Id); // <-- this line is problematic for unit tests and benchmark test. It changes source data.
                        return matchedBuildings.Add(building);
                    }).ToList();
            return matchedBuildings; // TOTAL: O(n)
        }

        public HashSet<Building> ProcessData3(List<Worker> workers, List<Building> buildings)
        {
            var groupedWorkers = workers
                .Where(n => n.IsEmployed) // O(n)
                .GroupBy(n => n.BuildingId); // O(n)
            HashSet<Building> matchedBuildings = new HashSet<Building>();
            var dictWorkers = groupedWorkers.ToDictionary(t => t.Key, t => t);

            foreach (var building in buildings) //O(n)
            {
                if (dictWorkers.ContainsKey(building.Id))
                {
                    //building.Workers.AddRange(dictWorkers[building.Id].Select(n => n.Id));  // <-- this line is problematic for unit tests and benchmark test. It changes source data.
                    matchedBuildings.Add(building);
                }
            }
            return matchedBuildings; // TOTAL: O(n)
        }

        public HashSet<Building> ProcessData4(List<Worker> workers, List<Building> buildings)
        {
            HashSet<Building> matchedBuildings = new HashSet<Building>();

            foreach (var worker in workers.Where(n => n.IsEmployed)) 
            {
                var building = buildings.FirstOrDefault(p => p.Id == worker.BuildingId);

                if (building != null)
                {
                    //building.Workers.Add(worker.Id); // <-- this line is problematic for unit tests and benchmark test. It changes source data.
                    matchedBuildings.Add(building);
                }
            }

            return matchedBuildings;
        }

        public HashSet<Building> ProcessData5(List<Worker> workers, List<Building> buildings)
        {
            var buildingsDict = buildings.ToDictionary(p => p.Id, p => p);
            HashSet<Building> matchedBuildings = new HashSet<Building>();

            foreach (var worker in workers.Where(n => n.IsEmployed))
            {
                if (buildingsDict.ContainsKey(worker.BuildingId))
                {
                    var building = buildingsDict[worker.BuildingId];

                    //building.Workers.Add(worker.Id); // <-- this line is problematic for unit tests and benchmark test. It changes source data.
                    matchedBuildings.Add(building);
                }
            }

            return matchedBuildings;
        }
    }
}
