#include <xc.h>
#include "UART_Protocol.h"
#include "CB_TX1.h"
#include "CB_RX1.h"
#include "Robot.h"
#include "IO.h"
#include "timer.h"
#include "RobotControlState.h"
#include "SendMessage.h"


// *************************************************************************/
// Fonctions correspondant au décodage et à l'encodage des messages
// *************************************************************************/

unsigned char UartCalculateChecksum(int msgFunction, int msgPayloadLength, unsigned char* msgPayload) {
    // Fonction prenant entree la trame et sa longueur pour calculer le checksum
    char checksum = 0;
    int i = 0;
    checksum ^= 0xFE;
    checksum ^= (char) (msgFunction >> 8);
    checksum ^= (char) (msgFunction >> 0);
    checksum ^= (char) (msgPayloadLength >> 8);
    checksum ^= (char) (msgPayloadLength >> 0);

    for (i = 0; i < msgPayloadLength; i++) {
        checksum ^= msgPayload[i];
    }
    return checksum;
}//End UartCalculateChecksum

void UartEncodeAndSendMessage(int msgFunction, int msgPayloadLength, unsigned char* msgPayload) {
    // Fonction d?encodage et d'envoi d?un message
    unsigned char trame[6 + msgPayloadLength];
    int pos = 0;
    int i = 0;
    trame[pos++] = 0xFE;
    trame[pos++] = (char) (msgFunction >> 8);
    trame[pos++] = (char) (msgFunction >> 0);
    trame[pos++] = (char) (msgPayloadLength >> 8);
    trame[pos++] = (char) (msgPayloadLength >> 0);

    for (i = 0; i < msgPayloadLength; i++) {
        trame[pos++] = msgPayload[i];
    }
    trame[pos++] = UartCalculateChecksum(msgFunction, msgPayloadLength, msgPayload);

    SendMessage(trame, pos);
}//End UartEncodeAndSendMessage


// *************************************************************************/
// Défintion UartDecodeMessage
// *************************************************************************/
int msgDecodedFunction = 0;
int msgDecodedPayloadLength = 0;
unsigned char msgDecodedPayload [255];
int msgDecodedPayloadIndex = 0;

typedef enum {
    Waiting,
    FunctionMSB,
    FunctionLSB,
    PayloadLengthMSB,
    PayloadLengthLSB,
    Payload,
    Checksum,
    Test
} StateReception;
StateReception rcvState;

void UartDecodeMessage(unsigned char c) {
    // Fonction prenant en entree un octet et servant a reconstituer les trames    
    switch (rcvState) {
        case Waiting:
            if (c == 0xFE) {
                rcvState = FunctionMSB;
            }
            msgDecodedFunction = 0;
            msgDecodedPayloadLength = 0;
            msgDecodedPayloadIndex = 0;
            break;

        case FunctionMSB:
            msgDecodedFunction = (int) (c >> 8)*100;
            rcvState = FunctionLSB;
            break;

        case FunctionLSB:
            msgDecodedFunction += (int) (c >> 0);
            rcvState = PayloadLengthMSB;
            break;

        case PayloadLengthMSB:
            msgDecodedPayloadLength = (int) (c >> 8)*100;
            rcvState = PayloadLengthLSB;
            break;

        case PayloadLengthLSB:
            msgDecodedPayloadLength += (int) (c >> 0);
            if ((msgDecodedPayloadLength > 255) || (msgDecodedPayloadLength < 0)) {
                rcvState = Waiting;
            } else {
                rcvState = Payload;
            }
            break;

        case Payload:
            msgDecodedPayload[msgDecodedPayloadIndex++] = c;
            if (msgDecodedPayloadIndex >= msgDecodedPayloadLength) {
                rcvState = Checksum;
            }
            break;

        case Checksum:;
            unsigned char receivedChecksum = c;
            unsigned char calculatedCheksum = UartCalculateChecksum(
                    msgDecodedFunction, msgDecodedPayloadLength, msgDecodedPayload);
            if (calculatedCheksum == receivedChecksum)
                UartProcessDecodedMessage(msgDecodedFunction, msgDecodedPayloadLength, msgDecodedPayload);
            rcvState = Waiting;
            break;

        default:
            rcvState = Waiting;
            break;
    }
}//End UartDecodeMessage


// *************************************************************************/
// Défintion UartProcessDecodedMessage
// *************************************************************************/
void UartProcessDecodedMessage(int msgFunction, int msgPayloadLength, unsigned char* msgPayload) {
    // Fonction appelee a pres le decodage pour executer l'action
    // correspondant au message recu
    switch (msgFunction) {
        case SET_ROBOT_STATE:
            if (robotAutoControl == 0){
                SetRobotState(msgPayload[0]);
                UartEncodeAndSendMessage(CHECK_INFORMATION, 1, msgPayload);
            }                
            else
                UartEncodeAndSendMessage(MESSAGE_PROTOCOL, 9,(unsigned char*)"Mode Auto");
            break;
        case SET_ROBOT_MANUAL_CONTROL:
            SetRobotAutoControlState(msgPayload[0]);
            UartEncodeAndSendMessage(CHECK_MODE_AUTO, 1, msgPayload);
            break;
        default:
            break;
    }
}//End UartProcessDecodeMessage */
