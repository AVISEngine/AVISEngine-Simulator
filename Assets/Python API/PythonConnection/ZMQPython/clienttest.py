import zmq
import sys

# Function to handle image data
def handle_image_data(image_data):
    # Process the image data, save to a file, display, etc.
    # As an example, let's save the image to a file
    with open("front_image.jpg", "wb") as f:
        f.write(image_data)

# Function to connect to the server and process incoming messages
def subscriber_client(address="tcp://localhost:12345"):
    context = zmq.Context()
    socket = context.socket(zmq.SUB)
    
    # Connect to the server's bind address
    socket.connect(address)

    # Subscribe to all topics of interest
    topics = ["/car/scan", "/car/steering", "/car/vel", "/car/image_front", "/car/segment", "/car/depth"]
    for topic in topics:
        socket.setsockopt_string(zmq.SUBSCRIBE, topic)
    
    print("Starting subscriber for topics: {}".format(", ".join(topics)))

    try:
        while True:
            # Receive the topic frame
            topic_frame = socket.recv_string()
            message_frame = socket.recv()

            # Print the message or handle special case for image
            if topic_frame == "/car/image_front":
                handle_image_data(message_frame)
                print("Received image data")
            else:
                print(f"Received message on topic '{topic_frame}': {message_frame.decode('utf-8')}")

    except KeyboardInterrupt:
        print("Subscriber shutdown.")

    finally:
        # Clean up on close
        socket.close()
        context.term()

if __name__ == "__main__":
    if len(sys.argv) > 1:
        # Allows command line parameter to specify IP and Port.
        # Example: python subscriber_client.py tcp://192.168.1.10:12345
        subscriber_ip_port = sys.argv[1]
        subscriber_client(subscriber_ip_port)
    else:
        # Defaults to localhost if no command line parameter is given
        subscriber_client()