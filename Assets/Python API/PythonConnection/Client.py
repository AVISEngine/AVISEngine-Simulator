import socket
import time
host = "127.0.0.1"
port = 25001
data = "Speed:100,Steering:10"

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
try:
    sock.connect((host, port))
    
    #Data template : Speed:x,Steering:y
    #Data Regex : /[a-zA-Z]+:\d+/sg
    for i in range(1,100):
        data = "Speed:" + str(10) + ",Steering:" + str(i)
        print(data)
        sock.sendall(data.encode("utf-8"))
        data = sock.recv(1024).decode("utf-8")
        print(data)
        print("\n")
        #data = data.split(' ')
        #newdata = list(map(int, data))
        #print(newdata)
        time.sleep(0.5)
finally:
    # sock.close()
    print("done")
