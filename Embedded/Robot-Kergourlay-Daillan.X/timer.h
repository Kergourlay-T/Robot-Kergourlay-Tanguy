#ifndef TIMER_H
#define TIMER_H

extern unsigned long timestamp;

#define FREQ_ECH_QEI 250
#define FCY 40000000

void InitTimer23(void);
void InitTimer1(float freq);
void InitTimer4(float freq);

#endif /* TIMER_H */