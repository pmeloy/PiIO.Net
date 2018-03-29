using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PiIO.SPI
{
	/// <summary>
	/// Provides SPI port functionality
	/// </summary>
	public class SPICmd
	{
		/// <summary>
		/// Configures the SPI channel specified on the Raspberry Pi
		/// </summary>
		/// <param name="channel">Selects either Channel 0 or 1 for use</param>
		/// <param name="speed">Selects speed, 500,000 to 32,000,000</param>
		/// <returns>-1 for an error, or the linux file descriptor the channel uses</returns>
		[DllImport("libPiIO.so", EntryPoint = "PiIOSPISetup")]
		public static extern int Setup(int channel, int speed);

		/// <summary>
		/// Read and Write data over the SPI bus, don't forget to configure it first
		/// </summary>
		/// <param name="channel">Selects Channel 0 or Channel 1 for this operation</param>
		/// <param name="data">signed byte array pointer which holds the data to send and will then hold the received data</param>
		/// <param name="len">How many bytes to write and read</param>
		/// <returns>-1 for an error, or the linux file descriptor the channel uses</returns>
		[DllImport("libPiIO.so", EntryPoint = "PiIOSPIDataRW")]
		public static unsafe extern int DataRW(int channel, byte* data, int len);  //char is a signed byte
	}

}
