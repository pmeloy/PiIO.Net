﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PiIO;
using PiIO.SPI;
using PiIO.GPIO;
using System.Threading;

namespace SPITest
{
    class OLEDDriver
    {
        #region SSD1306_commandname codes
        private const byte SSD1306_SETCONTRAST = 0x81;
        private const byte SSD1306_DISPLAYALLON_RESUME = 0xA4;
        private const byte SSD1306_DISPLAYALLON = 0xA5;
        private const byte SSD1306_NORMALDISPLAY = 0xA6;
        private const byte SSD1306_INVERTDISPLAY = 0xA7;
        private const byte SSD1306_DISPLAYOFF = 0xAE;
        private const byte SSD1306_DISPLAYON = 0xAF;
        private const byte SSD1306_SETDISPLAYOFFSET = 0xD3;
        private const byte SSD1306_SETCOMPINS = 0xDA;
        private const byte SSD1306_SETVCOMDETECT = 0xDB;
        private const byte SSD1306_SETDISPLAYCLOCKDIV = 0xD5;
        private const byte SSD1306_SETPRECHARGE = 0xD9;
        private const byte SSD1306_SETMULTIPLEX = 0xA8;
        private const byte SSD1306_SETLOWCOLUMN = 0x00;
        private const byte SSD1306_SETHIGHCOLUMN = 0x10;
        private const byte SSD1306_SETSTARTLINE = 0x40;
        private const byte SSD1306_MEMORYMODE = 0x20;
        private const byte SSD1306_COMSCANINC = 0xC0;
        private const byte SSD1306_COMSCANDEC = 0xC8;
        private const byte SSD1306_SEGREMAP = 0xA0;
        private const byte SSD1306_CHARGEPUMP = 0x8D;
        private const byte SSD1306_EXTERNALVCC = 0x1;
        private const byte SSD1306_SWITCHCAPVCC = 0x2;

        // Scrolling private static const bytes
        private const byte SSD1306_ACTIVATE_SCROLL = 0x2F;
        private const byte SSD1306_DEACTIVATE_SCROLL = 0x2E;
        private const byte SSD1306_SET_VERTICAL_SCROLL_AREA = 0xA3;
        private const byte SSD1306_RIGHT_HORIZONTAL_SCROLL = 0x26;
        private const byte SSD1306_LEFT_HORIZONTAL_SCROLL = 0x27;
        private const byte SSD1306_VERTICAL_AND_RIGHT_HORIZONTAL_SCROLL = 0x29;
        private const byte SSD1306_VERTICAL_AND_LEFT_HORIZONTAL_SCROLL = 0x2A;
        #endregion

