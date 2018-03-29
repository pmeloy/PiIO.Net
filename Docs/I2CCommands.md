# I2C Command Reference #

Namespace PiIO.I2C

## Class I2CCmd ##
-----
### Setup(int) ###
**Returns** int - Error returns negative value

 Gets a Linux file handle for the supplied I2C device address which is then used to identify the device in all commands related to it.
 
_Example_
```C#
deviceHandle = I2CCmd.Setup(0x77);
if (deviceHandle < 0) MyErrorHandler();
```
-----
### ReadByte(int) ###
**Returns** int

Read a single byte from a device without specifying a register address. Some chips, like the PCF8574, have no registers so you just read from the chip.

_Example_
```C#
	myByte = I2CCmd.ReadByte(int deviceHandle);
```
-----
### ReadReg8(int, int) ###
**Returns int**

Read a single byte from specified address on the device.

_Example_
```C#
	myByte = I2CCmd.ReadByte(deviceHandle, registerAddress);
```
-----
### ReadRegU16(int, int, EndianType = EndianType.Big) ###
**Returns** Unsigned Int

Read two consecutive bytes starting at the supplied address, defaults to big endian

_Example_
```C#
	myByte = I2CCmd.ReadRegU16(deviceHandle, registerAddress, EndianType.BigEndian);
```
-----
### ReadRegS16(int, int, EndianType = EndianType.Big) ###
**Returns** Signed Int

Read two consecutive bytes starting at the supplied address, defaults to big endian

_Example_
```C#
	myByte = I2CCmd.ReadS16(deviceHandle, registerAddress, EndianType.BigEndian);
```
-----
### WriteByte(int, int) ###
**Returns** Void

Write single byte to a device without specifying a register address. Some chips, like the PCF8574, have no registers so you just write to the chip.

_Example_
```C#
	I2CCmd.WriteByte(deviceHandle, data);
```
-----
### Write16(int, int, int) ###
**Returns** Void

NOT IMPLEMENTED YET: Write two consecutive bytes to a device.

_Example_
```C#
	I2CCmd.WriteReg16(deviceHandle, data);
```
-----
### WriteReg16(int, int, int) ###
**Returns** Void

Write two consecutive bytes to a device starting at register address

_Example_
```C#
	I2CCmd.WriteReg16(deviceHandle, address, data);
```
-----
