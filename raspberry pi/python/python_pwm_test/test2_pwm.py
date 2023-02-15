#!/usr/bin/env python3
#-- coding: utf-8 --
import RPi.GPIO as GPIO
import time


#Set function to calculate percent from angle
def angle_to_percent (angle) :
    if angle > 180 or angle < 0 :
        return False

    start = 4
    end = 12.5
    ratio = (end - start)/180 #Calcul ratio from angle to percent

    angle_as_percent = angle * ratio

    return start + angle_as_percent


GPIO.setmode(GPIO.BOARD) #Use Board numerotation mode
GPIO.setwarnings(False) #Disable warnings

#Use pin 12 for PWM signal
pwm_gpio = 12
pwm_gpio2 = 18
frequence = 50
GPIO.setup(pwm_gpio, GPIO.OUT)
GPIO.setup(pwm_gpio2, GPIO.OUT)
pwm = GPIO.PWM(pwm_gpio, frequence)
pwm2 = GPIO.PWM(pwm_gpio2, frequence)
#Init at 0°
print("set init")
pwm.start(angle_to_percent(80))
pwm2.start(angle_to_percent(80))
time.sleep(5)
#Close GPIO & cleanup
pwm.stop()
pwm2.stop()
GPIO.cleanup()
