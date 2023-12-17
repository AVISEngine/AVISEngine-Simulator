import zmq
import numpy as np
import cv2
import time

class Car:
    """
    A class used to subscribe to topics from a Unity ZMQ publisher.

    ...

    Attributes
    ----------
    context : zmq.Context
        The ZMQ context.
    socket : zmq.Socket
        The ZMQ socket.
    topics : list
        The list of topics to subscribe to.
    frame_count : int
        The count of frames received.
    start_time : float
        The time when the frame count started.

    Methods
    -------
    connect(ip, port):
        Connects the socket to the specified IP and port.
    subscribe_to_topics():
        Subscribes the socket to the specified topics.
    receive_data():
        Receives data from the socket and processes it based on the topic.
    cleanup():
        Closes the socket and terminates the context.
    """

    def __init__(self, topics):
        self.context = zmq.Context()
        self.socket = self.context.socket(zmq.SUB)
        self.topics = topics
        self.frame_count = 0
        self.start_time = time.perf_counter()
        self._camera_front_image = []
        self._camera_left_image = []
        self._camera_right_image = []

    def connect(self, ip="127.0.0.1", port="12345"):
        """Connects the socket to the specified IP and port."""
        self.socket.connect(f"tcp://{ip}:{port}")

    def subscribe_to_topics(self):
        """Subscribes the socket to the specified topics."""
        for topic in self.topics:
            self.socket.setsockopt(zmq.SUBSCRIBE, topic)

    def receive_data(self):
        """Receives data from the socket and processes it based on the topic."""
        while True:
            # Receive the topic and data
            [topic, data] = self.socket.recv_multipart()

            # Process the received data based on the topic
            if topic == b"/car/image_front":
                self.frame_count += 1
                image_array = np.frombuffer(data, dtype=np.uint8)
                image = cv2.imdecode(image_array, cv2.IMREAD_COLOR)
                self._camera_front_image = image
                cv2.imshow("Front Image", image)

                # Calculate FPS
                end_time = time.perf_counter()
                time_diff = end_time - self.start_time
                if time_diff >= 1.0:  # Every second, update the FPS value
                    fps = self.frame_count / time_diff
                    print("FPS: ", fps)
                    self.frame_count = 0
                    self.start_time = time.perf_counter()
                    
            if topic == b"/car/image_left":
                self.frame_count += 1
                image_array = np.frombuffer(data, dtype=np.uint8)
                image = cv2.imdecode(image_array, cv2.IMREAD_COLOR)
                self._camera_left_image = image
                cv2.imshow("Left Image", image)

                # Calculate FPS
                end_time = time.perf_counter()
                time_diff = end_time - self.start_time
                if time_diff >= 1.0:  # Every second, update the FPS value
                    fps = self.frame_count / time_diff
                    print("FPS: ", fps)
                    self.frame_count = 0
                    self.start_time = time.perf_counter()
                    
            if topic == b"/car/image_right":
                image_array = np.frombuffer(data, dtype=np.uint8)
                image = cv2.imdecode(image_array, cv2.IMREAD_COLOR)
                self._camera_right_image = image
                cv2.imshow("Right Image", image)

            
            
            

            # Add more processing if required for other topics
                
            if cv2.waitKey(10) & 0xFF == ord('q'):
                break
                
    def get_front_image(self):
        return self._camera_front_image
    def get_left_image(self):
        return self._camera_left_image
    def get_right_image(self):
        return self._camera_right_image
        
    def cleanup(self):
        """Closes the socket and terminates the context."""
        cv2.destroyAllWindows()
        self.socket.close()
        self.context.term()

topics = [b"/car/scan", b"/car/steering", b"/car/vel", b"/car/image_front", b"/car/image_left", b"/car/image_right", b"/car/depth"]
subscriber = Car(topics)
subscriber.connect()
subscriber.subscribe_to_topics()
subscriber.receive_data()
subscriber.cleanup()