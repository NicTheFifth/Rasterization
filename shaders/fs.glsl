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
vec3 lightpos;
vec3 lightcolour;

// fragment shader
void main()
{

    vec3 lightpos =vec3(1,0,0)*  light;
    vec3 lightcolour =  vec3(0,1,0)*light;
   
    vec3 L = lightpos - worldPos.xyz;
    float dist = L.length();
    L = normalize( L );
    vec3 materialColor = texture( pixels, uv ).xyz;
    float attenuation =1.0f / (dist*dist);
    outputColor = vec4( materialColor * max( 0.0f, dot( L, normal.xyz ) ) * attenuation * lightcolour, 1 );
}