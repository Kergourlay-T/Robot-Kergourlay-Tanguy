#ifndef MSGENCODER_H
#define	MSGENCODER_H

unsigned char UartCalculateChecksum(unsigned char msgFunction, 
        unsigned int msgPayloadLenght, unsigned char* msgPayload);
void EncodeWithoutChecksum(unsigned char* payload, unsigned char msgFunction, 
        unsigned int msgPayloadLength, unsigned char* msgPayload);
void UartEncodeAndSendMessage(unsigned char msgFunction,
        unsigned int msgPayloadLenght, unsigned char* msgPayload);
  
#endif	/* MSGENCODER_H */

