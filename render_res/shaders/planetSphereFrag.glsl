#version 460 core

layout(location = 0) in vec3 normal;
layout(location = 1) in vec3 worldPos;

layout(location = 0) out vec4 color;

//TODO: make this a uniform or more than one uniform
const vec3 light_pos = vec3(-3.0f,0.0f,0.0f);

const vec3 camara_orientation = vec3(0.0f,0.0f,-1.0f);


void main(){
	//distance to light
	float dtl = distance(worldPos, light_pos);

	//subject to change
	float attenuation = (9/(dtl*dtl));
	//float attenuation = 1;

	vec3 light_dir = normalize(worldPos-light_pos);
	//vec3 light_dir = vec3(1.0f,0.0f,0.0f);


	//diffuse lighting
	vec3 flat_color = vec3(0.2f,0.2f,0.7f);
	vec3 diffuseColor = flat_color;
	vec3 glossColor = flat_color;

	

	//we calculate the final colour with 2 multiplications,
	//first the diffuse times the angle of incidence of the light upon the normal
	//throught the dot product to make it so it is cos(angle)
	//and then we apply a scaling function with a parameter of distance from light source
	//intensity of the light is thus inversely proportional to the distance.
	vec3 final_diffuse = diffuseColor*max(dot(light_dir,-normal),0.0f)*attenuation;
	

	//glossy lighting
	vec3 reflectVector = light_dir-2*(min(dot(light_dir,normal),0.0f))*normal;
	//uses a hardcoded camara "view" to calculate, its pointing towards the -z axis.
	//TODO: implement a non-fixed non-hardcoded camara with some data from the vertex shader, that in turn is sent from the dll.
	vec3 final_gloss = glossColor*max(dot(reflectVector, -camara_orientation),0.0f)*attenuation;
	//vec3 reflectVector = reflect(light_dir,normal);
	


	//ambient amount of light
	vec3 ambient = vec3(0.2f,0.2f,0.2f);
	//then we add the ambient lighting by adding the multiplication of it by the diffuse colour to
	//and we also add gloss
	//the final result
	vec3 final_color = clamp(final_diffuse+(diffuseColor*ambient)+final_gloss,0.0f,1.0f);
	color = vec4(final_color,1.0f);
	
}