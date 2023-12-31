using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;

namespace Hj.SutFactory.Benchmarking;

public class Config : ManualConfig
{
  public Config()
  {
    var alphabeticalOrderer = new DefaultOrderer(SummaryOrderPolicy.Declared, MethodOrderPolicy.Alphabetical);

    AddLogger(ConsoleLogger.Default)
      .AddJob(Job.MediumRun.WithRuntime(ClrRuntime.Net462).AsBaseline())
      .AddJob(Job.MediumRun.WithRuntime(CoreRuntime.Core60))
      .AddJob(Job.MediumRun.WithRuntime(CoreRuntime.Core80))
      .WithOptions(ConfigOptions.StopOnFirstError | ConfigOptions.JoinSummary | ConfigOptions.DisableLogFile)
      .AddDiagnoser(MemoryDiagnoser.Default)
      .AddColumnProvider(DefaultColumnProviders.Instance)
      .AddExporter(MarkdownExporter.GitHub)
      .WithOrderer(alphabeticalOrderer);
  }
}
