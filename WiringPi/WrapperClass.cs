/************************************************************************************************
 * This wrapper class was written by Daniel J Riches for Gordon Hendersons WiringPi C library   *
 * I take no responsibility for this wrapper class providing proper functionality and give no   *
 * warranty of any kind, nor it's use or fitness for any purpose. You use this wrapper at your  *
 * own risk.                                                                                    *
 *                                                                                              *
 * This code is released as Open Source under GNU GPL license, please ensure that you have a    *
 * copy of the license and understand the usage terms and conditions.                           *
 *                                                                                              *
 * I take no credit for the underlying functionality that this wrapper provides.                *
 * Authored: 29/04/2013                                                                         *
 ************************************************************************************************
 * Changelog
 * Date         Changed By          Details of change
 * 08 May 2013  Daniel Riches       Corrected c library mappings for I2C and SPI, added this header
 * 
 ************************************************************************************************
 * Changelog
 * Date         Changed By          Details of change  
 * 23 Nov 2013  Gerhard de Clercq   Changed digitalread to return int and implemented wiringPiISR
 * 
 ************************************************************************************************
 * Changelog
 * Date         Changed By          Details of change  
 * 18 Jan 2016  Marcus Lum          Updated imported methods to current wiringPi 
 * 
 ************************************************************************************************
 * Changelog
 * Date         Changed By          Details of change  
 * 05 Jan 2017  Ilmar Kruis         Added PullUp/Down enum 
 *
 ************************************************************************************************
 * Changelog
 * Date         Changed By          Details of change
 * 14 Sep 2017  Daniel Riches       Added softTone support, tested with GPIO 18 only so far
 *
 ************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

namespace WiringPi
{
	[Serializable]
	public class Measurements
	{
		public enum Units : int { Metric, Imperial }
	}
    /// <summary>
    /// Used to initialise Gordon's library, there's 4 different ways to initialise and we're going to support all 4
    /// </summary>
    public class Init
    {
        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSetup")]     //This is an example of how to call a method / function in a c library from c#
        public static extern int WiringPiSetup();

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSetupGpio")]
        public static extern int WiringPiSetupGpio();

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSetupSys")]
        public static extern int WiringPiSetupSys();

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSetupPhys")]
        public static extern int WiringPiSetupPhys();
    }

    /// <summary>
    /// Used to configure a GPIO pin's direction and provide read & write functions to a GPIO pin
    /// </summary>
    public class GPIO
    {
        [DllImport("libwiringPi.so", EntryPoint = "pinMode")]           //Uses Gpio pin numbers
        public static extern void pinMode(int pin, int mode);

        [DllImport("libwiringPi.so", EntryPoint = "digitalWrite")]      //Uses Gpio pin numbers
        public static extern void digitalWrite(int pin, int value);

        [DllImport("libwiringPi.so", EntryPoint = "digitalWriteByte")]      //Uses Gpio pin numbers
        public static extern void digitalWriteByte(int value);

        [DllImport("libwiringPi.so", EntryPoint = "digitalRead")]           //Uses Gpio pin numbers
        public static extern int digitalRead(int pin);

        [DllImport("libwiringPi.so", EntryPoint = "pullUpDnControl")]         //Uses Gpio pin numbers  
        public static extern void pullUpDnControl(int pin, int pud);

        //This pwm mode cannot be used when using GpioSys mode!!
        [DllImport("libwiringPi.so", EntryPoint = "pwmWrite")]              //Uses Gpio pin numbers
        public static extern void pwmWrite(int pin, int value);

        [DllImport("libwiringPi.so", EntryPoint = "pwmSetMode")]             //Uses Gpio pin numbers
        public static extern void pwmSetMode(int mode);

        [DllImport("libwiringPi.so", EntryPoint = "pwmSetRange")]             //Uses Gpio pin numbers
        public static extern void pwmSetRange(uint range);

        [DllImport("libwiringPi.so", EntryPoint = "pwmSetClock")]             //Uses Gpio pin numbers
        public static extern void pwmSetClock(int divisor);

        [DllImport("libwiringPi.so", EntryPoint = "gpioClockSet")]              //Uses Gpio pin numbers
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

    public class SoftPwm {
        [DllImport("libwiringPi.so", EntryPoint = "softPwmCreate")]
        public static extern int Create(int pin, int initialValue, int pwmRange);

        [DllImport("libwiringPi.so", EntryPoint = "softPwmWrite")]
        public static extern void Write(int pin, int value);

        [DllImport("libwiringPi.so", EntryPoint = "softPwmStop")]
        public static extern void Stop(int pin);
    }

    /// <summary>
    /// Provides use of the Timing functions such as delays
    /// </summary>
    public class Timing
    {
        [DllImport("libwiringPi.so", EntryPoint = "millis")]
        public static extern uint millis();

        [DllImport("libwiringPi.so", EntryPoint = "micros")]
        public static extern uint micros();

        [DllImport("libwiringPi.so", EntryPoint = "delay")]
        public static extern void delay(uint howLong);

        [DllImport("libwiringPi.so", EntryPoint = "delayMicroseconds")]
        public static extern void delayMicroseconds(uint howLong);
    }

    /// <summary>
    /// Provides access to the Thread priority and interrupts for IO
    /// </summary>
    public class PiThreadInterrupts
    {
        [DllImport("libwiringPi.so", EntryPoint = "piHiPri")]
        public static extern int piHiPri(int priority);

        [DllImport("libwiringPi.so", EntryPoint = "waitForInterrupt")]
        public static extern int waitForInterrupt(int pin, int timeout);

        //This is the C# equivelant to "void (*function)(void))" required by wiringPi to define a callback method
        public delegate void ISRCallback();

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiISR")]
        public static extern int wiringPiISR(int pin, int mode, ISRCallback method);

        public enum InterruptLevels
        {
            INT_EDGE_SETUP = 0,
            INT_EDGE_FALLING = 1,
            INT_EDGE_RISING = 2,
            INT_EDGE_BOTH = 3
        }

        //static extern int piThreadCreate(string name);
    }

    public class MiscFunctions
    {
        [DllImport("libwiringPi.so", EntryPoint = "piBoardRev")]
        public static extern int piBoardRev();

        [DllImport("libwiringPi.so", EntryPoint = "wpiPinToGpio")]
        public static extern int wpiPinToGpio(int wPiPin);

        [DllImport("libwiringPi.so", EntryPoint = "physPinToGpio")]
        public static extern int physPinToGpio(int physPin);

        [DllImport("libwiringPi.so", EntryPoint = "setPadDrive")]
        public static extern int setPadDrive(int group, int value);
    }

    /// <summary>
    /// Provides SPI port functionality
    /// </summary>
    public class SPI
    {
        /// <summary>
        /// Configures the SPI channel specified on the Raspberry Pi
        /// </summary>
        /// <param name="channel">Selects either Channel 0 or 1 for use</param>
        /// <param name="speed">Selects speed, 500,000 to 32,000,000</param>
        /// <returns>-1 for an error, or the linux file descriptor the channel uses</returns>
        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSPISetup")]
        public static extern int wiringPiSPISetup(int channel, int speed);

        /// <summary>
        /// Read and Write data over the SPI bus, don't forget to configure it first
        /// </summary>
        /// <param name="channel">Selects Channel 0 or Channel 1 for this operation</param>
        /// <param name="data">signed byte array pointer which holds the data to send and will then hold the received data</param>
        /// <param name="len">How many bytes to write and read</param>
        /// <returns>-1 for an error, or the linux file descriptor the channel uses</returns>
        [DllImport("libwiringPi.so", EntryPoint = "wiringPiSPIDataRW")]
        public static unsafe extern int wiringPiSPIDataRW(int channel, byte* data, int len);  //char is a signed byte
    }

    /// <summary>
    /// Provides access to the I2C port
    /// </summary>
    public class I2C
    {
		public enum EndianType { Big, Little }
        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CSetup")]
        private static extern int wiringPiI2CSetup(int devId);

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CRead")]
        private static extern int wiringPiI2CRead(int fd);

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CWrite")]
        private static extern int wiringPiI2CWrite(int fd, int data);

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CWriteReg8")]
        private static extern int wiringPiI2CWriteReg8(int fd, int reg, int data);

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CWriteReg16")]
        private static extern int wiringPiI2CWriteReg16(int fd, int reg, int data);

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CReadReg8")]
        private static extern int wiringPiI2CReadReg8(int fd, int reg);

        [DllImport("libwiringPi.so", EntryPoint = "wiringPiI2CReadReg16")]
        private static extern int wiringPiI2CReadReg16(int fd, int reg);

		/// <summary>
		/// Register I2C Device
		/// </summary>
		/// <param name="I2CAddress">I2C Address of the device</param>
		/// <returns>Device Handle</returns>
		public static int Setup(int I2CAddress)
		{
			return wiringPiI2CSetup(I2CAddress);
		}

		/// <summary>
		/// Read one byte from I2C device
		/// </summary>
		/// <param name="deviceHandle">Device handle from wiringPiI2CSetup()</param>
		/// <returns>int</returns>
		public static int ReadByte(int deviceHandle)
		{
			return wiringPiI2CRead(deviceHandle);
		}

		/// <summary>
		/// Read one byte from address on I2C device
		/// </summary>
		/// <param name="deviceHandle">Device handle from wiringPiI2CSetup()</param>
		/// <param name="address">Address to read from</param>
		/// <returns></returns>
		public static int ReadReg8(int deviceHandle,int address)
		{
			return wiringPiI2CReadReg8(deviceHandle, address);
		}

		/// <summary>
		///	Read unsigned 16 bit integer from I2C device
		/// </summary>
		/// <param name="fd">Device handle from wiringPiI2CSetup()</param>
		/// <param name="address">Address of starting register</param>
		/// <param name="endian">Big or little endian</param>
		/// <returns>Unsigned Int</returns>
		public static int ReadU16(int deviceHandle, int address, EndianType endian = EndianType.Big)
		{
			int result = 0;
			int byte1, byte2;

			byte1 = wiringPiI2CReadReg8(deviceHandle, address);
			byte2 = wiringPiI2CReadReg8(deviceHandle, address + 1);

			if (endian == EndianType.Big)
			{
				result = byte1 * 256 + byte2;
			}
			else
			{
				result = byte2 * 256 + byte1;
			}
			return result;
		}

		/// <summary>
		/// Read signed 16 bit integer from I2C device
		/// </summary>
		/// <param name="deviceHandle">Device handle from wiringPiI2CSetup()</param>
		/// <param name="address">Address of starting register</param>
		/// <param name="endian">Big or little endian</param>
		/// <returns>Signed int</returns>
		public static int ReadS16(int deviceHandle, int address, EndianType endian = EndianType.Big)
		{
			int result = 0;
			result = ReadU16(deviceHandle, address, endian);
			if (result > 32767) result -= 65536;
			return result;
		}

		/// <summary>
		/// Write byte to device
		/// </summary>
		/// <param name="deviceHandle">Device handle from wiringPiI2CSetup()</param>
		/// <param name="value">Value to write</param>
		public static void WriteByte(int deviceHandle, int value)
		{
			value &= 0xff;
			I2C.wiringPiI2CWrite(deviceHandle, value);
		}

		/// <summary>
		/// Write byte to register on device
		/// </summary>
		/// <param name="deviceHandle">Device handle from wiringPiI2CSetup()</param>
		/// <param name="address">Address of starting register</param>
		/// <param name="value">Value to write</param>
		public static void WriteReg8(int deviceHandle, int address, int value)
		{
			value &= 0xff;
			I2C.wiringPiI2CWriteReg8(deviceHandle, address, value);
		}
	}

	/// <summary>
	///  Provides the ability to use the Software Tone functions in WiringPi
	/// </summary>
	public class Tone
    {
        [DllImport("libwiringPi.so", EntryPoint = "softToneCreate")]
        public static extern int softToneCreate(int pin);

        [DllImport("libwiringPi.so", EntryPoint = "softToneWrite")]
        public static extern void softToneWrite(int pin, int freq);
    }
}
