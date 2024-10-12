using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAppearance
{
    AppearanceSchema Schema { get; }

    void Render(AppearanceRenderData appearanceRenderData);
}
