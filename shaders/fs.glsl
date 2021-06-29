#version 330
 
// shader input
in vec2 uv;            // interpolated texture coordinates
in vec4 normal;            // interpolated normal
in vec4 worldPos;

uniform vec3 ambientColour; // ambient colour 
uniform sampler2D pixels;    // texture sampler
uniform mat3 light;

// shader output
out vec4 outputColor; 

// fragment shader
void main()
{
    vec3 lightpos =vec3(1,0,0) * light;
    vec3 lightcolour =  vec3(0,1,0) * light;
    vec3 lightspec = vec3(0,0,1) * light;
    vec3 materialColor = texture( pixels, uv ).xyz;

    vec3 rv = vec3(0,0,0);
    vec3 l = lightpos - worldPos.xyz;
    float dist = length(l);
    vec3 lnorm = l/dist;
    
    float attenuation =1.0f / (dist*dist);

    float dif = dot(normal.xyz, lnorm);
    if(dot(lnorm,normal.xyz)>0)
    rv = -lnorm + 2*dot( lnorm,normal.xyz ) * normal.xyz;

    float spec = dot(lnorm, rv);

    outputColor = 0.1f*texture( pixels, uv); //background light
    outputColor += 0.9f*vec4( materialColor * max( 0.0f ,dot( lnorm, normal.xyz) ) * attenuation * lightcolour+vec3(1,1,1) * pow( max( 0.0f , spec), 1.0f) *attenuation* lightspec, 1.0f); //blin phong shading
}