        #region LCDBuffer
        //This large array is the memory of the oled lcd screen (128 x 64)
        private static byte[] LCDBuffer = 
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80,
            0x80, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x80, 0x80, 0xC0, 0xC0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x80, 0xC0, 0xE0, 0xF0, 0xF8, 0xFC, 0xF8, 0xE0, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x80, 0x80,
            0x80, 0x80, 0x00, 0x80, 0x80, 0x00, 0x00, 0x00, 0x00, 0x80, 0x80, 0x80, 0x80, 0x80, 0x00, 0xFF,
            0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x80, 0x80, 0x80, 0x80, 0x00, 0x00, 0x80, 0x80, 0x00, 0x00,
            0x80, 0xFF, 0xFF, 0x80, 0x80, 0x00, 0x80, 0x80, 0x00, 0x80, 0x80, 0x80, 0x80, 0x00, 0x80, 0x80,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x80, 0x00, 0x00, 0x8C, 0x8E, 0x84, 0x00, 0x00, 0x80, 0xF8,
            0xF8, 0xF8, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xF0, 0xE0, 0xE0, 0xC0, 0x80,
            0x00, 0xE0, 0xFC, 0xFE, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0xC7, 0x01, 0x01,
            0x01, 0x01, 0x83, 0xFF, 0xFF, 0x00, 0x00, 0x7C, 0xFE, 0xC7, 0x01, 0x01, 0x01, 0x01, 0x83, 0xFF,
            0xFF, 0xFF, 0x00, 0x38, 0xFE, 0xC7, 0x83, 0x01, 0x01, 0x01, 0x83, 0xC7, 0xFF, 0xFF, 0x00, 0x00,
            0x01, 0xFF, 0xFF, 0x01, 0x01, 0x00, 0xFF, 0xFF, 0x07, 0x01, 0x01, 0x01, 0x00, 0x00, 0x7F, 0xFF,
            0x80, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x7F, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x01, 0xFF,
            0xFF, 0xFF, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x03, 0x0F, 0x3F, 0x7F, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xE7, 0xC7, 0xC7, 0x8F,
            0x8F, 0x9F, 0xBF, 0xFF, 0xFF, 0xC3, 0xC0, 0xF0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFC, 0xFC, 0xFC,
            0xFC, 0xFC, 0xFC, 0xFC, 0xFC, 0xF8, 0xF8, 0xF0, 0xF0, 0xE0, 0xC0, 0x00, 0x01, 0x03, 0x03, 0x03,
            0x03, 0x03, 0x01, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00, 0x01, 0x03, 0x03, 0x03, 0x03, 0x01, 0x01,
            0x03, 0x01, 0x00, 0x00, 0x00, 0x01, 0x03, 0x03, 0x03, 0x03, 0x01, 0x01, 0x03, 0x03, 0x00, 0x00,
            0x00, 0x03, 0x03, 0x00, 0x00, 0x00, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
            0x03, 0x03, 0x03, 0x03, 0x03, 0x01, 0x00, 0x00, 0x00, 0x01, 0x03, 0x01, 0x00, 0x00, 0x00, 0x03,
            0x03, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x80, 0xC0, 0xE0, 0xF0, 0xF9, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x3F, 0x1F, 0x0F,
            0x87, 0xC7, 0xF7, 0xFF, 0xFF, 0x1F, 0x1F, 0x3D, 0xFC, 0xF8, 0xF8, 0xF8, 0xF8, 0x7C, 0x7D, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0x3F, 0x0F, 0x07, 0x00, 0x30, 0x30, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0xFE, 0xFE, 0xFC, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xE0, 0xC0, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x30, 0x30, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0xC0, 0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0x7F, 0x3F, 0x1F,
            0x0F, 0x07, 0x1F, 0x7F, 0xFF, 0xFF, 0xF8, 0xF8, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFE, 0xF8, 0xE0,
            0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFE, 0xFE, 0x00, 0x00,
            0x00, 0xFC, 0xFE, 0xFC, 0x0C, 0x06, 0x06, 0x0E, 0xFC, 0xF8, 0x00, 0x00, 0xF0, 0xF8, 0x1C, 0x0E,
            0x06, 0x06, 0x06, 0x0C, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0xFE, 0xFE, 0x00, 0x00, 0x00, 0x00, 0xFC,
            0xFE, 0xFC, 0x00, 0x18, 0x3C, 0x7E, 0x66, 0xE6, 0xCE, 0x84, 0x00, 0x00, 0x06, 0xFF, 0xFF, 0x06,
            0x06, 0xFC, 0xFE, 0xFC, 0x0C, 0x06, 0x06, 0x06, 0x00, 0x00, 0xFE, 0xFE, 0x00, 0x00, 0xC0, 0xF8,
            0xFC, 0x4E, 0x46, 0x46, 0x46, 0x4E, 0x7C, 0x78, 0x40, 0x18, 0x3C, 0x76, 0xE6, 0xCE, 0xCC, 0x80,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x01, 0x07, 0x0F, 0x1F, 0x1F, 0x3F, 0x3F, 0x3F, 0x3F, 0x1F, 0x0F, 0x03,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x0F, 0x00, 0x00,
            0x00, 0x0F, 0x0F, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x0F, 0x00, 0x00, 0x03, 0x07, 0x0E, 0x0C,
            0x18, 0x18, 0x0C, 0x06, 0x0F, 0x0F, 0x0F, 0x00, 0x00, 0x01, 0x0F, 0x0E, 0x0C, 0x18, 0x0C, 0x0F,
            0x07, 0x01, 0x00, 0x04, 0x0E, 0x0C, 0x18, 0x0C, 0x0F, 0x07, 0x00, 0x00, 0x00, 0x0F, 0x0F, 0x00,
            0x00, 0x0F, 0x0F, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x0F, 0x00, 0x00, 0x00, 0x07,
            0x07, 0x0C, 0x0C, 0x18, 0x1C, 0x0C, 0x06, 0x06, 0x00, 0x04, 0x0E, 0x0C, 0x18, 0x0C, 0x0F, 0x07,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00      //Last line
            };
        #endregion

