1. Targeting Rules
 - targeting is live (updates every frame during cast)
 - point abilities use raycast from camera crosshair
 - target abilities use raycast enemy under crosshair
 - target validity is checked during targeting phase

2. Range Rules
 - all target abilities must respect AbiltiyData.castRange
 - range validation happens during targeting calculation (NOT execution)
 - Self abilities ignore range

3. Cast Flow Rules
 - instant cast:
  - build targeting data
  - execute immediately
 - confirm cast:
  - enter abilityCast session
  - update targeting every frame
  - confirm uses latest targeting data snapshot
  - cancel removes session

4. Cancelation Rules
 - same abilities pressed again -> ignored
 - different abilities pressed -> replaced current cast
 - cancel input -> destroy cast session

 5. Execution Rules
 - execution always use AbilityTargetingData snapshot
 - execution does not perform targeting validation (exept safety null checks)
 - Area effects use resolved impact point or cast target

 6. Known Edge Cases
 - target may move after being selected
 - target may die after selection (current not validated at confirm)
 - chain lightning uses same targeting rules as other Target abilities