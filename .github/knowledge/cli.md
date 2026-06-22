---
domain: cli
related: [character-creation, rendering, systems]
---

# CLI — Auto-Populated Details

## program

- OWNS: entrypoint Main, orchestrates presets load, profile creation, render/play flows
- READS_FROM: parsed args (--seed, --race, --class, --preset, --map-width, --map-height, --viewport-width, --viewport-height, --render-once, --non-interactive)
- WRITES_TO: Console output, may call Environment.Exit for render-once
- INVARIANT: non-interactive requires both --race and --class
- FLOW: ParseArgs → load presets.json (relative path) → choose race/class/preset (interactive fallback) → PrintProfile → optionally RenderOnce or PlayLoop

## arg-parsing

- OWNS: ParseArgs supports --key, --key=value, and lookahead values; keys case-insensitive
- TENSION: simplistic parsing is adequate for current CLI but lacks short flags and validation

## render-once

- OWNS: RenderOnce mode builds RandomDungeonGenerator(seed) → Generate(map) → set up EntityManager and player → ContentSpawner.Populate → AsciiRenderer.RenderViewport → write to stdout
- DECIDED: Render-once exits immediately to support CI and redirected I/O

## play-loop

- OWNS: continuous loop reading Console.ReadKey, mapping to InputCommand, creating IntentComponent on player and resolving via UpdateEngine.ResolveTurn
- READS_FROM: keyboard input, viewport/map sizes, RNG seed
- WRITES_TO: console buffer via AsciiRenderer, MessageLog for event messages
- TENSION: interactive terminal handling (SetCursorPosition) complicates headless testing; render-once and render-once mode exist as mitigations

## presets-loading

- OWNS: loads presets.json via PresetLoader.LoadFromJson using AppContext.BaseDirectory relative path
- INVARIANT: missing presets file falls back to empty preset list
- INVARIANT: presets path is resolved relative to AppContext.BaseDirectory (..\\..\\..\\src\\Eldritch.Core\\Data\\presets.json)
- FLOW: Load file → deserialize → present choices in interactive mode → apply chosen preset to profile
## program

- OWNS: command-line entrypoint, argument parsing, interactive vs non-interactive flows
- READS_FROM: args (--seed, --race, --class, --preset, --render-once, etc.), presets file on disk
- WRITES_TO: console output, may launch PlayLoop or RenderOnce
- INVARIANT: non-interactive mode requires --race and --class
- FLOW: ParseArgs → load presets → decide interactive/noninteractive → create profile → PrintProfile → optionally PlayLoop or RenderOnce
- DECIDED: Render-once mode exits immediately after writing output to support CI and redirected I/O

## arg-parsing

- OWNS: ParseArgs utility that supports --key or --key=value and lookahead values
- INVARIANT: keys are case-insensitive

## play-loop / render-once

- OWNS: simple terminal-based rendering and input loop; uses UpdateEngine to resolve turns
- READS_FROM: RNG seed, viewport/map sizes
- WRITES_TO: console buffer via AsciiRenderer
- TENSION: interactive console requires cursor control; render-once exists to support CI and piping

## presets-loading

- OWNS: loading presets.json from src path (relative to AppContext.BaseDirectory)
- READS_FROM: presets.json via PresetLoader.LoadFromJson
- INVARIANT: presets are optional; missing file falls back to empty presets
