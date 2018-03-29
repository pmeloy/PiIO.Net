using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using PiIO;

namespace PiIO.I2C
{
	public class I2CCmd
	{
#region "Imports"
		[DllImport("wiringPi.so", EntryPoint = "wiringPiI2CSetup")]
		private static extern int wiringPiI2CSetup(int devId);

		[DllImport("wiringPi.so", EntryPoint = "wiringPiI2CRead")]
		private static extern int wiringPiI2CRead(int fd);

		[DllImport("wiringPi.so", EntryPoint = "wiringPiI2CWrite")]
		private static extern int wiringPiI2CWrite(int fd, int data);

		[DllImport("wiringPi.so", EntryPoint = "wiringPiI2CWriteReg8")]
		private static extern int wiringPiI2CWriteReg8(int fd, int reg, int data);

		[DllImport("wiringPi.so", EntryPoint = "wiringPiI2CWriteReg16")]
		private static extern int wiringPiI2CWriteReg16(int fd, int reg, int data);

		[DllImport("wiringPi.so", EntryPoint = "wiringPiI2CReadReg8")]
		private static extern int wiringPiI2CReadReg8(int fd, int reg);

		[DllImport("wiringPi.so", EntryPoint = "wiringPiI2CReadReg16")]
		private static extern int wiringPiI2CReadReg16(int fd, int reg);
#endregion


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
		/// <param name="deviceHandle">Device handle from PiIOI2CSetup()</param>
		/// <returns>int</returns>
		public static int ReadByte(int deviceHandle)
		{
			return wiringPiI2CRead(deviceHandle);
		}

		/// <summary>
		/// Read one byte from address on I2C device
		/// </summary>
		/// <param name="deviceHandle">Device handle from PiIOI2CSetup()</param>
		/// <param name="address">Address to read from</param>
		/// <returns></returns>
		public static int ReadReg8(int deviceHandle, int address)
		{
			return ReadReg8(deviceHandle, address);
		}

		/// <summary>
		///	Read unsigned 16 bit integer from I2C device
		/// </summary>
		/// <param name="fd">Device handle from PiIOI2CSetup()</param>
		/// <param name="address">Address of starting register</param>
		/// <param name="endian">Big or little endian</param>
		/// <returns>Unsigned Int</returns>
		public static int ReadRegU16(int deviceHandle, int address, EndianType endian = EndianType.Big)
		{
			int result = 0;
			int byte1, byte2;

			byte1 = ReadReg8(deviceHandle, address);
			byte2 = ReadReg8(deviceHandle, address + 1);

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
		/// <param name="deviceHandle">Device handle from PiIOI2CSetup()</param>
		/// <param name="address">Address of starting register</param>
		/// <param name="endian">Big or little endian</param>
		/// <returns>Signed int</returns>
		public static int ReadRegS16(int deviceHandle, int address, EndianType endian = EndianType.Big)
		{
			int result = 0;
			result = ReadRegU16(deviceHandle, address, endian);
			if (result > 32767) result -= 65536;
			return result;
		}

		/// <summary>
		/// Write byte to device
		/// </summary>
		/// <param name="deviceHandle">Device handle from PiIOI2CSetup()</param>
		/// <param name="value">Value to write</param>
		public static void WriteByte(int deviceHandle, int data)
		{
			data &= 0xff;
			WriteByte(deviceHandle, data);
		}

		/// <summary>
		/// Write byte to register on device
		/// </summary>
		/// <param name="deviceHandle">Device handle from PiIOI2CSetup()</param>
		/// <param name="address">Address of register</param>
		/// <param name="value">Value to write</param>
		public static void WriteReg8(int deviceHandle, int address, int data)
		{
			data &= 0xff;
			WriteReg8(deviceHandle, address, data);
		}

		/// <summary>
		/// Write two bytes to a device
		/// </summary>
		/// <param name="deviceHandle">Device handle from PiIOI2CSetup()</param>
		/// <param name="value">Value to write</param>
		public static void Write16(int deviceHandle, int data)
		{
			//todo: Write16 - two bytes without an address		
			data &= 0xffff;

		}

		/// <summary>
		/// Write two bytes to register on device
		/// </summary>
		/// <param name="deviceHandle">Device handle from PiIOI2CSetup()</param>
		/// <param name="address">Address of starting register</param>
		/// <param name="value">Value to write</param>
		public static void WriteReg16(int deviceHandle, int address, int data)
		{
			data &= 0xffff;
			WriteReg16(deviceHandle, address, data);
		}

	}
	/// <summary>
	///  Provides the ability to use the Software Tone functions in PiIO
	/// </summary>
}
