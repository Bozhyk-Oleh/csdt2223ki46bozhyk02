#include <Wire.h>
#define ERROR_REPORT 44

#define WAIT 99
#define SEND_ARR_RESULT 10
#define READ_ARR_RESULT 9 
#define SEND_ARR_MAP 8
#define READ_ARR_MAP 7
#define SEND_RESULT 6
#define READ_RESULT 5 
#define SEND_MAP 4
#define READ_MAP 3
#define SEND_CNT_INPT_MESSAGE 2
#define CNT_INPT_MESSAGE 1
//bridge slave
byte states = 0;
byte response = 0;
byte result;
bool anti_spam = false;

int conncetion_check_period = 1000;
unsigned long ContTest = 0;

const int BUFFER_SIZE = 14;
byte buf[BUFFER_SIZE];
const int ARRANGE_BUFFER_SIZE = 16;
byte ar_buf[ARRANGE_BUFFER_SIZE];
byte ar_result;


void requestEvent() 
{
  ContTest = millis();
  anti_spam = false;
  switch(states)
  {
    case SEND_CNT_INPT_MESSAGE:
      Wire.write(response);
      states = WAIT;
    break;
    
    case SEND_MAP:
      Wire.write(buf, BUFFER_SIZE);
    for(int i =0; i< BUFFER_SIZE;i++){
      buf[i]=0;
    }
    break;  
    case SEND_ARR_MAP:
      Wire.write(ar_buf, ARRANGE_BUFFER_SIZE);
    for(int i =0; i< ARRANGE_BUFFER_SIZE;i++){
      ar_buf[i]=0;
    }
    break;  
  }
}
void receiveEvent(int i)
{
    if(states == READ_ARR_RESULT){
      ar_result = Wire.read();
      states = SEND_ARR_RESULT;
    }else{    
      if(states == READ_RESULT){
        result = Wire.read();
        states = SEND_RESULT;
      }else{
        states = Wire.read();
      }
    }
}

void setup()
{
  Serial.begin(9600);
  Wire.begin(8);
  Wire.onReceive(receiveEvent);
  Wire.onRequest(requestEvent); 
  pinMode(LED_BUILTIN, OUTPUT);   
}

void loop()
{
  if(millis()> ContTest + conncetion_check_period && !anti_spam){
    states = ERROR_REPORT;
    anti_spam = true; 
  }
  switch(states){
    case CNT_INPT_MESSAGE:
    if(Serial.available() > 0){
            response = Serial.peek();
        	  states = SEND_CNT_INPT_MESSAGE;
    }
       
    break;
    
    case READ_MAP:
    // check if data is available
    if(Serial.available() > 0){ 
        Serial.read();
        Serial.readBytes(buf, BUFFER_SIZE); 
        digitalWrite(LED_BUILTIN, HIGH); 
        states = SEND_MAP;
    }
    break;
    
    case SEND_RESULT:
    Serial.print(result);
    states = 0;
    break;

    case READ_ARR_MAP:
     if(Serial.available() > 0){ 
        Serial.read();
        Serial.readBytes(ar_buf, ARRANGE_BUFFER_SIZE);             
        states = SEND_ARR_MAP;
    }
    break;

    case SEND_ARR_RESULT:
    {
      Serial.print(ar_result);     
      states = 0;      
    }    
    break;
    
    case ERROR_REPORT:
      if(Serial.available() > 0){
        delay(5);
      while(Serial.available() > 0){
        Serial.read();
      }
      Serial.print("e");
    }
    break;
    
  }
}