        #region Font - 5 x 7 font pinched from somewhere
        private static readonly byte[] Font =
        {
            0x00, 0x00, 0x00, 0x00, 0x00, // Space
	        0x00, 0x00, 0x4f, 0x00, 0x00, // !
	        0x00, 0x03, 0x00, 0x03, 0x00, // "
	        0x14, 0x3e, 0x14, 0x3e, 0x14, // #
	        0x24, 0x2a, 0x7f, 0x2a, 0x12, // $
	        0x63, 0x13, 0x08, 0x64, 0x63, // %
	        0x36, 0x49, 0x55, 0x22, 0x50, // &
	        0x00, 0x00, 0x07, 0x00, 0x00, // '
	        0x00, 0x1c, 0x22, 0x41, 0x00, // (
	        0x00, 0x41, 0x22, 0x1c, 0x00, // )
	        0x0a, 0x04, 0x1f, 0x04, 0x0a, // *
	        0x04, 0x04, 0x1f, 0x04, 0x04, // +
	        0x50, 0x30, 0x00, 0x00, 0x00, // ,
	        0x08, 0x08, 0x08, 0x08, 0x08, // -
	        0x60, 0x60, 0x00, 0x00, 0x00, // .
	        0x00, 0x60, 0x1c, 0x03, 0x00, // /
	        0x3e, 0x41, 0x49, 0x41, 0x3e, // 0
	        0x00, 0x02, 0x7f, 0x00, 0x00, // 1
	        0x46, 0x61, 0x51, 0x49, 0x46, // 2
	        0x21, 0x49, 0x4d, 0x4b, 0x31, // 3
	        0x18, 0x14, 0x12, 0x7f, 0x10, // 4
	        0x4f, 0x49, 0x49, 0x49, 0x31, // 5
	        0x3e, 0x51, 0x49, 0x49, 0x32, // 6
	        0x01, 0x01, 0x71, 0x0d, 0x03, // 7
	        0x36, 0x49, 0x49, 0x49, 0x36, // 8
	        0x26, 0x49, 0x49, 0x49, 0x3e, // 9
	        0x00, 0x33, 0x33, 0x00, 0x00, // :
	        0x00, 0x53, 0x33, 0x00, 0x00, // ;
	        0x00, 0x08, 0x14, 0x22, 0x41, // <
	        0x14, 0x14, 0x14, 0x14, 0x14, // =
	        0x41, 0x22, 0x14, 0x08, 0x00, // >
	        0x06, 0x01, 0x51, 0x09, 0x06, // ?
	        0x3e, 0x41, 0x49, 0x15, 0x1e, // @
	        0x78, 0x16, 0x11, 0x16, 0x78, // A
	        0x7f, 0x49, 0x49, 0x49, 0x36, // B
	        0x3e, 0x41, 0x41, 0x41, 0x22, // C
	        0x7f, 0x41, 0x41, 0x41, 0x3e, // D
	        0x7f, 0x49, 0x49, 0x49, 0x49, // E
	        0x7f, 0x09, 0x09, 0x09, 0x09, // F
	        0x3e, 0x41, 0x41, 0x49, 0x7b, // G
	        0x7f, 0x08, 0x08, 0x08, 0x7f, // H
	        0x00, 0x00, 0x7f, 0x00, 0x00, // I
	        0x38, 0x40, 0x40, 0x41, 0x3f, // J
	        0x7f, 0x08, 0x08, 0x14, 0x63, // K
	        0x7f, 0x40, 0x40, 0x40, 0x40, // L
	        0x7f, 0x06, 0x18, 0x06, 0x7f, // M
	        0x7f, 0x06, 0x18, 0x60, 0x7f, // N
	        0x3e, 0x41, 0x41, 0x41, 0x3e, // O
	        0x7f, 0x09, 0x09, 0x09, 0x06, // P
	        0x3e, 0x41, 0x51, 0x21, 0x5e, // Q
	        0x7f, 0x09, 0x19, 0x29, 0x46, // R
	        0x26, 0x49, 0x49, 0x49, 0x32, // S
	        0x01, 0x01, 0x7f, 0x01, 0x01, // T
	        0x3f, 0x40, 0x40, 0x40, 0x7f, // U
	        0x0f, 0x30, 0x40, 0x30, 0x0f, // V
	        0x1f, 0x60, 0x1c, 0x60, 0x1f, // W
	        0x63, 0x14, 0x08, 0x14, 0x63, // X
	        0x03, 0x04, 0x78, 0x04, 0x03, // Y
	        0x61, 0x51, 0x49, 0x45, 0x43, // Z
	        0x00, 0x7f, 0x41, 0x00, 0x00, // [
	        0x03, 0x1c, 0x60, 0x00, 0x00, // / other way around
	        0x00, 0x41, 0x7f, 0x00, 0x00, // ]
	        0x0c, 0x02, 0x01, 0x02, 0x0c, // ^
	        0x40, 0x40, 0x40, 0x40, 0x40, // _
	        0x00, 0x01, 0x02, 0x04, 0x00, // `
	        0x20, 0x54, 0x54, 0x54, 0x78, // a
	        0x7f, 0x48, 0x44, 0x44, 0x38, // b
	        0x38, 0x44, 0x44, 0x44, 0x44, // c
	        0x38, 0x44, 0x44, 0x48, 0x7f, // d
	        0x38, 0x54, 0x54, 0x54, 0x18, // e
	        0x08, 0x7e, 0x09, 0x09, 0x00, // f
	        0x0c, 0x52, 0x52, 0x54, 0x3e, // g
	        0x7f, 0x08, 0x04, 0x04, 0x78, // h
	        0x00, 0x00, 0x7d, 0x00, 0x00, // i
	        0x00, 0x40, 0x3d, 0x00, 0x00, // j
	        0x7f, 0x10, 0x28, 0x44, 0x00, // k
	        0x00, 0x00, 0x3f, 0x40, 0x00, // l
	        0x7c, 0x04, 0x18, 0x04, 0x78, // m
	        0x7c, 0x08, 0x04, 0x04, 0x78, // n
	        0x38, 0x44, 0x44, 0x44, 0x38, // o
	        0x7f, 0x12, 0x11, 0x11, 0x0e, // p
	        0x0e, 0x11, 0x11, 0x12, 0x7f, // q
	        0x00, 0x7c, 0x08, 0x04, 0x04, // r
	        0x48, 0x54, 0x54, 0x54, 0x24, // s
	        0x04, 0x3e, 0x44, 0x44, 0x00, // t
	        0x3c, 0x40, 0x40, 0x20, 0x7c, // u
	        0x1c, 0x20, 0x40, 0x20, 0x1c, // v
	        0x1c, 0x60, 0x18, 0x60, 0x1c, // w
	        0x44, 0x28, 0x10, 0x28, 0x44, // x
	        0x46, 0x28, 0x10, 0x08, 0x06, // y
	        0x44, 0x64, 0x54, 0x4c, 0x44, // z
	        0x00, 0x08, 0x77, 0x41, 0x00, // {
	        0x00, 0x00, 0x7f, 0x00, 0x00, // |
	        0x00, 0x41, 0x77, 0x08, 0x00, // }
	        0x10, 0x08, 0x18, 0x10, 0x08  // ~

        };
		#endregion

