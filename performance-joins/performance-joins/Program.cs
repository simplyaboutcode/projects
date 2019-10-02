using System;
using System.Diagnostics;
using BenchmarkDotNet.Running;

namespace performance_joins
{
    class Program
    {
        static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
