using Consumers;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace SincronizzatorePOC
{
    public class ConsumersLauncher : BackgroundService
    {
        private readonly List<ConsumerOptions> _consumers;
        private List<Process?> processes = new();

        public ConsumersLauncher(List<ConsumerOptions> consumers)
        {
            _consumers = consumers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            processes = _consumers.Select(opt => StartConsumer(opt)).ToList();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }

            Dispose();
        }

        private Process? StartConsumer(ConsumerOptions opt)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = @"..\..\..\..\Consumers.Concrete\bin\Debug\net8.0\Consumers.Concrete.exe",
                UseShellExecute = true,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = $"\"{opt.Id}\" \"{opt.Name}\" \"{opt.Address}\"",
            });


        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var process in processes)
            {
                if (process != null)
                {
                    process.Kill(true);
                    process.Dispose();
                }
            }
        }
    }
}
