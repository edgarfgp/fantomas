---
category: Contributors
categoryindex: 2
index: 3
---
# Getting started

Fantomas has a fairly straightforward setup.

## Recommended workflow

We recommend the following overall workflow when developing for this repository:

* Fork this repository
* Always work in your fork
* Always keep your fork up to date

Before updating your fork, run this command:

```shell
git remote add upstream https://github.com/fsprojects/fantomas.git
```

This will make management of multiple forks and your own work easier over time.

## Updating your fork

We recommend the following commands to update your fork:

```shell
git checkout main
git clean -xdf
git fetch upstream
git rebase upstream/main
git push
```

Or more succinctly:

```shell
git checkout main && git clean -xdf && git fetch upstream && git rebase upstream/main && git push
```

This will update your fork with the latest from `fsprojects/fantomas` on your machine and push those updates to your remote fork.

## Preview SDK

In this repository, we wish to dog-food the latest F# compiler features. Compiler flag [--test:GraphBasedChecking](https://github.com/dotnet/fsharp/pull/14494) in particular.  
You can download a preview SDK from [here](https://github.com/dotnet/installer#installers-and-binaries).  
Make sure to install the `Release/7.0.4xx` SDK.

## Initial build

After cloning the repository, you should restore the local dotnet tools and the solution:

```shell
dotnet tool restore
dotnet restore
```

The restore of project `Fantomas.FCS` (via the solution restore) will download the F# compiler source code.

Afterwards, you can run the default build script.
This will build the solution, run all unit tests and do everything that the CI build does.

```shell
dotnet fsi build.fsx
```

Alternately, you can also run some other pipelines using `-p`.
Examples:

- `dotnet fsi build.fsx -p FormatChanged` will format all modified files detected by `git`.
- `dotnet fsi build.fsx -p Docs` will serve the documentation website locally.
-  `dotnet fsi build.fsx -p EnsureRepoConfig` sets up some git repo-level configuration to ensure
that formatting of new code is consistent before it is pushed up to a remote repository.

<fantomas-nav previous="{{fsdocs-previous-page-link}}" next="{{fsdocs-next-page-link}}"></fantomas-nav>