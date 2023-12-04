import zmq
import numpy as np
import cv2

# Connect to the Unity ZMQ publisher
context = zmq.Context()
socket = context.socket(zmq.SUB)
socket.connect("tcp://127.0.0.1:12345")  # Replace with the appropriate IP and port

# Subscribe to the topics
topics = [b"/car/scan", b"/car/steering", b"/car/vel", b"/car/image_front", b"/car/segment", b"/car/depth"]
for topic in topics:
    socket.setsockopt(zmq.SUBSCRIBE, topic)

while True:
    # Receive the topic and data
    [topic, data] = socket.recv_multipart()
    print(topic)
    # Process the received data based on the topic
    if topic == b"/car/image_front":
        # Assuming the data is an image represented as a byte array
        image_array = np.frombuffer(data, dtype=np.uint8)
        image = cv2.imdecode(image_array, cv2.IMREAD_COLOR)
        cv2.imshow("Front Image", image)
        cv2.waitKey(1)  # Adjust as needed

    elif topic in [b"/car/scan", b"/car/steering", b"/car/vel", b"/car/segment", b"/car/depth"]:
        # Assuming other topics contain string data
        print(f"Received data for topic {topic.decode()}: {data.decode()}")

# Clean up
cv2.destroyAllWindows()
socket.close()
context.term()