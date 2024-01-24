#include <BluetoothSerial.h>
#include<esp_websocket_client.h>

#include <ESP32Servo.h>
class StepEmulServo{
  private:
    const float vl=0.30769; //l 135 0.30769  =12*180/7020; 
    const float vr =0.30527;//r 45 0.30527  =17*180/10024;
    const int xl=80;
    const int xr=30;
    Servo servo;
    int currentAngle=0;
    void rotate(bool left, int delta){
      int vect,x; float v;
      if(left)
      { vect=135; v=vl; x=xl; }
      else
      { vect=45; v=vr; x=xr; }
      int timeSpan=((int)round((delta/v)))+x;    
      servo.write(vect); delay(timeSpan); servo.write(90);
    }
  public:
    StepEmulServo(){}
    void attach(int PIN){
      servo.attach(PIN); servo.write(90); delay(50);
    }
    void write(int angle){
      if(angle>360 || angle <0){angle=0;}
      int delta=abs(currentAngle-angle), vecDelta=0;
      if(delta>180)
        vecDelta=360-delta;
      else
        vecDelta = delta;
      bool isLeft=((bool)(delta>180))^((bool)(angle>currentAngle));
      rotate(isLeft,vecDelta);
      currentAngle=angle;
    }
};

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


class GeneralServo{
  private:
    int begin;
    int end;
    Servo servo;
  public:
    GeneralServo(int _begin,int _end){
      begin=_begin;
      end=_end;
    }
    void attach(int PIN){servo.attach(PIN); }

    void write(int angle){
      int delta= end-begin;
      int native = (int)(round( begin + ( ((float)angle) / ((float)100)  * ((float)delta) ) ));
      servo.write(native);
    }
};
#define LIGHT 4
#define EYE 16 


//bool is_Wake=false;

StepEmulServo servo1;
GeneralServo servo2(0,134), 
servo3(105,185), servo4(21,119);//  134  46  118
//Servo servo2, servo3, servo4;
void ServoInit()
{
    pinMode(LIGHT, OUTPUT);
    pinMode(EYE, OUTPUT);
    digitalWrite(LIGHT, LOW);
    digitalWrite(EYE, HIGH);
 
    servo1.attach(17);
    servo2.attach(5);
    servo3.attach(18);
    servo4.attach(19);

    servo1.write(180);
    servo2.write(50);
    servo3.write(50);
    servo4.write(50);
}

void RunServoCommand(int num, int angle)
{
    if (angle < 0)
    {
        angle = 0;
        Serial.println("angle <0.");
    }
    else if (angle > 360)
    {
        angle = 360;
        Serial.println("angle >180.");
    }
    if (num == 1)
    {
        servo1.write(angle);
        Serial.println("Servo 1 rotate.");
    }
    else if (num == 2)
    {
        servo2.write(angle);
        Serial.println("Servo 2 rotate.");
    }
    else if (num == 3)
    {
        servo3.write(angle);
        Serial.println("Servo 3 rotate.");
    }
    else if (num == 4)
    {
        servo4.write(angle);
        Serial.println("Servo 4 rotate.");
    }
    else
    {
        Serial.println("Servo undefinded.");
    }
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

void RunAnimation_undefined_0(){
  Serial.println("Animation undefined ran.");

}

void RunAnimation_dance_5(){
  for( int i =0; i<250;i++){
          digitalWrite(LIGHT, HIGH);
          delay(30);
          digitalWrite(LIGHT, LOW);
                    delay(30);

  }
}

void Wakeup(){
  Serial.println(" Wakeup ran.");
 // is_Wake=true;

  }


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void RunAnimationCommand(int num)
{
  if(num==0){
        RunAnimation_undefined_0();
     }else if(num==1){
      RunAnimation_bitls_1();
    }else if(num==2){
        RunAnimation_show_2();
    }else if(num==3){
      RunAnimation_joke_3();
    } else if(num==4){
      RunAnimation_fact_4();
    }else if(num==5){
      RunAnimation_dance_5();
    }
    else{ Serial.println("Animation not found.");}
}
void RunLumen(int num, int pow){
if(num==1){
  if(pow<50){
      Serial.println("Light 1 off.");
      digitalWrite(LIGHT, LOW);
    }else{
      Serial.println("Light 1 on.");
      digitalWrite(LIGHT, HIGH);
    }
  }else if(num==2){
    if(pow<50){
      Serial.println("Light 2 off.");
      digitalWrite(EYE, LOW);
    }else{
      Serial.println("Light 2 on.");
      digitalWrite(EYE, HIGH);
    }
  }else{ Serial.println("Light not found.");}
}


void RunCommand(String command)
{
    char deviceType = command[1];
    int deviceNumber = command[2] - 48;
    int deviceParam = (100 * (command[3] - 48)) + (10 * (command[4] - 48)) + ((command[5] - 48));
    if (deviceType == 'L')
      RunLumen(deviceNumber,deviceParam);
    else if (deviceType == 'S')
      RunServoCommand(deviceNumber, deviceParam);
    else if (deviceType == 'A')
      RunAnimationCommand(deviceNumber);
    else
      Serial.println("device type undefined.");
}

BluetoothSerial SerialBT; // Объявляем объект для работы с Bluetooth
void ProcessInput()
{
    if (SerialBT.available())
    { // Проверяем, есть ли данные в буфере Bluetooth
        String receivedString = SerialBT.readString(); // Считываем строку из буфера
        Serial.println(receivedString);
        if (receivedString.length() == 7)
        {
            if (receivedString[0] == '{' && receivedString[6] == '}')
              RunCommand(receivedString);
        }
        else
            Serial.println("not a command.");
    }
}
void setup()
{
    Serial.begin(9600);
    SerialBT.begin("GLA"); // Инициализируем Bluetooth с именем "GLA"
    ServoInit(); // Инициализация стерв
}

void loop() { ProcessInput(); }
