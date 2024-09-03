The File Posses the instructions to how to Manage and Create code:

New elements:

    • A new thing that the project code will have are scriptable objects which work like any data struct that we have used before but better in a sense that we can make multiple objects of one data script and will also keep its values between scenes.
    • Enums that are useful when a scrip or object has multiple mode that will change base on the selected enum value.

Code management and adding methods:

    • The most important thing about the code method is every thing should have its own place/file if needed to make sure that the code is easy to understand and organize so as not to clutter the code base
    • Every type of controller/handaler should have its own object.
    • If we know we are going to make multiple types of the game mechanic then make a scripable object for its data fields, like I have done for the guns.
    • Make sure the code is will commented but not over commented adding comments where their is absolute need to explain the code other wise name the variables and functions in such a way so the names do the explanation.
    • Always make sure you are not adding code for the functionality in the preset functions like Start(), Update() etc instead make functions that have the code and are called in the appropriate position in the preset functions.

Examples for all of these guidelines are in the code because I have created the code by following them.