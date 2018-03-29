using System;
using System.Threading;
using PiIO;
using PiIO.SoftPWM;

namespace TestPwm {
    class Program {

        static void Main(string[] args) {
            //int ret = Init.Setup();
            int ret = Init.SetupGpio();
            if(ret == -1) {
                Console.WriteLine("Init failed: {0}", ret);
                return;
            }
            int range = -1;
            int value = -1;
            int pin = 0;
            try {
                pin = Int32.Parse(args[0]);
                range = Int32.Parse(args[1]);
                value = Int32.Parse(args[2]);
            } catch {
                Console.WriteLine("Parse Error");
                return;
            }
            Console.WriteLine("range:{0}, value:{1}", range, value);
            SoftPwmCmd.Create(pin, value, range);
            Console.WriteLine("Init succeeded");

            SoftPwmCmd.Write(pin, value);
            Console.ReadKey(true);
            SoftPwmCmd.Stop(pin);
            Thread.Sleep(100);
        }
    }
}
