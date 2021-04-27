#ifndef TIMER_H
#define TIMER_H

void InitTimer23(void);
void InitTimer1(float freq);
void InitTimer4(float freq);

void __attribute__((interrupt, no_auto_psv)) _T4Interrupt(void);
void __attribute__((interrupt, no_auto_psv)) _T1Interrupt(void);
void __attribute__((interrupt, no_auto_psv))_T3Interrupt(void);


extern unsigned long timestamp;

#endif /* TIMER_H */