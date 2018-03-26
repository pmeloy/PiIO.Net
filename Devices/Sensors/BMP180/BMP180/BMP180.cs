using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WiringPi;



namespace WiringPi.Devices.Sensors
{
	public class BMP180
	{
		#region "Constants"
		// Power modes
		public const int BMP_ULTRALOWPOWER = 0;
		public const int BMP_STANDARDPOWER = 1;
		public const int BMP_HIGHRES = 2;
		public const int BMP_ULTRAHIGHRES = 3;

		const int ADDR_AC1 = 0xAA;
		const int ADDR_AC2 = 0xAC;
		const int ADDR_AC3 = 0xAE;
		const int ADDR_AC4 = 0xB0;
		const int ADDR_AC5 = 0xB2;
		const int ADDR_AC6 = 0xB4;
		const int ADDR_B1 = 0xB6;
		const int ADDR_B2 = 0xB8;
		const int ADDR_MB = 0xBA;
		const int ADDR_MC = 0xBC;
		const int ADDR_MD = 0xBE;

		const int ADDR_CONTROL = 0xF4;
		const int ADDR_TEMP = 0xF6;
		const int ADDR_PRESS = 0xF6;

		const int CMD_READ_TEMP = 0x2E;
		const int CMD_READ_PRESS = 0x34;
		#endregion

		#region "Variables"
		private int _bmpHandle;
		private int _overSampleMode;
		private const int MODE_ONE = 0;
		private const int MODE_TWO = 1;
		private const int MODE_FOUR = 2;
		private const int MODE_EIGHT = 3;
		private int _cal_AC1, _cal_AC2, _cal_AC3;
		private int _cal_AC4, _cal_AC5, _cal_AC6;
		private int _cal_B1, _cal_B2;
		private int _cal_MB, _cal_MC, _cal_MD;

		#endregion

		#region "Getters and Setters"
		public double TempC
		{
			get
			{
				return ReadTemp();
			}
		}
		public double PresshPa
		{
			get
			{
				return ReadPress() / 10d;
			}
		}
		#endregion

		//		static double c5, c6, mc, md, x0, x1, x2, yy0, yy1, yy2, p0, p1, p2;
		//static double cPress, cTemp;
		//static int altitude;

		public BMP180(int address = 0x77, int osMode = MODE_ONE)
		{
			if (osMode < 0) osMode = 0;
			if (osMode > BMP_ULTRAHIGHRES) osMode = BMP_ULTRAHIGHRES;

			_overSampleMode = osMode; //bmpMode; //Todo: Do something with this!
			_bmpHandle = I2C.Setup(address);
			LoadCalibration();
		}

		#region "Init functions"

		private void TestCalibrations()
		{
			_cal_AC1 = 408;
			_cal_AC2 = -72;
			_cal_AC3 = -14383;
			_cal_AC4 = 32741;
			_cal_AC5 = 32767;
			_cal_AC6 = 23153;
			_cal_B1 = 6190;
			_cal_B2 = 4;
			_cal_MB = -32768;
			_cal_MC = -8711;
			_cal_MD = 2868;
		}
		private void LoadCalibration()
		{

			_cal_AC1 = I2C.ReadS16(_bmpHandle, ADDR_AC1);
			Debug.Print("AC1 = 0x{0:X} ({0})", _cal_AC1);
			_cal_AC2 = I2C.ReadS16(_bmpHandle, ADDR_AC2);
			Debug.Print("AC2 = 0x{0:X} ({0})", _cal_AC2);
			_cal_AC3 = I2C.ReadS16(_bmpHandle, ADDR_AC3);
			Debug.Print("AC3 = 0x{0:X} ({0})", _cal_AC3);
			_cal_AC4 = I2C.ReadU16(_bmpHandle, ADDR_AC4);
			Debug.Print("AC4 = 0x{0:X} ({0})", _cal_AC4);
			_cal_AC5 = I2C.ReadU16(_bmpHandle, ADDR_AC5);
			Debug.Print("AC5 = 0x{0:X} ({0})", _cal_AC5);
			_cal_AC6 = I2C.ReadU16(_bmpHandle, ADDR_AC6);
			Debug.Print("AC6 = 0x{0:X} ({0})", _cal_AC6);
			_cal_B1 = I2C.ReadS16(_bmpHandle, ADDR_B1);
			Debug.Print("B1 = 0x{0:X} ({0})", _cal_B1);
			_cal_B2 = I2C.ReadS16(_bmpHandle, ADDR_B2);
			Debug.Print("B2 = 0x{0:X} ({0})", _cal_B2);
			_cal_MB = I2C.ReadS16(_bmpHandle, ADDR_MB);
			Debug.Print("MB = 0x{0:X} ({0})", _cal_MB);
			_cal_MC = I2C.ReadS16(_bmpHandle, ADDR_MC);
			Debug.Print("MC = 0x{0:X} ({0})", _cal_MC);
			_cal_MD = I2C.ReadS16(_bmpHandle, ADDR_MD);
			Debug.Print("MD = 0x{0:X} ({0})", _cal_MD);
		}
		#endregion

