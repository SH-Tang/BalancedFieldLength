# Migrating to the SDK Project Format

## Context and Problem Statement
The current projects are defined in the legacy project format which make the solution bloated. A new format was proposed by Microsoft which is lighter and contains less boiler plating. This new format is the so called SDK project format. Additionally, this new format will also enable the possibility to use the `dotnet` command to build and run unit tests from the dotnet core CLI.

## Considered options

The following options are considered:

### Do nothing

**Pros:**
* No effort needs to be spent to convert the `*.csproj` files to the new file format

**Cons:**
* Heavily dependent on `MSBuild` and thus less platform independent
* Risk of merge conflicts due to many lines in the project format
* Does not allow in-situ modifications, project must be unloaded before the `*.csproj` file can be modified
* Tight coupling with the file locations. Although files are specified with relative paths, there still exists a tight coupling with the files that are part of the project. This coupling might prevent the (partial) reuse of components in another application.

### Converting the files to the SDK file format

**Pros:**
* Enables the solution to be compiled with the dotnet cli and making it more platform independent. The `dotnet` CLI operatibility also makes the solution more suitable to work in combination with Github Actions (see [ADR-00012](00012-Selecting-NUnit-test-logger-to-report-results-in-GithubActions.md))
* Leaner file format which prevents the risk of merge conflicts
* No references with the files that are part of the project. Source files are automatically recognised from the directory

**Cons:**
* More investigation is necessary to regarding the compatibility of components with the new framework

A proof of concept was deployed to investigate the conversion of the project files. The effort was relatively low as `dotnet` provides a tool to convert the legacy projects. There are, however, a few points that should be taken into account while converting these projects. These are further elaborated in Section 'Instructions to migrate Legacy Projects to the SDK file format'.

## Decision outcome

The proof of concept shows that almost no effort is required to (fully) convert the legacy projects to the new file format. Due to the advantages the new format provides, it was decided to continue the conversion and finalise the proof of concept for production.

## Resources

* [Moving to SDK style projects and package references in Visual Studio Part 1](http://hermit.no/moving-to-sdk-style-projects-and-package-references-in-visual-studio-part-1/)
* [Moving to SDK style projects and package references in Visual Studio Part 2](http://hermit.no/moving-to-sdk-style-projects-and-package-references-in-visual-studio-part-2/)

## Instructions to migrate Legacy Projects to the SDK file format

The `dotnet` cli provides a tool to perform the conversion in an automatic way. to enable the tool:

```
dotnet tool install --global Project2015To2017.Migrate2019.Tool
```

When invoking the following command, the solution will be migrated in an interactive manner:
```
dotnet migrate-2019 wizard <Project>.sln
```

The issues that were encountered during this process are documented in the subsequent sections.

### .NET target version
Due to the use of a `.target` file, the tool is unable to determine which framework it needs to set for the projects. Also keep in mind that the framework tag is changed and needs to be updated:

```
<TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
```
to
```
<TargetFramework>net472</TargetFramework>
```
The tags that are present after the migration can be safely removed afterwards once the `*.targets` file is accordingly updated.

### The use of PropertyGroups for each build configuration
The dotnet file format provides prefixed configurations for the debug and the release build configuration, potentially rendering the following statements in the `*.targets` file obsolete:

```
<PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
</PropertyGroup>
<PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
</PropertyGroup>
```

To still use these configuration settings, the following package must be included:

```
<ItemGroup>
    <PackageReference Include="MSBuildConfigurationDefaults" Version="1.0.1" />
</ItemGroup>
```

### Migrating assemblies that contain WPF components / XAML View files
After migration, these project files will contain the following statements for each view component:
```
<Compile Update="View.xaml.cs">
      <DependentUpon>View.xaml</DependentUpon>
</Compile>
....
<Page Include="View.xaml">
      <Generator>MSBuild:Compile</Generator>
      <SubType>Designer</SubType>
</Page>
```
The affected assembly will compile without issues in Visual Studio, but will generate an error when trying to compile with the dotnet CLI. To be able to compile with the CLI, delete the aforementioned statement and make sure that the root element contains:

```
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
  <PropertyGroup>
    <UseWPF>true</UseWPF>
```
### Assemblies containing the GeneralAssemblyInfo
The assembly which contains the `GeneralAssemblyInfo.cs` might generate an error due to duplicate attribute definitions. This compilation error is caused due to an autogenerated `AssemblyInfo.cs` when building. This generation can be easily disabled with the tag: 
```
<GenerateAssemblyInfo>false</GenerateAssemblyInfo>
``` 