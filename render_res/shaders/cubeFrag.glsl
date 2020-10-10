#version 460 core


layout(location = 0) in vec3 flat_color;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec3 worldPos;

layout(location = 0) out vec4 color;

//const vec3 light_dir = vec3(1.0f,0.0f,0.0f);

const vec3 light_pos = vec3(-10.0f,0.0f,0.0f);

void main(){
	
	float dtl = distance(worldPos, light_pos);

	float attenuation = (10/dtl);

	vec3 light_dir = normalize(worldPos-light_pos);


	//diffuse lighting
	vec3 diffuseColor = vec3(0.8f,0.3f,0.3f);
	//vec3 diffuseColor = flat_color;
	//distance to light
	

	//we calculate the final colour with 2 multiplications,
	//first the diffuse times the angle of incidence of the light upon the normal
	//throught the dot product to make it so it is cos(angle)
	//and then we apply a scaling function with a parameter of distance from light source
	//intensity of the light is thus inversely proportional to the distance.
	vec3 final_diffuse = diffuseColor*max(dot(light_dir,-normal),0.0f)*attenuation;
	
	//glossy lighting
	vec3 final_gloss = diffuseColor;
	vec3 reflectVector = light_dir-2*(min(dot(light_dir,normal),0.0f))*normal;
	//uses a hardcoded camara "view" to calculate, its pointing towards the -z axis.
	final_gloss *= max(dot(reflectVector, -vec3(0.0f,0.0f,-1.0f)),0.0f)*attenuation;
	//vec3 reflectVector = reflect(light_dir,normal);
	
	//ambient amount of light
	vec3 ambient = vec3(0.2f,0.2f,0.2f);
	//then we add the ambient lighting by adding the multiplication of it by the diffuse colour to
	//and we also add gloss
	//the final result
	vec3 final_color = clamp(final_diffuse+(diffuseColor*ambient)+final_gloss,0.0f,1.0f);
	color = vec4(final_color,1.0f);
}