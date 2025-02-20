using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAppearance
{
    AppearanceSchema Schema { get; }

    bool IsInitialized { get; }
    void Render(AppearanceAnimationData appearanceRenderData);
}
