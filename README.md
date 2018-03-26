pmeloy/WiringPi.Net
===================
A different take on WiringPi.Net with emphasis on functionality and ease of use.

I've departed from the original WiringPi naming so methods like wiringPi.I2C.wiringPiReadReg8()
are now called wiringPi.I2C.ReadReg8. Didn't see the point in doubling up on the name wiringPi.

The danriches/WiringPi.Net I2C functions were simply exports from WiringPi even though the 16 bit
methods were actually specific to device examples, lacking sign and endian options, and often
simply couldn't deal with data correctly. I've added wrappers for the wrappers!

- Getting Started
Download the repository then choose either adding the WiringPi.Net project to your solution or just
use the wiringPi.Net.dll. For detailed instructions on adding a reference see "AddingReference.txt






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