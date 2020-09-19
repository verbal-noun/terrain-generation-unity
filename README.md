**The University of Melbourne**

# COMP30019 – Graphics and Interaction

# Project-1 README

Remember that _"this document"_ should be `well written` and formatted **appropriately**. It should be easily readable within Github. Modify this file...
this is just an example of different formating tools available for you. For help with the format you can find a guide [here](https://docs.github.com/en/github/writing-on-github).

## Table of contents

- [Team Members](#team-members)
- [General Info](#general-info)
- [Technologies](#technologies)
- [Diamond-Square implementation](#diamond-square-implementation)
- [Camera Motion](#camera-motion)
- [Vertex Shader](#vertex-shader)

## Team Members

| Name           |        Task        |       State |
| :------------- | :----------------: | ----------: |
| Kaif Ahsan     | Phong Illumination |        Done |
| Kaif Ahsan     |  Terrain Texture   | In Progress |
| Khant Thurein Han |   Water Shader (Phong)    |     Done |
| Khant Thurein Han |   Diamond Square Algorithm    |     Done |
| Hanyong Zhou |   Diamond Square Algorithm    |    Done |
| Hanyong Zhou |   Camera Motion   |    Done |


## General info

This is project - 1 ...
Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum

## Technologies

Project is created with:

- Unity 2019.4.3f1
- Ipsum version: 2.33
- Ament library version: 999

## Diamond-Square implementation

The Diamond-Square algorithm was implemented using recursion. 

You can include a code snippet here, but make sure to explain it!
Do not just copy all your code, only explain the important parts.

```c#
public class meshGenerator : MonoBehaviour
{
    //This function run once when Unity is in Play
     void Start ()
    {
      GenerateMesh();
    }

    void GenerateMesh()
    {
      .
      .
      .
    }
}
```

## Camera Motion

You can use images/gif by adding them to a folder in your repo:

<p align="center">
  <img src="Gifs/Q1-1.gif"  width="300" >
</p>

To create a gif from a video you can follow this [link](https://ezgif.com/video-to-gif/ezgif-6-55f4b3b086d4.mov).

## Wave generation

The wave uses a custom shader that uses the Phong illumination model. The wave motion is created by the shader, by adding a displacement value to the height of a flat plane with respect to its x, z, and Time values. 

<p align="center">
  <img src="Gifs/Wave-Generation.gif"  width="300" >
</p>

## Vertex Shaders

### Phong Illumination

## Sun Implementation

The sun rotation is implemented by a simple Z rotation script. By nesting the sphere and directional light in an empty game object, the sphere was set to a specfic distance relative to the parent game object. Rotating the parent object would thus rotate the sun and the direction of the light around the terrain.

<p align="center">
  <img src="Gifs/Sun-Implementation.gif"  width="300" >
</p>

**Now Get ready to complete all the tasks:**

- [x] Read the handout for Project-1 carefully
- [x] Modelling of fractal landscape
- [x] Camera motion
- [ ] Surface properties
- [ ] Project organisation and documentation
