﻿#version 330

out vec4 FragColor;
out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;
uniform sampler2D texture1;

void main()
{
    outputColor = texture(texture0, texCoord);
    FragColor = mix(texture(texture0, texCoord), texture(texture1, texCoord), 0.2);
}