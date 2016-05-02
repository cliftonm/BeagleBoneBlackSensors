"""

sudo apt-get update
sudo apt-get install build-essential python-dev python-setuptools python-pip python-smbus -y
sudo pip install Adafruit_BBIO
sudo pip install bitstring        
sudo pip install pika

Links: 
https://pythonhosted.org/bitstring/
https://learn.adafruit.com/setting-up-io-python-library-on-beaglebone-black/installation-on-ubuntu
https://pypi.python.org/pypi/pika

"""

import smbus
import bitstring        
import time

# For outputing padded hex strings
# def padhex(val, digits = 4):
#   return '0x%0*X' % (digits ,val)


class Gyro(object):
    ADDRESS = 0x69
    BUS1 = 1

    def __init__(self, callback):
        self.bus1 = smbus.SMBus(Gyro.BUS1)
        self.initializeDevice()
        self.callback = callback
        print "Calibrating..."
        self.calibrate();
        print "Rest Offsets: " + str(self.restOffsetX) + ", " + str(self.restOffsetY) + ", " + str(self.restOffsetZ)

    def initializeDevice(self):
        self.bus1.write_byte_data(Gyro.ADDRESS, 0x20, 0x0F)      # normal, x/y/z enabled

    def calibrate(self):
        """ Average 500 samples to get the rest state offset for each vector. """
        sampleSize = 500
        dx = 0.0
        dy = 0.0
        dz = 0.0

        for n in xrange(sampleSize):
            x, y, z = self.readRaw()
            dx += x
            dy += y
            dz += z

        self.restOffsetX = int(dx / sampleSize)
        self.restOffsetY = int(dy / sampleSize)
        self.restOffsetZ = int(dz / sampleSize)

    def readRaw(self):
        xl = self.bus1.read_byte_data(Gyro.ADDRESS, 0x28) 
        xh = self.bus1.read_byte_data(Gyro.ADDRESS, 0x29) 
        yl = self.bus1.read_byte_data(Gyro.ADDRESS, 0x2A)
        yh = self.bus1.read_byte_data(Gyro.ADDRESS, 0x2B)
        zl = self.bus1.read_byte_data(Gyro.ADDRESS, 0x2C)
        zh = self.bus1.read_byte_data(Gyro.ADDRESS, 0x2D)
        status = self.bus1.read_byte_data(Gyro.ADDRESS, 0x27)

        x = bitstring.Bits(uint = (xh << 8) | xl, length=16).unpack('int')[0]
        y = bitstring.Bits(uint = (yh << 8) | yl, length=16).unpack('int')[0]
        z = bitstring.Bits(uint = (zh << 8) | zl, length=16).unpack('int')[0]

        return x, y, z

    def readAdjusted(self):
        """ Adjust raw vectors by the rest offsets.  """
        x, y, z = self.readRaw()
        x -= self.restOffsetX
        y -= self.restOffsetY
        z -= self.restOffsetZ
        return x, y, z

    def run(self):
        while True:
            x, y, z = self.readAdjusted()
            self.callback(x, y, z)
            time.sleep(0.01)

if __name__ == '__main__':
    Gyro().run()
