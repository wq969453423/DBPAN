using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBPAN
{
    public class QuartzStartUp
    {
        //调度工厂
        private static ISchedulerFactory _factory;
        //调度器
        private static IScheduler _scheduler;
        public async Task<string> Start()
        {

            _factory = new StdSchedulerFactory();
            _scheduler = await _factory.GetScheduler();

            IJobDetail jobpickinfo = JobBuilder.Create<QuartzJob>().Build();

            ITrigger triggerpickinfo = TriggerBuilder.Create()
                            .WithIdentity("JobPickInfo", "PickInfo")
                            .WithCronSchedule("0 04 * * * ?")
                            .Build();

            //将触发器和任务器绑定到调度器中
            await _scheduler.ScheduleJob(jobpickinfo, triggerpickinfo);

            await _scheduler.Start();
            return "将触发器和任务器绑定到调度器中完成";
        }

        public void Stop()
        {
            if (_scheduler != null)
            {
                _scheduler.Clear();
                _scheduler.Shutdown();
            }
            _scheduler = null;
            _factory = null;
        }

    }
}
