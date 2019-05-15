MUSIC Tribe AR/VR Test

The basics of a audio-visual controller are in this project. The following systems have been started;

- AudioManager.cs which stores and creates Audio.cs classes for each provided AudioSource
- Audio.cs which samples a AudioSource and provides a callback for the sampled Spectrum Data
- VisualManager.cs which stores Visual.cs classes and handles assigning callbacks
- Visual.cs which is a base class defining the core behaviour
- TextureVisual.cs which extends Visual and inserts the received spectrum data into a texture.

You may perform any number of the tasks below. It is not necessary to complete all tasks! 

You will be scored on the code quality and structure.

Tasks:
- Using Polymorphism override the Visuals class to create a few varients (at least 2 new visuals) of an audio visualiser. These should integrate with the Visual.cs class in the same way as TextureVisual.cs.
-- Examples: Visual effect shown with cubes, Model surface displacement, Line Rendering etc...
- Modify the Visual.cs class to be Abstract along with any methods in Visual.cs that are empty.
- Add UI Controls for the audio (Play, Stop, Pause & Resume, Next/Previous track and Volume).
- Modify the UI to allow the user to change colour of the visual effects. This should be integrated into the base visual.cs class and compatible with all visual effects (Where possible).
- Modify the UI to allow the user to change the visual effect at runtime.
- Add any modifications that you think complement the project that show off your skills!

Please submit your test in a zipped directory, including all Git metadata, with all your work committed – git diff should return nothing.