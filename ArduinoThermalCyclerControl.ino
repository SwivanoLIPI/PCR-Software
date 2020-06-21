#include "max6675.h"
#include "rgb_lcd.h"
char incomingOption;
int ktcSO = 8;  //Pin 8
int ktcCS = 9;  //Pin 9
int ktcCLK = 10;  //Clock mengambil sinyal PWM pada Pin 10 Arduino
int relayPin1 = 2; //Fan berada pada PIN Digital 2 dari arduino
int relayPin2 = 4; //Fan Berada pada PIN Digital 4 dari arduino
int relayPin3 = 7;//machine fan
int delayValue = 1000;
rgb_lcd lcd;
const int colorR = 255;
const int colorG = 255;
const int colorB = 0;
MAX6675 ktc(ktcCLK, ktcCS, ktcSO);

void setup() {
  Serial.begin(9600); //Baud rate 9600
  pinMode(relayPin1, OUTPUT); //relay 1 mendapatkan sinyal pada PIN 2
  pinMode(relayPin2, OUTPUT); //relay 2 mendapatkan sinyal pada PIN 4
  pinMode(relayPin3, OUTPUT);
  digitalWrite(relayPin1, LOW);
  digitalWrite(relayPin2, HIGH);
  digitalWrite(relayPin3,LOW);
  //digitalWrite(relayPin1, LOW);
  lcd.begin(16, 3);
  lcd.print("PCR Start");
  lcd.setRGB(colorR, colorG, colorB);
 delay(5000); 
}

void loop() {
  
incomingOption = Serial.read();
Serial.println(ktc.readCelsius());
lcd.setCursor(0, 0); 
lcd.print("Tempratur Heater : ");
lcd.setCursor(5, 1);
lcd.print(ktc.readCelsius());
lcd.print((char) 223);
lcd.print("C");
//digitalWrite(relayPin2, LOW);
delay(1000);
// Menyalakan Fan  
if (incomingOption=='0')  
{  
digitalWrite(relayPin1, HIGH);  //Fan Off - Relay On
}  
else if (incomingOption=='1')  
{  
digitalWrite(relayPin1, LOW);  //Fan On - Relay off
}  
 
//Menyalakan Heater
if (incomingOption=='2')  
{  
digitalWrite(relayPin2, HIGH);  //Heater Off - Relay On
}  
else if (incomingOption=='3')  
{  
digitalWrite(relayPin2, LOW); //Heater On - Relay Off  
}
}
