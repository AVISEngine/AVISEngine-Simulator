import cv2
import os
import io
import re
import time
import base64
import socket
import numpy as np
from PIL import Image
from array import array


#Take in base64 string and return PIL image
def stringToImage(base64_string):
    imgdata = base64.b64decode(base64_string)
    return Image.open(io.BytesIO(imgdata))

#convert PIL Image to an RGB image( technically a numpy array ) that's compatible with opencv
def toRGB(image):
    return cv2.cvtColor(np.array(image), cv2.COLOR_BGR2RGB)


#Defining server connection string
host = "127.0.0.1"
port = 25001
data = "Speed:100,Steering:10"
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

#ignoring DeprecationWarning
import warnings
warnings.simplefilter("ignore", DeprecationWarning)

#Init data to send to server 
finaldata = ''
counter = 0
try:
    sock.connect((host, port))
    
    #Data template : Speed:x,Steering:y
    #Data Regex : /[a-zA-Z]+:\d+/sg
    while(True):
        start_time = time.time()
        data = "Speed:" + str(0) + ",Steering:" + str(100) + ",ImageStatus:" + str(1) + ",SensorStatus:" + str(1)

        #Sending and Reciving Data
        sock.sendall(data.encode("utf-8"))
        recive = sock.recv(80000).decode("utf-8")
        counter = counter + 1
        imageTagCheck = re.search('<image>(.*?)<\/image>', recive)
        sensorTagCheck = re.search('<sensor>(.*?)<\/sensor>', recive)
        try:
            if(imageTagCheck):
                imageData = imageTagCheck.group(1)
                im_bytes = base64.b64decode(imageData)
                im_arr = np.frombuffer(im_bytes, dtype=np.uint8)  # im_arr is one-dim Numpy array
                img = cv2.imdecode(im_arr, flags=cv2.IMREAD_COLOR)
                cv2.imshow('frames', img)
                if cv2.waitKey(10) & 0xFF == ord('q'):
                    break

            if(sensorTagCheck):
                sensorData = sensorTagCheck.group(1)
                #print("Sensor=>",sensorData)
        except:
            print("Unvalid Image data!")
            #print(recive)
        
        time.sleep(0.001)
        
        #print("FPS: ", 1.0 / (time.time() - start_time))

finally:
    sock.sendall("stop".encode("utf-8"))
    sock.close()
    print("done")
