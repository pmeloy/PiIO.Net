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
		public enum EndianType { Big, Little }
		[DllImport("libPiIO.so", EntryPoint = "PiIOI2CSetup")]
		private static extern int PiIOI2CSetup(int devId);

		[DllImport("libPiIO.so", EntryPoint = "PiIOI2CRead")]
		private static extern int PiIOI2CRead(int fd);

		[DllImport("libPiIO.so", EntryPoint = "PiIOI2CWrite")]
		private static extern int PiIOI2CWrite(int fd, int data);

		[DllImport("libPiIO.so", EntryPoint = "PiIOI2CWriteReg8")]
		private static extern int PiIOI2CWriteReg8(int fd, int reg, int data);

		[DllImport("libPiIO.so", EntryPoint = "PiIOI2CWriteReg16")]
		private static extern int PiIOI2CWriteReg16(int fd, int reg, int data);

		[DllImport("libPiIO.so", EntryPoint = "PiIOI2CReadReg8")]
		private static extern int PiIOI2CReadReg8(int fd, int reg);

		[DllImport("libPiIO.so", EntryPoint = "PiIOI2CReadReg16")]
		private static extern int PiIOI2CReadReg16(int fd, int reg);

		/// <summary>
		/// Register I2C Device
		/// </summary>
		/// <param name="I2CAddress">I2C Address of the device</param>
		/// <returns>Device Handle</returns>
		public static int Setup(int I2CAddress)
		{
			return PiIOI2CSetup(I2CAddress);
		}

		/// <summary>
		/// Read one byte from I2C device
		/// </summary>
		/// <param name="deviceHandle">Device handle from PiIOI2CSetup()</param>
		/// <returns>int</returns>
		public static int ReadByte(int deviceHandle)
		{
			return PiIOI2CRead(deviceHandle);
		}

		/// <summary>
		/// Read one byte from address on I2C device
		/// </summary>
		/// <param name="deviceHandle">Device handle from PiIOI2CSetup()</param>
		/// <param name="address">Address to read from</param>
		/// <returns></returns>
		public static int ReadReg8(int deviceHandle, int address)
		{
			return PiIOI2CReadReg8(deviceHandle, address);
		}

		/// <summary>
		///	Read unsigned 16 bit integer from I2C device
		/// </summary>
		/// <param name="fd">Device handle from PiIOI2CSetup()</param>
		/// <param name="address">Address of starting register</param>
		/// <param name="endian">Big or little endian</param>
		/// <returns>Unsigned Int</returns>
		public static int ReadU16(int deviceHandle, int address, EndianType endian = EndianType.Big)
		{
			int result = 0;
			int byte1, byte2;

			byte1 = PiIOI2CReadReg8(deviceHandle, address);
			byte2 = PiIOI2CReadReg8(deviceHandle, address + 1);

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
		/// <param name="deviceHandle">Device handle from PiIOI2CSetup()</param>
		/// <param name="value">Value to write</param>
		public static void WriteByte(int deviceHandle, int data)
		{
			data &= 0xff;
			PiIOI2CWrite(deviceHandle, data);
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
			PiIOI2CWriteReg8(deviceHandle, address, data);
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
			PiIOI2CWriteReg16(deviceHandle, address, data);
		}

	}
	/// <summary>
	///  Provides the ability to use the Software Tone functions in PiIO
	/// </summary>
}
