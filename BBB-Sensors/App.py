from L3G4200D import *
from QueueSender import *

class App(object):
    RABBITMQ_PORT = 5672
    SERVER_QUEUE_NAME = "bbbsensors"

    def __init__(self):
        self.gyro = Gyro(self.callback)
        self.queue = QueueSender('cliftonm', 'Laranzu1', '192.168.0.3', App.RABBITMQ_PORT, App.SERVER_QUEUE_NAME)

    def callback(self, x, y, z):
        self.queue.send({"x":str(x), "y":str(y), "z":str(z)})    

if __name__ == '__main__':
    App().gyro.run()

