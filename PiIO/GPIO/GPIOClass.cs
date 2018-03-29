using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

//Todo: Investigate
namespace PiIO.GPIO
{
	public class GPIOCmd
	{
		[DllImport("wiringPi.so", EntryPoint = "pinMode")]           //Uses Gpio pin numbers
		public static extern void pinMode(int pin, int mode);

		[DllImport("wiringPi.so", EntryPoint = "digitalWrite")]      //Uses Gpio pin numbers
		public static extern void digitalWrite(int pin, int value);

		[DllImport("wiringPi.so", EntryPoint = "digitalWriteByte")]      //Uses Gpio pin numbers
		public static extern void digitalWriteByte(int value);

		[DllImport("wiringPi.so", EntryPoint = "digitalRead")]           //Uses Gpio pin numbers
		public static extern int digitalRead(int pin);

		[DllImport("wiringPi.so", EntryPoint = "pullUpDnControl")]         //Uses Gpio pin numbers  
		public static extern void pullUpDnControl(int pin, int pud);

		//This pwm mode cannot be used when using GpioSys mode!!
		[DllImport("wiringPi.so", EntryPoint = "pwmWrite")]              //Uses Gpio pin numbers
		public static extern void pwmWrite(int pin, int value);

		[DllImport("wiringPi.so", EntryPoint = "pwmSetMode")]             //Uses Gpio pin numbers
		public static extern void pwmSetMode(int mode);

		[DllImport("wiringPi.so", EntryPoint = "pwmSetRange")]             //Uses Gpio pin numbers
		public static extern void pwmSetRange(uint range);

		[DllImport("wiringPi.so", EntryPoint = "pwmSetClock")]             //Uses Gpio pin numbers
		public static extern void pwmSetClock(int divisor);

		[DllImport("wiringPi.so", EntryPoint = "gpioClockSet")]              //Uses Gpio pin numbers
		public static extern void ClockSetGpio(int pin, int freq);

		public enum GPIOpinmode
		{
			Input = 0,
			Output = 1,
			PWMOutput = 2,
			GPIOClock = 3,
			SoftPWMOutput = 4,
			SoftToneOutput = 5,
			PWMToneOutput = 6
		}

		public enum GPIOpinvalue
		{
			High = 1,
			Low = 0
		}

		public enum PullUpDnValue
		{
			Off = 0,
			Down = 1,
			Up = 2
		}
	}

}
