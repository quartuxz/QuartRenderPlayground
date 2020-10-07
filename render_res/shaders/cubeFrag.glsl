#version 460 core


layout(location = 0) in vec3 flat_color;

layout(location = 0) out vec4 color;



void main(){
	color = flat_color;
}