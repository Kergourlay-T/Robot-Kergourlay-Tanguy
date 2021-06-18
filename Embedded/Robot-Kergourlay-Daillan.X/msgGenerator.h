#ifndef MSGGENERATOR_H
#define	MSGGENERATOR_H

void GenerateLEDMessage(int nbLED, int LEDState);
void GenerateTelemeterMessage(void);
void GenerateMotorConsigneMessage(void);
void GenerateMotorMeasuredMessage(void);
void GenerateMotorErrordMessage(void);
void GenerateRobotStateMessage(unsigned char stateRobot);
void GenerateManualControlMessage(int manualControl);
void GeneratePositionData(void);
void GenerateTextMessage(unsigned char* message, unsigned int lenght); 

void GenerateSpeedPolarOdometryMessage();
void GenerateSpeedPolarGainsMessage();
void GenerateSpeedPolarCorrectionsMessage();
void GenerateSpeedPolarLimitGainsMessage() ;

#endif	/* MSGGENERATOR_H */

