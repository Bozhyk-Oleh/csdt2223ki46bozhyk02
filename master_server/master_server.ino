#include <Wire.h>
#include <avr/wdt.h>
//server master
int choices;
byte responseNOB, SLAVE_ID = 8; ;

void setup() {
  wdt_disable;
  Wire.begin();        // join i2c bus (address optional for master)
  pinMode(LED_BUILTIN, OUTPUT);  
  wdt_enable (WDTO_2S);
}

void loop() {
    digitalWrite(LED_BUILTIN, HIGH); // sets the digital pin 13 on
    delay(500);                      // waits for a  half second
    digitalWrite(LED_BUILTIN, LOW);  // sets the digital pin 13 off
    wdt_reset();

  	choices = 1;
  	Wire.beginTransmission(SLAVE_ID);  // Transmit to device number 8 
    Wire.write(choices);             // Sends value byte
    Wire.endTransmission();      // Stop transmitting
	
  	delay(50);
  	
	//Receive number of bytes of data from slave
  	Wire.requestFrom(SLAVE_ID, (uint8_t)1);
    while (Wire.available()== 0){}
    responseNOB = Wire.read(); 
  	
  	//if responseNOB = 0 restart
  if(responseNOB){
              
  	choices = 2;
  	Wire.beginTransmission(SLAVE_ID);  // Transmit to device number 8
    Wire.write(choices);             // Sends value byte
    Wire.endTransmission();      // Stop transmitting
    
    delay(100);            // waits for a second
  	
    //Received string of responseNOB bytes from bridge
  	Wire.requestFrom(SLAVE_ID, responseNOB);
    String data = "";
    while (Wire.available()) {
        char b = Wire.read();
        data += b;
    } 
    
    //modify the string 
    //string start with choice command, then data and with 
    //number of cherecters in original string 
    choices = 3;
    byte response[data.length()+2];
    response[0] = choices;
    for (byte i = 1; i < data.length()+1; i++) {
      response[i] = (byte)data.charAt(i-1);
    }
    response[data.length()+1] = data.length();
    
  	Wire.beginTransmission(SLAVE_ID);  // Transmit to device number 8
    Wire.write(response, data.length()+2);      // Sends value array of bytes
    Wire.endTransmission();      // Stop transmitting
  }
  
  delay(500);
}