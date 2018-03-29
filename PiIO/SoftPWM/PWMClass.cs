using System.Runtime.InteropServices;

namespace PiIO.SoftPWM
{
	/// <summary>
	/// Used to configure a GPIO pin's direction and provide read & write functions to a GPIO pin
	/// </summary>
	public class SoftPwmCmd
	{
		[DllImport("wiringPi.so", EntryPoint = "softPwmCreate")]
		public static extern int Create(int pin, int initialValue, int pwmRange);

		[DllImport("wiringPi.so", EntryPoint = "softPwmWrite")]
		public static extern void Write(int pin, int value);

		[DllImport("wiringPi.so", EntryPoint = "softPwmStop")]
		public static extern void Stop(int pin);
	}

}
