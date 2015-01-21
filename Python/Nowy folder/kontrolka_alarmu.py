import RPi.GPIO as GPIO
import subprocess
import re
import sys
import time
import datetime
import gspread
import MySQLdb

PIR = 23
LED = 3

GPIO.setmode(GPIO.BCM)
GPIO.setup(PIR,GPIO.IN)
GPIO.setup(LED,GPIO.OUT)

dbUser = 'root'
dbPassword = 'mikipi123'
dbHost = 'rpi.bitnamiapp.com'
dbFolder = 'RPI_ALARM'
port = 3306

t = 30
if len(sys.argv) == 2:
        t = float(sys.argv[1])



def moveDetectorFunction(channel):	
#	print "Move detected: ",datetime.datetime.now()
	GPIO.output(LED, True)   
	insert("INSERT INTO `MOVE_DETECTION`(`DATE`) VALUES ('%s')" % datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S"))	
#	time.sleep(2)
	GPIO.output(LED,False)

def insert(query):
	try:
		db = MySQLdb.connect(dbHost, dbUser, dbPassword, dbFolder)
		curs=db.cursor()
		curs.execute(query)
		db.commit()
	except:
	#	print("ERROR MySQL, query: ")
	#	print(query)
		db.rollback()

GPIO.add_event_detect(PIR, GPIO.RISING, callback=moveDetectorFunction)

try:
	while(True):
	  # Run the DHT program to get the humidity and temperature readings!
	  GPIO.output(LED,False)
	  output = subprocess.check_output(["./Adafruit_DHT", "11", "4"]);
#	  print output
	  matches = re.search("Temp =\s+([0-9.]+)", output)
	  if (not matches):
	    time.sleep(3)
	    continue
	  temp = float(matches.group(1))
	  
	  # search for humidity printout
	  matches = re.search("Hum =\s+([0-9.]+)", output)
	  if (not matches):
	    time.sleep(3)
	    continue
	  humidity = float(matches.group(1))
#         print "Temperature: %.1f C" % temp
#         print "Humidity:    %.1f %%" % humidity
	  insert("INSERT INTO `HUMIDITY`(`DATE`, `VALUE`) VALUES ('%s','%.1f')" % (datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S"),humidity))
	  insert("INSERT INTO `TEMPERATURE`(`DATE`, `VALUE`) VALUES ('%s','%.1f')" % (datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S"),temp))
	  time.sleep(t)
except KeyboardInterrupt:
	GPIO.cleanup()
