#include <Wire.h>
#include <avr/wdt.h>
#define MAP_COORD_SIZE 14
#define COORD 13
#define MAP_SIZE 13
#define READ_ARR_RESULT 9 
#define READ_ARR_MAP 7
#define READ_MAP 3
#define READ_RESULT 5 
#define CNT_INPT_MESSAGE 1
#define ONE 1
//Placement types
#define Solo  1 
#define Horizontal 2
#define Vertical 3
#define Invalid 4 
#define Occupied 5
#define Connection_error 9
//Ship type
#define PatrolBoat  1 
#define Submarine 2
#define Battleship 3
#define AircraftCarrier 4 
// C++ code
//master server
const byte SLAVE_ID = 8;
byte state = 0;
byte data[13];
byte coord_state[100];
byte target_coordinate;
byte target_coordinate_X;
byte target_coordinate_Y;
byte arr_coordinate[2];
byte ship_coord[4];
byte result;
byte responseNOB;
byte test;
byte placement_type;
byte ship_type;


void Transmit_stage(){
    Wire.beginTransmission(SLAVE_ID);  // Transmit to device number 8 
    Wire.write(state);             // Sends value byte
    Wire.endTransmission();      // Stop transmitting
}

void stage_1(){
    Wire.requestFrom((int)SLAVE_ID, (int)1);
    if(Wire.available())
    	responseNOB = Wire.read(); 
}

void stage_2(){
    Wire.requestFrom((int)SLAVE_ID, (int)MAP_COORD_SIZE);
  	int k = 0;
    while (Wire.available()) {
          if(k == COORD){
            target_coordinate = Wire.read();
          }else{
          data[k] = Wire.read();
          }
          k++;
    } 
}

byte ProccessData(){
  for(int i=0, j =0; i < MAP_SIZE; i++, j += 8){
    if(j == 96){
      coord_state[j]   =  data[i] &  1; 
      coord_state[j+1] = (data[i] & (1 << 1))>> 1;
      coord_state[j+2] = (data[i] & (1 << 2))>> 2;
      coord_state[j+3] = (data[i] & (1 << 3))>> 3;
    }else{
      coord_state[j]   =  data[i] &  1;
      coord_state[j+1] = (data[i] & (1 << 1))>> 1;
      coord_state[j+2] = (data[i] & (1 << 2))>> 2;
      coord_state[j+3] = (data[i] & (1 << 3))>> 3;
      coord_state[j+4] = (data[i] & (1 << 4))>> 4;
      coord_state[j+5] = (data[i] & (1 << 5))>> 5;
      coord_state[j+6] = (data[i] & (1 << 6))>> 6;
      coord_state[j+7] = (data[i] & (1 << 7))>> 7;
    }
  }
  
  target_coordinate_X = (target_coordinate & (15 << 4))>> 4;
  target_coordinate_Y =  target_coordinate &  15;
  
  if(coord_state[target_coordinate_X + target_coordinate_Y*10])
                 return 1;
  else
                 return 5;
}

