#include "U8g2lib.h"
#include <Wire.h>  //Library for I2C interface

U8G2_SH1106_128X64_NONAME_F_HW_I2C OLED_1(U8G2_R0, U8X8_PIN_NONE);
U8G2_SH1106_128X64_NONAME_F_HW_I2C OLED_2(U8G2_R0, U8X8_PIN_NONE);

char inputString[8] = { 100, 101, 102, 103, 104, 105, 7 };
bool stringComplete = true;
int inputStringIndex = 0;

void setup() {
  Serial.begin(9600);
  Serial.println("START");

  OLED_1.setI2CAddress(0x3C * 2);
  OLED_2.setI2CAddress(0x3D * 2);
  OLED_1.begin();
  OLED_2.begin();
  OLED_1.setFont(u8g2_font_luBS18_te);
  OLED_2.setFont(u8g2_font_luBS18_te);
}

void loop() {
  
  if (stringComplete) {
    drawOLED_1();
    drawOLED_2();

    inputStringIndex = 0;
    stringComplete = false;
  }
}

void drawOLED_1(void) {
  char buffer[10];
  OLED_1.clearBuffer();  // clear the internal memory

  sprintf(buffer, "CPU: %d", inputString[0]);
  OLED_1.drawStr(0, 20, buffer);

  sprintf(buffer, "TMP: %d", inputString[1]);
  OLED_1.drawStr(0, 42, buffer);

  sprintf(buffer, "MEM: %d", inputString[2]);
  OLED_1.drawStr(0, 64, buffer);

  OLED_1.sendBuffer();
}

void drawOLED_2(void) {
  char buffer[10];
  OLED_2.clearBuffer();  // clear the internal memory

  sprintf(buffer, "GPU: %d", inputString[3]);
  OLED_1.drawStr(0, 20, buffer);

  sprintf(buffer, "TMP: %d", inputString[4]);
  OLED_1.drawStr(0, 42, buffer);

  sprintf(buffer, "MEM: %d", inputString[5]);
  OLED_1.drawStr(0, 64, buffer);
  OLED_2.sendBuffer();  // transfer internal memory to the display
}

void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    byte inChar = Serial.read() - 32;
    // add it to the inputString:
    inputString[inputStringIndex] = inChar;
    // if the incoming character is a newline, set a flag so the main loop can
    // do something about it:
    if (inChar == '\n') {
      stringComplete = true;
    }
    inputStringIndex++;
  }  
  Serial.println(inputString);
}