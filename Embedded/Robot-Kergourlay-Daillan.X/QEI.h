#ifndef QEI_H
#define	QEI_H

#define DISTROUES 0.2812
#define DIAMETRE_ROUE 0.0426
#define NUMBER_OF_POINTS 8192
#define POINT_TO_METER (DIAMETRE_ROUE * PI / NUMBER_OF_POINTS)

void InitQEI1(void);
void InitQEI2 (void);

void QEIUpdateData(void);
void QEIReset(void);
void QEISetPosition(float xPos, float yPos, float angleRadian);


#endif	/* QEI_H */

