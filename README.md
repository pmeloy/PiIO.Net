# PiIO.Net # Work in progress! Not for consumption yet.
I was searching for a way to use Winforms and C# on the Pi with Mono and found several projects aimed at this goal. All seemed to not be in a usable state with compilation errors and slow or non-responsive authors. Then I ran across danriches/WiringPi.Net and it worked perfectly after a bit of investigation into how to get the original C wiringPi.o onto my Pi.

When I looked at the code I realized what danriches meant when he said it was a "simple" wrapper. There wasn't much more there than imports from the C library so it was a great starting place for me. I could have just written my own wrapper using the danriches source for a tutorial buy why bother when he'd already done it?

So using WiringPi.Net as a starting point I'm building PiIO.net. The goal is to have a complete yet still simple Pi IO library for C# (and probably VB but I haven't investigated that yet).

The original wrapper had a single namespace for everything which doesn't fit with my idea of how to do things so I've split that into several like PiIO.I2C, PiIO.GPIO, and the like. This lets me add devices under the appropriate namespace. What I don't see is any hardware PWM but I'll investigate the C wiringPi methods and add wrappers/classes for that as well.

Right now I'm working in I2C and have the following
PiIO.I2C.I2CCmd - 8 and 16 bit read and write operations with endian and sign
PiIO.I2C.Devices.Sensors.Barometric.BMPx80 a class to use the BMP family barometric sensors

I'll be adding more devices shortly (the ones I have on hand) like
- PiIO.I2C.Devices.ADC.ADS1x15
- PiIO.I2C.Devices.Temperature.DeviceWhichNameICantRememberAtTheMoment
- PiIO.I2C.Devices.Humidity.Si7021

I've also got some SPI devices which I can't remember the name of but I'll add those as well.

### Prerequisites ###
- A .NET Ide like Visual Studio or MonoDevelop.
- The original C wiringPi source installed on your Pi (http://wiringpi.com/)

### Installing ###
Download the pmeloy/PiIO.Net repository to your development PC or Pi then choose either
adding the PiIO.Net project to your solution or just use the PiIO.Net.dll. For detailed
instructions on adding a reference see "AddingReference.txt".

I was unable to find wiringPi.o in the default Raspbian distro so I removed PiIO with apt and
downloaded/built from the wiringPi site.

Download the original wiringPi to your Pi (not PC) and follow the instructions to build it.

You'll have to create a shared library from the wiringPi.o (in /wiringPi/wiringPi) with
	
	cc -shared PiIO.o -o libPiIO.so

NOTE: The original WiringPi.Net Readme shows three files that had to be copied but now it's only the one.

At this point you should have:
* wiringPi built on your Pi and the shared library created.
* pmeloy/PiIO.Net downloaded to your PC or Pi
* A reference to PiIO.Net added to your project and set Copy Local to True
* Optionally PiIO.Net source added to your solution

### Remote Deploy and Debug ###
See RemoteDeployDebug.txt for tips on how to streamline development. I prefer keeping all projects on
the Pi so I can avoid manually copying and debug remotely.

### License ###
GNU GPL in keeping with danriches original license.

### Authors ###
Just me (well, the parts that I changed).

### Acknowledgements ###
* The original wiringPi author since this wouldn't exist if that didn't!
* danriches for the code to build upon
* Adafruit and wiringPi for the example code (in the wrong languages) that let me figure out how to use WiringPi.NET
	in the first place.
