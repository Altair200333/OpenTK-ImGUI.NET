#version 330 core
out vec4 FragColor;

in vec3 FragPos;  
in vec3 Normal;  
in vec3 localV;
in vec3 camPosObj;

uniform vec3 viewPos;
uniform vec3 diffuse;
uniform int val = 1;

void main()
{  	
    // diffuse 
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);
        
    //vec3 result = max(CalcDirLight(dirLight, norm, FragPos, viewDir), 0) + 
    //max(CalcPointLight(light, norm, FragPos,viewDir),0);
    float cc = max(abs(dot(viewDir,norm)), 0.3f);
    vec3 res;
    if(val == 1) 
    {
        res = localV;
    }
    else 
    	res = diffuse;
    
    FragColor = vec4(res, 1.0);
} 