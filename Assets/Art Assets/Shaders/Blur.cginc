void Blur_float(UnityTexture2D Tex, float2 UV, float BlurScale, out float4 Blur){
    float4 col = float4(0.0, 0.0, 0.0, 0.0);
    float kernelSum = 0.0;

    int upper = ((BlurScale - 1) / 2);
    int lower = -upper;

    for(int x = lower; x <= upper; x++){
        for(int y = lower; y <= upper; y++){
            kernelSum++;

            float2 offset = float2(Tex.texelSize.x * x, Tex.texelSize.y * y);
            col += SAMPLE_TEXTURE2D(Tex, Tex.samplerstate, UV + offset);
        }
    }
    col /= kernelSum;

    Blur = col;
}

void Blur_half(UnityTexture2D Tex, half2 UV, half BlurScale, out half4 Blur) {
    half4 col = half4(0.0, 0.0, 0.0, 0.0);
    half kernelSum = 0.0;

    int upper = ((BlurScale - 1) / 2);
    int lower = -upper;

    for (int x = lower; x <= upper; x++) {
        for (int y = lower; y <= upper; y++) {
            kernelSum++;

            half2 offset = half2(Tex.texelSize.x * x, Tex.texelSize.y * y);
            col += SAMPLE_TEXTURE2D(Tex, Tex.samplerstate, UV + offset);
        }
    }
    col /= kernelSum;

    Blur = col;
}