using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLinInterpolationUtil
{
    public enum EaseType{None,Quadratic}
 public static float getQuadratic(float start,float end,float pos){
    return Mathf.Lerp(start,end,Mathf.Pow(pos,2));
 }
 public static float Interpolate(float start,float end,float pos,EaseType easeIn,EaseType easeOut){

  return Mathf.Lerp(GetEase(start,end,pos,easeIn),1-GetEase(start,end,1-pos,easeOut),pos);
        
    

 }
 public static float Interpolate(float t,EaseType ei,EaseType eo){
    return Interpolate(0,1,t,ei,eo);
 }
 public static float GetEase(float start, float end,float t, EaseType e){
    switch(e){
        case EaseType.None:
            return Mathf.Lerp(start,end,t);
        case EaseType.Quadratic:
            return getQuadratic(start,end,t);

    }
    return Mathf.Lerp(start,end,t);
 }



}
