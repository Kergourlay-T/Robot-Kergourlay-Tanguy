#ifndef MSGPROCESSOR_H
#define	MSGPROCESSOR_H

void UartProcessDecodedMessage(unsigned char function,
        unsigned char payloadLength, unsigned char* payload);

#endif	/* MSGPROCESSOR_H */

