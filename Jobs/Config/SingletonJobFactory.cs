using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Quartz;
using System;

namespace Scoring.API.Jobs.Config
{
    public class SingletonJobFactory : IJobFactory
    {
        private readonly IServiceScope _serviceScope;
        public SingletonJobFactory(IServiceProvider serviceProvider)
        {
            _serviceScope = serviceProvider.CreateScope();
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return _serviceScope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            (job as IDisposable)?.Dispose();
        }
    }
}