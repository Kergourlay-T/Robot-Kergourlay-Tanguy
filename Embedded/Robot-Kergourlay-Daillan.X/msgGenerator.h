#ifndef MSGGENERATOR_H
#define	MSGGENERATOR_H

void GenerateLEDMessage(int nbLED, int LEDState);
void GenerateTelemeterMessage(void);
void GenerateMotorMessage(void);
void GenerateRobotStateMessage(unsigned char stateRobot);
void GenerateManualControlMessage(int manualControl);
void GeneratePositionData(void);
void GenerateTextMessage(unsigned char* message, unsigned int lenght); 

#endif	/* MSGGENERATOR_H */

