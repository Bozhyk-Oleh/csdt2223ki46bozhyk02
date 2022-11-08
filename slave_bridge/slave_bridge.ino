#include <Wire.h>
//bridge slave
//arduino
//--------------------------------------------------
String str;
unsigned long ContTest;
byte choices;
byte response = 0;
bool max_uart = 0;
int firt_period = 1000;
bool dr_error_connect = 1, dr_lost_connect = 0, no_connect = 0, stop_spam_lost_connect =0;

//invoked when request treeger actived.
//Send data requered by master 

void requestEvent() 
{
  no_connect = 0;
  stop_spam_lost_connect = 0;
  ContTest = millis();
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
  ContTest = millis();
  choices = Wire.read();
  if(choices == 1)
     dr_error_connect = 0;
  if(choices == 2){
    max_uart = 1;
    dr_lost_connect = 0;
  }
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
  if(millis()> ContTest + firt_period && !dr_lost_connect && !no_connect && !stop_spam_lost_connect){
    Serial.println("Lost connection with server");
    stop_spam_lost_connect = 1;
    }
  switch(choices)
  {
    case 1 :
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
      break;
    }  
    default:
      while(millis() < 5000 && dr_error_connect){
          //wait approx. [period] ms
      }
      if(dr_error_connect){
        Serial.println("No connection with server");
        dr_error_connect = 0;
        no_connect = 1;
      }
    break;
  }
}