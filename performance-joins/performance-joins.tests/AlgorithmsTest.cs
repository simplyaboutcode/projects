using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace performance_joins.tests
{
    public class AlgorithmsTest
    {
        public List<Worker> Workers { get; set; }
        public List<Building> Buildings { get; set; }

        public AlgorithmsTest()
        {
            //From 2 sets of data (workers and buildings) find workers which are working in building
            //Worker is working in buildng if building.Id == worker.BuildingId and worker.IsEmployed == true
            //if there is a match add worker to MatchedBuildings prop

            Workers = JsonConvert.DeserializeObject<List<Worker>>(File.ReadAllText(@"workers.json"));
            Buildings = JsonConvert.DeserializeObject<List<Building>>(File.ReadAllText(@"buildings.json"));
        }

        [Fact]
        public void FirstAlgorithmIsDeterministic()
        {
            var sut = new Algorithms();
            var result = sut.ProcessData(Workers, Buildings);
            var result2 = sut.ProcessData(Workers, Buildings);

            result.Should().BeEquivalentTo(result2);
        }

        [Fact]
        public void SecondAlgorithmIsDeterministic()
        {
            var sut = new Algorithms();
            var result = sut.ProcessData2(Workers, Buildings);
            var result2 = sut.ProcessData2(Workers, Buildings);

            result.Should().BeEquivalentTo(result2);
        }

        [Fact]
        public void ThirdAlgorithmIsDeterministic()
        {
            var sut = new Algorithms();
            var result = sut.ProcessData3(Workers, Buildings);
            var result2 = sut.ProcessData3(Workers, Buildings);

            result.Should().BeEquivalentTo(result2);
        }

        [Fact]
        public void ForthAlgorithmIsDeterministic()
        {
            var sut = new Algorithms();
            var result = sut.ProcessData4(Workers, Buildings);
            var result2 = sut.ProcessData4(Workers, Buildings);

            result.Should().BeEquivalentTo(result2);
        }


        [Fact]
        public void FifthAlgorithmIsDeterministic()
        {
            var sut = new Algorithms();
            var result = sut.ProcessData5(Workers, Buildings);
            var result2 = sut.ProcessData5(Workers, Buildings);

            result.Should().BeEquivalentTo(result2);
        }

        [Fact]
        public void FirstAlgorithmIsEquivalentToSecond()
        {
            var sut = new Algorithms();
            var result = sut.ProcessData(Workers, Buildings);
            var result2 = sut.ProcessData2(Workers, Buildings);

            result.Should().BeEquivalentTo(result2);
        }

        [Fact]
        public void SecondAlgorithmIsEquivalentToThird()
        {
            var sut = new Algorithms();
            var result = sut.ProcessData2(Workers, Buildings);
            var result2 = sut.ProcessData3(Workers, Buildings);

            result.Should().BeEquivalentTo(result2);
        }

        [Fact]
        public void ThirdAlgorithmIsEquivalentToForth()
        {
            var sut = new Algorithms();
            var result = sut.ProcessData3(Workers, Buildings);
            var result2 = sut.ProcessData4(Workers, Buildings);

            result.Should().BeEquivalentTo(result2);
        }

        [Fact]
        public void ForthAlgorithmIsEquivalentToFifth()
        {
            var sut = new Algorithms();
            var result = sut.ProcessData4(Workers, Buildings);
            var result2 = sut.ProcessData5(Workers, Buildings);

            result.Should().BeEquivalentTo(result2);
        }
    }
}
