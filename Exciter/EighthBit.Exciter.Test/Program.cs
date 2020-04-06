using System;
using System.Threading;
using EighthBit.Nmg.ManagedCan;

namespace EighthBit.Exciter.Test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var can = new NmgManagedCanControl();
            Thread.Sleep(1000);
            can.Initialize(ExciterCanBaudRate.C10Kbs);

            var manager = new ExciterManager(can);
            
            //can.Received += can_Received;
            var api = new ExciterApi2Kw(can);
            //api.PowerOn(1500);
            //Thread.Sleep(500);
            //api.RequestStatus();
            api.RequestData(19);

            Console.ReadKey();

            
            //api.Sweep(1000, ExciterModulation.Fm, 200);
            //api.RequestStatus();
            //api.PowerOn(1500);
            //api.SweepDomain(3000000,6000000);
            Thread.Sleep(500);
            api.Spot(4250000, ExciterModulation.Lsb);
            Thread.Sleep(500);
            api.ControlMode(AccessMode.Local);

            //api.ToneFrequency(3000, 3500);
            //api.Noise(ExciterNoise.R3, ExciterNoise.R13);
            //api.MultiSpot(2, ExciterModulation.Am);
            //api.Reset();
            Console.ReadKey();

        }

        static void can_Received(object sender, Can.CanFrame e)
        {
            Console.WriteLine(e);
        }

    }
}
