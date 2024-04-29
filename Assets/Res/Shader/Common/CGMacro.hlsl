#ifndef CG_MACRO_INCLUDE
#define CG_MACRO_INCLUDE

#define COMPUTE_EYEDEPTH(o) o = -mul(UNITY_MATRIX_MV, v.vertex).z
#define fixed half
#define fixed2 half2
#define fixed3 half3
#define fixed4 half4

#endif