		public static int RunTest()
		{
			//Init PiIO library
			int result = Init.Setup();

			if (result == -1)
			{
				Console.WriteLine("PiIO init failed!");
				return result;
			}

			//Init PiIO SPI library
			result = SPICmd.Setup(0, 20000000); ;
			if (result == -1)
			{
				Console.WriteLine("SPI init failed!");
				return result;
			}

			Console.WriteLine("SPI init completed, using channel 0 at 20MHz for OLED Display");

			//Now initialise the OLED device and write a hello world message on it!
			//GPIO4 is used for data/~command in 4 wire spi mode            
			GPIOCmd.pinMode(4, (int)GPIOCmd.GPIOpinmode.Output);
			GPIOCmd.pinMode(17, (int)GPIOCmd.GPIOpinmode.Output);

			InitDisplay();

			WriteCommand(SSD1306_INVERTDISPLAY);

			ShortDelay();

			WriteCommand(SSD1306_NORMALDISPLAY);

			ClearScreen();
			WriteScreen();

			ShortDelay();
			ShortDelay();

			SetScreen();
			WriteScreen();

			ShortDelay();
			ShortDelay();

			ClearScreen();
			WriteScreen();

			while (true)
			{
				//Write Hello World on display
				ShortDelay();
				ShortDelay();
				WriteText(" " + System.DateTime.Now.ToLongTimeString());
				ShortDelay();
				ShortDelay();
				WriteScreen();

				ShortDelay();
				ShortDelay();

				ShortDelay();
				ShortDelay();

				InitDisplay();
			}
		}

