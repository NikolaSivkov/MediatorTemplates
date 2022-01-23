# Simple Mediator templates to help save time

## Usage
> `cd path/to/command/dir`
> `dotnet new cg --typeName MyType --rootNamespace MyNameSpaceRoot`

## Docs

https://github.com/dotnet/templating/tree/main/docs

## Installation
> `dotnet new -i .\mediatorTemplates`

## Uninstallation
> `dotnet new --uninstall .\mediatorTemplates`

### Notes
`_typeName_` is used as replacement variable inside the files    
`__typeName__` is used as replacement variable in file names  
You can change both  
To add new param, insert the following in `template.json`

```json
"MyParamName": {
    "type": "parameter",
    "isRequired": true/false
},
### replace text in file
```
To have the param be replaced in files add : `replaces : "$somethingToReplace$"`  
the escape symbols are not necessary, but help with false matches, use whatever

### Replace part of file name

```json
"MyParamName": {
    "type": "parameter",
    "isRequired": true/false,
    "replaces": "__typeName__",
    "fileRename": "__typeName__"
},
```

you can also replace parts of the path if files have paths 
```json
"name": "Test",
"sourceName": "ClassName",
"PrimaryOutputs":
  [
    { "path": "Param_ProjectName.Test\\ClassNameTests.cs" },
    { "path": "Param_ProjectName\\ClassName.cs" }
  ],
 "symbols":
  {
    "projectName":
    {
      "type": "parameter",
      "replaces": "Param_ProjectName",
      "fileRename": "Param_ProjectName"
    }
}
```