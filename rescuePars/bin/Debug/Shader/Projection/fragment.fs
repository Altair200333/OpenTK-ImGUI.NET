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
    pos.y = pos.y * (d-1);
    pos.z = pos.z * (h-1);
    int i = int(pos.x);
    int j = int(pos.z);
    int k = int(pos.y);
    return texelFetch(VertexSampler0, i + j * w + k * w * h);
}
vec3 scaledRay(vec3 ray)
{
    ray.x = ray.x * (w-1);
    ray.y = ray.y * (d-1);
    ray.z = ray.z * (h-1);
    return ray;
}
vec4 getScaledVal(vec3 pos)
{
    int i = int(pos.x);
    int j = int(pos.z);
    int k = int(pos.y);
    return texelFetch(VertexSampler0, i + j * w + k * w * h);
}

bool inBounds(vec3 v)
{
    float tol = 0.01f;
    return v.x>=0 - tol && v.x<(w - 1 + tol) &&
         v.y>=0 - tol && v.y<=(d - 1 + tol ) && v.z>=0 - tol && v.z<=(h - 1 + tol) ;
}
vec4 BlendUnder(vec4 color, vec4 newColor)
{
	color.rgb += (1.0 - color.a) * newColor.a * newColor.rgb;
	color.a += (1.0 - color.a) * newColor.a;
	return color;
}
vec4 traverse2(vec3 v3dStart, vec3 v3dEnd)
{
    //float tmp = v3dStart.y;
    //v3dStart.y = v3dStart.z;
    //v3dStart.z = tmp;
    //tmp = v3dEnd.y;
    //v3dEnd.y = v3dEnd.z;
    //v3dEnd.z = tmp;

	float x1 = v3dStart.x + 0.5f;
	float y1 = v3dStart.y + 0.5f;
	float z1 = v3dStart.z + 0.5f;
	float x2 = v3dEnd.x + 0.5f;
	float y2 = v3dEnd.y + 0.5f;
	float z2 = v3dEnd.z + 0.5f;

	int i = int(floor(x1));
	int j = int(floor(y1));
	int k = int(floor(z1));

	int iend = int(floor(x2));
	int jend = int(floor(y2));
	int kend = int(floor(z2));

	int di = ((x1 < x2) ? 1 : ((x1 > x2) ? -1 : 0));
	int dj = ((y1 < y2) ? 1 : ((y1 > y2) ? -1 : 0));
	int dk = ((z1 < z2) ? 1 : ((z1 > z2) ? -1 : 0));

	float deltatx = 1.0f / abs(x2 - x1);
	float deltaty = 1.0f / abs(y2 - y1);
	float deltatz = 1.0f / abs(z2 - z1);

	float minx = floor(x1), maxx = minx + 1.0f;
	float tx = ((x1 > x2) ? (x1 - minx) : (maxx - x1))* deltatx;
	float miny = floor(y1), maxy = miny + 1.0f;
	float ty = ((y1 > y2) ? (y1 - miny) : (maxy - y1))* deltaty;
	float minz = floor(z1), maxz = minz + 1.0f;
	float tz = ((z1 > z2) ? (z1 - minz) : (maxz - z1))* deltatz;

	vec3 curPos = vec3( i,j,k );
    vec4 res = vec4(0,0,0,0);
	while(true)
	{
        if(!inBounds(curPos))
            break;
		
        res = BlendUnder(res, getScaledVal(curPos));
        if(res.a>1)
            return res;
		if (tx <= ty && tx <= tz)
		{
			if (i == iend) break;
			tx += deltatx;
			i += di;

			if (di == 1) curPos.x+=1;
			if (di == -1) curPos.x -= 1;
		}
		else if (ty <= tz)
		{
			if (j == jend) break;
			ty += deltaty;
			j += dj;

			if (dj == 1) curPos.y += 1;
			if (dj == -1) curPos.y -= 1;
		}
		else
		{
			if (k == kend) break;
			tz += deltatz;
			k += dk;

			if (dk == 1) curPos.z += 1;
			if (dk == -1) curPos.z -= 1;
		}
	}
	return res;
}
vec4 traverse()
{
    vec3 viewDir = localV - camPosObj;
    vec3 rayDir = normalize(viewDir);
    vec3 rayStartPos = localV + vec3(0.5f, 0.5f, 0.5f);

    vec3 start = scaledRay(rayStartPos);
    vec3 end = scaledRay(rayStartPos + rayDir * 2.0f);

    return traverse2(start, end);
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
        res = traverse();// vec4(abs(viewDir.x), 0, 0, 0.7f);
    }
    else 
    	res = vec4(diffuse, 1.0);
    
    FragColor = res;
} 
