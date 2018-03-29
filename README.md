pmeloy/PiIO.Net
===================
A different take on WiringPi.Net with emphasis on functionality and ease of use.

I've departed from the original naming so a method like wiringPiReadReg8()
is now called ReadReg8(). Didn't see the point in doubling up on the name PiIO.

The danriches/WiringPi.Net I2C functions were simply exports from PiIO even though the 16 bit
methods were actually specific to device examples and lacking sign and endian options, and often
simply couldn't deal with data correctly. I've added wrappers for the wrappers!

I'm also reorganizing namespaces. Instead of everything being under the main namespace it will be split up by topic such as I2C and SPI. The non-specific classes and methods will stay in the main namespace.

As an example, the former WiringPi.I2C class will now be PiIO.I2C.I2CCmd so the using statement to get I2C commands will be "using PiIO.I2C". This lets me add devices to the namespace.

Prerequisites
-------------
- A .NET Ide like Visual Studio or MonoDevelop.
- The original PiIO installed on your Pi (http://PiIO.com/)

Installing
----------
Download the pmeloy/PiIO.Net repository to your development PC or Pi then choose either
adding the PiIO.Net project to your solution or just use the PiIO.Net.dll. For detailed
instructions on adding a reference see "AddingReference.txt".

I was unable to find PiIO.o in the default Raspbian distro so I removed PiIO with apt and
downloaded/built from the PiIO site.

Download the original PiIO to your Pi (not PC) and follow the instructions to build it.

You'll have to create a shared library from the PiIO.o (in /PiIO/PiIO) with
	
	cc -shared PiIO.o -o libPiIO.so

NOTE: The original PiIO.Net Readme shows three files that had to be copied but now it's only the one.

At this point you should have:
* PiIO built on your Pi and the shared library created.
* pmeloy/PiIO.Net downloaded to your PC or Pi
* A reference to PiIO.Net added to your project and set Copy Local to True
* Optionally PiIO.Net source added to your solution

Remote Deploy and Debug
-----------------------
See RemoteDeployDebug.txt for tips on how to streamline development. I prefer keeping all projects on
the Pi so I can avoid manually copying and debug remotely.

License
-------
GNU GPL in keeping with danriches original license.

Authors
-------
Just me (well, the parts that I changed).

Acknowledgements
----------------
* The original PiIO author since this wouldn't exist if that didn't!
* danriches for the code to build upon
* Adafruit and PiIO for the example code (in the wrong languages) that let me figure out how to use PiIO.NET
	in the first place.

EOF----------------------------------------------------------------------------------------------------------------

Original danriches ReadMe contents.
-----------------------------------

A simple C# wrapper for Gordon's PiIO library, also on GitHub. Please note this was only tested with
the hardfloat version of Raspbian using CrashOverrides mono build which can be found here: 
http://www.raspberrypi.org/phpBB3/viewtopic.php?f=34&t=37174&hilit=c%23+experimental 

Simply install Gordon's PiIO library on your Raspberry Pi and create the shared libraries as follows:

cc -shared PiIO.o -o libPiIO.so

cc -shared PiIOI2C.o -o libPiIOI2C.so

cc -shared PiIOSPI.o -o libPiIOSPI.so

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
