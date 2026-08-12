# Trapezoid BoxCollider2D Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add one axis-aligned `BoxCollider2D` covering the complete local bounds of every generated trapezoid.

**Architecture:** `TrapezoidRunner` will own a small private collider configurator. Both new-object and existing-prefab generation paths will invoke it so XML and serialized levels behave identically, and the configurator will reuse an existing collider.

**Tech Stack:** Unity C#, Unity Physics 2D, NUnit EditMode tests

## Global Constraints

- Use one bounding `BoxCollider2D`, not an exact or stepped slope approximation.
- Do not change the level serialization format or legacy custom collision calculations.
- Preserve unrelated working-tree changes.

---

### Task 1: Trapezoid collider generation

**Files:**
- Create: `Assets/Tests/Editor/TrapezoidRunnerTests.cs`
- Modify: `Assets/Scripts/Nekki/Vector/Core/Location/TrapezoidRunner.cs`

**Interfaces:**
- Consumes: `TrapezoidRunner(string name, int type, float x, float y, float width, float height, float height1, bool sticky)`, `Runner.Generate()`, and `Runner.Generate(GameObject)`.
- Produces: one configured `BoxCollider2D` on `TrapezoidRunner.UnityObject` for either generation path.

- [ ] **Step 1: Write the failing EditMode tests**

Create parameterized tests that construct `(x: 10, y: 20, width: 8)` trapezoids with `(height, height1)` values `(4,4)`, `(6,2)`, and `(2,6)`, call `Generate()`, and assert collider `size == (8, maxY-minY)` and local `offset == ((minX+maxX)/2-x, (minY+maxY)/2-y)`. Add a test that supplies an existing GameObject with a `BoxCollider2D`, calls `Generate(existing)`, and asserts the same component is reused and the component count remains one.

- [ ] **Step 2: Run the focused tests and verify RED**

Run the Unity EditMode test class `TrapezoidRunnerTests`.

Expected: the new-object cases fail because no `BoxCollider2D` exists, and the existing-object case fails because its collider retains its default size and offset.

- [ ] **Step 3: Add the minimal shared collider configurator**

In `TrapezoidRunner`, add a private method that calls `CalcPoints()`, computes min/max X and Y across `_Point1` through `_Point4`, converts the bounds center to GameObject-local coordinates by subtracting `_CachedTransform.localPosition`, reuses `GetComponent<BoxCollider2D>()` or adds one, and assigns positive `size` and the calculated `offset`. Call it after `base.GenerateObject()` in `GenerateObject()`, and override `Generate(GameObject existRunner)` to call `base.Generate(existRunner)` followed by the configurator when the object is non-null.

- [ ] **Step 4: Run focused and full EditMode tests and verify GREEN**

Run `TrapezoidRunnerTests`, then all EditMode tests.

Expected: all tests pass with no compilation errors or unexpected warnings.

- [ ] **Step 5: Verify Unity compilation and console state**

Wait until Unity reports compilation complete, then inspect the console for errors. Expected: no compilation or runtime errors related to the change.

- [ ] **Step 6: Commit the implementation**

Stage only `Assets/Tests/Editor/TrapezoidRunnerTests.cs`, its Unity `.meta` file if generated, and `Assets/Scripts/Nekki/Vector/Core/Location/TrapezoidRunner.cs`, then commit them with a focused feature message.