        private static void ShortDelay()
        {
            int i = 64;
            while (i != 0)
            {
                System.Diagnostics.Debug.Print("");
                i--;
            }
        }

        private static void ClearScreen()
        {
            int size = LCDBuffer.Length;
            int pointer = 0;
            while (pointer < size)
            {
                LCDBuffer[pointer] = 0;
                pointer++;
            }
        }

        private static void SetScreen()
        {
            int size = LCDBuffer.Length;
            int pointer = 0;
            while (pointer < size)
            {
                LCDBuffer[pointer] = 255;
                pointer++;
            }
        }

        private static void WriteTest()
        {
            //Character A - Test
            //LCDBuffer[0] = (byte)126;
            //LCDBuffer[1] = (byte)9;
            //LCDBuffer[2] = (byte)9;
            //LCDBuffer[3] = (byte)9;
            //LCDBuffer[4] = (byte)126;
            //LCDBuffer[5] = (byte)0;
                
            //Character 1 - Test 
            //LCDBuffer[6] = (byte)0;
            //LCDBuffer[7] = (byte)66;
            //LCDBuffer[8] = (byte)127;
            //LCDBuffer[9] = (byte)64;
            //LCDBuffer[10] = (byte)0;
            //LCDBuffer[11] = (byte)0;

            string s = "Hello 007!";        //H is ASCII 
            byte[] b = Encoding.ASCII.GetBytes(s);
            int CharNo = 0;

            foreach (byte a in b)
            {
                byte OffsetToWrite = (byte)(a - 16);
                Console.WriteLine("OffsetByte: " + OffsetToWrite.ToString());
                for (int loop = 0; loop < 6; loop++)
                {
                    LCDBuffer[CharNo + loop] = Font[OffsetToWrite + loop];
                }
                CharNo++;
            }
            
        }

        private static void SetPixel(int x, int y)
        {
            //first calc the sequential array address to set
            //There are 16 bytes wide by 64 high, so pixel 13 is in the 2nd byte, 5th bit
            int bitNumber = x % 8;
            int ColumnByte = (x / 8);
            int pointer = (y * 16) + ColumnByte;

            LCDBuffer[pointer] = LCDBuffer[pointer] |= (byte)(1 << bitNumber);
        }

        private static void ClearPixel(int x, int y)
        {
            //first calc the sequential array address to set
            //There are 16 bytes wide by 64 high, so pixel 13 is in the 2nd byte, 5th bit
            int bitNumber = x % 8;
            int ColumnByte = (x / 8);
            int pointer = (y * 16) + ColumnByte;

            LCDBuffer[pointer] = LCDBuffer[pointer] &= (byte)(~(1 << bitNumber));
        }

        private static void CursorHome()
        {
            WriteCommand(SSD1306_SETSTARTLINE | 0x0);            // line #0
        }

        private static void WriteScreen()
        {
            foreach (byte b in LCDBuffer)
            {
                WriteData(b);
                ShortDelay();
            }
        }

        private static void WriteCommand(byte command)
        {
            //set D/C pin 7 low for command
            GPIOCmd.digitalWrite(4, 0);

            WriteByte(command);
            ShortDelay();
        }

