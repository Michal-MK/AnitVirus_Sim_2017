﻿using UnityEngine;

/// <summary>
/// Delegate for handling <see cref="Player"/>'s collision with other <see cref="GameObject"/>s. 
/// </summary>
public delegate void PlayerColisionEventHandler(Player sender, GameObject other);