void stage_arr_2(){
  Wire.requestFrom((int)SLAVE_ID, 16);
  	int k = 0;
    while (Wire.available()) {
          if(k == 0){  
            ship_type = Wire.read();  
          }else{        
            if(k >= COORD + 1){
              arr_coordinate[k%14] = Wire.read();
            }else{
            data[k-1] = Wire.read();
            }
          }
          k++;
    } 
}
byte CheckPlacement(){
  for(int i=0, j =0; i < MAP_SIZE; i++, j += 8){
    if(j == 96){
      coord_state[j]   =  data[i] &  1; 
      coord_state[j+1] = (data[i] & (1 << 1))>> 1;
      coord_state[j+2] = (data[i] & (1 << 2))>> 2;
      coord_state[j+3] = (data[i] & (1 << 3))>> 3;
    }else{
      coord_state[j]   =  data[i] &  1;
      coord_state[j+1] = (data[i] & (1 << 1))>> 1;
      coord_state[j+2] = (data[i] & (1 << 2))>> 2;
      coord_state[j+3] = (data[i] & (1 << 3))>> 3;
      coord_state[j+4] = (data[i] & (1 << 4))>> 4;
      coord_state[j+5] = (data[i] & (1 << 5))>> 5;
      coord_state[j+6] = (data[i] & (1 << 6))>> 6;
      coord_state[j+7] = (data[i] & (1 << 7))>> 7;
    }
  }
  byte length = 0;
  byte y1 =  arr_coordinate[0] & 15, x1 = (arr_coordinate[0] & (15 << 4))>> 4,
  y2 = arr_coordinate[1] & 15, x2 = (arr_coordinate[1] & (15 << 4))>> 4; 
  if(arr_coordinate[0] == arr_coordinate[1]){
     ship_coord[0] =  (x1 << 4) | y1;
     placement_type = Solo;
     length++;
  }else {
    if (y1 == y2) //Horizontal
    {
      for (int x = x1; x <= x2; x++)
      {
          ship_coord[length] =  (x << 4) | y1;        
          length++;
      }
      placement_type = Horizontal;
    }else{
      if(x1 == x2){//Vertical
      for (int y = y1; y <= y2; y++)
      {
          ship_coord[length] =  (x1 << 4) | y;        
          length++;
      }
      placement_type = Vertical;

      }else
        placement_type = Invalid;
    }
  }
  //Check if already exists
  for(int h =0; h < length; h++){
    if(coord_state[((ship_coord[h] & (15 << 4))>> 4) + (ship_coord[h]&15)*10])
                  placement_type = Occupied;
  }
  if(ship_type == PatrolBoat && length != 1) placement_type = Invalid;
  if(ship_type ==  Submarine && length != 2) placement_type = Invalid;
  if(ship_type == Battleship && length != 3) placement_type = Invalid;
  if(ship_type ==  AircraftCarrier && length != 4) placement_type = Invalid;
}    

void setup()
{
  wdt_disable;
  Wire.begin();
  pinMode(LED_BUILTIN, OUTPUT);
  wdt_enable (WDTO_8S);
}

void loop()
{
    delay(50);
  	state = CNT_INPT_MESSAGE;
  	Transmit_stage();
    delay(50);
   	stage_1();
  if(responseNOB == 1){
    wdt_reset();
      state = READ_MAP;
  	  Transmit_stage();
      digitalWrite(LED_BUILTIN, HIGH); // sets the digital pin 13 on
      delay(50);                      // waits for a  half second
      digitalWrite(LED_BUILTIN, LOW);  // sets the digital pin 13 off
      delay(50);            // waits for a 0,05 of a second
      
      stage_2();
    
      result = ProccessData();
    
      state = READ_RESULT;
  	  Transmit_stage();
    
      Wire.beginTransmission(SLAVE_ID);  // Transmit to device number 8
      Wire.write(result);             // Sends value array of bytes
      Wire.endTransmission();            // Stop transmitting
      delay(50);
  }else{
    if(responseNOB == 2 ){//check placement
      wdt_reset();
      digitalWrite(LED_BUILTIN, HIGH); // sets the digital pin 13 on
      delay(50);                      // waits for a  half second
      digitalWrite(LED_BUILTIN, LOW);  // sets the digital pin 13 off
      state = READ_ARR_MAP;
  	  Transmit_stage();
      delay(50);            // waits for a 0,05 of a second
      
      stage_arr_2();
      CheckPlacement();
    
      state = READ_ARR_RESULT;
  	  Transmit_stage();
  
      Wire.beginTransmission(SLAVE_ID);  // Transmit to device number 8
      Wire.write(placement_type);             // Sends value array of bytes
      Wire.endTransmission();            // Stop transmitting
    }
  }
  
}