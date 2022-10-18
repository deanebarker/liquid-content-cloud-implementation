# Implementation Code for Liquid Content Cloud

1. Create a "Templates" folder in the Multi-Channel Content Gadget. Note the ID.

2. Create a "Templates" folder in the Media tree. Note the ID.

3. Create a folder on the file system for the Liquid templates. Note the path.

3. The code in `Startup.cs.txt` should be added to your `Startup.cs` file. Change the IDs and paths as necessary.

4. Adjust the values in the constructors passed to `MultiSourceTemplateProvider` (or, delete ones you don't want to use)

4. Change the `ClientEditor` in `TemplateBlock.cs` to match the path of `poor-mans-code-editor.js` (wherever you end up putting it)

5. Compile

The folder paths under the two `Templates` folders should match where the temlpates would be on the file system. For example: `Templates/Shared/Blocks/MyBlock.liquid`. The "filename" should be the name of whatever content object is acting as the template (block or media asset).
