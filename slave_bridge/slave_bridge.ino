#include <Wire.h>
//bridge slave
//arduino
//--------------------------------------------------
String str;
byte choices;
byte response = 0;
bool max_uart = 0;

//invoked when request treeger actived.
//Send data requered by master 

void requestEvent() 
{
  switch(choices)
  {
    case 1 :
      Wire.write(response);
    break;
    
    case 2:
      Wire.write(str.c_str());
    break;  
  }
}

//invoked when receive treeger actived.
//Modifies global veriable(choices) and prints result 
void receiveEvent(int i)
{
  choices = Wire.read();
  if(choices == 2)
    max_uart = 1;
     if(choices == 3){
        String responseStr = "";
    	byte j = 0;
    	byte size;
        while (Wire.available()) {
          if(j < response){
            char b = Wire.read();
            responseStr += b;
            j++;
          }else
            size = Wire.read();
        } 
        Serial.println(responseStr + size);
        response = 0;
  }
}

void setup() {
Serial.begin(9600);
Wire.begin(8);
Wire.onReceive(receiveEvent);
Wire.onRequest(requestEvent);    
  
}


//this loop function modifies received data from serial port
void loop() {

  switch(choices)
  {
    case 1 :
    {
    	str = "";
        if(response == 0){
        while(Serial.available() > 0){
          delay(50);
          response = Serial.available();
          if(response > 30){
           response = 30;
          }
          break;
        }
      }
    }
      break;
    
    case 2 :
    {
       int k = response;
       while(Serial.available() > 0 && k > 0 && max_uart == 1 ){
             char c = Serial.read();
             str += c;
         	 k--;
      }
      max_uart = 0;
    }
      break;
  }

}