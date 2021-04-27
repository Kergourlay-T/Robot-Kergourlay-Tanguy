#ifndef UART_PROFILE_H
#define	UART_PROFILE_H

unsigned char UartCalculateChecksum(int msgFunction, int msgPayloadLength, unsigned char* msgPayload);
void UartEncodeAndSendMessage(int msgFunction, int msgPayloadLength, unsigned char* msgPayload);
void UartDecodeMessage(unsigned char c);
void UartProcessDecodedMessage(int msgFunction, int msgPayloadLength, unsigned char* msgPayload);

void SetRobotState(unsigned char ReceivedSetRobotState);
void SetRobotAutoControlState(unsigned char SetRobotControlState);
void SendRobotInformations(void);

#endif	/* UART_PROFILE_H */

