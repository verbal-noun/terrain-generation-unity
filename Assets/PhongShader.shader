// Implementation of the Phong Illumination Model 

Shader "PhongShader" {

    Properties {
        // Colour of our object - White by default 
        _Colour ("Colour", Color) = (1, 1, 1, 1)
        
        // Shininess 
        _Shininess ("Shininess", Float) = 10 
        // Hightlights colour 
        _SpecColour ("Specular Colour", Color) = (1, 1, 1, 1) 
    }

    SubShader {
        
    }
}
