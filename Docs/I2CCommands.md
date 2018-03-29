# I2C Command Reference #

Namespace PiIO.I2C

## I2CCmd ##
### Setup() ###
**Returns int: Error returns negative value**
 Gets a Linux file handle for the supplied I2C device address which is then used to identify the device in all commands related to it.
_Example_
```C#
deviceHandle = I2CCmd.Setup(0x77);
if (deviceHandle < 0) MyErrorHandler();
```
### ReadByte() ###
Returns byte

Read a single byte from a device without specifying a register address. Some chips, like the PCF8574, have no registers so you just read from the chip.

Example
	myByte = I2CCmd.ReadByte(int deviceHandle);

ReadReg8()
----------
Returns byte

Read a single byte from specified address on the device.

Example
	myByte = I2CCmd.ReadByte(int deviceHandle, byte registerAddress, endian);

ReadReg16()
-----------
Returns int16

Read two consecutive bytes starting at the supplied address.
