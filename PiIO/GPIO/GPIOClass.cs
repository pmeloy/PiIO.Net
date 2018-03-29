using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PiIO.GPIO
{
	public class GPIOCmd
	{
		[DllImport("libPiIO.so", EntryPoint = "pinMode")]           //Uses Gpio pin numbers
		public static extern void pinMode(int pin, int mode);

		[DllImport("libPiIO.so", EntryPoint = "digitalWrite")]      //Uses Gpio pin numbers
		public static extern void digitalWrite(int pin, int value);

		[DllImport("libPiIO.so", EntryPoint = "digitalWriteByte")]      //Uses Gpio pin numbers
		public static extern void digitalWriteByte(int value);

		[DllImport("libPiIO.so", EntryPoint = "digitalRead")]           //Uses Gpio pin numbers
		public static extern int digitalRead(int pin);

		[DllImport("libPiIO.so", EntryPoint = "pullUpDnControl")]         //Uses Gpio pin numbers  
		public static extern void pullUpDnControl(int pin, int pud);

		//This pwm mode cannot be used when using GpioSys mode!!
		[DllImport("libPiIO.so", EntryPoint = "pwmWrite")]              //Uses Gpio pin numbers
		public static extern void pwmWrite(int pin, int value);

		[DllImport("libPiIO.so", EntryPoint = "pwmSetMode")]             //Uses Gpio pin numbers
		public static extern void pwmSetMode(int mode);

		[DllImport("libPiIO.so", EntryPoint = "pwmSetRange")]             //Uses Gpio pin numbers
		public static extern void pwmSetRange(uint range);

		[DllImport("libPiIO.so", EntryPoint = "pwmSetClock")]             //Uses Gpio pin numbers
		public static extern void pwmSetClock(int divisor);

		[DllImport("libPiIO.so", EntryPoint = "gpioClockSet")]              //Uses Gpio pin numbers
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