		#region "Helper Methods"

		#endregion

		#region "BMP Methods"
		private int ReadRawTemp()
		{

			int raw;
			I2C.WriteReg8(_bmpHandle, ADDR_CONTROL, CMD_READ_TEMP);
			Thread.Sleep(50);
			raw = I2C.ReadU16(_bmpHandle, ADDR_TEMP);

			return raw;
		}
		private int ReadRawPress()
		{
			int delay, msb, lsb, xlsb, raw;
			I2C.WriteReg8(_bmpHandle, ADDR_CONTROL, CMD_READ_PRESS | (_overSampleMode << 6));
			switch (_overSampleMode)
			{
				case (MODE_ONE):
					delay = 5;
					break;
				case (MODE_TWO):
					delay = 8;
					break;
				case (MODE_FOUR):
					delay = 14;
					break;
				case (MODE_EIGHT):
					delay = 26;
					break;
				default:
					delay = 5;
					break;
			}

			Thread.Sleep(delay);
			msb = I2C.ReadReg8(_bmpHandle, ADDR_PRESS);
			lsb = I2C.ReadReg8(_bmpHandle, ADDR_PRESS + 1);
			xlsb = I2C.ReadReg8(_bmpHandle, ADDR_PRESS + 2);
			raw = ((msb << 16) + (lsb << 8) + xlsb) >> (8 - _overSampleMode);
			return raw;
		}
		private double ReadTemp()
		{
			long UT, X1, X2, B5;
			double tempC;

			UT = ReadRawTemp();

			X1 = (UT - _cal_AC6) * _cal_AC5 >> 15;
			X2 = (_cal_MC << 11) / (X1 + _cal_MD);
			B5 = X1 + X2;
			Debug.Print("Temp {0}", (B5 + 8) / 16);
			tempC = ((B5 + 8) / 16) / 10d;
			return tempC;
		}
		private double ReadPress()
		{
			long X1, X2, X3, B3, B4, B5, B6, B7, p;
			long UT = ReadRawTemp();


			long UP = ReadRawPress();

			X1 = (UT - _cal_AC6) * _cal_AC5 >> 15;
			Debug.Print("X1 {0}", X1);
			X2 = (_cal_MC << 11) / (X1 + _cal_MD);
			Debug.Print("X2 {0}", X2);
			B5 = X1 + X2;
			Debug.Print("B5 {0}", B5);
			Debug.Print("T {0}\n", (B5 + 8) / 16);

			B6 = B5 - 4000;
			Debug.Print("B6 {0}", B6);
			X1 = (_cal_B2 * (B6 * B6) >> 12) >> 11;
			Debug.Print("X1 {0}", X1);
			X2 = (_cal_AC2 * B6) >> 11;
			Debug.Print("X2 {0}", X2);
			X3 = X1 + X2;
			Debug.Print("X3 {0}", X3);
			B3 = (((_cal_AC1 * 4 + X3) << _overSampleMode) + 2) / 4;
			Debug.Print("B3 {0}", B3);
			X1 = _cal_AC3 * B6 >> 13;
			Debug.Print("X1 {0}", X1);
			X2 = (_cal_B1 * ((B6 * B6) >> 12)) >> 16;
			Debug.Print("X2 {0}", X2);
			X3 = ((X1 + X2) + 2) >> 2;
			Debug.Print("X3 {0}", X3);

			B4 = _cal_AC4 * (long)((ulong)(X3 + 32768)) >> 15;
			Debug.Print("B4 {0}", B4);
			B7 = ((long)(ulong)(UP - B3)) * (50000 >> _overSampleMode);
			Debug.Print("B7 {0}", B7);
			if (B7 < 0x80000000)
			{
				p = (B7 * 2) / B4;
			}
			else
			{
				p = (B7 / B4) * 2;
			}
			Debug.Print("P {0}\n", p);
			X1 = (p >> 8) * (p >> 8);
			Debug.Print("X1 {0}", X1);
			X1 = (X1 * 3038) >> 16;
			Debug.Print("X1 {0}", X1);
			X2 = (-7357 * p) >> 16;
			Debug.Print("X2 {0}", X2);
			p = p + (X1 + X2 + 3791) / 16;
			Debug.Print("P {0}", p);

			return p / 1d;
		}

		#endregion
	}
}