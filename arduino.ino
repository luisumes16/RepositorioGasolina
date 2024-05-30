#include <ArduinoJson.h>
int numero = 0;
String mensaje = "";//{"preciodia":20,"bomba":1,"cantidad":30}
long litro = 27500.0;


int bomba1 = 13;
int bomba2 = 12;
int bomba3 = 11;
int bomba4 = 10;

int sensor1 = 7;
void setup(){
    Serial.begin(9600);//Iniciar la comunicación serial
    pinMode(7, INPUT);
    pinMode(bomba1, OUTPUT);
    pinMode(bomba2, OUTPUT);
    pinMode(bomba3, OUTPUT);
    pinMode(bomba4, OUTPUT);
    pinMode(sensor1, INPUT);

}
void loop(){

    //Leer el JSON y guardarlo en un char
   
    if(Serial.available()){//Si hay datos en el puerto serial
        char input = Serial.read();//Leer el dato
        mensaje += input;//Concatenar el dato
        //Serial.println  (mensaje);//Imprimir el mensaje
        char* allchar = mensaje.c_str();//Convertir el mensaje a un char*
        if(input == '}'){ //Si el dato es igual a }
            StaticJsonDocument<64> doc;//Crear un documento JSON
            deserializeJson(doc, allchar);//Deserializar el mensaje
            int preciodia = doc["preciodia"]; // 2
            int bomba = doc["bomba"]; // 1
            float cantidad = doc["cantidad"]; // 30
            Serial.println(String(bomba));
            delay(1000);
            int abastecido = abastecimiento(cantidad, bomba);//Llamar a la función abastecimiento
            Senrializar(abastecido);//Serializar el mensaje
            mensaje = "";//Limpiar el mensaje
        }
    }
    delay(50);
}

void Senrializar(int abastecido){
//Serializar el mensaje

    StaticJsonDocument<16> doc;

    doc["bomba"] = 1;
    doc["cantidad"] = abastecido;

    serializeJson(doc, Serial);
    
    Serial.println();//Imprimir un salto de línea
}



int abastecimiento(float cantidad, int bomba){
    //Regla de 3, si para llenar 1 litro se necesitan 2000ms
    //Para llenar 30 litros se necesitan 60000ms
    //Serial.println("Cantidad: "+ String(cantidad) );
    //delay(1000);
    long tiempo = 25500.0 * cantidad;
    //Serial.print("TIEMPO "+ String(tiempo));
    //delay(1000);
    //Si la bomba es 1, encender el pin 13
    int bombaSeleccionada = 0;
    if(bomba == 1){
        bombaSeleccionada = 13;
    }else if(bomba == 2){
        bombaSeleccionada = 12;
    }else if(bomba == 3){
        bombaSeleccionada = 11;
    }else if(bomba == 4){
        bombaSeleccionada = 10;
    }
    int contador = 0;
    //Serial.println(tiempo);
    if(cantidad != 0){
         while(contador <= tiempo){
     
        digitalWrite(bombaSeleccionada, HIGH);
        delay(500);
        
        if(digitalRead(sensor1) == HIGH){
        contador = tiempo;
        break;
      }
      contador += 500;
      
         }
    }else {
      while(digitalRead(sensor1) != HIGH){
        digitalWrite(bombaSeleccionada, HIGH);
        delay(500);
        contador += 500;

      }
    }
   

        
        //Serial.println(contador);
        
    
    //Serial.println(contador);
    digitalWrite(bombaSeleccionada, LOW);
    int litrosDados = contador / litro;
    contador =0;
    return litrosDados;
    
    
}