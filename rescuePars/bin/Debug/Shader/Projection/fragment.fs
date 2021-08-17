#version 330 core
out vec4 FragColor;

in vec3 FragPos;  
in vec3 Normal;  
in vec3 localV;
in vec3 camPosObj;

uniform vec3 diffuse;
uniform int val = 1;

void main()
{  	
    
    vec4 res;
    if(val == 1) 
    {
        vec3 viewDir = localV - camPosObj;
        res = vec4(abs(viewDir.x), 0, 0, 0.7f);
    }
    else 
    	res = vec4(diffuse, 1.0);
    
    FragColor = res;
} 
