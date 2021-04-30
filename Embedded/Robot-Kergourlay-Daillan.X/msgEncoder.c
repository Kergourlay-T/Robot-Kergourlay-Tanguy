#include "msgEncoder.h"
#include "CB_TX1.h"
#include "Protocol.h"

unsigned char UartCalculateChecksum(unsigned char msgFunction,
        unsigned int msgPayloadLenght, unsigned char* msgPayload) {
    unsigned char payload[6 + msgPayloadLenght];
    EncodeWithoutChecksum(payload, msgFunction, msgPayloadLenght, msgPayload);
    unsigned char checksum = payload[0];
    unsigned int i;
    for (i = 1; i < 5 + msgPayloadLenght; i++) {
        checksum ^= payload[i];
    }
    return checksum;
}

void EncodeWithoutChecksum(unsigned char * payload, unsigned char msgFunction,
        unsigned int msgPayloadLength, unsigned char* msgPayload) {
    payload[0] = SOF_BYTE;
    payload[1] = (unsigned char) (msgFunction >> 8);
    payload[2] = (unsigned char) (msgFunction >> 0);
    payload[3] = (unsigned char) (msgPayloadLength >> 8);
    payload[4] = (unsigned char) (msgPayloadLength >> 0);

    int i;
    for (i = 0; i < msgPayloadLength; i++) {
        payload[5 + i] = msgPayload[i];
    }
}

void UartEncodeAndSendMessage(unsigned char msgFunction,
        unsigned int msgPayloadLenght, unsigned char* msgPayload) {
    unsigned char payload[6 + msgPayloadLenght];
    EncodeWithoutChecksum(payload, msgFunction, msgPayloadLenght, msgPayload);
    unsigned char checksum = UartCalculateChecksum(msgFunction, msgPayloadLenght, msgPayload);
    payload[5 + msgPayloadLenght] = checksum;
    SendMessage(payload, 6 + msgPayloadLenght);
}