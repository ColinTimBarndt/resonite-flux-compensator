# Resonite Flux Compensator

What if ProtoFlux was compiled into static DotNet IL using the latest .NET 10 features?
This project aims to implement a ProtoFlux-like node-based programming language with this
goal in mind.

## Goals

These are the main goals of Flux Compensator

- Compile to static DotNet IL
- ProtoFlux can be translated into the Flux Compensator model
- Allocate all Locals and Stores on the stack by using a ref struct for the context
- Emit debug information such as pseudo source-code with the compiled IL for proper debugger support (such as stepping through it step by step)
- Define ProtoFlux Nodes using regular C# methods which are then transpiled and inlined
- Ability to cache compiled ProtoFlux groups as DLL+PDB files to avoid compilation hitches

## Potential Impact on Performance

Compiling ProtoFlux to static IL can massively increase performance because it..

- avoids dynamic calls: allows the CPU hardware to predict execution flow
- does not interpret: Allows the CPU to better predict branches by adding no interpreter noise to the branch predictor hardware
- keeps locals and stores on the stack: Cache locality is a big factor in improving performance

Benchmarks will be needed to empirically compare the performance to interpreted ProtoFlux.
