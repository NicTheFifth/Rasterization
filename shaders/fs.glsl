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

    vec3 l = lightpos - worldPos.xyz;
    float dist = l.length();
    vec3 lnorm = normalize( l );
    
    float attenuation =1.0f / (dist*dist);

    float dif = dot(normal.xyz, lnorm);
    vec3 rv = -l + 2*max( 0.0f , dif ) * normal.xyz;
    float spec = dot(l, rv);

    outputColor = vec4(vec3(0.05f,0.05f,0.05f), 1); //background light
    outputColor += vec4( materialColor * max( 0.0f , dif ) * attenuation * lightcolour, 1 ); //lambertian
    outputColor += vec4(vec3(1,1,1) * pow( max( 0.0f , spec), 10) * lightspec, 1); //blin phong shading
}