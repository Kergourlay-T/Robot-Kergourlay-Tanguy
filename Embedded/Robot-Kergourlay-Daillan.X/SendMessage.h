#ifndef SENDMESSAGE_H
#define	SENDMESSAGE_H

#define LED_PROTOCOL 0x0020
#define TELEMETRE_PROTOCOL 0x0030
#define MOTORS_PROTOCOL 0x0040
#define STATE_PROTOCOL 0x0050
#define SET_ROBOT_STATE 0x0051
#define SET_ROBOT_MANUAL_CONTROL 0x0052
#define CHECK_INFORMATION 0x0053
#define CHECK_MODE_AUTO 0X0054
#define POSITION_DATA 0x0061
#define MESSAGE_PROTOCOL 0x0080

void SendRobotInformations(void); 
void SendPositionData(void);
   
#endif	/* SENDMESSAGE_H */

