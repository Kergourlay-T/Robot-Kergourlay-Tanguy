#ifndef TIMER_H
#define TIMER_H

extern unsigned long timestamp;

#define FREQ_ECH_QEI 250
#define FCY 40000000

void InitTimer23(void) ;
void InitTimer1(void) ;
void InitTimer4(void) ;
void SetFreqTimer1 (float);
void SetFreqTimer4 (float);

#endif /* TIMER_H */