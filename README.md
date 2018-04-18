# Unity Image Based Lighting

This demo project is example code for my blog post about IBL [Link to article](https://medium.com/@bmind/benefits-of-image-based-lighting-on-mobile-284cda30d6d8)

Example of IBL lighting for low end mobile device, 
which allow calculate complex lights(multiple directional light, 
many point lights and etc) with only one texture sample.

Example video (check out Assets/Scenes/DemoScene):
https://youtu.be/M8Wsd4JoPyY

Workflow:
- Create or modify lighting rig in Assets/Scenes/LightingRigSendbox
- Example rigs can be found in Assets/Actors/Lighting_Rig/ folder
- Setup Assets/Sources/Data/Lighting/Lighting_Rig_Factory with MatCapSnapshot object, Renderer & Camera Ref
- Create 3D object for matcap material with "IBL/MatCap_01" shader on it
- Add LightingDataListener to the 3D object with MatCap_Snapshot reference
- Subscribe to event in Assets/Sources/Data/Lighting/MatCap_Snapshot.OnMatCapUpdate 
- Call Assets/Sources/Data/Lighting/Lighting_Rig_Factory.BuildRig() scriptable object whenever you need to bake textures

Light texture should be automatically updated now on 3D object

## Implementation notes
- It's very basic implementation with a lot of room for improvements, most of them mentioned in TODO comments
- Current version supports only CPU raymarchine
