#version 460 core

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec3 color;

//DEPRECATED:
//colors I picked at a whim
//const vec3 vertexColors[] = vec3[](
//	vec3(0.5f,0.5f,0.5f),
//	vec3(0.3,0.6f,0.9f),
//	vec3(0.1f,0.3f,0.1f),
//	vec3(0.2f,0.3f,0.4f),
//	vec3(0.9f,0.8f,0.5f),
//	vec3(0.2f,0.2f,0.3f)
//)


layout(location = 0) out vec3 v_color;
layout(location = 1) out vec3 v_normal;
layout(location = 2) out vec3 v_worldPos;

uniform mat4 u_MVP;
uniform mat4 u_model;
//figure out if we need and how to use the view matrix for lighting effects
//say glossyness
//uniform mat4 u_view;

void main(){
	vec4 transformed = (u_MVP) * vec4(position,1.0f);
	v_normal = normalize(mat3(u_model)*normal);
	v_worldPos = vec3(u_model*vec4(position,1.0f));
	gl_Position = transformed;
	
	
	v_color = color;
}