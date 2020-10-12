#version 460 core

layout(location = 0) in vec3 position;

layout(location = 0) out vec3 normal;
layout(location = 1) out vec3 worldPos;

uniform mat4 u_3DMVP;
uniform mat4 u_model;

void main(){
	gl_Position = u_3DMVP * vec4(position,1.0f);
	normal = normalize(mat3(u_model)*position);
	worldPos = vec3(u_model*vec4(position,1.0f));
}