pmeloy/WiringPi.Net
===================
A different take on WiringPi.Net with emphasis on functionality and ease of use.

I've departed from the original WiringPi naming so methods like wiringPi.I2C.wiringPiReadReg8()
are now called wiringPi.I2C.ReadReg8. Didn't see the point in doubling up on the name wiringPi.

The danriches/WiringPi.Net I2C functions were simply exports from WiringPi even though the 16 bit
methods were actually specific to device examples, lacking sign and endian options, and often
simply couldn't deal with data correctly. I've added wrappers for the wrappers!



Prerequisites
-------------
- A .NET Ide like Visual Studio or MonoDevelop.
- The original WiringPi installed on your Pi (http://wiringpi.com/)

Installing
----------
Download the pmeloy/WiringPi.Net repository to your development PC (NOT the Pi!) then choose either
adding the WiringPi.Net project to your solution or just use the wiringPi.Net.dll. For detailed
instructions on adding a reference see "AddingReference.txt".

I was unable to find wiringpi.o in the default Raspbian distro so I removed wiringpi with apt and
downloaded/built from the wiringPi site.

Download the original wiringPi to your Pi (NOT the PC) and follow the instructions to build it.

You'll have to create a shared library from the wiringPi.o (in /wiringPi/wiringPi) with
	
	cc -shared wiringPi.o -o libwiringPi.so

NOTE: The original WiringPi.Net Readme shows three files that had to be copied but now it's only the one.

At this point you should have:
* wiringPi built on your Pi and the shared library created.
* pmeloy/WiringPi.Net downloaded to your PC
* A reference to WiringPi.Net added to your project and set Copy Local to True
* Optionally WiringPi.Net source added to your solution

Remote Deploy and Debug
-----------------------

You can build on the local machine and copy the files to the Pi but you cannot debug on the local machine
because there is no WiringPi library available. I find it easier to create a Samba share and have the solution
folder on the Pi. That way when I build the files are already on the Pi and I don't have to copy them manually.
Remember that WiringPi.Net needs to be on the PC and the original wiringPi on the Pi.

For remote debugging I use MonoRemoteDebugger (https://github.com/techl/MonoRemoteDebugger)

I use the MonoRemoteDebugger.Server.exe on the Pi (run with "mono MonoRemoteDebugger.Server.exe"). I don't detach
from the CLI, I just leave the window minimized and use a second one for everything else. The server will give you
tracebacks if there any exceptions.
For VisualStudio I use the MonoRemoteDebugger add-in from Microsoft Marketplace 
(https://marketplace.visualstudio.com/items?itemName=Bongho.MonoRemoteDebugger)

The two together let you deploy and do run-time debugging with a single menu item selection. What it doesn't do
is report build errors! So build normally first then choose "MonoRemoteDebugger->Debug with Mono (remote)".

License
-------
GNU GPL in keeping with danriches original license.

Authors
-------
Just me (well, the parts that I changed).

Acknowledgements
----------------
* The original wiringPi author since this wouldn't exist if that didn't!
* danriches for the code to build upon
* Adafruit and wiringPi for the example code (in the wrong languages) that let me figure out how to use WiringPi.NET
	in the first place.

EOF----------------------------------------------------------------------------------------------------------------

Original danriches ReadMe contents.
-----------------------------------

A simple C# wrapper for Gordon's WiringPi library, also on GitHub. Please note this was only tested with
the hardfloat version of Raspbian using CrashOverrides mono build which can be found here: 
http://www.raspberrypi.org/phpBB3/viewtopic.php?f=34&t=37174&hilit=c%23+experimental 

Simply install Gordon's WiringPi library on your Raspberry Pi and create the shared libraries as follows:

cc -shared wiringPi.o -o libwiringPi.so

cc -shared wiringPiI2C.o -o libwiringPiI2C.so

cc -shared wiringPiSPI.o -o libwiringPiSPI.so

Compile the project in Visual Studio 2010 and copy to your RasPi via FileZilla or some other SFTP client. Then 
run with: sudo mono SPITest.exe 
With nothing connected to the GPIO header you should see:

SPI init completed, using channel 0 at 32MHz for loopback testing
All zeros read back

If you short the MISO and MOSI pins together you should see:

SPI init completed, using channel 0 at 32MHz for loopback testing
Loopback is connected!

This is a different project to another .Net io library in that the SPI is hardware based and not software driven, 
the same with the I2C interface.

All praise should really go to Gordon@drogon for his great library which exposes all the required interfaces making 
my life and others that much easier. Cheers Gordon!!