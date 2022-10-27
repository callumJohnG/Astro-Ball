#define H 255

void GameBoyColor_float(float3 col, float3 highCol, float3 highMidCol, float3 midCol, float3 lowMidCol,float3 lowCol, out float3 Out){
    float bound1 = 0.80;
    float bound2 = 0.6;
    float bound3 = 0.4;
    float bound4 = 0.2;

    
    
    if(col.r > bound1){
        //High screen colour palette
        //col = half3(202./H, 220./H, 159./H);
        col = highCol;
    }

    else if(col.r <= bound1 && col.r > bound2){
        //High middle screen colour palette
        //col = half3(139./H, 172./H, 15./H);
        col = highMidCol;
    }

    else if (col.r <= bound2 && col.r < bound3){
        //Middle screen colour palette
        //col = half3(48./H, 98./H, 48./H);
        col = midCol;
    }

    else if (col.r <= bound3 && col.r < bound4){
        //Middle screen colour palette
        //col = half3(48./H, 98./H, 48./H);
        col = lowMidCol;
    }

    else {
        //Low screen colour palette
        //col = half3(15./H, 56./H, 15./H);
        col = lowCol;
    }

    Out = col;
}