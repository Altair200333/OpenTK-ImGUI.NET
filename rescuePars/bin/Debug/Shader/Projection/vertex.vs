#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;

out vec3 FragPos;
out vec3 Normal;
out vec3 localV;
out vec3 camPosObj;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 viewPos;

void main()
{
    localV = vec3(aPos);
    FragPos = vec3(model * vec4(aPos, 1.0));
    Normal = mat3(transpose(inverse(model))) * aNormal;  
    camPosObj = vec3(inverse(model)*inverse(view)*vec4(viewPos, 1.0));

    gl_Position = projection * view * vec4(FragPos, 1.0);
}