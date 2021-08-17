#version 330 core
out vec4 FragColor;

in vec3 FragPos;  
in vec3 Normal;  
in vec3 localV;
in vec3 camPosObj;

uniform vec3 diffuse;
uniform int val = 1;
uniform samplerBuffer VertexSampler0;

uniform int w;
uniform int h;
uniform int d;

#define NUM_STEPS 500
vec4 getVal(vec3 pos)
{
    pos.x = pos.x * (w-1);
    pos.y = pos.y * (h-1);
    pos.z = pos.z * (d-1);
    int i = int(pos.x);
    int j = int(pos.y);
    int k = int(pos.z);
    return texelFetch(VertexSampler0, i + j * w + k * w * h);
}
vec4 computeColor()
{
    vec3 viewDir = localV - camPosObj;
    vec3 rayDir = normalize(viewDir);
    float stepSize = 1.732f / NUM_STEPS;

    vec3 rayStartPos = localV + vec3(0.5f, 0.5f, 0.5f);
    float tol = 0.01f;

    vec4 result = vec4(0,0,0,0);
    //return vec4(texelFetch(VertexSampler0, 0));
    for (int iStep = 0; iStep < NUM_STEPS; iStep++)
	{
        float t = iStep * stepSize;
        vec3 currPos = rayStartPos + rayDir * t;

        if (currPos.x < 0.0f - tol || currPos.x >= 1.0f + tol 
			|| currPos.y < 0.0f - tol || currPos.y > 1.0f + tol
			|| currPos.z < 0.0f - tol || currPos.z > 1.0f + tol) // TODO: avoid branch?
		{
            break;
        }
        vec4 val = getVal(currPos);
        float density = 0.003f;
        result += val * density;
    }
    return result;
    return vec4(abs(viewDir.x), 0, 0, 0.7f);
}

void main()
{  	
    
    vec4 res;
    if(val == 1) 
    {
        vec3 viewDir = localV - camPosObj;
        res = computeColor();// vec4(abs(viewDir.x), 0, 0, 0.7f);
    }
    else 
    	res = vec4(diffuse, 1.0);
    
    FragColor = res;
} 
