# System Info To Serial
> This aplication uses LibreHardwareMonitor to collect information about your system and sends it over a serial port.
> An Arduino Nano or compatable board receves the data and displays it on a SSD1306 OLED display.
> Also includes a WebSocketServer for remote monitoring at port 8080

## Usage example

The application needs administrator privlages to get the hardware info from LibreHardwareMonitor

It will create a icon on you notificaion area of your task bar
    Controls include
        Turning on and off the Serial Port Connection
        Choosing the Port
        Turning on and off the Web Socket Server

## Arduino Setup

Note: This uses one I2C bus, you will need two displays with diffrent adresses
    The ones found had a surface mount resister that could be moved to change the adress


Uses the U8g2lib display library

A3 to SDL
A4 to SDA

![Alt Text](https://raw.githubusercontent.com/Kal47/SysInfoToSerial/main/SysInfoToSerialArduino/Wireing.JPG))


