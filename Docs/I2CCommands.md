# I2C Command Reference #

Namespace PiIO.I2C

## Class I2CCmd ##

### Setup(int) ###
**Returns** int - Error returns negative value

 Gets a Linux file handle for the supplied I2C device address which is then used to identify the device in all commands related to it.
 
_Example_
```C#
deviceHandle = I2CCmd.Setup(0x77);
if (deviceHandle < 0) MyErrorHandler();
```
### ReadByte(int) ###
**Returns** int

Read a single byte from a device without specifying a register address. Some chips, like the PCF8574, have no registers so you just read from the chip.

_Example_
```C#
	myByte = I2CCmd.ReadByte(int deviceHandle);
```

### ReadReg8(int, int) ###
----------
**Returns int**

Read a single byte from specified address on the device.

_Example_
```C#
	myByte = I2CCmd.ReadByte(int deviceHandle, int registerAddress);
```

### ReadReg16(int, int, EndianType = EndianType.Big) ###
-----------
Returns int16

Read two consecutive bytes starting at the supplied address, defaults to big endian

_Example_
```C#
	myByte = I2CCmd.ReadByte(int deviceHandle, int registerAddress);
```
