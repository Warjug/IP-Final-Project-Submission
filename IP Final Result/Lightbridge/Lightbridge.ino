/*
   Name: Vault Assignment
   Date: 20/09/2019
   Author: Kenneth Luper
*/

#include "Display.h"
#include "enumlist.h"
#include "util.h"
#include <Streaming.h>

const int LDR_IN = A2;
int count = 0;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);

  // input setup for buttons
  pinMode(PIN_KEY1, INPUT_PULLUP);
  pinMode(PIN_KEY2, INPUT_PULLUP);

  // output for green LED and red LED
  pinMode(GREEN_LED, OUTPUT);
  //pinMode(RED_LED, OUTPUT);

  //light sensor input
  pinMode(LDR_IN, INPUT);

  // onboard 4digitLED display
  Display.show(count);
  Serial.println(count);
}


bool pizzaReady = false;

/**
   while loop for brightness
   when ldr detects obstruction led turns on
   buzzer beeps
*/
void loop() {
  // delcaring variables
  int pizzaBelt = 200;
  const int brightness = analogRead(LDR_IN);
  delay(250);

  //if brightness is low, LED lights up
  if (brightness < pizzaBelt) {
    if (!pizzaReady) {
      pizzaReady = true;
      pizzaBelt = brightness;
      Util::flashLed(GREEN_LED);
      Serial << "Pizza Is Ready: " << brightness << "\n";
    }
  } else {
    pizzaReady = false;
    Serial << "Your order is on the way" << brightness << "\n";
  }
}
