#include <FastLED.h>

#define LED_PIN 7
#define NUM_LEDS 35
#define updateLEDS 1

bool ledState = false;
bool firstTimePulseEffect = true;
bool firstTimeRGBEffect = true;
int RGBColors[3] = {255, 0, 0};
int RGBColorsStatic[3] = {255, 0, 0};
String cmd = "10", lastCmd = "10";

CRGB leds[NUM_LEDS];

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);

  FastLED.addLeds<WS2812B, LED_PIN, GRB>(leds, NUM_LEDS);

  for (int i = 0; i < NUM_LEDS; i++) {
    leds[i] = CRGB(0, 0, 0);
    delay(1);
  }

  FastLED.show();
}

void loop() {
  if (Serial.available() > 0) {
    cmd = Serial.readString();
  }
  else {
    cmd = lastCmd;
  }

  // State of the leds on/off
  if (cmd[0] == '1') {
    if (cmd[1] == '1') {
      for (int i = 0; i < NUM_LEDS; i++) {
        leds[i] = CRGB(RGBColors[0], RGBColors[1], RGBColors[2]);
      }
      ledState = true;
    }
    else {
      for (int i = 0; i < NUM_LEDS; i++) {
        leds[i] = CRGB(0, 0, 0);
      }
      ledState = false;
      FastLED.show();
    }
  }
  // Color of the leds
  else if (cmd[0] == '2' && ledState) {
    String rs = "", gs = "", bs = "";
    for (int i = 1; i < 10; i++) {
      if (i != 1 || i != 4 || i != 7) {
        if (i < 4) {
          rs += cmd[i];
        }
        else if (i < 7 && i > 3) {
          gs += cmd[i];
        }
        else {
          bs += cmd[i];
        }
      }
      else if (cmd[i] != '0') {
        if (i == 1) {
          rs += cmd[i];
        }
        else if (i == 4) {
          gs += cmd[i];
        }
        else {
          bs += cmd[i];
        }
      }
    }

    RGBColors[0] = rs.toInt();
    RGBColors[1] = gs.toInt();
    RGBColors[2] = bs.toInt();
    RGBColorsStatic[0] = RGBColors[0];
    RGBColorsStatic[1] = RGBColors[1];
    RGBColorsStatic[2] = RGBColors[2];
  }
  // Effect of the leds
  else if (cmd[0] == '3' && ledState) {
    // Rainbow effect
    if (cmd[1] == '1') {
      if (RGBColors[0] == 255 && RGBColors[1] == 0 && RGBColors[2] == 0) {
        RGBColors[1] = 255;
      }
      else if (RGBColors[0] == 255 && RGBColors[1] == 255 && RGBColors[2] == 0) {
        RGBColors[0] = 0;
      }
      else if (RGBColors[0] == 0 && RGBColors[1] == 255 && RGBColors[2] == 0) {
        RGBColors[2] = 255;
      }
      else if (RGBColors[0] == 0 && RGBColors[1] == 255 && RGBColors[2] == 255) {
        RGBColors[1] = 0;
      }
      else if (RGBColors[0] == 0 && RGBColors[1] == 0 && RGBColors[2] == 255) {
        RGBColors[0] = 255;
      }
      else if (RGBColors[0] == 255 && RGBColors[1] == 0 && RGBColors[2] == 255) {
        RGBColors[2] = 0;
      }
      else {
        RGBColors[0] = 255;
        RGBColors[1] = 0;
        RGBColors[2] = 0;
      }
      delay(60);
    }
    // Pulse effect
    else if (cmd[1] == '2') {
      // WHITE
      if (cmd[2] == 'a') {
        if (firstTimePulseEffect || RGBColors[0] != RGBColors[1] || RGBColors[0] != RGBColors[2] || RGBColors[1] != RGBColors[2]) {
          RGBColors[0] = 255;
          RGBColors[1] = 255;
          RGBColors[2] = 255;
        }
        else {
          if (RGBColors[0] == 0 && RGBColors[1] == 0 && RGBColors[2] == 0) {
            RGBColors[0] = 255;
            RGBColors[1] = 255;
            RGBColors[2] = 255;
          }
          else {
            RGBColors[0] = RGBColors[0] - 1;
            RGBColors[1] = RGBColors[1] - 1;
            RGBColors[2] = RGBColors[2] - 1;
          }
        }
      }
      // RED
      else if (cmd[2] == 'b') {
        if (firstTimePulseEffect || RGBColors[1] != 0 || RGBColors[2] != 0) {
          RGBColors[0] = 255;
          RGBColors[1] = 0;
          RGBColors[2] = 0;
        }
        else {
          if (RGBColors[0] == 0) {
            RGBColors[0] = 255;
          }
          else {
            RGBColors[0]--;
          }
        }
      }
      // GREEN
      else if (cmd[2] == 'c') {
        if (firstTimePulseEffect || RGBColors[0] != 0 || RGBColors[2] != 0) {
          RGBColors[0] = 0;
          RGBColors[1] = 255;
          RGBColors[2] = 0;
        }
        else {
          if (RGBColors[1] == 0) {
            RGBColors[1] = 255;
          }
          else {
            RGBColors[1]--;
          }
        }
      }
      // BLUE
      else if (cmd[2] == 'd') {
        if (firstTimePulseEffect || RGBColors[0] != 0 || RGBColors[1] != 0) {
          RGBColors[0] = 0;
          RGBColors[1] = 0;
          RGBColors[2] = 255;
        }
        else {
          if (RGBColors[2] == 0) {
            RGBColors[2] = 255;
          }
          else {
            RGBColors[2]--;
          }
        }
      }
      delay(10);
      firstTimePulseEffect = false;
    }
    // Static color effect
    else if (cmd[1] == '3') {
      RGBColors[0] = RGBColorsStatic[0];
      RGBColors[1] = RGBColorsStatic[1];
      RGBColors[2] = RGBColorsStatic[2];
    }
    // RGB scale
    else if (cmd[1] == '4') {
      if (firstTimeRGBEffect) {
        RGBColors[0] = 255;
        RGBColors[1] = 0;
        RGBColors[2] = 0;
        firstTimeRGBEffect = false;
      }
      else
      {
        if (RGBColors[0] == 255 && RGBColors[2] == 0 && RGBColors[1] < 255) {
          if (RGBColors[1] == 0) {
            delay(250);
          }
          RGBColors[1]++;
        }
        else if (RGBColors[1] == 255 && RGBColors[2] == 0 && RGBColors[0] > 0) {
          RGBColors[0]--;
        }
        else if (RGBColors[0] == 0 && RGBColors[1] == 255 && RGBColors[2] < 255) {
          RGBColors[2]++;
        }
        else if (RGBColors[0] == 0 && RGBColors[2] == 255 && RGBColors[1] > 0) {
          RGBColors[1]--;
        }
        else if (RGBColors[1] == 0 && RGBColors[2] == 255 && RGBColors[0] < 255) {
          RGBColors[0]++;
        }
        else {
          RGBColors[2]--;
        }
      }
      delay(10);
    }
  }

  if (ledState) {
    for (int i = 0; i < NUM_LEDS; i++) {
      leds[i] = CRGB(RGBColors[0], RGBColors[1], RGBColors[2]);
    }
    FastLED.show();
  }
  
  lastCmd = cmd;
}