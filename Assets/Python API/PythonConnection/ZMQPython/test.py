import zmq

def subscribe_to_server(topics):
    context = zmq.Context()

    # Create a Subscriber socket
    subscriber = context.socket(zmq.SUB)

    # Connect to the server
    subscriber.connect("tcp://localhost:5556")

    # Subscribe to multiple topics
    for topic in topics:
        subscriber.setsockopt_string(zmq.SUBSCRIBE, topic)

    # Continuously read and process messages
    try:
        while True:
            received_data = subscriber.recv_multipart()
            topic, message = received_data[0], received_data[1]
            print(f"Received data on topic '{topic.decode()}': {message.decode()}")
    except KeyboardInterrupt:
        print("Subscriber interrupted")
    finally:
        subscriber.close()
        context.term()

if __name__ == '__main__':
    topics_list = ["Speed", "Steering", "Command", "Sensor", "GetSpeed", "SensorAngle"]
    subscribe_to_server(topics_list)