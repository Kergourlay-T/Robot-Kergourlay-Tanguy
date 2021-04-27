#ifndef QEI_H
#define	QEI_H

#define DISTROUES 281.2
#define FREQ_ECH_QEI 0 //à définir

void InitQEI1(void);
void InitQEI2 (void);

void QEIUpdateData(void);
void SendPositionData(void);


#endif	/* QEI_H */

