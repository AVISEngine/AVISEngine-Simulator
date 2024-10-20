import websocket
import threading
import time
import json
import cv2
import numpy as np
import base64

class WSClient:
    def __init__(self, url="ws://localhost:4567/pubsub"):
        self.url = url
        self.ws = None
        self.connected = False
        self.last_image_time = 0
        self.frame_interval = 1 / 30  # 30 FPS

    def on_message(self, ws, message):
        topic, data = message.split(':', 1)
        if topic == "/car/camera":
            self.handle_camera_image(data)
        elif topic == "/car/speed":
            self.handle_speed(data)
        elif topic == "/car/sensors":
            self.handle_sensors(data)

    def handle_camera_image(self, data):
        current_time = time.time()
        if current_time - self.last_image_time < self.frame_interval:
            return  # Skip this frame to maintain desired FPS

        # Extract base64 encoded image data from XML-like string
        image_data = data.split("<image>")[1].split("</image>")[0]
        
        # Decode base64 string to bytes
        image_bytes = base64.b64decode(image_data)
        
        # Convert bytes to numpy array
        nparr = np.frombuffer(image_bytes, np.uint8)
        
        # Decode image
        img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
        
        # Display image
        cv2.imshow("Car Camera", img)
        cv2.waitKey(1)  # Refresh the window
        
        self.last_image_time = current_time

    def handle_speed(self, data):
        speed = int(data.split("<speed>")[1].split("</speed>")[0])
        print(f"Current speed: {speed}")

    def handle_sensors(self, data):
        sensor_data = data.split("<sensor>")[1].split("</sensor>")[0]
        print(f"Sensor data: {sensor_data}")

    def on_error(self, ws, error):
        pass

    def on_close(self, ws, close_status_code, close_msg):
        self.connected = False

    def on_open(self, ws):
        self.connected = True

    def connect(self):
        websocket.enableTrace(False)  # Disable WebSocket logging
        self.ws = websocket.WebSocketApp(self.url,
                                         on_open=self.on_open,
                                         on_message=self.on_message, 
                                         on_error=self.on_error,
                                         on_close=self.on_close)

        wst = threading.Thread(target=self.ws.run_forever)
        wst.daemon = True
        wst.start()

        # Wait for connection to be established
        while not self.connected:
            time.sleep(0.1)

    def subscribe(self, topic):
        if self.connected:
            self.ws.send(f"subscribe:{topic}")
        else:
            pass

    def unsubscribe(self, topic):
        if self.connected:
            self.ws.send(f"unsubscribe:{topic}")
        else:
            pass

    def publish(self, topic, data):
        if self.connected:
            message = f"publish:{topic}:{data}"
            self.ws.send(message)
        else:
            print("Not connected. Cannot publish.")

    def close(self):
        if self.connected:
            self.ws.close()
        else:
            pass

# Example usage
if __name__ == "__main__":
    client = WSClient()
    client.connect()

    # Subscribe to topics
    client.subscribe("/car/speed")
    client.subscribe("/car/sensors")
    client.subscribe("/car/camera")

    # Publish car control data more frequently
    try:
        while True:
            control_data = "10,0,0,0,0,30"  # Example: speed, steering, command, sensor, get_speed, sensor_angle
            client.publish("/car/control", control_data)
            time.sleep(0.05)  # Publish control data every 50ms
    except KeyboardInterrupt:
        client.close()
        cv2.destroyAllWindows()
