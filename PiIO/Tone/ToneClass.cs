using System.Runtime.InteropServices;

namespace PiIO.Tone
{
	public class ToneCmd
	{
		[DllImport("libPiIO.so", EntryPoint = "softToneCreate")]
		public static extern int softToneCreate(int pin);

		[DllImport("libPiIO.so", EntryPoint = "softToneWrite")]
		public static extern void softToneWrite(int pin, int freq);
	}

}
