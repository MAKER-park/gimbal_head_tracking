#include <wiringPi.h>
#include <stdio.h>
#include <stdlib.h>


const int BCM12_PWM0 = 12;   // pwm 핀 선언
const int BCM18_PWM0 = 13;
//const int BCM13_PWM1 = 18;

int main(){
	int intensity;
 	wiringPiSetupGpio();      // BCM 핀번호 사용
	pwmSetMode(PWM_MODE_MS);  // 마크-스페이스 모드 사용
	pinMode(BCM12_PWM0, OUTPUT);   // 일반 GPIO 핀으로 설정
	pinMode(BCM18_PWM0, OUTPUT);   // 일반 GPIO 핀으로 설정
	//pinMode(BCM13_PWM1, OUTPUT);   // 일반 GPIO 핀으로 설정
	
	softPwmCreate(BCM12_PWM0, 1, 100);   // SOFTWARE PWM에서 사용할 범위 지정, 여기서는 0-100
	softPwmCreate(BCM18_PWM0, 1, 100);
	//softPwmCreate(BCM13_PWM1, 1, 100);

	while(1){
	
		for(intensity=0; intensity<100; ++intensity){
			softPwmWrite(BCM12_PWM0, intensity);	// PWM 출력
			softPwmWrite(BCM18_PWM0, 100-intensity);	// PWM 출력
			//softPwmWrite(BCM13_PWM1, intensity);	// PWM 출력

			delay(10);

		}	

		
		for(intensity=100; intensity>=0; --intensity){
			softPwmWrite(BCM12_PWM0, intensity);	
			softPwmWrite(BCM18_PWM0, 100-intensity);	
			//softPwmWrite(BCM13_PWM1, intensity);	
			delay(10);

		}	
		delay(1);
	
	
	} 

	return 0;
}

