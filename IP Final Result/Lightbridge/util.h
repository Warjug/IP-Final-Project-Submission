/**
 * This file contains utility functions for interaction with the arduino
 * @file
 */

#pragma once
#include <Arduino.h>
#include "enumlist.h"
/**
 * Contains utility functions for communication with the arduino
 */
namespace Util {
/**
 * Flash a led for a certain time
 * @param [in] led - the led to flash
 * @param [in] duration - the duration of the flash in miliseconds
 */
void flashLed(LED led, int duration = 250) {
  digitalWrite(led, HIGH);
  delay(duration);
  digitalWrite(led, LOW);
}

/**
 * Lock the input of keys to prevent the user from spamming by holding the
 * button
 */
bool bLock = false;

/**
 * If key is pressed, return a value, and then lock the block.
 * @returns the key pressed
 */
keyPress readKey() {
  // read both key values
  int key1 = digitalRead(PIN_KEY1);
  int key2 = digitalRead(PIN_KEY2);

  if (key1 == LOW) {
    if (!bLock) {
      bLock = true;
      return keyPress::KEY1;
    }
  } else if (key2 == LOW) {
    if (!bLock) {
      bLock = true;
      return keyPress::KEY2;
    }
  } else {
    bLock = false;
  }
  return keyPress::NO_KEY;
}

} // namespace Util
