#include "Toolbox.h"
#include<math.h>

float Abs(float value) {
    if (value >= 0)
        return value;
    else return -value;
}

float Max(float value, float value2) {
    if (value > value2)
        return value;
    else
        return value2;
}

float Min(float value, float value2) {
    if (value < value2)
        return value;
    else
        return value2;
}

float MinDistance(float *value) {
    int min = value[0];
    int i;
    for (i = 0; i < 5; i++) {
        if (value[i] < min;) {
            min = value[i];
        }
    }
    return min;
}

float LimitToInterval(float value, float lowLimit, float highLimit) {
    if (value > highLimit)
        value = highLimit;
    else if (value < lowLimit)
        value = lowLimit;
    return value;
}

double racine_cubique(double x) {
    return pow(x, 1.0 / 3.0);
}

