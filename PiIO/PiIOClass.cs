/************************************************************************************************
 * This wrapper class was written by Daniel J Riches for Gordon Hendersons PiIO C library   *
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
 * 23 Nov 2013  Gerhard de Clercq   Changed digitalread to return int and implemented PiIOISR
 * 
 ************************************************************************************************
 * Changelog
 * Date         Changed By          Details of change  
 * 18 Jan 2016  Marcus Lum          Updated imported methods to current PiIO 
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

namespace PiIO
{

	[Serializable]
	public enum Measures : int { Metric, Imperial }
	public enum EndianType : int { Big, Little }
	
	/// <summary>
    /// Used to initialise Gordon's library, there's 4 different ways to initialise and we're going to support all 4
    /// </summary>
    public class Init
    {
        [DllImport("wiringPi.so", EntryPoint = "wiringPiSetup")]     //This is an example of how to call a method / function in a c library from c#
        public static extern int Setup();

        [DllImport("wiringPi.so", EntryPoint = "wiringPiSetupGpio")]
        public static extern int SetupGpio();

        [DllImport("wiringPi.so", EntryPoint = "wiringPiSetupSys")]
        public static extern int SetupSys();

        [DllImport("wiringPi.so", EntryPoint = "wiringPiSetupPhys")]
        public static extern int SetupPhys();
    }
	/// <summary>
    /// Provides use of the Timing functions such as delays
    /// </summary>
    public class Timing
    {
        [DllImport("wiringPi.so", EntryPoint = "millis")]
        public static extern uint millis();

        [DllImport("wiringPi.so", EntryPoint = "micros")]
        public static extern uint micros();

        [DllImport("wiringPi.so", EntryPoint = "delay")]
        public static extern void delay(uint howLong);

        [DllImport("wiringPi.so", EntryPoint = "delayMicroseconds")]
        public static extern void delayMicroseconds(uint howLong);
    }
    /// <summary>
    /// Provides access to the Thread priority and interrupts for IO
    /// </summary>
    public class PiThreadInterrupts
    {
        [DllImport("wiringPi.so", EntryPoint = "piHiPri")]
        public static extern int piHiPri(int priority);

        [DllImport("wiringPi.so", EntryPoint = "waitForInterrupt")]
        public static extern int waitForInterrupt(int pin, int timeout);

        //This is the C# equivelant to "void (*function)(void))" required by PiIO to define a callback method
        public delegate void ISRCallback();

        [DllImport("wiringPi.so", EntryPoint = "PiIOISR")]
        public static extern int PiIOISR(int pin, int mode, ISRCallback method);

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
        [DllImport("wiringPi.so", EntryPoint = "piBoardRev")]
        public static extern int piBoardRev();

        [DllImport("wiringPi.so", EntryPoint = "wpiPinToGpio")]
        public static extern int wpiPinToGpio(int wPiPin);

        [DllImport("wiringPi.so", EntryPoint = "physPinToGpio")]
        public static extern int physPinToGpio(int physPin);

        [DllImport("wiringPi.so", EntryPoint = "setPadDrive")]
        public static extern int setPadDrive(int group, int value);
    }
    /// <summary>
    /// Provides access to the I2C port
    /// </summary>
 
}
