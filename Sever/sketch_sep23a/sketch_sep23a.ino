void setup() {
  // put your setup code here, to run once:
pinMode(LED_BUILTIN, OUTPUT);
Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available()>0){
    char input = Serial.read();
    if(input == '1'){
      digitalWrite(LED_BUILTIN, HIGH);
    }
     if(input == '0'){
      digitalWrite(LED_BUILTIN, LOW);
    }
    
    delay(100);
  }
}
