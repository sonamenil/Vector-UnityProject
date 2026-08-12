# Trapezoid BoxCollider2D Support

## Goal

Give every generated trapezoid one axis-aligned `BoxCollider2D` that covers the trapezoid's complete local bounds, matching the existing collider behavior used by platforms.

## Design

`TrapezoidRunner` will use one shared collider configurator from both generation paths: `GenerateObject()` for XML-created trapezoids and `Generate(GameObject)` for prefab-deserialized trapezoids. The configurator will calculate the trapezoid's four points and derive their minimum and maximum local coordinates. It will add or reuse a single `BoxCollider2D`, then set the collider's `size` and `offset` from those bounds.

The points are stored in level coordinates, while the generated GameObject is positioned at the trapezoid origin. Collider bounds must therefore be converted to coordinates local to that GameObject before assigning `size` and `offset`.

Using calculated points keeps the collider consistent with the trapezoid geometry for equal-height rectangles, left-high slopes, and right-high slopes. Reusing an existing collider prevents duplicate components if generation is repeated or a serialized prefab already contains one.

Both XML-created and prefab-deserialized trapezoids will call the same collider configurator, so no serialization format changes are required.

## Error Handling

Valid source data is expected to have non-negative width and heights, as in the current level format. The implementation will normalize min/max point bounds, so either slope direction produces a positive collider size. It will not introduce unrelated validation or repair malformed level data.

## Tests

EditMode tests will generate trapezoids and verify:

- equal heights produce the expected rectangular bounds;
- a left-high trapezoid covers the full vertical range;
- a right-high trapezoid covers the full vertical range with the correct local offset;
- an existing `BoxCollider2D` is reused instead of adding a duplicate.

## Out of Scope

The collider is intentionally a bounding box, not an exact or stepped approximation of the sloped trapezoid surface. Existing custom trapezoid collision calculations remain unchanged.
