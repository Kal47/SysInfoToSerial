#include "U8g2lib.h"
#include <Wire.h>  //Library for I2C interface

U8G2_SH1106_128X64_NONAME_F_HW_I2C OLED_1(U8G2_R0, U8X8_PIN_NONE);
U8G2_SH1106_128X64_NONAME_F_HW_I2C OLED_2(U8G2_R0, U8X8_PIN_NONE);

char inputString[10] = { 100, 101, 102, 103, 104, 105, 42, 107, 108,109 };
bool stringComplete = true;
int inputStringIndex = 0;

void setup() {
  Serial.begin(9600);
  Serial.println("START");

  OLED_1.setI2CAddress(0x3C * 2);
  OLED_2.setI2CAddress(0x3D * 2);
  OLED_1.begin();
  OLED_2.begin();
  OLED_1.setFont(u8g2_font_t0_22b_tf     );
  OLED_2.setFont(u8g2_font_t0_22b_tf     );
}

void loop() {
  
  if (stringComplete) {
    stringComplete = false;
    drawOLED_1(); //ran in seperate functions because we dont have enough ram!
    drawOLED_2();
  }
}

void drawOLED_1(void) {
  char buffer[10];
  OLED_1.clearBuffer();  // clear the internal memory

  sprintf(buffer, "CPU: %d", inputString[0]);
  OLED_1.drawStr(0, 13, buffer);

  char strBuffer[4];
  float float_value = inputString[1];
  dtostrf(float_value / 10, 3, 1, strBuffer);
  sprintf(buffer, "CLK: %s", strBuffer);
  OLED_1.drawStr(0, 30, buffer);

  sprintf(buffer, "TMP: %d", inputString[2]);
  OLED_1.drawStr(0, 46, buffer);

  sprintf(buffer, "MEM: %d", inputString[3]);
  OLED_1.drawStr(0, 64, buffer);

  OLED_1.sendBuffer();
}

void drawOLED_2(void) {
  char buffer[10];
  OLED_2.clearBuffer();  // clear the internal memory

  sprintf(buffer, "GPU: %d", inputString[4]);
  OLED_2.drawStr(0, 13, buffer);

  char strBuffer[4];
  float float_value = inputString[5];
  dtostrf(float_value / 10, 3, 1, strBuffer);
  sprintf(buffer, "CLK: %s", strBuffer);
  OLED_2.drawStr(0, 30, buffer);

  sprintf(buffer, "TMP: %d", inputString[6]);
  OLED_2.drawStr(0, 46, buffer);
  
  sprintf(buffer, "MEM: %d", inputString[7]);
  OLED_2.drawStr(0, 64, buffer);
  
  OLED_2.sendBuffer();  // transfer internal memory to the display
}

void serialEvent() {
  while (Serial.available()) { //probobly should set this to a fixed length

    byte inChar = Serial.read() - 1; //removes the added one that was there to keep any 0 value from ending the read loop
    
    inputString[inputStringIndex] = inChar;
    inputStringIndex++;
    
    if (inChar == 0) {
      stringComplete = true; //flag screens to print
      
      Serial.flush(); //clean any extra crap from buffer
      Serial.println("---");
      for (int i = 0; i > inputStringIndex; i++ )
        Serial.println(inputString, DEC); //send back what we got for debug      
      inputStringIndex = 0;
      //return //I dont think we need this because of the serial flush

    }    
  }
}