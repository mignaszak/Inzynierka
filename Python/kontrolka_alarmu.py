import RPi.GPIO as GPIO #Modu� obs�uguj�cy porty Raspberry Pi
import subprocess
import re
import sys
import time
import datetime
import gspread
import MySQLdb #Modu� zapewniaj�cy kontakt z baz� danych

#Zmienne przechowuj�ce numer�w pin�w, do kt�rych podpi�te s� czujnik ruchu i dioda
PIR = 23 
LED = 3

#Ustawienie typ�w pin�w
GPIO.setmode(GPIO.BCM) 
GPIO.setup(PIR,GPIO.IN)
GPIO.setup(LED,GPIO.OUT)

#Dane bazy danych i u�ytkownika
dbUser = 'exten'
dbPassword = 'mp12345'
dbHost = 'rpi.bitnamiapp.com'
dbFolder = 'RPI_ALARM'
port = 3306

#Zmienna wskazuj�ca okres pomiaru temperatury oraz wilgotno�ci powietrza je�eli nie zosta�a podana przy uruchamianiu skryptu
t = 30
if len(sys.argv) == 2:
        t = float(sys.argv[1])
		
#Funkcja obs�ugj�ca zdarzenie wykrycia ruchu przez czujnik
def moveDetectorFunction(channel):	
	GPIO.output(LED, True)   
	insert("INSERT INTO `MOVE_DETECTION`(`DATE`) VALUES ('%s')" % datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S"))	
	GPIO.output(LED,False)

#Funkcja wysy�aj�ca zapytanie do bazy danych
def insert(query):
	try:
		db = MySQLdb.connect(dbHost, dbUser, dbPassword, dbFolder)
		curs=db.cursor()
		curs.execute(query)
		db.commit()
	except:
		db.rollback()

#Ustawienie wywo�ania funkcji obs�ugi zdarzenia podczas wyst�pienia zbocza narastaj�cego na pinie z podpbi�tym czujnikiem ruchu
GPIO.add_event_detect(PIR, GPIO.RISING, callback=moveDetectorFunction)

#G��wna p�tla programu
try:
	while(True):
	  GPIO.output(LED,False)
	  #Uruchomienie programu odczytuj�cego dane z DHT11
	  output = subprocess.check_output(["./Adafruit_DHT", "11", "4"]);
	  #Wydzielenie informacji o temperaturze powietrza z wyniku dzia�ania programu
	  matches = re.search("Temp =\s+([0-9.]+)", output)
	  if (not matches):
	    time.sleep(3)
	    continue
	  temp = float(matches.group(1))
	  
	  #Wydzielenie informacji o wilgotno�ci powietrza z wyniku dzia�ania programu
	  matches = re.search("Hum =\s+([0-9.]+)", output)
	  if (not matches):
	    time.sleep(3)
	    continue
	  humidity = float(matches.group(1))
	  #Wys�anie do bazy danych informacji o temperaturze i wilgotno�ci powietrza
	  insert("INSERT INTO `HUMIDITY`(`DATE`, `VALUE`) VALUES ('%s','%.1f')" % (datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S"),humidity))
	  insert("INSERT INTO `TEMPERATURE`(`DATE`, `VALUE`) VALUES ('%s','%.1f')" % (datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S"),temp))
	  #Zatrzymanie dzia�ania p�tli na zadany czas
	  time.sleep(t)
except KeyboardInterrupt:
	GPIO.cleanup()
