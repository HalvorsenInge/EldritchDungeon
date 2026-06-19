# Copilot Instructions — Knowledge Read/Write Gates

> **Knowledge files — read before editing, write after learning:**
>
> - **Read gate:** Before editing any file in src/**, or making a non-trivial change anywhere, you MUST first read the knowledge files that cover the affected domain. Trivial edits (typos, comments, formatting) are exempt.
> - **Write gate:** During or after any task, if you discover a new invariant, state-ownership fact, data-flow edge, design decision, or tension that is not already in a knowledge file, you MUST add it to the correct file using the structured notation (OWNS / READS FROM / WRITES TO / INVARIANT / FLOW / TENSION / DECIDED).
> - **Rule of thumb:** If a proposed change contradicts a `DECIDED` entry, cite the entry and require justification before implementing.

## Knowledge Reference

This repo loads knowledge files from: .github/knowledge/

| File | Domain |
|------|--------|
| .github/knowledge/game-loop.md | game-loop |
| .github/knowledge/character-creation.md | character-creation |
| .github/knowledge/sanity-system.md | sanity-system |
| .github/knowledge/religion-system.md | religion-system |
| .github/knowledge/combat-weapons.md | combat-weapons |
| .github/knowledge/items-inventory.md | items-inventory |
| .github/knowledge/map-generation.md | map-generation |
| .github/knowledge/persistence-save.md | persistence-save |

> After adding or updating knowledge files, update this table so future sessions can find them quickly.
