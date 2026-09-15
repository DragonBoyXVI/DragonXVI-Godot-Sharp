### DragonXVI Sharp Godot

This is a modified standalone module from DragonXVI Sharp.
It offers all the same functionality of the original, plus specific services for Godot.

Please note that unlike the previous version of this, the scripts here are NOT formated for GDScript access.

Also note that this is intended to be added as a git submodule, just like its parent repo.
Submodules act odd in godot, in that thier .imports are never tracked and are reimported on every new install.
If you wish is circumvent this, you can simply copy this repos files and place them in your project.

## HOW TO INSTALL
Besides downloading and dropping into a project you can add this as a git submodule:
```` git submodule add <link-to-this-repo>'

Please note that however you choose to install this, you will need to enable nullable in your .csproj.
<PropertyGroup>
    <Nullable>enable</Nullable>


### NOTE TO FUTURE ME
It would have been significantly easier to create this as a sub folder in a godot project with its own isolated git intsead of
trying to create this completely divorced from godot. I will be using this info moving on.
