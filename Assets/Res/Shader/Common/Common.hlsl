#ifndef COMMON_HLSL_INCLUDE
#define COMMON_HLSL_INCLUDE

//TODO:考虑对负数取模的情况?
//TODO:希望余数和除数正负性相同?
int imod(int x, int a)
{
    x += max(-x, 0) / a * a + a;
    return x % a;
}

int2 imod(int2 x, int2 a)
{
    x += max(-x, 0) / a * a + a;
    return x % a;
}

int3 imod(int3 x, int3 a)
{
    x += max(-x, 0) / a * a + a;
    return x % a;
}

int4 imod(int4 x, int4 a)
{
    x += max(-x, 0) / a * a + a;
    return x % a;
}


#endif