        private static void WriteData(byte data)
        {
            //set D/C pin 7 high for data
            GPIOCmd.digitalWrite(4, 1);

            WriteByte(data);

        }

        private static bool InitDisplay()
        {
            bool result = false;

            //Reset first!!
            GPIOCmd.digitalWrite(17, 0);
            ShortDelay();
            GPIOCmd.digitalWrite(17, 1);   //release reset
            ShortDelay();

            // Init sequence for 128x64 OLED module
            WriteCommand(SSD1306_DISPLAYOFF);                    // 0xAE
            WriteCommand(SSD1306_SETDISPLAYCLOCKDIV);            // 0xD5
            WriteCommand(0x80);                                  // the suggested ratio 0x80
            WriteCommand(SSD1306_SETMULTIPLEX);                  // 0xA8
            WriteCommand(0x3F);
            WriteCommand(SSD1306_SETDISPLAYOFFSET);              // 0xD3
            WriteCommand(0x0);                                   // no offset
            WriteCommand(SSD1306_SETSTARTLINE | 0x0);            // line #0
            WriteCommand(SSD1306_CHARGEPUMP);                    // 0x8D
            WriteCommand(0x14);                                  //I always use the onboard switcher
            WriteCommand(SSD1306_MEMORYMODE);                    // 0x20
            WriteCommand(0x00);                                  // 0x0 act like ks0108
            WriteCommand(SSD1306_SEGREMAP | 0x1);
            WriteCommand(SSD1306_COMSCANDEC);
            WriteCommand(SSD1306_SETCOMPINS);                    // 0xDA
            WriteCommand(0x12);
            WriteCommand(SSD1306_SETCONTRAST);                   // 0x81
            WriteCommand(0xCF);                                  //I always use the onboard switcher
            WriteCommand(SSD1306_SETPRECHARGE);                  // 0xd9
            WriteCommand(0xF1);                              //I always use the onboard switcher
            WriteCommand(SSD1306_SETVCOMDETECT);                 // 0xDB
            WriteCommand(0x40);
            WriteCommand(SSD1306_DISPLAYALLON_RESUME);           // 0xA4
            WriteCommand(SSD1306_NORMALDISPLAY);                 // 0xA6

            //Finally turn on the display
            WriteCommand(SSD1306_DISPLAYON);

            //WriteCommand(SSD1306_DISPLAYALLON);                   //Screen should be white here!!
            return result;
        }

        /// <summary>
        /// Shows text on the screen at position x, y
        /// </summary>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void WriteText(string text)
        {
            byte[] b = Encoding.ASCII.GetBytes(text);
            int CharNo = 0;

            foreach (byte a in b)
            {
                int OffsetToWrite = ((a - 32) * 5);
                for (int loop = 0; loop < 6; loop++)
                {
                    if (loop < 5)
                        LCDBuffer[(CharNo * 6) + loop] = Font[OffsetToWrite + loop];
                    else
                        LCDBuffer[(CharNo * 6) + loop + 1] = 0;
                }
                CharNo++;
            }
        }

        /// <summary>
        /// Writes a single byte out to the spi interface
        /// </summary>
        /// <param name="data"></param>
        private static void WriteByte(byte data)
        {
            byte[] buffer = new byte[1];

            buffer[0] = data;

            //a bit of unsafe fixed memory pointer action going on here, make sure the array you're pointing to is the right size!!!
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    // Do all pointer work, ie external calls within the fixed area. The gc or clr wont try to move the object in memory while we use it.
                    SPICmd.DataRW(0, p, 1); //0 is SPI channel, p is byte array, 1 is number of bytes
                }
            }
        }

        /// <summary>
        /// Writes the supplied byte and reads a single byte which is returned
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte WriteAndReadByte(byte data)
        {
            byte[] buffer = new byte[1];

            buffer[0] = data;

            //a bit of unsafe fixed memory pointer action going on here, make sure the array you're pointing to is the right size!!!
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    // Do all pointer work, ie external calls within the fixed area. The gc or clr wont try to move the object in memory while we use it.
                    SPICmd.DataRW(0, p, 1); //0 is SPI channel, p is byte array, 1 is number of bytes
                }
            }

            return buffer[0];
        }
    }
}
