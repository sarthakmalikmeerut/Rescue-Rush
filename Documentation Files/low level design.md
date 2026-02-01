┌───────────────────────────┐
│        GAME LOOP          │
└───────────┬───────────────┘
            │
            ▼
┌───────────────────────────┐
│ Check Patient Spawn Timer │
└───────────┬───────────────┘
            │
     Yes ───┴─── No
      │           │
      ▼           ▼
┌───────────┐   (Skip)
│ Spawn     │
│ Patient   │
└─────┬─────┘
      │
      ▼
┌───────────────────────────┐
│ Update Patient Timers     │
│ (Critical / Expire)       │
└───────────┬───────────────┘
            │
            ▼
┌───────────────────────────┐
│ Player Input (Movement)   │
│ Acceleration / Turning    │
└───────────┬───────────────┘
            │
            ▼
┌───────────────────────────┐
│ Collision Check           │
│ • Patient Pickup          │
│ • Hospital Drop           │
│ • Obstacles               │
└───────────┬───────────────┘
            │
            ▼
┌───────────────────────────┐
│ Update Game State         │
│ • Score                   │
│ • Active Patients         │
│ • Ambulance Capacity      │
└───────────┬───────────────┘
            │
            ▼
┌───────────────────────────┐
│ Check Fail Conditions     │
│ • Patient Died            │
│ • Time Over               │
└───────────┬───────────────┘
            │
      Yes ──┴── No
       │         │
       ▼         ▼
┌───────────┐  (Loop Back)
│ GAME OVER │
└───────────┘
