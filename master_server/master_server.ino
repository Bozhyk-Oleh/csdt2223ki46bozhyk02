#include <Wire.h>
#include <avr/wdt.h>
//server master
int choices;
byte responseNOB, SLAVE_ID = 8; 

void Transmit_choice(){
    Wire.beginTransmission(SLAVE_ID);  // Transmit to device number 8 
    Wire.write(choices);             // Sends value byte
    Wire.endTransmission();      // Stop transmitting
}
	//Receive number of bytes of data from slave
void stage_1(){
    Wire.requestFrom(SLAVE_ID, (uint8_t)1);
    while (Wire.available()== 0){}
    responseNOB = Wire.read(); 
}

 //Received string of responseNOB bytes from bridge
String stage_2(){
    Wire.requestFrom(SLAVE_ID, responseNOB);
    String data = "";
    while (Wire.available()) {
        char b = Wire.read();
        data += b;
    } 
    return data;
}

void setup() {
  wdt_disable;
  Wire.begin();        // join i2c bus (address optional for master)
  pinMode(LED_BUILTIN, OUTPUT);  
  wdt_enable (WDTO_4S);
  
  digitalWrite(LED_BUILTIN, HIGH); // sets the digital pin 13 on
  delay(500);                      // waits for a  half second
  digitalWrite(LED_BUILTIN, LOW);  // sets the digital pin 13 off

}

void loop() {
    digitalWrite(LED_BUILTIN, HIGH); // sets the digital pin 13 on
    delay(100);                      // waits for a  0,1 second
    digitalWrite(LED_BUILTIN, LOW);  // sets the digital pin 13 off
    wdt_reset();

  	choices = 1;
    Transmit_choice();
    
  	
    //Receive number of bytes of data from slave
    //modifies the responseNOB global variable
    stage_1();
  	
      //if responseNOB = 0 restart
    if(responseNOB){
           
      choices = 2;
      Transmit_choice();
      
      delay(50);            // waits for a 0,05 of a second
      
      //Received string of responseNOB bytes from bridge
      String massage = stage_2();

      //modify the string 
      //string start with choice variable then data and with 
      //number of cherecters in original string 
      choices = 3;

      byte response[massage.length()+2];
      response[0] = choices;
      for (byte i = 1; i < massage.length()+1; i++) {
        response[i] = (byte)massage.charAt(i-1);
      }
      response[massage.length()+1] = massage.length();
      
      Wire.beginTransmission(SLAVE_ID);  // Transmit to device number 8
      Wire.write(response, massage.length()+2);      // Sends value array of bytes
      Wire.endTransmission();      // Stop transmitting
    }
  
  delay(100);
}
