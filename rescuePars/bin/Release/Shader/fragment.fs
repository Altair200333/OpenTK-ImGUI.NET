#version 330 core
out vec4 FragColor;

in vec3 FragPos;  
in vec3 Normal;  
  
uniform vec3 viewPos;
uniform vec3 diffuse;
uniform bool checker = false;
void main()
{  	
    // diffuse 
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);
        
    //vec3 result = max(CalcDirLight(dirLight, norm, FragPos, viewDir), 0) + 
    //max(CalcPointLight(light, norm, FragPos,viewDir),0);
    float cc = max(abs(dot(viewDir,norm)), 0.3f);
    vec3 res = vec3(cc*diffuse);
    if(checker)
	    if((int(FragPos.x*10) + int(FragPos.z*10))%2==0)
	    	res*=0.1f;
    FragColor = vec4(res, 1.0);
} 
