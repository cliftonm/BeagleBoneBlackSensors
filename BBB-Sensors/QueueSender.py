import pika

class QueueSender(object):
    """Send RabbitMQ messages directed to a specific queue."""

    # create a queue and register it by name in outputQueues
    def __init__(self, username, password, ipAddress, port, queueName):
        self.username = username
        self.password = password
        self.ipAddress= ipAddress
        self.port = port
        self.queueName = queueName
        self.initialize()

    def initialize(self):
        self.credentials = pika.PlainCredentials(self.username, self.password)
        self.connection = pika.BlockingConnection(pika.ConnectionParameters(self.ipAddress, self.port, '/', self.credentials))
        self.channel = self.connection.channel()

    def send(self, msg):
        try:
            self.channel.basic_publish(exchange='', routing_key=self.queueName, body=str(msg))
        except:
            print("Exception on send.  Re-establishing connection...")
            self.initialize()
            try:
                self.channel.basic_publish(exchange='', routing_key=self.queueName, body=str(msg))
            except:
                print("Unable to send!")




