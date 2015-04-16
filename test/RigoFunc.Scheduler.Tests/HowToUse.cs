
namespace RigoFunc.Scheduler.Tests {
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HowToUse {
        ISchedulerManager schedulerMgr;
        bool? isCallbackCalled;

        [TestInitialize]
        public void Initialize() {
            schedulerMgr = new DefaultSchedulerManager();
        }

        [TestMethod]
        public void Scheduler_Manager_Can_Create_New_Scheduler_Test() {
            var scheduler = schedulerMgr.CreateScheduler("HowToUse", TimeSpan.FromSeconds(1));

            Assert.IsNotNull(scheduler);
        }

        [TestMethod]
        public void Scheduler_Can_Register_Callback_Test() {
            var scheduler = schedulerMgr.CreateScheduler("HowToUse", TimeSpan.FromSeconds(1));

            Assert.IsNotNull(scheduler);

            scheduler.Register(SchedulerCallback);
        }

        [TestMethod]
        public void Scheduler_Callback_Be_Correct_Called() {
            var scheduler = schedulerMgr.CreateScheduler("HowToUse", TimeSpan.Zero);

            Assert.IsNotNull(scheduler);

            scheduler.Register(SchedulerCallback);

            scheduler.Activate();

            Assert.IsTrue(scheduler.IsActive);

            System.Threading.Thread.Sleep(10);

            Assert.IsTrue(isCallbackCalled.HasValue);

            Assert.IsTrue(isCallbackCalled.Value);
        }

        private void SchedulerCallback() {
            isCallbackCalled = true;
        }
    }
}
