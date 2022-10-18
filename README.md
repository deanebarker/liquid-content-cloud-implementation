# Implementation Code for Liquid Content Cloud

1. Create a "Templates" folder in the Multi-Channel Content Gadget. Note the ID.

2. Create a "Templates" folder in the Media tree. Note the ID.

3. Create a folder on the file system for the Liquid templates. Note the path.

3. The code in `Startup.cs.txt` should be added to your `Startup.cs` file. Change the IDs and paths as necessary.

4. Adjust the values in the constructors passed to `MultiSourceTemplateProvider` (or, delete ones you don't want to use)

4. Change the `ClientEditor` in `TemplateBlock.cs` to match the path of `poor-mans-code-editor.js` (wherever you end up putting it)

5. Compile

The folder paths under the two `Templates` folders should match where the temlpates would be on the file system. For example: `Templates/Shared/Blocks/MyBlock.liquid`. The "filename" should be the name of whatever content object is acting as the template (block or media asset).

## ITemplateSourceProvider

To implement your own template source provider, create that a class that implements `ITemplateSourceProvider`.

Implement `string GetSource(string path)`.

The `path` is the path to the view the engine wants. This will be something like `Shared/Blocks/MyBlock.liquid`.

Perform literally any logic you like in here, and return either (1) a string representing the Liquid code, (2) `null` to indicate you can't provide a template, in which case the `MultiSourceTemplateProvider` will move to the next provider.

## Template Caching

The MVC implementation of Fluid has a template caching architecture built-in.

`TemplateCacheManager.Clear()` will invalidate the _entire_ template cache.

It has an event capturing implementation that will call `Clear` on `PublishingContent` and `MovedContent` events that involve a `TemplateBlock` or `TemplateFile`
