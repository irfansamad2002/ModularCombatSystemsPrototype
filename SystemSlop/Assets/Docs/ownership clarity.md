| System        | Owns                           | Does NOT own    |
| ------------- | ------------------------------ | --------------- |
| AbilityUser   | cooldowns, execution trigger   | targeting logic |
| AbilityCast   | cast lifecycle, cancel/confirm | ability effects |
| Calculator    | targeting rules                | execution       |
| Indicator     | visuals only                   | validation      |
| TargetingData | snapshot only                  | logic           |
